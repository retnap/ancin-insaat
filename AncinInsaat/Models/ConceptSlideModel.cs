namespace AncinInsaat.Models;

// Which underlying media a Concept-carousel slide holds — drives whether
// _ProjectConcept.cshtml renders a Play button wired to the shared video
// modal, or a plain Media Viewer trigger (Le Jardin Concept carousel
// generalization, 2026-08-09).
public enum ConceptMediaType
{
    Video,
    Image
}

// One Concept-section carousel slide — the generic, mixed-media
// successor to the old separate ConceptVideoSlideModel/ConceptImageSlideModel
// (Le Jardin Concept carousel generalization, 2026-08-09). A project's
// Concept carousel is simply whichever ProjectConceptVideo/ProjectConceptImage
// rows it has, merged and ordered by DisplayOrder across both — any mix of
// video and image slides, in any order, with no per-project code. PosterUrl
// is the frame shown in the media track for every slide (a video's poster
// image or an image slide's own photo); VideoUrl is set only for
// MediaType == Video.
public class ConceptSlideModel
{
    public required ConceptMediaType MediaType { get; init; }
    public required string PosterUrl { get; init; }
    public string? VideoUrl { get; init; }
    public required string Eyebrow { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
}
