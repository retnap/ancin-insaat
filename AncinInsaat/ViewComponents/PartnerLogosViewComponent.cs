using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.ViewComponents;

// PLACEHOLDER — real partner logos are not yet available (wwwroot/images/
// partners/ is still empty, see 07_AssetStructure.md). Generic numbered
// labels are used deliberately rather than invented company names, which
// would misrepresent an unconfirmed business relationship. Replace with
// real logo images before launch. Part of the shared layout (renders on
// every page below the CTA Banner), so content here must stay generic
// rather than page-specific.
public class PartnerLogosViewComponent : ViewComponent
{
    // Pre-uppercased literals rather than a CSS `text-transform: uppercase`
    // — see the Footer's "PEOPLE FIRST" fix in the Milestone 2 review:
    // under `lang="tr"` that transform case-folds a lowercase "i" to a
    // dotted "İ". None of these labels contain one, but the pattern is
    // avoided here on principle rather than by coincidence.
    private static readonly IReadOnlyList<PartnerLogoItem> PlaceholderPartners = new List<PartnerLogoItem>
    {
        new() { Label = "PARTNER 01" },
        new() { Label = "PARTNER 02" },
        new() { Label = "PARTNER 03" },
        new() { Label = "PARTNER 04" },
        new() { Label = "PARTNER 05" }
    };

    public IViewComponentResult Invoke()
    {
        var model = new PartnerLogosViewModel { Partners = PlaceholderPartners };
        return View(model);
    }
}
