using AncinInsaat.Models;

namespace AncinInsaat.Data;

// Single shared source for every "company history" milestone card on the
// site — Home's Company History section (decorative "Zaman Tüneli") and
// About Us's Our Journey section both read this same list via
// HistoryCarouselModel, rather than each keeping its own copy. Editing a
// milestone here updates every page that renders it.
//
// Moved out of CompanyHistorySectionViewComponent (its original, Home-only
// home) for the About Us Foundation phase — see docs/14_Decisions.md.
//
// PLACEHOLDER milestones — the client has not yet supplied real company
// history. Years are kept consistent with the "53 Yıllık Tecrübe" /
// founding-year-1973 fact already supplied by the project owner (Company
// Overview), but the milestone titles/descriptions themselves are generic
// corporate placeholder text, not researched or invented company facts.
// Card images reuse the same abstract placeholder graphic Company Overview
// uses rather than inventing per-era photography. Replace all of it before
// launch.
public static class CompanyHistoryData
{
    public static readonly IReadOnlyList<HistoryEntryModel> Milestones = new List<HistoryEntryModel>
    {
        new()
        {
            Year = "1973",
            Title = "Temellerin Atılması",
            Description = "Ancın İnşaat, Denizli'de ilk projeleriyle inşaat sektöründeki yolculuğuna başladı.",
            ImageSrc = "/images/company/overview-placeholder.svg",
            ImageAlt = "",
            LinkUrl = "/projects",
            LinkLabel = "devamı"
        },
        new()
        {
            Year = "1990",
            Title = "Kurumsal Büyüme",
            Description = "Artan proje hacmiyle birlikte kurumsal yapı ve saha organizasyonu güçlendirildi.",
            ImageSrc = "/images/company/overview-placeholder.svg",
            ImageAlt = "",
            LinkUrl = "/projects",
            LinkLabel = "devamı"
        },
        new()
        {
            Year = "2005",
            Title = "Yeni Nesil Konut Anlayışı",
            Description = "Modern mimari yaklaşımlar ve daha yüksek yapı standartları projelere yansıtıldı.",
            ImageSrc = "/images/company/overview-placeholder.svg",
            ImageAlt = "",
            LinkUrl = "/projects",
            LinkLabel = "devamı"
        },
        new()
        {
            Year = "2015",
            Title = "Bölgesel Genişleme",
            Description = "Farklı bölgelerdeki yeni projelerle şirketin proje portföyü genişledi.",
            ImageSrc = "/images/company/overview-placeholder.svg",
            ImageAlt = "",
            LinkUrl = "/projects",
            LinkLabel = "devamı"
        },
        new()
        {
            Year = "2026",
            Title = "Bugün",
            Description = "Ancın İnşaat, güven ve kaliteyi önceliklendirerek yoluna kararlılıkla devam ediyor.",
            ImageSrc = "/images/company/overview-placeholder.svg",
            ImageAlt = "",
            LinkUrl = "/projects",
            LinkLabel = "devamı"
        }
    };
}
