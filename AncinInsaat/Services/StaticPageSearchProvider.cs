using AncinInsaat.Models;

namespace AncinInsaat.Services;

// Every static public page (docs/01_SiteMap.md) except Project Detail,
// which ProjectSearchProvider supplies from the database instead. SortOrder
// groups pages by section (Sayfa 0-9, Kurumsal 10-19, Kariyer/İletişim
// 40-59) leaving 20-39 free for Projeler + its detail pages.
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

    public Task<IReadOnlyList<SearchResultItem>> GetItemsAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Items);
    }
}
