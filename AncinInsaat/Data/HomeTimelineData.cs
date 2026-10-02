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
//
// English localization (2026-10-02) — ImageAltEn/SummaryEn/DetailEn are
// natural corporate-English renderings of the Turkish copy, not literal
// translations; Title stays untranslated for every entry (each building's
// own name is a proper noun, same convention as Project.Name).
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
            Detail = "Şirketin kuruluş döneminde hayata geçirilen Seda Apartmanı, tek yapı ölçeğindeki konut anlayışını yansıtan bir örnektir. Proje, Ancın İnşaat'ın uzun soluklu inşaat geçmişinin ilk taşlarından birini oluşturur.",
            ImageAltEn = "Seda Apartmanı exterior view",
            SummaryEn = "Completed in 1973, Seda Apartmanı is one of Ançın İnşaat's earliest projects.",
            DetailEn = "Built during the company's founding years, Seda Apartmanı reflects a single-building approach to residential construction. The project represents one of the first milestones in Ançın İnşaat's long construction history."
        },
        new()
        {
            Year = "1975",
            Title = "Çam Apartmanı",
            ImageSrc = "/images/timeline/1975-Çam-Apartmanı.jpeg",
            ImageAlt = "Çam Apartmanı dış cephe görünümü",
            Summary = "1975 yılında tamamlanan Çam Apartmanı, şirketin erken dönem konut projeleri arasında yer alır.",
            Detail = "Dönemin ihtiyaçlarına uygun sade ve işlevsel bir yaklaşımla inşa edilen Çam Apartmanı, Ancın İnşaat'ın ilk yıllardaki apartman ölçeğindeki yapılaşma deneyimini yansıtır. Proje, şirketin büyüyen konut portföyüne katkı sağladı.",
            ImageAltEn = "Çam Apartmanı exterior view",
            SummaryEn = "Completed in 1975, Çam Apartmanı is among the company's early residential projects.",
            DetailEn = "Built with a simple, functional approach suited to the needs of its time, Çam Apartmanı reflects Ançın İnşaat's early experience with single-building residential construction. The project contributed to the company's growing residential portfolio."
        },
        new()
        {
            Year = "1977",
            Title = "Kardeş Apartmanı",
            ImageSrc = "/images/timeline/1977-Kardeş-Apartmanı.jpeg",
            ImageAlt = "Kardeş Apartmanı dış cephe görünümü",
            Summary = "1977 yılında tamamlanan Kardeş Apartmanı, şirketin 1970'li yıllarda sürdürdüğü konut projelerinden biridir.",
            Detail = "Kardeş Apartmanı, Ancın İnşaat'ın bu dönemde art arda hayata geçirdiği tek yapı ölçeğindeki projelerin bir parçası olarak inşa edildi. Proje, şirketin yerel ölçekteki inşaat deneyimini pekiştiren yapılardan biri oldu.",
            ImageAltEn = "Kardeş Apartmanı exterior view",
            SummaryEn = "Completed in 1977, Kardeş Apartmanı is one of the residential projects the company carried out during the 1970s.",
            DetailEn = "Kardeş Apartmanı was built as part of the series of single-building projects Ançın İnşaat completed one after another during this period. The project became one of the buildings that reinforced the company's local construction experience."
        },
        new()
        {
            Year = "1978",
            Title = "Çaltılı Cami ve Yurdu",
            ImageSrc = "/images/timeline/1978-Çaltılı-Cami-ve-Yurdu.jpeg",
            ImageAlt = "Çaltılı Cami ve Yurdu dış cephe görünümü",
            Summary = "1978 yılında tamamlanan Çaltılı Cami ve Yurdu, Ancın İnşaat'ın konut dışı yapılara da imza attığı projelerden biridir.",
            Detail = "Çaltılı Cami ve Yurdu, şirketin apartman projelerinin yanı sıra toplumsal ihtiyaçlara yönelik yapılarda da yer aldığını gösteren örneklerden biridir. Proje, Ancın İnşaat'ın yerel ölçekteki inşaat faaliyetlerinin çeşitliliğini yansıtır.",
            ImageAltEn = "Çaltılı Cami ve Yurdu exterior view",
            SummaryEn = "Completed in 1978, Çaltılı Cami ve Yurdu is one of the projects through which Ançın İnşaat also left its mark on non-residential buildings.",
            DetailEn = "Çaltılı Cami ve Yurdu is one of the examples showing that, alongside its apartment projects, the company also took part in buildings serving community needs. The project reflects the diversity of Ançın İnşaat's local construction activity."
        },
        new()
        {
            Year = "1979",
            Title = "Çaltılı Apartmanı",
            ImageSrc = "/images/timeline/1979-Çaltılı-Apartmanı.jpeg",
            ImageAlt = "Çaltılı Apartmanı dış cephe görünümü",
            Summary = "1979 yılında tamamlanan Çaltılı Apartmanı, şirketin 1970'lerin sonundaki konut projelerinden biridir.",
            Detail = "Çaltılı Apartmanı, Ancın İnşaat'ın bu dönemde sürdürdüğü tek yapı ölçeğindeki inşaat faaliyetinin bir örneğidir. Proje, şirketin büyüyen konut portföyüne katkı sağlayan yapılardan biri oldu.",
            ImageAltEn = "Çaltılı Apartmanı exterior view",
            SummaryEn = "Completed in 1979, Çaltılı Apartmanı is one of the company's residential projects from the late 1970s.",
            DetailEn = "Çaltılı Apartmanı is an example of the single-building construction activity Ançın İnşaat carried out during this period. The project was one of the buildings that contributed to the company's growing residential portfolio."
        },
        new()
        {
            Year = "1981",
            Title = "Cevher Apartmanı",
            ImageSrc = "/images/timeline/1981-Cevher-Apartmanı kopyası.jpeg",
            ImageAlt = "Cevher Apartmanı dış cephe görünümü",
            Summary = "1981 yılında tamamlanan Cevher Apartmanı, Ancın İnşaat'ın 1980'li yıllara taşınan inşaat deneyiminin bir parçasıdır.",
            Detail = "Cevher Apartmanı, şirketin önceki yıllarda edindiği apartman inşası birikimini sürdürdüğü projelerden biri olarak hayata geçirildi. Proje, Ancın İnşaat'ın istikrarlı büyüme sürecine katkı sağladı.",
            ImageAltEn = "Cevher Apartmanı exterior view",
            SummaryEn = "Completed in 1981, Cevher Apartmanı is part of Ançın İnşaat's construction experience carried into the 1980s.",
            DetailEn = "Cevher Apartmanı was brought to life as one of the projects continuing the apartment-building expertise the company had gained in previous years. The project contributed to Ançın İnşaat's steady growth."
        },
        new()
        {
            Year = "1983",
            Title = "Toker Apartmanı",
            ImageSrc = "/images/timeline/1983-Toker-Apartmanı kopyası.jpeg",
            ImageAlt = "Toker Apartmanı dış cephe görünümü",
            Summary = "1983 yılında tamamlanan Toker Apartmanı, şirketin 1980'li yıllardaki konut projelerinden biridir.",
            Detail = "Toker Apartmanı, Ancın İnşaat'ın bu dönemde sürdürdüğü apartman ölçeğindeki yapılaşma anlayışını yansıtan projelerden biri olarak inşa edildi. Proje, şirketin yerel ölçekteki inşaat deneyimini pekiştirdi.",
            ImageAltEn = "Toker Apartmanı exterior view",
            SummaryEn = "Completed in 1983, Toker Apartmanı is one of the company's residential projects from the 1980s.",
            DetailEn = "Toker Apartmanı was built as one of the projects reflecting Ançın İnşaat's apartment-scale construction approach during this period. The project reinforced the company's local construction experience."
        },
        new()
        {
            Year = "1984",
            Title = "Ancın Apartmanı",
            ImageSrc = "/images/timeline/1984-Ancın-Apartmanı.jpeg",
            ImageAlt = "Ancın Apartmanı dış cephe görünümü",
            Summary = "1984 yılında tamamlanan Ancın Apartmanı, şirketin adını taşıyan projelerinden biri olarak öne çıkar.",
            Detail = "Ancın Apartmanı, 1980'li yıllardaki konut projeleri arasında yer alan, ismiyle de dikkat çeken bir yapıdır. Proje, Ancın İnşaat'ın sürdürdüğü apartman ölçeğindeki inşaat faaliyetinin bir örneğini oluşturur.",
            ImageAltEn = "Ancın Apartmanı exterior view",
            SummaryEn = "Completed in 1984, Ancın Apartmanı stands out as one of the company's projects bearing its own name.",
            DetailEn = "Ancın Apartmanı is a building from among the residential projects of the 1980s that also drew attention for its name. The project is an example of the apartment-scale construction activity Ançın İnşaat continued to carry out."
        },
        new()
        {
            Year = "1985",
            Title = "Testiciler Apartmanı",
            ImageSrc = "/images/timeline/1985-Testiciler-Apartmanı kopyası.jpeg",
            ImageAlt = "Testiciler Apartmanı dış cephe görünümü",
            Summary = "1985 yılında tamamlanan Testiciler Apartmanı, şirketin 1980'li yıllardaki konut projelerinden biridir.",
            Detail = "Testiciler Apartmanı, Ancın İnşaat'ın bu dönemde sürdürdüğü tek yapı ölçeğindeki inşaat faaliyetinin bir parçası olarak hayata geçirildi. Proje, şirketin büyüyen konut portföyüne katkı sağladı.",
            ImageAltEn = "Testiciler Apartmanı exterior view",
            SummaryEn = "Completed in 1985, Testiciler Apartmanı is one of the company's residential projects from the 1980s.",
            DetailEn = "Testiciler Apartmanı was brought to life as part of the single-building construction activity Ançın İnşaat carried out during this period. The project contributed to the company's growing residential portfolio."
        },
        new()
        {
            Year = "1986",
            Title = "Esgin Apartmanı",
            ImageSrc = "/images/timeline/1986-Esgin-Apartmanı.jpeg",
            ImageAlt = "Esgin Apartmanı dış cephe görünümü",
            Summary = "1986 yılında tamamlanan Esgin Apartmanı, Ancın İnşaat'ın 1980'li yıllardaki konut projeleri arasında yer alır.",
            Detail = "Esgin Apartmanı, şirketin bu dönemde sürdürdüğü apartman ölçeğindeki yapılaşma deneyimini yansıtan projelerden biri olarak inşa edildi. Proje, Ancın İnşaat'ın yerel ölçekteki inşaat geçmişine katkı sağladı.",
            ImageAltEn = "Esgin Apartmanı exterior view",
            SummaryEn = "Completed in 1986, Esgin Apartmanı is among Ançın İnşaat's residential projects from the 1980s.",
            DetailEn = "Esgin Apartmanı was built as one of the projects reflecting the company's apartment-scale construction experience during this period. The project contributed to Ançın İnşaat's local construction history."
        },
        new()
        {
            Year = "1987",
            Title = "Karaoğlan Apartmanı",
            ImageSrc = "/images/timeline/1987-Karaoğlan-Apartmanı.jpeg",
            ImageAlt = "Karaoğlan Apartmanı dış cephe görünümü",
            Summary = "1987 yılında tamamlanan Karaoğlan Apartmanı, şirketin 1980'li yıllardaki konut projelerinden biridir.",
            Detail = "Karaoğlan Apartmanı, Ancın İnşaat'ın bu dönemde sürdürdüğü apartman inşası deneyiminin bir örneği olarak hayata geçirildi. Proje, şirketin istikrarlı büyüme sürecine katkı sağlayan yapılardan biri oldu.",
            ImageAltEn = "Karaoğlan Apartmanı exterior view",
            SummaryEn = "Completed in 1987, Karaoğlan Apartmanı is one of the company's residential projects from the 1980s.",
            DetailEn = "Karaoğlan Apartmanı was brought to life as an example of Ançın İnşaat's apartment-building experience during this period. The project became one of the buildings that contributed to the company's steady growth."
        },
        new()
        {
            Year = "1988",
            Title = "Ökten Apartmanı",
            ImageSrc = "/images/timeline/1988-Ökten-Apartmanı.jpeg",
            ImageAlt = "Ökten Apartmanı dış cephe görünümü",
            Summary = "1988 yılında tamamlanan Ökten Apartmanı, Ancın İnşaat'ın 1980'li yılların sonundaki konut projeleri arasında yer alır.",
            Detail = "Ökten Apartmanı, şirketin bu dönemde sürdürdüğü apartman ölçeğindeki inşaat faaliyetinin bir parçası olarak tasarlandı. Proje, Ancın İnşaat'ın büyüyen konut portföyüne katkı sağladı.",
            ImageAltEn = "Ökten Apartmanı exterior view",
            SummaryEn = "Completed in 1988, Ökten Apartmanı is among Ançın İnşaat's residential projects from the late 1980s.",
            DetailEn = "Ökten Apartmanı was designed as part of the apartment-scale construction activity the company carried out during this period. The project contributed to Ançın İnşaat's growing residential portfolio."
        },
        new()
        {
            Year = "1990",
            Title = "Zafer Apartmanı",
            ImageSrc = "/images/timeline/1990-Zafer-Apartmanı.jpeg",
            ImageAlt = "Zafer Apartmanı dış cephe görünümü",
            Summary = "1990 yılında tamamlanan Zafer Apartmanı, şirketin 1990'lı yıllara taşınan inşaat deneyiminin ilk örneklerinden biridir.",
            Detail = "Zafer Apartmanı, Ancın İnşaat'ın önceki on yıllarda edindiği apartman inşası birikimini sürdürdüğü projelerden biri olarak hayata geçirildi. Proje, şirketin yerel ölçekteki inşaat geçmişine katkı sağladı.",
            ImageAltEn = "Zafer Apartmanı exterior view",
            SummaryEn = "Completed in 1990, Zafer Apartmanı is one of the first examples of the company's construction experience carried into the 1990s.",
            DetailEn = "Zafer Apartmanı was brought to life as one of the projects continuing the apartment-building expertise Ançın İnşaat had gained over the previous decades. The project contributed to the company's local construction history."
        },
        new()
        {
            Year = "1991",
            Title = "Çakmakoğlu Apartmanı",
            ImageSrc = "/images/timeline/1991-Çakmakoğlu-Apartmanı.jpeg",
            ImageAlt = "Çakmakoğlu Apartmanı dış cephe görünümü",
            Summary = "1991 yılında tamamlanan Çakmakoğlu Apartmanı, şirketin 1990'lı yıllardaki konut projelerinden biridir.",
            Detail = "Çakmakoğlu Apartmanı, Ancın İnşaat'ın bu dönemde sürdürdüğü apartman ölçeğindeki yapılaşma anlayışını yansıtan projelerden biri olarak inşa edildi. Proje, şirketin büyüyen konut portföyüne katkı sağladı.",
            ImageAltEn = "Çakmakoğlu Apartmanı exterior view",
            SummaryEn = "Completed in 1991, Çakmakoğlu Apartmanı is one of the company's residential projects from the 1990s.",
            DetailEn = "Çakmakoğlu Apartmanı was built as one of the projects reflecting Ançın İnşaat's apartment-scale construction approach during this period. The project contributed to the company's growing residential portfolio."
        },
        new()
        {
            Year = "1992",
            Title = "Şahan Apartmanı",
            ImageSrc = "/images/timeline/1992-Şahan-Apartmanı.jpeg",
            ImageAlt = "Şahan Apartmanı dış cephe görünümü",
            Summary = "1992 yılında tamamlanan Şahan Apartmanı, Ancın İnşaat'ın 1990'lı yıllardaki konut projeleri arasında yer alır.",
            Detail = "Şahan Apartmanı, şirketin bu dönemde sürdürdüğü tek yapı ölçeğindeki inşaat faaliyetinin bir örneği olarak hayata geçirildi. Proje, Ancın İnşaat'ın 1998'de Samanyolu Sitesi ile başlayacak çok bloklu site projelerine geçiş öncesindeki son dönem apartman projelerinden biri oldu.",
            ImageAltEn = "Şahan Apartmanı exterior view",
            SummaryEn = "Completed in 1992, Şahan Apartmanı is among the company's residential projects from the 1990s.",
            DetailEn = "Şahan Apartmanı was brought to life as an example of the single-building construction activity the company carried out during this period. The project was one of the last apartment projects before Ançın İnşaat's transition to multi-block housing developments, which began with Samanyolu Sitesi in 1998."
        },
        new()
        {
            Year = "1998",
            Title = "Samanyolu Sitesi",
            ImageSrc = "/images/timeline/1993-samanyolu-sitesi.jpg",
            ImageAlt = "Samanyolu Sitesi dış cephe görünümü",
            Summary = "Ancın İnşaat'ın tek yapılardan çok bloklu site organizasyonuna geçişini simgeleyen Samanyolu Sitesi, 1998 yılında hayata geçirildi.",
            Detail = "1973'ten bu yana tamamlanan onlarca apartman projesinin ardından şirketin ilk büyük ölçekli site projesi olan Samanyolu Sitesi, çok bloklu yerleşim planı ve dönemin standartlarının üzerinde yapı kalitesiyle dikkat çekti. Proje, Ancın İnşaat'ın sonraki yıllarda yürüttüğü site ölçeğindeki projelere referans teşkil etti.",
            ImageAltEn = "Samanyolu Sitesi exterior view",
            SummaryEn = "Samanyolu Sitesi, which symbolizes Ançın İnşaat's transition from single buildings to multi-block housing developments, was completed in 1998.",
            DetailEn = "Following dozens of apartment projects completed since 1973, Samanyolu Sitesi — the company's first large-scale housing development — stood out for its multi-block layout and a build quality above the standards of its time. The project served as a reference for the development-scale projects Ançın İnşaat carried out in later years."
        },
        new()
        {
            Year = "2008",
            Title = "Meral Hanım Apt.",
            ImageSrc = "/images/timeline/2008-meral-hanim-apt.jpg",
            ImageAlt = "Meral Hanım Apt. dış cephe görünümü",
            Summary = "2008 yılında tamamlanan Meral Hanım Apt., şehir merkezine yakın konumuyla öne çıkan bir konut projesidir.",
            Detail = "Proje kapsamında daire kullanım alanları ve ortak yaşam alanları, dönemin ihtiyaçları gözetilerek planlandı. Meral Hanım Apt., Ancın İnşaat'ın orta ölçekli şehir içi projelerdeki deneyimini pekiştiren yapılardan biri oldu.",
            ImageAltEn = "Meral Hanım Apt. exterior view",
            SummaryEn = "Completed in 2008, Meral Hanım Apt. is a residential project notable for its location close to the city center.",
            DetailEn = "The project's unit layouts and shared living areas were planned with the needs of the time in mind. Meral Hanım Apt. became one of the buildings that reinforced Ançın İnşaat's experience with mid-scale projects within the city."
        },
        new()
        {
            Year = "2011",
            Title = "Tralles Gold",
            ImageSrc = "/images/timeline/2011-tralles-gold.jpg",
            ImageAlt = "Tralles Gold dış cephe görünümü",
            Summary = "Kasım 2011'de tamamlanan Tralles Gold, şirketin daha kapsamlı site projelerine geçiş sürecindeki önemli adımlarından biridir.",
            Detail = "Proje, sosyal donatı alanları ve peyzaj düzenlemesiyle birlikte planlandı. Tralles Gold, Ancın İnşaat'ın büyüyen proje ölçeğine uygun yapı ve saha organizasyonu yaklaşımının geliştirildiği dönemi temsil eder.",
            ImageAltEn = "Tralles Gold exterior view",
            SummaryEn = "Completed in November 2011, Tralles Gold was an important step in the company's transition toward larger-scale developments.",
            DetailEn = "The project was planned together with its social amenity areas and landscaping. Tralles Gold represents the period in which Ançın İnşaat developed a building and site-organization approach suited to its growing project scale."
        },
        new()
        {
            Year = "2014",
            Title = "Alinda Gold",
            ImageSrc = "/images/timeline/2014-alinda-gold.jpg",
            ImageAlt = "Alinda Gold dış cephe görünümü",
            Summary = "Mayıs 2014'te tamamlanan Alinda Gold, dönemin modern mimari anlayışını yansıtan bir konut projesidir.",
            Detail = "Proje, iç mekân planlamasında işlevselliği ön planda tutan bir yaklaşımla tasarlandı. Alinda Gold, Ancın İnşaat'ın yapı standartlarını sürekli geliştirme çabasının bir yansıması olarak tamamlandı.",
            ImageAltEn = "Alinda Gold exterior view",
            SummaryEn = "Completed in May 2014, Alinda Gold is a residential project reflecting the modern architectural approach of its time.",
            DetailEn = "The project was designed with an approach that prioritized functionality in its interior planning. Alinda Gold was completed as a reflection of Ançın İnşaat's continuous effort to raise its construction standards."
        },
        new()
        {
            Year = "2019",
            Title = "N-Latis",
            ImageSrc = "/images/projects/nlatis/gallery/exterior/originals/exterior-01.jpg",
            ImageAlt = "N-Latis dış cephe görünümü",
            Summary = "2019 yılında tamamlanan N-Latis, Ancın İnşaat'ın Kuşadası'ndaki mimari vizyonunu yansıtan çarpıcı bir projedir.",
            Detail = "Kıvrımlı çatı hattı ve cam ağırlıklı cephesiyle dikkat çeken proje, zemin katındaki sosyal kullanım alanlarıyla şehir yaşamını binaya taşıdı. N-Latis, Ancın İnşaat'ın kıyı bölgelerindeki modern konut anlayışını temsil eden projelerden biri oldu.",
            ImageAltEn = "N-Latis exterior view",
            SummaryEn = "Completed in 2019, N-Latis is a striking project reflecting Ançın İnşaat's architectural vision in Kuşadası.",
            DetailEn = "Notable for its curved roofline and glass-dominant façade, the project brought city life into the building through its ground-floor social amenity areas. N-Latis became one of the projects representing Ançın İnşaat's modern residential approach in coastal regions."
        },
        new()
        {
            Year = "2021",
            Title = "Magnesia Gold",
            ImageSrc = "/images/projects/magnesia-gold/gallery/exterior/originals/dis-mekan-1.jpeg",
            ImageAlt = "Magnesia Gold dış cephe görünümü",
            Summary = "2021 yılında tamamlanan Magnesia Gold, Ancın İnşaat'ın Aydın'daki en kapsamlı site projelerinden biri olarak hayata geçirildi.",
            Detail = "Çok bloklu yerleşim planı, geniş peyzaj alanları ve sosyal donatılarıyla tasarlanan proje, şirketin büyük ölçekli site organizasyonundaki deneyimini bir üst seviyeye taşıdı. Magnesia Gold, Ancın İnşaat'ın konut projelerinde sürdürdüğü kalite ve yaşam standardı anlayışının güçlü bir yansımasıdır.",
            ImageAltEn = "Magnesia Gold exterior view",
            SummaryEn = "Completed in 2021, Magnesia Gold was brought to life as one of Ançın İnşaat's most comprehensive housing developments in Aydın.",
            DetailEn = "Designed with a multi-block layout, extensive landscaped areas and social amenities, the project took the company's experience in large-scale development organization to the next level. Magnesia Gold is a strong reflection of the quality and standard of living Ançın İnşaat maintains across its residential projects."
        },
        // Ferhunde Hanım Apartmanı ve La Fiore Karabağ 1. Etap
        // (Q-Latis/D-Latis revision, 2026-09-28 — client-requested Zaman
        // Tüneli additions). Images reuse each project's own existing
        // gallery/exterior thumbnails (already-optimized .webp files, no
        // new assets created) rather than their raw banner originals, which
        // are multi-megabyte source photos unsuited to a timeline card
        // thumbnail — see ProjectsController's own precedent (La Fiore
        // Karabağ 1. Etap's 8.3MB Hero PNG replaced with a proper .webp
        // for the same reason). Summary/Detail copy is drawn from each
        // project's own seeded Description (DbSeeder.cs) — no new claims,
        // specs or dates introduced beyond what that record already states.
        new()
        {
            Year = "2026",
            Title = "Ferhunde Hanım Apartmanı",
            ImageSrc = "/images/projects/ferhunde-hanim-apt/gallery/exterior/thumbnails/exterior-02.webp",
            ImageAlt = "Ferhunde Hanım Apartmanı dış cephe görünümü",
            Summary = "2026 yılında tamamlanan Ferhunde Hanım Apartmanı, Aydın Efeler'de yumuşak hatlı balkonları ve özenle seçilmiş cephe dokusuyla dikkat çeken bir konut projesidir.",
            Detail = "Geniş camları ve ferah balkonlarıyla her kata bol doğal ışık taşıyan proje, yüksek çitlerle çevrili özel bahçe alanıyla sakinlerine güvenli ve huzurlu bir dış mekân yaşamı sunuyor. Ferhunde Hanım Apartmanı, Ancın İnşaat'ın Aydın Efeler'deki şehir içi konut projelerindeki deneyimini yansıtan yapılardan biri oldu.",
            ImageAltEn = "Ferhunde Hanım Apartmanı exterior view",
            SummaryEn = "Completed in 2026, Ferhunde Hanım Apartmanı is a residential project in Aydın Efeler notable for its softly curved balconies and carefully chosen façade texture.",
            DetailEn = "With large windows and spacious balconies bringing abundant natural light to every floor, the project offers its residents a safe and peaceful outdoor living experience through a private garden enclosed by tall hedges. Ferhunde Hanım Apartmanı became one of the buildings reflecting Ançın İnşaat's experience with urban residential projects in Aydın Efeler."
        },
        new()
        {
            Year = "2026",
            Title = "La Fiore Karabağ 1. Etap",
            ImageSrc = "/images/projects/la-fiore-karabag/gallery/exterior/thumbnails/3.webp",
            ImageAlt = "La Fiore Karabağ 1. Etap dış cephe görünümü",
            Summary = "2026 yılında tamamlanan La Fiore Karabağ 1. Etap, Aydın İncirliova'da gür bir çam ormanının içine yerleştirilmiş tek katlı villalarıyla sakin ve mahrem bir yaşam alanı sunuyor.",
            Detail = "Taş kaplı cepheleri ve ahşap detaylarıyla dikkat çeken villalar, peyzajlı bahçeleri ve güvenlikli giriş noktasıyla doğayla iç içe bir günlük yaşam vaat ediyor. La Fiore Karabağ 1. Etap, Ancın İnşaat'ın villa ölçeğindeki proje deneyimini yansıtan yapılardan biri oldu.",
            ImageAltEn = "La Fiore Karabağ 1. Etap exterior view",
            SummaryEn = "Completed in 2026, La Fiore Karabağ 1. Etap offers a calm, private living environment with its single-story villas set within a lush pine forest in Aydın İncirliova.",
            DetailEn = "Notable for their stone-clad façades and timber detailing, the villas promise a daily life immersed in nature, with landscaped gardens and a secured entry point. La Fiore Karabağ 1. Etap became one of the buildings reflecting Ançın İnşaat's experience with villa-scale projects."
        }
    };
}
