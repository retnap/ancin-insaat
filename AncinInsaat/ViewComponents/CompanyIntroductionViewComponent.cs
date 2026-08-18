using AncinInsaat.Models;
using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.ViewComponents;

// About Us "existing text content", now split around Mission & Vision
// (2026-08-13 Folkart-reference corrective revision — docs/14_Decisions.md).
// Renders in two calls — "intro" (before the panel) and "closing" (after
// it) — each with three paragraphs, matching the project owner's explicit
// "multiple paragraphs before / multiple paragraphs after" requirement.
//
// PLACEHOLDER copy — the client has not yet supplied the real company
// story text, and the project owner explicitly instructed against
// inventing additional corporate copy to fill the page. Standard Lorem
// Ipsum is used here instead until real content is provided; no heading
// accompanies it per the same instruction (the "KİMİZ" / "Aydın'da Yarım
// Asırlık Bir Yolculuk" heading has been removed, not renamed).
public class CompanyIntroductionViewComponent : ViewComponent
{
    private static readonly IReadOnlyList<string> IntroParagraphs = new List<string>
    {
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor " +
            "incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud " +
            "exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
        "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu " +
            "fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in " +
            "culpa qui officia deserunt mollit anim id est laborum.",
        "Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium " +
            "doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore " +
            "veritatis et quasi architecto beatae vitae dicta sunt explicabo."
    };

    private static readonly IReadOnlyList<string> ClosingParagraphs = new List<string>
    {
        "Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed " +
            "quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. " +
            "Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet.",
        "Ut enim ad minima veniam, quis nostrum exercitationem ullam corporis suscipit " +
            "laboriosam, nisi ut aliquid ex ea commodi consequatur. Quis autem vel eum iure " +
            "reprehenderit qui in ea voluptate velit esse quam nihil molestiae consequatur.",
        "At vero eos et accusamus et iusto odio dignissimos ducimus qui blanditiis " +
            "praesentium voluptatum deleniti atque corrupti quos dolores et quas molestias " +
            "excepturi sint occaecati cupiditate non provident."
    };

    public IViewComponentResult Invoke(string part)
    {
        var model = part switch
        {
            "intro" => new CompanyIntroductionViewModel { SectionId = "about-intro", Paragraphs = IntroParagraphs },
            "closing" => new CompanyIntroductionViewModel { SectionId = "about-closing", Paragraphs = ClosingParagraphs },
            _ => throw new ArgumentOutOfRangeException(nameof(part), part, "Expected \"intro\" or \"closing\".")
        };

        return View(model);
    }
}
