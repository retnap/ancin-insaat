using System.Globalization;
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
// the client or legal counsel. They must be replaced with Ancın İnşaat's
// lawyer-approved KVKK disclosure text before this page goes to production.
// ============================================================================
public class KvkkPolicyViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        // English localization (English copy revision, 2026-11-03) — an
        // informational English translation of the Turkish notice, closed
        // with a "Turkish version prevails" statement (client request).
        var isEnglish = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.Equals("en", StringComparison.OrdinalIgnoreCase);

        if (isEnglish)
        {
            return View(BuildEnglishModel());
        }

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

    private static KvkkPolicyViewModel BuildEnglishModel() => new()
    {
        SectionHeader = new SectionHeaderModel
        {
            HeadingId = "kvkk-policy-heading",
            Eyebrow = "PERSONAL DATA PROTECTION",
            Title = "Information Notice",
            CssClass = "section-header--center"
        },
        IntroParagraphs = new List<string>
        {
            "Ancın İnşaat attaches great importance to the security of your personal data under " +
                "the Turkish Personal Data Protection Law No. 6698 (\"KVKK\"). This notice has " +
                "been prepared to inform you of the purposes for which personal data collected " +
                "through our website and services is processed, the parties with whom it may be " +
                "shared, the method and legal basis of its collection, and your rights under the KVKK."
        },
        Sections = new List<KvkkPolicySection>
        {
            new()
            {
                Heading = "1. Data Controller",
                Paragraphs = new List<string>
                {
                    "Your personal data is processed by Ancın İnşaat, as data controller, for the " +
                        "purposes described below and in accordance with the applicable legislation."
                }
            },
            new()
            {
                Heading = "2. Purposes of Processing",
                Paragraphs = new List<string>
                {
                    "The personal data you provide to us is processed for the purposes of evaluating " +
                        "your requests and complaints, carrying out communication activities, " +
                        "evaluating job applications, providing information about our projects and " +
                        "services, and fulfilling our legal obligations."
                }
            },
            new()
            {
                Heading = "3. Recipients of Personal Data and Purposes of Transfer",
                Paragraphs = new List<string>
                {
                    "Your personal data may be shared, to a limited extent, with authorised public " +
                        "institutions and organisations in order to fulfil our legal obligations, and " +
                        "with the business partners and suppliers from whom we receive services, in " +
                        "line with the security and confidentiality principles set out in the KVKK."
                }
            },
            new()
            {
                Heading = "4. Method and Legal Basis of Collection",
                Paragraphs = new List<string>
                {
                    "Your personal data is collected through the contact and application forms on " +
                        "our website, email, telephone and similar channels, on the legal grounds set " +
                        "out in Articles 5 and 6 of the KVKK, such as your explicit consent, the " +
                        "processing being directly related to the conclusion or performance of a " +
                        "contract, and our legitimate interests."
                }
            },
            new()
            {
                Heading = "5. Rights of the Data Subject",
                Paragraphs = new List<string>
                {
                    "Under Article 11 of the KVKK, you have the right to learn whether your personal " +
                        "data is processed; to request information if it has been processed; to learn " +
                        "the purpose of processing and whether it is used in line with that purpose; " +
                        "to know the third parties in Türkiye or abroad to whom it is transferred; to " +
                        "request its correction if it is incomplete or inaccurate; to request its " +
                        "erasure or destruction under the conditions set out in the KVKK; and to " +
                        "object to any outcome against you arising from the analysis of the processed " +
                        "data exclusively by automated systems."
                }
            },
            new()
            {
                Heading = "6. How to Apply",
                Paragraphs = new List<string>
                {
                    "You may submit requests concerning the rights listed above to Ancın İnşaat in " +
                        "writing, using the contact details at the bottom of this page. Requests will " +
                        "be concluded as soon as possible, depending on their nature, and within " +
                        "thirty days at the latest."
                }
            }
        },
        ClosingNote = "This English version is provided for information purposes only. In case of " +
            "any discrepancy, the Turkish version shall prevail."
    };
}
