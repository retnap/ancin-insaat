using AncinInsaat.Models;

namespace AncinInsaat.ViewComponents;

public class KvkkPolicyViewModel
{
    public required SectionHeaderModel SectionHeader { get; init; }
    public required IReadOnlyList<string> IntroParagraphs { get; init; }
    public required IReadOnlyList<KvkkPolicySection> Sections { get; init; }

    // Optional closing note rendered after the last section (English copy
    // revision, 2026-11-03) — used only by the English version for the
    // "Turkish version shall prevail" statement; null for Turkish.
    public string? ClosingNote { get; init; }
}

// One numbered subsection of the Aydınlatma Metni (e.g. "1. Veri
// Sorumlusu") — a semantic heading followed by its own paragraphs, per the
// project owner's 2026-08-03 decision to render the document as plain
// headings/paragraphs rather than an Accordion.
public class KvkkPolicySection
{
    public required string Heading { get; init; }
    public required IReadOnlyList<string> Paragraphs { get; init; }
}
