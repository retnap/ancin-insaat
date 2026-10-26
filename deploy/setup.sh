#!/usr/bin/env bash
# Ancın İnşaat — sunucu ilk kurulum betiği (Ubuntu 24.04, root olarak bir kez çalıştırılır)
#
# Hostinger tarayıcı terminalinde:
#   curl -fsSL https://raw.githubusercontent.com/retnap/ancin-insaat/main/deploy/setup.sh | bash
#
# Yaptıkları:
#   - .NET 8 SDK, Nginx, Git LFS, SQLite araçları ve güvenlik duvarını kurar
#   - Projeyi GitHub'dan /opt/ancin/src klasörüne indirir (fotoğraf/videolar dahil)
#   - Canlı veritabanını ve CV dosyalarını proje dışında, /var/lib/ancin içinde tutar
#   - Siteyi "ancin" adlı bir servis olarak çalıştırır (sunucu yeniden başlasa da açılır)
#   - Nginx'i ayarlar: fotoğraf/video/PDF'leri doğrudan diskten sunar, gerisini siteye iletir
#   - Her gece veritabanı + CV yedeği alır (/var/backups/ancin, 14 gün saklanır)
# Tekrar çalıştırmak güvenlidir: mevcut veritabanına ve yüklenen dosyalara dokunmaz.

set -euo pipefail

REPO_URL="https://github.com/retnap/ancin-insaat.git"
BRANCH="main"
SRC=/opt/ancin/src
DATA=/var/lib/ancin
WWW=/var/www/ancin

if [ "$(id -u)" -ne 0 ]; then
  echo "Bu betik root olarak çalıştırılmalı." >&2
  exit 1
fi

step() { printf '\n\033[1;34m==> %s\033[0m\n' "$1"; }

step "1/7 Gerekli programlar kuruluyor (birkaç dakika sürebilir)"
export DEBIAN_FRONTEND=noninteractive
apt-get update -q
apt-get install -y -q git git-lfs nginx sqlite3 rsync ufw curl dotnet-sdk-8.0
git lfs install --system >/dev/null
export DOTNET_CLI_TELEMETRY_OPTOUT=1 DOTNET_NOLOGO=1

step "2/7 Servis kullanıcısı ve klasörler hazırlanıyor"
id ancin >/dev/null 2>&1 || useradd --system --home-dir "$DATA" --shell /usr/sbin/nologin ancin
mkdir -p "$DATA/uploads" "$WWW" /opt/ancin /var/backups/ancin
chmod 700 /var/backups/ancin

step "3/7 Proje GitHub'dan indiriliyor (fotoğraf ve videolarla birlikte ~9 GB, sabır)"
if [ ! -d "$SRC/.git" ]; then
  git clone --depth 1 --branch "$BRANCH" "$REPO_URL" "$SRC"
else
  echo "Proje zaten indirilmiş, güncelleme adımında yenilenecek."
fi

# Kontrol: fotoğraflar gerçekten indi mi, yoksa sadece LFS işaretçisi mi geldi?
SAMPLE=$(cd "$SRC" && git lfs ls-files -n | head -n 1 || true)
if [ -n "$SAMPLE" ] && head -c 100 "$SRC/$SAMPLE" | grep -q "git-lfs.github.com/spec"; then
  echo "UYARI: Fotoğraf/videolar indirilemedi (GitHub LFS kotası dolmuş olabilir)." >&2
  echo "Kurulum durduruldu. Bu mesajı Claude'a iletin." >&2
  exit 1
fi

step "4/7 Canlı veritabanı hazırlanıyor"
if [ ! -f "$DATA/ancinInsaat.db" ]; then
  cp "$SRC/AncinInsaat/App_Data/ancinInsaat.db" "$DATA/ancinInsaat.db"
  echo "Başlangıç veritabanı kopyalandı: $DATA/ancinInsaat.db"
else
  echo "Mevcut canlı veritabanı korunuyor."
fi
chown -R ancin:ancin "$DATA"
chmod 750 "$DATA"

step "5/7 Site servisi tanımlanıyor"
cat > /etc/systemd/system/ancin.service <<'EOF'
[Unit]
Description=Ancin Insaat web sitesi
After=network.target

[Service]
WorkingDirectory=/var/www/ancin
ExecStart=/usr/bin/dotnet /var/www/ancin/AncinInsaat.dll
Restart=always
RestartSec=5
User=ancin
Group=ancin
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=PORT=5000
Environment=ASPNETCORE_FORWARDEDHEADERS_ENABLED=true
Environment="ConnectionStrings__DefaultConnection=Data Source=/var/lib/ancin/ancinInsaat.db"
Environment=DOTNET_CLI_TELEMETRY_OPTOUT=1

[Install]
WantedBy=multi-user.target
EOF
systemctl daemon-reload
systemctl enable ancin >/dev/null

step "6/7 Nginx ve güvenlik duvarı ayarlanıyor"
cat > /etc/nginx/sites-available/ancin <<'EOF'
server {
    listen 80 default_server;
    listen [::]:80 default_server;
    server_name ancin.com.tr www.ancin.com.tr _;

    # CV yüklemeleri 5 MB ile sınırlı; form verisiyle birlikte rahat sığsın
    client_max_body_size 10M;

    gzip on;
    gzip_types text/css application/javascript image/svg+xml application/json text/plain;

    root /var/www/ancin/wwwroot;

    # Fotoğraf, video, PDF ve fontlar doğrudan diskten (siteyi yormadan) sunulur
    location ~* \.(?:jpe?g|png|webp|gif|svg|ico|avif|mp4|webm|mov|pdf|woff2?|ttf)$ {
        try_files $uri @app;
        expires 7d;
        add_header Cache-Control "public";
        access_log off;
    }

    # CSS/JS sürüm numarasıyla çağrılıyor, uzun süre önbellekte kalabilir
    location ~* \.(?:css|js)$ {
        try_files $uri @app;
        expires 30d;
        add_header Cache-Control "public";
    }

    location / {
        proxy_pass         http://127.0.0.1:5000;
        proxy_http_version 1.1;
        proxy_set_header   Host              $host;
        proxy_set_header   X-Forwarded-For   $proxy_add_x_forwarded_for;
        proxy_set_header   X-Forwarded-Proto $scheme;
        proxy_read_timeout 120s;
    }

    location @app {
        proxy_pass         http://127.0.0.1:5000;
        proxy_http_version 1.1;
        proxy_set_header   Host              $host;
        proxy_set_header   X-Forwarded-For   $proxy_add_x_forwarded_for;
        proxy_set_header   X-Forwarded-Proto $scheme;
        proxy_read_timeout 120s;
    }
}
EOF
ln -sfn /etc/nginx/sites-available/ancin /etc/nginx/sites-enabled/ancin
rm -f /etc/nginx/sites-enabled/default
nginx -t
systemctl reload nginx

ufw allow OpenSSH >/dev/null
ufw allow 'Nginx Full' >/dev/null
ufw --force enable >/dev/null

# Gece yedeği: veritabanı + CV dosyaları, 14 gün saklanır
cat > /etc/cron.daily/ancin-yedek <<'EOF'
#!/bin/sh
set -e
D=/var/backups/ancin
T=$(date +%F)
sqlite3 /var/lib/ancin/ancinInsaat.db ".backup '$D/veritabani-$T.db'"
tar -czf "$D/cv-dosyalari-$T.tar.gz" -C /var/lib/ancin uploads
find "$D" -type f -mtime +14 -delete
EOF
chmod 755 /etc/cron.daily/ancin-yedek

step "7/7 Site derlenip yayına alınıyor"
bash "$SRC/deploy/update.sh"

IP=$(curl -fsS -4 https://ifconfig.me 2>/dev/null || hostname -I | awk '{print $1}')
printf '\n\033[1;32mKurulum tamamlandı.\033[0m Tarayıcıda açın: http://%s\n' "$IP"
