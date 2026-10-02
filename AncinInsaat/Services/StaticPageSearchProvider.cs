using System.Globalization;
using AncinInsaat.Models;

namespace AncinInsaat.Services;

// Every static public page (docs/01_SiteMap.md) except Project Detail,
// which ProjectSearchProvider supplies from the database instead. SortOrder
// groups pages by section (Sayfa 0-9, Kurumsal 10-19, Kariyer/İletişim
// 40-59) leaving 20-39 free for Projeler + its detail pages.
//
// English localization (2026-10-02) — a second, English index (Url
// prefixed "/en", same SortOrder/Category grouping) so a search performed
// on an English page returns English results linking to English URLs;
// GetItemsAsync picks whichever list matches the current request's
// culture (set by RouteSegmentRequestCultureProvider from the URL).
public class StaticPageSearchProvider : ISearchIndexProvider
{
    private static readonly IReadOnlyList<SearchResultItem> Items = new List<SearchResultItem>
    {
        new()
        {
            Title = "Anasayfa",
            Description = "Ancın İnşaat hakkında genel bakış ve öne çıkan projeler.",
            Url = "/",
            Category = "Sayfa",
            Keywords = "ana sayfa giriş inşaat firması",
            SortOrder = 0
        },
        new()
        {
            Title = "Hakkımızda",
            Description = "Kurumsal geçmişimiz, hikayemiz ve yolculuğumuz.",
            Url = "/about-us",
            Category = "Kurumsal",
            Keywords = "şirket tarihçe kurumsal kimlik hakkında",
            SortOrder = 10
        },
        new()
        {
            Title = "Değerlerimiz",
            Description = "Bizi biz yapan ilkeler ve kalite anlayışımız.",
            Url = "/values",
            Category = "Kurumsal",
            Keywords = "ilkeler misyon vizyon kalite anlayışı",
            SortOrder = 11
        },
        new()
        {
            Title = "İnsan Kaynakları Politikası",
            Description = "Çalışan gelişimi ve kurum kültürümüz hakkında bilgi.",
            Url = "/hr-policy",
            Category = "Kurumsal",
            Keywords = "ik insan kaynakları çalışan kurum kültürü politika",
            SortOrder = 12
        },
        new()
        {
            Title = "KVKK",
            Description = "Kişisel verilerin korunması hakkında aydınlatma metni.",
            Url = "/kvkk",
            Category = "Kurumsal",
            Keywords = "kişisel verilerin korunması kanunu aydınlatma metni gizlilik",
            SortOrder = 13
        },
        new()
        {
            Title = "Sosyal Sorumluluk Projelerimiz",
            Description = "Topluma katkı sağladığımız sosyal sorumluluk projelerimiz.",
            Url = "/sosyal-sorumluluk-projelerimiz",
            Category = "Kurumsal",
            Keywords = "sosyal sorumluluk toplum gençlik spor etkinlik",
            SortOrder = 14
        },
        new()
        {
            Title = "Projeler",
            Description = "Tüm devam eden ve tamamlanan projelerimiz.",
            Url = "/projects",
            Category = "Projeler",
            Keywords = "konut villa ofis ticari proje listesi",
            SortOrder = 20
        },
        new()
        {
            Title = "Kariyer",
            Description = "Açık pozisyonlar ve iş başvuru formu.",
            Url = "/career",
            Category = "Sayfa",
            Keywords = "iş ilanları başvuru pozisyon istihdam",
            SortOrder = 40
        },
        new()
        {
            Title = "İletişim",
            Description = "Bize ulaşın, adres, telefon ve harita bilgileri.",
            Url = "/contact",
            Category = "Sayfa",
            Keywords = "adres telefon harita bize ulaşın e-posta",
            SortOrder = 50
        }
    };

    private static readonly IReadOnlyList<SearchResultItem> ItemsEn = new List<SearchResultItem>
    {
        new()
        {
            Title = "Home",
            Description = "An overview of Ançın İnşaat and our featured projects.",
            Url = "/en",
            Category = "Page",
            Keywords = "home landing construction company",
            SortOrder = 0
        },
        new()
        {
            Title = "About Us",
            Description = "Our corporate history, story and journey.",
            Url = "/en/about-us",
            Category = "Corporate",
            Keywords = "company history corporate identity about",
            SortOrder = 10
        },
        new()
        {
            Title = "Our Values",
            Description = "The principles that define us and our approach to quality.",
            Url = "/en/values",
            Category = "Corporate",
            Keywords = "principles mission vision quality approach",
            SortOrder = 11
        },
        new()
        {
            Title = "Human Resources Policy",
            Description = "Information about employee development and our corporate culture.",
            Url = "/en/hr-policy",
            Category = "Corporate",
            Keywords = "hr human resources employee corporate culture policy",
            SortOrder = 12
        },
        new()
        {
            Title = "KVKK",
            Description = "Our notice on the protection of personal data.",
            Url = "/en/kvkk",
            Category = "Corporate",
            Keywords = "personal data protection privacy notice",
            SortOrder = 13
        },
        new()
        {
            Title = "Social Responsibility",
            Description = "Our social responsibility projects that give back to the community.",
            Url = "/en/social-responsibility",
            Category = "Corporate",
            Keywords = "social responsibility community youth sport event",
            SortOrder = 14
        },
        new()
        {
            Title = "Projects",
            Description = "All of our ongoing and completed projects.",
            Url = "/en/projects",
            Category = "Projects",
            Keywords = "residence villa office commercial project list",
            SortOrder = 20
        },
        new()
        {
            Title = "Career",
            Description = "Open positions and the job application form.",
            Url = "/en/career",
            Category = "Page",
            Keywords = "jobs application position employment",
            SortOrder = 40
        },
        new()
        {
            Title = "Contact",
            Description = "Get in touch with us — address, phone and map information.",
            Url = "/en/contact",
            Category = "Page",
            Keywords = "address phone map contact email",
            SortOrder = 50
        }
    };

    public Task<IReadOnlyList<SearchResultItem>> GetItemsAsync(CancellationToken cancellationToken = default)
    {
        var isEnglish = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.Equals("en", StringComparison.OrdinalIgnoreCase);
        return Task.FromResult(isEnglish ? ItemsEn : Items);
    }
}
