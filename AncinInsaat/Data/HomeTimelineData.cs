using AncinInsaat.Models;

namespace AncinInsaat.Data;

// Fixed four-entry timeline for Home's "Zaman Tüneli" section only
// (project owner-supplied dates/titles/images, 2026-08-19). Deliberately
// not merged into CompanyHistoryData — that list stays shared with About
// Us's "Our Journey" section unchanged; this one is Home-only.
public static class HomeTimelineData
{
    public static readonly IReadOnlyList<HomeTimelineEntryModel> Entries = new List<HomeTimelineEntryModel>
    {
        new()
        {
            Year = "1993",
            Title = "Samanyolu Sitesi",
            ImageSrc = "/images/timeline/1993-samanyolu-sitesi.jpg",
            ImageAlt = "Samanyolu Sitesi dış cephe görünümü",
            Summary = "Ancın İnşaat'ın ilk büyük ölçekli konut projelerinden biri olan Samanyolu Sitesi, 1993 yılında hayata geçirildi.",
            Detail = "Proje, çok bloklu yerleşim planı ve dönemin standartlarının üzerinde yapı kalitesiyle dikkat çekti. Samanyolu Sitesi, şirketin konut alanındaki tecrübesinin temelini oluşturan projelerden biri oldu ve sonraki yıllarda yürütülen projelere referans teşkil etti."
        },
        new()
        {
            Year = "2008",
            Title = "Meral Hanım Apt.",
            ImageSrc = "/images/timeline/2008-meral-hanim-apt.jpg",
            ImageAlt = "Meral Hanım Apt. dış cephe görünümü",
            Summary = "2008 yılında tamamlanan Meral Hanım Apt., şehir merkezine yakın konumuyla öne çıkan bir konut projesidir.",
            Detail = "Proje kapsamında daire kullanım alanları ve ortak yaşam alanları, dönemin ihtiyaçları gözetilerek planlandı. Meral Hanım Apt., Ancın İnşaat'ın orta ölçekli şehir içi projelerdeki deneyimini pekiştiren yapılardan biri oldu."
        },
        new()
        {
            Year = "2011",
            Title = "Tralles Gold",
            ImageSrc = "/images/timeline/2011-tralles-gold.jpg",
            ImageAlt = "Tralles Gold dış cephe görünümü",
            Summary = "Kasım 2011'de tamamlanan Tralles Gold, şirketin daha kapsamlı site projelerine geçiş sürecindeki önemli adımlarından biridir.",
            Detail = "Proje, sosyal donatı alanları ve peyzaj düzenlemesiyle birlikte planlandı. Tralles Gold, Ancın İnşaat'ın büyüyen proje ölçeğine uygun yapı ve saha organizasyonu yaklaşımının geliştirildiği dönemi temsil eder."
        },
        new()
        {
            Year = "2014",
            Title = "Alinda Gold",
            ImageSrc = "/images/timeline/2014-alinda-gold.jpg",
            ImageAlt = "Alinda Gold dış cephe görünümü",
            Summary = "Mayıs 2014'te tamamlanan Alinda Gold, dönemin modern mimari anlayışını yansıtan bir konut projesidir.",
            Detail = "Proje, iç mekân planlamasında işlevselliği ön planda tutan bir yaklaşımla tasarlandı. Alinda Gold, Ancın İnşaat'ın yapı standartlarını sürekli geliştirme çabasının bir yansıması olarak tamamlandı."
        }
    };
}
