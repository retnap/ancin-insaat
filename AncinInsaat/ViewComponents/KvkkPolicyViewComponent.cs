using AncinInsaat.Models;
using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.ViewComponents;

// KVKK "Introduction" + "Privacy Policy" sections (docs/03_PageBlueprints.md
// — Page KVKK) folded into one continuous document, per the project owner's
// 2026-08-03 approval: a single centered Section Header (Employee
// Development's "section-header--center" + narrow-copy-column shape,
// Section 33/34 of site.css) followed by a short opening paragraph and then
// the numbered legal sections — rather than two separate <section> blocks
// (Introduction, then Privacy Policy) with a full section gap between two
// short pieces of the same read. Explicitly no Accordion (plain
// headings/paragraphs) and no Download Button (no placeholder PDF) per the
// same approval — see KvkkPolicySection.
//
// ============================================================================
// PLACEHOLDER LEGAL TEXT — DO NOT SHIP AS FINAL.
// The paragraphs below follow the standard structure of a Turkish KVKK
// Aydınlatma Metni (6698 sayılı Kanun) but are NOT reviewed or approved by
// the client or legal counsel. They must be replaced with Ançın İnşaat's
// lawyer-approved KVKK disclosure text before this page goes to production.
// ============================================================================
public class KvkkPolicyViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var model = new KvkkPolicyViewModel
        {
            SectionHeader = new SectionHeaderModel
            {
                HeadingId = "kvkk-policy-heading",
                Eyebrow = "KİŞİSEL VERİLERİN KORUNMASI",
                Title = "Aydınlatma Metni",
                CssClass = "section-header--center"
            },
            IntroParagraphs = new List<string>
            {
                "Ancın İnşaat olarak, 6698 sayılı Kişisel Verilerin Korunması Kanunu (\"KVKK\") " +
                    "kapsamında kişisel verilerinizin güvenliğine büyük önem veriyoruz. Bu " +
                    "aydınlatma metni; web sitemiz ve hizmetlerimiz aracılığıyla topladığımız " +
                    "kişisel verilerin hangi amaçla işlendiği, kimlerle paylaşılabileceği, toplama " +
                    "yöntemi ve hukuki sebebi ile KVKK kapsamındaki haklarınız hakkında sizi " +
                    "bilgilendirmek amacıyla hazırlanmıştır."
            },
            Sections = new List<KvkkPolicySection>
            {
                new()
                {
                    Heading = "1. Veri Sorumlusu",
                    Paragraphs = new List<string>
                    {
                        "Kişisel verileriniz, veri sorumlusu sıfatıyla Ancın İnşaat tarafından, " +
                            "aşağıda açıklanan amaçlar doğrultusunda ve mevzuata uygun şekilde " +
                            "işlenmektedir."
                    }
                },
                new()
                {
                    Heading = "2. Kişisel Verilerin Hangi Amaçla İşleneceği",
                    Paragraphs = new List<string>
                    {
                        "Tarafımıza ilettiğiniz kişisel veriler; talep ve şikayetlerinizin " +
                            "değerlendirilmesi, iletişim faaliyetlerinin yürütülmesi, iş " +
                            "başvurularının değerlendirilmesi, proje ve hizmetlerimize ilişkin " +
                            "bilgilendirme yapılması ile yasal yükümlülüklerimizin yerine " +
                            "getirilmesi amaçlarıyla işlenmektedir."
                    }
                },
                new()
                {
                    Heading = "3. İşlenen Kişisel Verilerin Kimlere ve Hangi Amaçla Aktarılabileceği",
                    Paragraphs = new List<string>
                    {
                        "Kişisel verileriniz, yasal yükümlülüklerimizin yerine getirilmesi " +
                            "amacıyla yetkili kamu kurum ve kuruluşlarıyla, hizmet aldığımız iş " +
                            "ortaklarımız ve tedarikçilerimizle, KVKK'nın öngördüğü güvenlik ve " +
                            "gizlilik ilkelerine uygun olarak sınırlı ölçüde paylaşılabilir."
                    }
                },
                new()
                {
                    Heading = "4. Kişisel Veri Toplamanın Yöntemi ve Hukuki Sebebi",
                    Paragraphs = new List<string>
                    {
                        "Kişisel verileriniz; web sitemizdeki iletişim ve başvuru formları, " +
                            "e-posta, telefon ve benzeri kanallar aracılığıyla, açık rızanızın " +
                            "bulunması, bir sözleşmenin kurulması veya ifasıyla doğrudan ilgili " +
                            "olması ve meşru menfaatlerimiz gibi KVKK'nın 5. ve 6. maddelerinde " +
                            "belirtilen hukuki sebeplere dayanılarak toplanmaktadır."
                    }
                },
                new()
                {
                    Heading = "5. Kişisel Veri Sahibinin Hakları",
                    Paragraphs = new List<string>
                    {
                        "KVKK'nın 11. maddesi uyarınca; kişisel verilerinizin işlenip " +
                            "işlenmediğini öğrenme, işlenmişse buna ilişkin bilgi talep etme, " +
                            "işlenme amacını ve amacına uygun kullanılıp kullanılmadığını öğrenme, " +
                            "yurt içinde veya yurt dışında aktarıldığı üçüncü kişileri bilme, eksik " +
                            "veya yanlış işlenmişse düzeltilmesini isteme, KVKK'da öngörülen " +
                            "şartlar çerçevesinde silinmesini veya yok edilmesini isteme ve işlenen " +
                            "verilerin münhasıran otomatik sistemler ile analiz edilmesi nedeniyle " +
                            "aleyhinize bir sonucun ortaya çıkmasına itiraz etme haklarına " +
                            "sahipsiniz."
                    }
                },
                new()
                {
                    Heading = "6. Başvuru Yöntemi",
                    Paragraphs = new List<string>
                    {
                        "Yukarıda sayılan haklarınıza ilişkin taleplerinizi, bu sayfanın altında " +
                            "yer alan iletişim bilgilerimiz üzerinden Ancın İnşaat'a yazılı olarak " +
                            "iletebilirsiniz. Talepleriniz, niteliğine göre en kısa sürede ve en " +
                            "geç otuz gün içinde sonuçlandırılacaktır."
                    }
                }
            }
        };

        return View(model);
    }
}
