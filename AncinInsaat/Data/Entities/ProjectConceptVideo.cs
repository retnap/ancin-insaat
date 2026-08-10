namespace AncinInsaat.Data.Entities;

// One row per Concept-section video slide (Davutlar D Latis Concept
// redesign, 2026-08-09) — supersedes Project.ConceptVideoPath/
// ConceptVideoPosterPath for any project with 1+ rows here, letting the
// Concept section carousel through an arbitrary number of videos instead of
// a single hardcoded one. Projects with zero rows keep rendering exactly as
// before via Project.ConceptVideoPath (see _ProjectConcept.cshtml).
public class ProjectConceptVideo
{
    public int Id { get; set; }
    public int ProjectId { get; set; }

    public required string VideoPath { get; set; }
    public required string PosterPath { get; set; }

    // Short script-style line shown above Title (e.g. "Termal Wellness
    // Ayrıcalığı" as the eyebrow for a "Modern Mimari" slide's neighbour) —
    // matches the Folkart reference's small cursive line above the bold
    // headline.
    public required string Eyebrow { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }

    public int DisplayOrder { get; set; }

    public Project Project { get; set; } = null!;
}
