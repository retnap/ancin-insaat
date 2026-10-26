#!/usr/bin/env bash
# Ancın İnşaat — siteyi GitHub'daki son sürüme günceller.
#
# Hostinger tarayıcı terminalinde:
#   bash /opt/ancin/src/deploy/update.sh
#
# Canlı veritabanına (/var/lib/ancin) ve yüklenen CV'lere dokunmaz.

set -euo pipefail

SRC=/opt/ancin/src
PUBLISH=/opt/ancin/publish
WWW=/var/www/ancin
BRANCH="main"
export DOTNET_CLI_TELEMETRY_OPTOUT=1 DOTNET_NOLOGO=1

step() { printf '\n\033[1;34m==> %s\033[0m\n' "$1"; }

step "GitHub'dan son sürüm alınıyor"
cd "$SRC"
git fetch --depth 1 origin "$BRANCH"
git reset --hard "origin/$BRANCH"
git lfs pull
git lfs prune >/dev/null 2>&1 || true

step "Site derleniyor"
dotnet publish AncinInsaat/AncinInsaat.csproj -c Release -o "$PUBLISH" --nologo

step "Yayına alınıyor"
systemctl stop ancin 2>/dev/null || true
rsync -a --delete --exclude /App_Data "$PUBLISH/" "$WWW/"
ln -sfn /var/lib/ancin "$WWW/App_Data"
systemctl start ancin

# Site ayağa kalkana kadar en fazla 60 sn bekle
for i in $(seq 1 30); do
  CODE=$(curl -s -o /dev/null -w '%{http_code}' http://127.0.0.1:5000/ || true)
  if [ "$CODE" = "200" ]; then
    printf '\n\033[1;32mSite çalışıyor.\033[0m\n'
    exit 0
  fi
  sleep 2
done

echo "UYARI: Site 60 saniyede açılmadı. Son kayıtlar:" >&2
journalctl -u ancin -n 40 --no-pager >&2
exit 1
