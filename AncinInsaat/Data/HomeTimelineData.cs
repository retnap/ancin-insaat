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
// Photos for 1973-1992 replaced 2026-11-04 with a new client-supplied
// set (WebP thumbnails only, via AncinInsaat.ImagePipeline; originals
// archived outside the repo).
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
            ImageSrc = "/images/timeline/thumbnails/1973-seda-apartmani.webp",
            ImageAlt = "Seda Apartmanı dış cephe görünümü",
            Summary = "1973 yılında tamamlanan Seda Apartmanı, Ancın İnşaat'ın ilk yıllarından kalma projeler arasında yer alır.",
            Detail = "Şirketin kuruluş döneminde hayata geçirilen Seda Apartmanı, tek yapı ölçeğindeki konut anlayışını yansıtan bir örnektir. Proje, Ancın İnşaat'ın uzun soluklu inşaat geçmişinin ilk taşlarından birini oluşturur.",
            ImageAltEn = "Seda Apartmanı exterior view",
            SummaryEn = "Completed in 1973, Seda Apartmanı is one of Ancın İnşaat's earliest projects.",
            DetailEn = "Built during the company's founding years, Seda Apartmanı is an example of single-building residential construction and one of the cornerstones of Ancın İnşaat's long history in construction."
        },
        new()
        {
            Year = "1975",
            Title = "Çam Apartmanı",
            ImageSrc = "/images/timeline/thumbnails/1975-cam-apartmani.webp",
            ImageAlt = "Çam Apartmanı dış cephe görünümü",
            Summary = "1975 yılında tamamlanan Çam Apartmanı, şirketin erken dönem konut projeleri arasında yer alır.",
            Detail = "Dönemin ihtiyaçlarına uygun sade ve işlevsel bir yaklaşımla inşa edilen Çam Apartmanı, Ancın İnşaat'ın ilk yıllardaki apartman ölçeğindeki yapılaşma deneyimini yansıtır. Proje, şirketin büyüyen konut portföyüne katkı sağladı.",
            ImageAltEn = "Çam Apartmanı exterior view",
            SummaryEn = "Completed in 1975, Çam Apartmanı is one of the company's early residential projects.",
            DetailEn = "Built with a simple, functional approach suited to the needs of its time, Çam Apartmanı reflects Ancın İnşaat's early experience in apartment construction and added to the company's growing residential portfolio."
        },
        new()
        {
            Year = "1977",
            Title = "Kardeş Apartmanı",
            ImageSrc = "/images/timeline/thumbnails/1977-kardes-apartmani.webp",
            ImageAlt = "Kardeş Apartmanı dış cephe görünümü",
            Summary = "1977 yılında tamamlanan Kardeş Apartmanı, şirketin 1970'li yıllarda sürdürdüğü konut projelerinden biridir.",
            Detail = "Kardeş Apartmanı, Ancın İnşaat'ın bu dönemde art arda hayata geçirdiği tek yapı ölçeğindeki projelerin bir parçası olarak inşa edildi. Proje, şirketin yerel ölçekteki inşaat deneyimini pekiştiren yapılardan biri oldu.",
            ImageAltEn = "Kardeş Apartmanı exterior view",
            SummaryEn = "Completed in 1977, Kardeş Apartmanı is one of the residential projects the company delivered in the 1970s.",
            DetailEn = "Kardeş Apartmanı was one of a series of single-building projects Ancın İnşaat completed in quick succession during this period, helping to consolidate the company's local construction experience."
        },
        new()
        {
            Year = "1978",
            Title = "Çaltılı Cami ve Yurdu",
            ImageSrc = "/images/timeline/thumbnails/1978-caltili-cami-ve-yurdu.webp",
            ImageAlt = "Çaltılı Cami ve Yurdu dış cephe görünümü",
            Summary = "1978 yılında tamamlanan Çaltılı Cami ve Yurdu, Ancın İnşaat'ın konut dışı yapılara da imza attığı projelerden biridir.",
            Detail = "Çaltılı Cami ve Yurdu, şirketin apartman projelerinin yanı sıra toplumsal ihtiyaçlara yönelik yapılarda da yer aldığını gösteren örneklerden biridir. Proje, Ancın İnşaat'ın yerel ölçekteki inşaat faaliyetlerinin çeşitliliğini yansıtır.",
            ImageAltEn = "Çaltılı Cami ve Yurdu exterior view",
            SummaryEn = "Completed in 1978, Çaltılı Cami ve Yurdu is one of the projects that saw Ancın İnşaat extend its work beyond residential buildings.",
            DetailEn = "Çaltılı Cami ve Yurdu shows that, alongside its apartment projects, the company also delivered buildings serving community needs. The project reflects the diversity of Ancın İnşaat's local construction work."
        },
        new()
        {
            Year = "1979",
            Title = "Çaltılı Apartmanı",
            ImageSrc = "/images/timeline/thumbnails/1979-caltili-apartmani.webp",
            ImageAlt = "Çaltılı Apartmanı dış cephe görünümü",
            Summary = "1979 yılında tamamlanan Çaltılı Apartmanı, şirketin 1970'lerin sonundaki konut projelerinden biridir.",
            Detail = "Çaltılı Apartmanı, Ancın İnşaat'ın bu dönemde sürdürdüğü tek yapı ölçeğindeki inşaat faaliyetinin bir örneğidir. Proje, şirketin büyüyen konut portföyüne katkı sağlayan yapılardan biri oldu.",
            ImageAltEn = "Çaltılı Apartmanı exterior view",
            SummaryEn = "Completed in 1979, Çaltılı Apartmanı is one of the company's residential projects from the late 1970s.",
            DetailEn = "Çaltılı Apartmanı is an example of the single-building projects Ancın İnşaat delivered during this period, and one of the buildings that added to the company's growing residential portfolio."
        },
        new()
        {
            Year = "1981",
            Title = "Cevher Apartmanı",
            ImageSrc = "/images/timeline/thumbnails/1981-cevher-apartmani.webp",
            ImageAlt = "Cevher Apartmanı dış cephe görünümü",
            Summary = "1981 yılında tamamlanan Cevher Apartmanı, Ancın İnşaat'ın 1980'li yıllara taşınan inşaat deneyiminin bir parçasıdır.",
            Detail = "Cevher Apartmanı, şirketin önceki yıllarda edindiği apartman inşası birikimini sürdürdüğü projelerden biri olarak hayata geçirildi. Proje, Ancın İnşaat'ın istikrarlı büyüme sürecine katkı sağladı.",
            ImageAltEn = "Cevher Apartmanı exterior view",
            SummaryEn = "Completed in 1981, Cevher Apartmanı marks the continuation of Ancın İnşaat's construction experience into the 1980s.",
            DetailEn = "Cevher Apartmanı built on the apartment construction expertise the company had gained in previous years and contributed to Ancın İnşaat's steady growth."
        },
        new()
        {
            Year = "1983",
            Title = "Toker Apartmanı",
            ImageSrc = "/images/timeline/thumbnails/1983-toker-apartmani.webp",
            ImageAlt = "Toker Apartmanı dış cephe görünümü",
            Summary = "1983 yılında tamamlanan Toker Apartmanı, şirketin 1980'li yıllardaki konut projelerinden biridir.",
            Detail = "Toker Apartmanı, Ancın İnşaat'ın bu dönemde sürdürdüğü apartman ölçeğindeki yapılaşma anlayışını yansıtan projelerden biri olarak inşa edildi. Proje, şirketin yerel ölçekteki inşaat deneyimini pekiştirdi.",
            ImageAltEn = "Toker Apartmanı exterior view",
            SummaryEn = "Completed in 1983, Toker Apartmanı is one of the company's residential projects from the 1980s.",
            DetailEn = "Toker Apartmanı reflects the apartment-scale approach Ancın İnşaat followed during this period and further strengthened the company's local construction experience."
        },
        new()
        {
            Year = "1984",
            Title = "Ancın Apartmanı",
            ImageSrc = "/images/timeline/thumbnails/1984-ancin-apartmani.webp",
            ImageAlt = "Ancın Apartmanı dış cephe görünümü",
            Summary = "1984 yılında tamamlanan Ancın Apartmanı, şirketin adını taşıyan projelerinden biri olarak öne çıkar.",
            Detail = "Ancın Apartmanı, 1980'li yıllardaki konut projeleri arasında yer alan, ismiyle de dikkat çeken bir yapıdır. Proje, Ancın İnşaat'ın sürdürdüğü apartman ölçeğindeki inşaat faaliyetinin bir örneğini oluşturur.",
            ImageAltEn = "Ancın Apartmanı exterior view",
            SummaryEn = "Completed in 1984, Ancın Apartmanı stands out as one of the projects that bears the company's name.",
            DetailEn = "One of the residential projects of the 1980s, Ancın Apartmanı is also notable for its name. The project is an example of the apartment-scale construction work Ancın İnşaat continued to carry out."
        },
        new()
        {
            Year = "1985",
            Title = "Testiciler Apartmanı",
            ImageSrc = "/images/timeline/thumbnails/1985-testiciler-apartmani.webp",
            ImageAlt = "Testiciler Apartmanı dış cephe görünümü",
            Summary = "1985 yılında tamamlanan Testiciler Apartmanı, şirketin 1980'li yıllardaki konut projelerinden biridir.",
            Detail = "Testiciler Apartmanı, Ancın İnşaat'ın bu dönemde sürdürdüğü tek yapı ölçeğindeki inşaat faaliyetinin bir parçası olarak hayata geçirildi. Proje, şirketin büyüyen konut portföyüne katkı sağladı.",
            ImageAltEn = "Testiciler Apartmanı exterior view",
            SummaryEn = "Completed in 1985, Testiciler Apartmanı is one of the company's residential projects from the 1980s.",
            DetailEn = "Testiciler Apartmanı was delivered as part of the single-building projects Ancın İnşaat carried out during this period and added to the company's growing residential portfolio."
        },
        new()
        {
            Year = "1986",
            Title = "Esgin Apartmanı",
            ImageSrc = "/images/timeline/thumbnails/1986-esgin-apartmani.webp",
            ImageAlt = "Esgin Apartmanı dış cephe görünümü",
            Summary = "1986 yılında tamamlanan Esgin Apartmanı, Ancın İnşaat'ın 1980'li yıllardaki konut projeleri arasında yer alır.",
            Detail = "Esgin Apartmanı, şirketin bu dönemde sürdürdüğü apartman ölçeğindeki yapılaşma deneyimini yansıtan projelerden biri olarak inşa edildi. Proje, Ancın İnşaat'ın yerel ölçekteki inşaat geçmişine katkı sağladı.",
            ImageAltEn = "Esgin Apartmanı exterior view",
            SummaryEn = "Completed in 1986, Esgin Apartmanı is one of Ancın İnşaat's residential projects from the 1980s.",
            DetailEn = "Esgin Apartmanı reflects the company's apartment-scale construction experience during this period and contributed to Ancın İnşaat's local track record."
        },
        new()
        {
            Year = "1987",
            Title = "Karaoğlan Apartmanı",
            ImageSrc = "/images/timeline/thumbnails/1987-karaoglan-apartmani.webp",
            ImageAlt = "Karaoğlan Apartmanı dış cephe görünümü",
            Summary = "1987 yılında tamamlanan Karaoğlan Apartmanı, şirketin 1980'li yıllardaki konut projelerinden biridir.",
            Detail = "Karaoğlan Apartmanı, Ancın İnşaat'ın bu dönemde sürdürdüğü apartman inşası deneyiminin bir örneği olarak hayata geçirildi. Proje, şirketin istikrarlı büyüme sürecine katkı sağlayan yapılardan biri oldu.",
            ImageAltEn = "Karaoğlan Apartmanı exterior view",
            SummaryEn = "Completed in 1987, Karaoğlan Apartmanı is one of the company's residential projects from the 1980s.",
            DetailEn = "Karaoğlan Apartmanı is an example of Ancın İnşaat's apartment construction experience during this period and one of the buildings that supported the company's steady growth."
        },
        new()
        {
            Year = "1988",
            Title = "Ökten Apartmanı",
            ImageSrc = "/images/timeline/thumbnails/1988-okten-apartmani.webp",
            ImageAlt = "Ökten Apartmanı dış cephe görünümü",
            Summary = "1988 yılında tamamlanan Ökten Apartmanı, Ancın İnşaat'ın 1980'li yılların sonundaki konut projeleri arasında yer alır.",
            Detail = "Ökten Apartmanı, şirketin bu dönemde sürdürdüğü apartman ölçeğindeki inşaat faaliyetinin bir parçası olarak tasarlandı. Proje, Ancın İnşaat'ın büyüyen konut portföyüne katkı sağladı.",
            ImageAltEn = "Ökten Apartmanı exterior view",
            SummaryEn = "Completed in 1988, Ökten Apartmanı is one of Ancın İnşaat's residential projects from the late 1980s.",
            DetailEn = "Ökten Apartmanı was designed as part of the apartment-scale projects the company carried out during this period and added to Ancın İnşaat's growing residential portfolio."
        },
        new()
        {
            Year = "1990",
            Title = "Zafer Apartmanı",
            ImageSrc = "/images/timeline/thumbnails/1990-zafer-apartmani.webp",
            ImageAlt = "Zafer Apartmanı dış cephe görünümü",
            Summary = "1990 yılında tamamlanan Zafer Apartmanı, şirketin 1990'lı yıllara taşınan inşaat deneyiminin ilk örneklerinden biridir.",
            Detail = "Zafer Apartmanı, Ancın İnşaat'ın önceki on yıllarda edindiği apartman inşası birikimini sürdürdüğü projelerden biri olarak hayata geçirildi. Proje, şirketin yerel ölçekteki inşaat geçmişine katkı sağladı.",
            ImageAltEn = "Zafer Apartmanı exterior view",
            SummaryEn = "Completed in 1990, Zafer Apartmanı is one of the first projects to carry the company's construction experience into the 1990s.",
            DetailEn = "Zafer Apartmanı built on the apartment construction expertise Ancın İnşaat had gained over the previous decades and added to the company's local track record."
        },
        new()
        {
            Year = "1991",
            Title = "Çakmakoğlu Apartmanı",
            ImageSrc = "/images/timeline/thumbnails/1991-cakmakoglu-apartmani.webp",
            ImageAlt = "Çakmakoğlu Apartmanı dış cephe görünümü",
            Summary = "1991 yılında tamamlanan Çakmakoğlu Apartmanı, şirketin 1990'lı yıllardaki konut projelerinden biridir.",
            Detail = "Çakmakoğlu Apartmanı, Ancın İnşaat'ın bu dönemde sürdürdüğü apartman ölçeğindeki yapılaşma anlayışını yansıtan projelerden biri olarak inşa edildi. Proje, şirketin büyüyen konut portföyüne katkı sağladı.",
            ImageAltEn = "Çakmakoğlu Apartmanı exterior view",
            SummaryEn = "Completed in 1991, Çakmakoğlu Apartmanı is one of the company's residential projects from the 1990s.",
            DetailEn = "Çakmakoğlu Apartmanı reflects the apartment-scale approach Ancın İnşaat followed during this period and added to the company's growing residential portfolio."
        },
        new()
        {
            Year = "1992",
            Title = "Şahan Apartmanı",
            ImageSrc = "/images/timeline/thumbnails/1992-sahan-apartmani.webp",
            ImageAlt = "Şahan Apartmanı dış cephe görünümü",
            Summary = "1992 yılında tamamlanan Şahan Apartmanı, Ancın İnşaat'ın 1990'lı yıllardaki konut projeleri arasında yer alır.",
            Detail = "Şahan Apartmanı, şirketin bu dönemde sürdürdüğü tek yapı ölçeğindeki inşaat faaliyetinin bir örneği olarak hayata geçirildi. Proje, Ancın İnşaat'ın 1998'de Samanyolu Sitesi ile başlayacak çok bloklu site projelerine geçiş öncesindeki son dönem apartman projelerinden biri oldu.",
            ImageAltEn = "Şahan Apartmanı exterior view",
            SummaryEn = "Completed in 1992, Şahan Apartmanı is one of the company's residential projects from the 1990s.",
            DetailEn = "Şahan Apartmanı is an example of the single-building projects the company carried out during this period. It was one of the last apartment projects before Ancın İnşaat moved on to multi-block residential complexes, beginning with Samanyolu Sitesi in 1998."
        },
        new()
        {
            Year = "1998",
            Title = "Samanyolu Sitesi",
            ImageSrc = "/images/timeline/thumbnails/1993-samanyolu-sitesi.webp",
            ImageAlt = "Samanyolu Sitesi dış cephe görünümü",
            Summary = "Ancın İnşaat'ın tek yapılardan çok bloklu site organizasyonuna geçişini simgeleyen Samanyolu Sitesi, 1998 yılında hayata geçirildi.",
            Detail = "1973'ten bu yana tamamlanan onlarca apartman projesinin ardından şirketin ilk büyük ölçekli site projesi olan Samanyolu Sitesi, çok bloklu yerleşim planı ve dönemin standartlarının üzerinde yapı kalitesiyle dikkat çekti. Proje, Ancın İnşaat'ın sonraki yıllarda yürüttüğü site ölçeğindeki projelere referans teşkil etti.",
            ImageAltEn = "Samanyolu Sitesi exterior view",
            SummaryEn = "Completed in 1998, Samanyolu Sitesi marks Ancın İnşaat's transition from single buildings to multi-block residential complexes.",
            DetailEn = "Following dozens of apartment projects completed since 1973, Samanyolu Sitesi, the company's first large-scale residential complex, stood out for its multi-block layout and build quality above the standards of its time. It became a benchmark for the residential complexes Ancın İnşaat developed in later years."
        },
        new()
        {
            Year = "2008",
            Title = "Meral Hanım Apt.",
            ImageSrc = "/images/timeline/thumbnails/2008-meral-hanim-apt.webp",
            ImageAlt = "Meral Hanım Apt. dış cephe görünümü",
            Summary = "2008 yılında tamamlanan Meral Hanım Apt., şehir merkezine yakın konumuyla öne çıkan bir konut projesidir.",
            Detail = "Proje kapsamında daire kullanım alanları ve ortak yaşam alanları, dönemin ihtiyaçları gözetilerek planlandı. Meral Hanım Apt., Ancın İnşaat'ın orta ölçekli şehir içi projelerdeki deneyimini pekiştiren yapılardan biri oldu.",
            ImageAltEn = "Meral Hanım Apt. exterior view",
            SummaryEn = "Completed in 2008, Meral Hanım Apt. is a residential project notable for its location close to the city centre.",
            DetailEn = "Apartment layouts and shared areas were planned around the needs of the time. Meral Hanım Apt. further strengthened Ancın İnşaat's experience in mid-sized urban projects."
        },
        // Client revision (2026-10-08): Tralles Gold/Alinda Gold cards now
        // point at each project's own gallery/exterior/originals/
        // dis-mekan-gorselleri-8 and -3 (client-selected photos), served via
        // the existing gallery/exterior/thumbnails/*.webp derivatives —
        // same reuse-the-gallery-thumbnail precedent as the Ferhunde/La
        // Fiore entries below. No new .webp files were generated.
        new()
        {
            Year = "2011",
            Title = "Tralles Gold",
            ImageSrc = "/images/projects/tralles-gold/gallery/exterior/thumbnails/dis-mekan-gorselleri-8.webp",
            ImageAlt = "Tralles Gold dış cephe görünümü",
            Summary = "Kasım 2011'de tamamlanan Tralles Gold, şirketin daha kapsamlı site projelerine geçiş sürecindeki önemli adımlarından biridir.",
            Detail = "Proje, sosyal donatı alanları ve peyzaj düzenlemesiyle birlikte planlandı. Tralles Gold, Ancın İnşaat'ın büyüyen proje ölçeğine uygun yapı ve saha organizasyonu yaklaşımının geliştirildiği dönemi temsil eder.",
            ImageAltEn = "Tralles Gold exterior view",
            SummaryEn = "Completed in November 2011, Tralles Gold was a key step in the company's move towards larger residential complexes.",
            DetailEn = "Planned together with its social facilities and landscaping, Tralles Gold represents the period in which Ancın İnşaat developed its building and site management approach to match its growing project scale."
        },
        new()
        {
            Year = "2014",
            Title = "Alinda Gold",
            ImageSrc = "/images/projects/alinda-gold/gallery/exterior/thumbnails/dis-mekan-gorselleri-3.webp",
            ImageAlt = "Alinda Gold dış cephe görünümü",
            Summary = "Mayıs 2014'te tamamlanan Alinda Gold, dönemin modern mimari anlayışını yansıtan bir konut projesidir.",
            Detail = "Proje, iç mekân planlamasında işlevselliği ön planda tutan bir yaklaşımla tasarlandı. Alinda Gold, Ancın İnşaat'ın yapı standartlarını sürekli geliştirme çabasının bir yansıması olarak tamamlandı.",
            ImageAltEn = "Alinda Gold exterior view",
            SummaryEn = "Completed in May 2014, Alinda Gold is a residential project reflecting the modern architecture of its time.",
            DetailEn = "Designed with a focus on functional interior planning, Alinda Gold reflects Ancın İnşaat's continuous effort to raise its building standards."
        },
        new()
        {
            Year = "2019",
            Title = "Nlatis",
            ImageSrc = "/images/projects/nlatis/gallery/exterior/originals/exterior-01.jpg",
            ImageAlt = "Nlatis dış cephe görünümü",
            Summary = "2019 yılında tamamlanan Nlatis, Ancın İnşaat'ın İzmir Narlıdere'deki mimari vizyonunu yansıtan çarpıcı bir projedir.",
            Detail = "Kıvrımlı çatı hattı ve cam ağırlıklı cephesiyle dikkat çeken proje, zemin katındaki sosyal kullanım alanlarıyla şehir yaşamını binaya taşıdı. Nlatis, Ancın İnşaat'ın kıyı bölgelerindeki modern konut anlayışını temsil eden projelerden biri oldu.",
            ImageAltEn = "Nlatis exterior view",
            SummaryEn = "Completed in 2019, Nlatis is a striking project reflecting Ancın İnşaat's architectural vision in Narlıdere, İzmir.",
            DetailEn = "Notable for its curved roofline and largely glazed façade, the project brings city life into the building through its ground-floor social spaces. Nlatis represents Ancın İnşaat's modern approach to residential design in coastal areas."
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
            SummaryEn = "Completed in 2021, Magnesia Gold is one of Ancın İnşaat's most comprehensive residential complexes in Aydın.",
            DetailEn = "Designed with a multi-block layout, extensive landscaping and social facilities, the project took the company's experience in large-scale residential complexes to the next level. Magnesia Gold is a strong reflection of the quality and living standards Ancın İnşaat maintains across its residential projects."
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
            SummaryEn = "Completed in 2026, Ferhunde Hanım Apartmanı is a residential project in Aydın Efeler, notable for its softly curved balconies and carefully chosen façade finish.",
            DetailEn = "Large windows and spacious balconies bring plenty of natural light to every floor, while a private garden enclosed by tall hedges gives residents a safe, peaceful outdoor space. Ferhunde Hanım Apartmanı reflects Ancın İnşaat's experience in urban residential projects in Aydın Efeler."
        },
        new()
        {
            Year = "2026",
            Title = "La Fiore Karabağ 1. Etap",
            TitleEn = "La Fiore Karabağ Phase 1",
            ImageSrc = "/images/projects/la-fiore-karabag/gallery/exterior/thumbnails/3.webp",
            ImageAlt = "La Fiore Karabağ 1. Etap dış cephe görünümü",
            Summary = "2026 yılında tamamlanan La Fiore Karabağ 1. Etap, Aydın İncirliova'da gür bir çam ormanının içine yerleştirilmiş tek katlı villalarıyla sakin ve mahrem bir yaşam alanı sunuyor.",
            Detail = "Taş kaplı cepheleri ve ahşap detaylarıyla dikkat çeken villalar, peyzajlı bahçeleri ve güvenlikli giriş noktasıyla doğayla iç içe bir günlük yaşam vaat ediyor. La Fiore Karabağ 1. Etap, Ancın İnşaat'ın villa ölçeğindeki proje deneyimini yansıtan yapılardan biri oldu.",
            ImageAltEn = "La Fiore Karabağ Phase 1 exterior view",
            SummaryEn = "Completed in 2026, La Fiore Karabağ Phase 1 offers a calm, private setting with single-storey villas nestled in a lush pine forest in Aydın İncirliova.",
            DetailEn = "With stone-clad façades, timber detailing, landscaped gardens and a secure entrance, the villas offer a daily life close to nature. La Fiore Karabağ Phase 1 reflects Ancın İnşaat's experience in villa developments."
        },
        // D-Latis / Q-Latis placeholders (2026-10-14 client request,
        // revised 2026-10-14) — appended after every existing card per
        // that request; neither project has a confirmed name, date or
        // detail yet, so Year/Title are literally "?" and Summary is a
        // short "more info later" placeholder rather than any invented
        // fact. Detail is intentionally omitted (null) — with nothing
        // real to reveal, a "devamı..." toggle would only re-show the
        // exact same "Detaylar yakında." sentence a second time, so
        // _HomeTimelineCarousel.cshtml skips the toggle/hidden-detail
        // block entirely for these two cards (see
        // HomeTimelineEntryModel.Detail). Images are the client-supplied
        // renders (wwwroot/images/timeline/dlatis.png and qlatis.png,
        // matched to each project by the render itself — the D-Latis one
        // is the thermal/forest-set massing shared with Davutlar D
        // Latis's own Concept carousel, the Q-Latis one is the sea-facing
        // render matching Hacıfeyzullah Q-Latis's coastal Concept
        // carousel), converted to WebP via the same ThumbnailTool
        // pipeline as every other Timeline card (800w/82q). This is a
        // Home Timeline placeholder only — it does not change either
        // project's own detail page or future-project behaviour.
        new()
        {
            Year = "?",
            Title = "?",
            ImageSrc = "/images/timeline/thumbnails/dlatis.webp",
            ImageAlt = "Yakında duyurulacak proje görseli",
            Summary = "Detaylar yakında.",
            ImageAltEn = "Upcoming project, to be announced",
            SummaryEn = "Details coming soon."
        },
        new()
        {
            Year = "?",
            Title = "?",
            ImageSrc = "/images/timeline/thumbnails/qlatis.webp",
            ImageAlt = "Yakında duyurulacak proje görseli",
            Summary = "Detaylar yakında.",
            ImageAltEn = "Upcoming project, to be announced",
            SummaryEn = "Details coming soon."
        }
    };
}
