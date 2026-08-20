using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.ViewComponents;

// HR Policy content sections (docs/03_PageBlueprints.md — Page HR Policy).
// Folkart-reference revision (2026-08-17, project owner-approved): eight
// heading + bullet-list sections reproducing the reference "İK
// Politikamız" screenshot's structure, heading order and per-section
// bullet counts exactly — no cards, icons or numbering, plain uppercase
// navy headings over plain bullet lists per HrPolicySections/Default.cshtml.
// Static content, same hardcoded-copy precedent as CoreValuesViewComponent
// — HR Policy has exactly one set of these sections, so no calling page
// needs to pass data in.
//
// Section headings are real content, reproduced verbatim from the Folkart
// reference (already uppercase, matching it — not relying on CSS
// text-transform, which mis-cases the Turkish dotted/dotless İ/I pair
// unless the document language is set correctly). Bullet copy (2026-08-20)
// replaces the earlier Lorem Ipsum placeholders with realistic-but-generic
// HR policy statements appropriate to a construction company, per CLAUDE.md's
// Placeholder Content rules — no specific company fact (headcount, benefit
// amounts, program names) is invented; replace with the client's approved
// policy wording before launch.
public class HrPolicySectionsViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var model = new HrPolicySectionsViewModel
        {
            Sections = new List<HrPolicySectionItem>
            {
                new()
                {
                    Heading = "YETENEK KAZANIMI",
                    Bullets = new List<string>
                    {
                        "İşe alım süreçlerimizi şeffaflık, fırsat eşitliği ve liyakat ilkesine dayalı olarak yürütürüz.",
                        "Her pozisyon için görev tanımına uygun, objektif değerlendirme kriterleri belirleriz.",
                        "Aday deneyimini önemseyerek başvurudan işe başlangıca kadar açık ve zamanında geri bildirim sağlarız.",
                        "Şirket kültürümüze ve değerlerimize uyum gösterecek yetenekleri bünyemize katmayı hedefleriz.",
                        "Oryantasyon sürecinde yeni çalışanlarımızı ekibimize ve çalışma ortamımıza kademeli olarak entegre ederiz."
                    }
                },
                new()
                {
                    Heading = "PERFORMANS YÖNETİMİ",
                    Bullets = new List<string>
                    {
                        "Performans değerlendirmelerini net, ölçülebilir ve önceden belirlenmiş hedefler üzerinden yaparız.",
                        "Düzenli geri bildirim görüşmeleriyle çalışanlarımızın gelişimini sürekli destekleriz.",
                        "Başarıyı adil bir şekilde tanır ve gelişim alanlarını yapıcı bir yaklaşımla ele alırız."
                    }
                },
                new()
                {
                    Heading = "ÜCRETLENDİRME VE ÖDÜL YÖNETİMİ",
                    Bullets = new List<string>
                    {
                        "Ücretlendirme politikamızı piyasa koşullarını ve iç adaleti gözeterek şekillendiririz.",
                        "Görev, sorumluluk ve performans arasında dengeli bir ücret yapısı kurarız.",
                        "Yasal yükümlülüklerimizi eksiksiz yerine getirerek çalışanlarımızın haklarını güvence altına alırız.",
                        "Başarılı performansı takdir eden ödüllendirme mekanizmaları uygularız.",
                        "Ücret ve yan haklar konusundaki uygulamalarımızı düzenli olarak gözden geçiririz.",
                        "Tüm ücretlendirme süreçlerini gizlilik ilkesiyle yürütürüz."
                    }
                },
                new()
                {
                    Heading = "KURUMSAL GELİŞİM VE ÖĞRENME",
                    Bullets = new List<string>
                    {
                        "Çalışanlarımızın mesleki ve kişisel gelişimini destekleyen eğitim programları sunarız.",
                        "Sektördeki teknik ve mimari gelişmeleri takip eden içerikleri ekiplerimizle paylaşırız.",
                        "İç ve dış kaynaklı eğitimlerle bilgi birikimini kurum genelinde yaygınlaştırırız.",
                        "Yeni beceriler kazanmayı teşvik eden bir öğrenme kültürünü destekleriz.",
                        "Gelişim planlarını çalışanlarımızın kariyer hedefleriyle uyumlu şekilde tasarlarız."
                    }
                },
                new()
                {
                    Heading = "KARİYER YÖNETİMİ",
                    Bullets = new List<string>
                    {
                        "Çalışanlarımıza şirket içinde net ve şeffaf kariyer gelişim yolları sunarız.",
                        "İç terfi ve rotasyon fırsatlarını önceliklendirerek uzun soluklu kariyerleri destekleriz."
                    }
                },
                new()
                {
                    Heading = "LİDERLİK",
                    Bullets = new List<string>
                    {
                        "Liderlerimizden şeffaf iletişim, adil karar alma ve örnek davranış bekleriz.",
                        "Yönetici gelişim programlarıyla geleceğin liderlerini bünyemizde yetiştiririz."
                    }
                },
                new()
                {
                    Heading = "ÇEŞİTLİLİK VE KAPSAYICILIK",
                    Bullets = new List<string>
                    {
                        "Cinsiyet, yaş, inanç veya köken gözetmeksizin fırsat eşitliğini temel ilke kabul ederiz.",
                        "Farklı bakış açılarının kurumumuza değer kattığına inanır, kapsayıcı bir çalışma ortamı oluştururuz.",
                        "Ayrımcılığa sıfır tolerans ilkesiyle yaklaşır, her çalışanımızın kendini güvende hissetmesini sağlarız.",
                        "Kapsayıcılık ilkelerimizi işe alımdan terfiye kadar her insan kaynakları sürecine yansıtırız."
                    }
                },
                new()
                {
                    Heading = "ETKİN İLETİŞİM VE İŞ BİRLİĞİ",
                    Bullets = new List<string>
                    {
                        "Açık ve karşılıklı saygıya dayalı bir iletişim kültürünü tüm kademelerde teşvik ederiz.",
                        "Ekipler arası iş birliğini güçlendiren düzenli bilgi paylaşım kanalları oluştururuz.",
                        "Çalışanlarımızın görüş ve önerilerini değerli buluruz, geri bildirim mekanizmalarını açık tutarız.",
                        "Sorunları çözüm odaklı bir yaklaşımla, yapıcı diyalog yoluyla ele alırız."
                    }
                }
            }
        };

        return View(model);
    }
}
