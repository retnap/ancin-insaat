using AncinInsaat.Models;

namespace AncinInsaat.Data;

// Fixed timeline for Home's "Zaman Tüneli" section only
// (project owner-supplied dates/titles/images, 2026-08-19). Deliberately
// not merged into CompanyHistoryData — that list stays shared with About
// Us's "Our Journey" section unchanged; this one is Home-only.
//
// 1973-1992 entries added 2026-09-07 at the project owner's explicit
// request to extend the timeline's early history. Real photos for these
// projects were supplied 2026-09-11 and matched to entries by filename
// (wwwroot/images/timeline/{year}-{title}.jpeg); Summary/Detail were
// replaced with period-appropriate Turkish copy at the same time.
public static class HomeTimelineData
{
    public static readonly IReadOnlyList<HomeTimelineEntryModel> Entries = new List<HomeTimelineEntryModel>
    {
        new()
        {
            Year = "1973",
            Title = "Seda Apartmanı",
            ImageSrc = "/images/timeline/1973-Seda-Apartmanı.jpeg",
            ImageAlt = "Seda Apartmanı dış cephe görünümü",
            Summary = "1973 yılında tamamlanan Seda Apartmanı, Ancın İnşaat'ın ilk yıllarından kalma projeler arasında yer alır.",
            Detail = "Şirketin kuruluş döneminde hayata geçirilen Seda Apartmanı, tek yapı ölçeğindeki konut anlayışını yansıtan bir örnektir. Proje, Ancın İnşaat'ın uzun soluklu inşaat geçmişinin ilk taşlarından birini oluşturur."
        },
        new()
        {
            Year = "1975",
            Title = "Çam Apartmanı",
            ImageSrc = "/images/timeline/1975-Çam-Apartmanı.jpeg",
            ImageAlt = "Çam Apartmanı dış cephe görünümü",
            Summary = "1975 yılında tamamlanan Çam Apartmanı, şirketin erken dönem konut projeleri arasında yer alır.",
            Detail = "Dönemin ihtiyaçlarına uygun sade ve işlevsel bir yaklaşımla inşa edilen Çam Apartmanı, Ancın İnşaat'ın ilk yıllardaki apartman ölçeğindeki yapılaşma deneyimini yansıtır. Proje, şirketin büyüyen konut portföyüne katkı sağladı."
        },
        new()
        {
            Year = "1977",
            Title = "Kardeş Apartmanı",
            ImageSrc = "/images/timeline/1977-Kardeş-Apartmanı.jpeg",
            ImageAlt = "Kardeş Apartmanı dış cephe görünümü",
            Summary = "1977 yılında tamamlanan Kardeş Apartmanı, şirketin 1970'li yıllarda sürdürdüğü konut projelerinden biridir.",
            Detail = "Kardeş Apartmanı, Ancın İnşaat'ın bu dönemde art arda hayata geçirdiği tek yapı ölçeğindeki projelerin bir parçası olarak inşa edildi. Proje, şirketin yerel ölçekteki inşaat deneyimini pekiştiren yapılardan biri oldu."
        },
        new()
        {
            Year = "1978",
            Title = "Çaltılı Cami ve Yurdu",
            ImageSrc = "/images/timeline/1978-Çaltılı-Cami-ve-Yurdu.jpeg",
            ImageAlt = "Çaltılı Cami ve Yurdu dış cephe görünümü",
            Summary = "1978 yılında tamamlanan Çaltılı Cami ve Yurdu, Ancın İnşaat'ın konut dışı yapılara da imza attığı projelerden biridir.",
            Detail = "Çaltılı Cami ve Yurdu, şirketin apartman projelerinin yanı sıra toplumsal ihtiyaçlara yönelik yapılarda da yer aldığını gösteren örneklerden biridir. Proje, Ancın İnşaat'ın yerel ölçekteki inşaat faaliyetlerinin çeşitliliğini yansıtır."
        },
        new()
        {
            Year = "1979",
            Title = "Çaltılı Apartmanı",
            ImageSrc = "/images/timeline/1979-Çaltılı-Apartmanı.jpeg",
            ImageAlt = "Çaltılı Apartmanı dış cephe görünümü",
            Summary = "1979 yılında tamamlanan Çaltılı Apartmanı, şirketin 1970'lerin sonundaki konut projelerinden biridir.",
            Detail = "Çaltılı Apartmanı, Ancın İnşaat'ın bu dönemde sürdürdüğü tek yapı ölçeğindeki inşaat faaliyetinin bir örneğidir. Proje, şirketin büyüyen konut portföyüne katkı sağlayan yapılardan biri oldu."
        },
        new()
        {
            Year = "1981",
            Title = "Cevher Apartmanı",
            ImageSrc = "/images/timeline/1981-Cevher-Apartmanı kopyası.jpeg",
            ImageAlt = "Cevher Apartmanı dış cephe görünümü",
            Summary = "1981 yılında tamamlanan Cevher Apartmanı, Ancın İnşaat'ın 1980'li yıllara taşınan inşaat deneyiminin bir parçasıdır.",
            Detail = "Cevher Apartmanı, şirketin önceki yıllarda edindiği apartman inşası birikimini sürdürdüğü projelerden biri olarak hayata geçirildi. Proje, Ancın İnşaat'ın istikrarlı büyüme sürecine katkı sağladı."
        },
        new()
        {
            Year = "1983",
            Title = "Toker Apartmanı",
            ImageSrc = "/images/timeline/1983-Toker-Apartmanı kopyası.jpeg",
            ImageAlt = "Toker Apartmanı dış cephe görünümü",
            Summary = "1983 yılında tamamlanan Toker Apartmanı, şirketin 1980'li yıllardaki konut projelerinden biridir.",
            Detail = "Toker Apartmanı, Ancın İnşaat'ın bu dönemde sürdürdüğü apartman ölçeğindeki yapılaşma anlayışını yansıtan projelerden biri olarak inşa edildi. Proje, şirketin yerel ölçekteki inşaat deneyimini pekiştirdi."
        },
        new()
        {
            Year = "1984",
            Title = "Ancın Apartmanı",
            ImageSrc = "/images/timeline/1984-Ancın-Apartmanı.jpeg",
            ImageAlt = "Ancın Apartmanı dış cephe görünümü",
            Summary = "1984 yılında tamamlanan Ancın Apartmanı, şirketin adını taşıyan projelerinden biri olarak öne çıkar.",
            Detail = "Ancın Apartmanı, 1980'li yıllardaki konut projeleri arasında yer alan, ismiyle de dikkat çeken bir yapıdır. Proje, Ancın İnşaat'ın sürdürdüğü apartman ölçeğindeki inşaat faaliyetinin bir örneğini oluşturur."
        },
        new()
        {
            Year = "1985",
            Title = "Testiciler Apartmanı",
            ImageSrc = "/images/timeline/1985-Testiciler-Apartmanı kopyası.jpeg",
            ImageAlt = "Testiciler Apartmanı dış cephe görünümü",
            Summary = "1985 yılında tamamlanan Testiciler Apartmanı, şirketin 1980'li yıllardaki konut projelerinden biridir.",
            Detail = "Testiciler Apartmanı, Ancın İnşaat'ın bu dönemde sürdürdüğü tek yapı ölçeğindeki inşaat faaliyetinin bir parçası olarak hayata geçirildi. Proje, şirketin büyüyen konut portföyüne katkı sağladı."
        },
        new()
        {
            Year = "1986",
            Title = "Esgin Apartmanı",
            ImageSrc = "/images/timeline/1986-Esgin-Apartmanı.jpeg",
            ImageAlt = "Esgin Apartmanı dış cephe görünümü",
            Summary = "1986 yılında tamamlanan Esgin Apartmanı, Ancın İnşaat'ın 1980'li yıllardaki konut projeleri arasında yer alır.",
            Detail = "Esgin Apartmanı, şirketin bu dönemde sürdürdüğü apartman ölçeğindeki yapılaşma deneyimini yansıtan projelerden biri olarak inşa edildi. Proje, Ancın İnşaat'ın yerel ölçekteki inşaat geçmişine katkı sağladı."
        },
        new()
        {
            Year = "1987",
            Title = "Karaoğlan Apartmanı",
            ImageSrc = "/images/timeline/1987-Karaoğlan-Apartmanı.jpeg",
            ImageAlt = "Karaoğlan Apartmanı dış cephe görünümü",
            Summary = "1987 yılında tamamlanan Karaoğlan Apartmanı, şirketin 1980'li yıllardaki konut projelerinden biridir.",
            Detail = "Karaoğlan Apartmanı, Ancın İnşaat'ın bu dönemde sürdürdüğü apartman inşası deneyiminin bir örneği olarak hayata geçirildi. Proje, şirketin istikrarlı büyüme sürecine katkı sağlayan yapılardan biri oldu."
        },
        new()
        {
            Year = "1988",
            Title = "Ökten Apartmanı",
            ImageSrc = "/images/timeline/1988-Ökten-Apartmanı.jpeg",
            ImageAlt = "Ökten Apartmanı dış cephe görünümü",
            Summary = "1988 yılında tamamlanan Ökten Apartmanı, Ancın İnşaat'ın 1980'li yılların sonundaki konut projeleri arasında yer alır.",
            Detail = "Ökten Apartmanı, şirketin bu dönemde sürdürdüğü apartman ölçeğindeki inşaat faaliyetinin bir parçası olarak tasarlandı. Proje, Ancın İnşaat'ın büyüyen konut portföyüne katkı sağladı."
        },
        new()
        {
            Year = "1990",
            Title = "Zafer Apartmanı",
            ImageSrc = "/images/timeline/1990-Zafer-Apartmanı.jpeg",
            ImageAlt = "Zafer Apartmanı dış cephe görünümü",
            Summary = "1990 yılında tamamlanan Zafer Apartmanı, şirketin 1990'lı yıllara taşınan inşaat deneyiminin ilk örneklerinden biridir.",
            Detail = "Zafer Apartmanı, Ancın İnşaat'ın önceki on yıllarda edindiği apartman inşası birikimini sürdürdüğü projelerden biri olarak hayata geçirildi. Proje, şirketin yerel ölçekteki inşaat geçmişine katkı sağladı."
        },
        new()
        {
            Year = "1991",
            Title = "Çakmakoğlu Apartmanı",
            ImageSrc = "/images/timeline/1991-Çakmakoğlu-Apartmanı.jpeg",
            ImageAlt = "Çakmakoğlu Apartmanı dış cephe görünümü",
            Summary = "1991 yılında tamamlanan Çakmakoğlu Apartmanı, şirketin 1990'lı yıllardaki konut projelerinden biridir.",
            Detail = "Çakmakoğlu Apartmanı, Ancın İnşaat'ın bu dönemde sürdürdüğü apartman ölçeğindeki yapılaşma anlayışını yansıtan projelerden biri olarak inşa edildi. Proje, şirketin büyüyen konut portföyüne katkı sağladı."
        },
        new()
        {
            Year = "1992",
            Title = "Şahan Apartmanı",
            ImageSrc = "/images/timeline/1992-Şahan-Apartmanı.jpeg",
            ImageAlt = "Şahan Apartmanı dış cephe görünümü",
            Summary = "1992 yılında tamamlanan Şahan Apartmanı, Ancın İnşaat'ın 1990'lı yıllardaki konut projeleri arasında yer alır.",
            Detail = "Şahan Apartmanı, şirketin bu dönemde sürdürdüğü tek yapı ölçeğindeki inşaat faaliyetinin bir örneği olarak hayata geçirildi. Proje, Ancın İnşaat'ın 1998'de Samanyolu Sitesi ile başlayacak çok bloklu site projelerine geçiş öncesindeki son dönem apartman projelerinden biri oldu."
        },
        new()
        {
            Year = "1998",
            Title = "Samanyolu Sitesi",
            ImageSrc = "/images/timeline/1993-samanyolu-sitesi.jpg",
            ImageAlt = "Samanyolu Sitesi dış cephe görünümü",
            Summary = "Ancın İnşaat'ın tek yapılardan çok bloklu site organizasyonuna geçişini simgeleyen Samanyolu Sitesi, 1998 yılında hayata geçirildi.",
            Detail = "1973'ten bu yana tamamlanan onlarca apartman projesinin ardından şirketin ilk büyük ölçekli site projesi olan Samanyolu Sitesi, çok bloklu yerleşim planı ve dönemin standartlarının üzerinde yapı kalitesiyle dikkat çekti. Proje, Ancın İnşaat'ın sonraki yıllarda yürüttüğü site ölçeğindeki projelere referans teşkil etti."
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
        },
        new()
        {
            Year = "2019",
            Title = "N-Latis",
            ImageSrc = "/images/projects/nlatis/gallery/exterior/originals/exterior-01.jpg",
            ImageAlt = "N-Latis dış cephe görünümü",
            Summary = "2019 yılında tamamlanan N-Latis, Ancın İnşaat'ın Kuşadası'ndaki mimari vizyonunu yansıtan çarpıcı bir projedir.",
            Detail = "Kıvrımlı çatı hattı ve cam ağırlıklı cephesiyle dikkat çeken proje, zemin katındaki sosyal kullanım alanlarıyla şehir yaşamını binaya taşıdı. N-Latis, Ancın İnşaat'ın kıyı bölgelerindeki modern konut anlayışını temsil eden projelerden biri oldu."
        },
        new()
        {
            Year = "2020",
            Title = "Magnesia Gold",
            ImageSrc = "/images/projects/magnesia-gold/gallery/exterior/originals/dis-mekan-1.jpeg",
            ImageAlt = "Magnesia Gold dış cephe görünümü",
            Summary = "2020 yılında tamamlanan Magnesia Gold, Ancın İnşaat'ın Aydın'daki en kapsamlı site projelerinden biri olarak hayata geçirildi.",
            Detail = "Çok bloklu yerleşim planı, geniş peyzaj alanları ve sosyal donatılarıyla tasarlanan proje, şirketin büyük ölçekli site organizasyonundaki deneyimini bir üst seviyeye taşıdı. Magnesia Gold, Ancın İnşaat'ın konut projelerinde sürdürdüğü kalite ve yaşam standardı anlayışının güçlü bir yansımasıdır."
        }
    };
}
