namespace AncinInsaat.Data.Entities;

// One row per Concept-section image-carousel slide (La Fiore Karabağ 2.
// Etap Vaziyet Planı/Concept/Gallery phase, 2026-08-09) — the image-only
// sibling of ProjectConceptVideo, for projects whose Concept carousel has no
// video asset. Same carousel markup/CSS/JS as ProjectConceptVideo's slides;
// only the trigger swaps a video-modal Play button for a Media Viewer
// trigger (see _ProjectConcept.cshtml). A project with 1+ rows here renders
// this carousel; a project with 1+ ConceptVideos rows still takes priority
// (today, only Davutlar D Latis); every other project keeps rendering the
// original single-image/video Concept card unchanged.
public class ProjectConceptImage
{
    public int Id { get; set; }
    public int ProjectId { get; set; }

    public required string ImagePath { get; set; }

    public required string Eyebrow { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }

    public int DisplayOrder { get; set; }

    public Project Project { get; set; } = null!;
}
