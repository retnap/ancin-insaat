namespace AncinInsaat.Models;

// Shared header for every Timeline/History carousel usage (Home's Company
// History, About Us's Our Journey) — an oversized decorative watermark
// title plus the section's real, centered heading. One design reused by
// both pages instead of each inventing its own (project owner's 2026-08-03
// Timeline header redesign request). See _TimelineHeader.cshtml and
// site.css's "Timeline Header (shared)" section for the rendering.
public class TimelineHeaderModel
{
    public required string HeadingId { get; init; }

    // Giant, low-opacity background text — purely decorative (rendered
    // aria-hidden by the partial), so this never needs to double as
    // accessible copy.
    public required string DecorativeTitle { get; init; }

    // The section's real, visible heading (the <h2> an assistive
    // technology announces via the section's aria-labelledby).
    public required string Subtitle { get; init; }
}
