# 15_Decisions.md

# Architectural Decisions

## Purpose

This document records important architectural decisions made throughout the project.

When implementation conflicts with assumptions, this document represents the latest agreed direction.

This document should be updated whenever a significant project decision changes.

---

# Project Vision

The website represents a premium construction company.

The focus is:

- Simplicity
- Elegance
- Performance
- Maintainability

---

# Version Strategy

Version 1 is a client presentation.

Priorities:

- Premium UI
- Responsive Design
- SEO Ready
- Clean Architecture

Excluded:

- Admin Panel
- Authentication
- CMS
- Multi-language
- Blog
- News

---

# Technology Decisions

Frontend

- ASP.NET Core MVC
- Razor Views
- HTML
- CSS
- JavaScript

Backend

- ASP.NET Core

Database

- SQLite

Deployment

- IIS

Version Control

- Git
- GitHub

---

# UI Decisions

The design should feel:

- Premium
- Minimal
- Elegant
- Corporate

The implementation must remain original.

Visual inspiration comes from:

- Inspirationals/Folkart
- Inspirationals/Terzioglu

---

# Documentation Strategy

Documentation exists to guide Claude Code.

It is intentionally concise.

Avoid duplicated information.

Each topic should have a single authoritative document.

---

# Component Strategy

Prefer reusable components.

Avoid page-specific components unless necessary.

Component behaviour belongs in:

04_ComponentLibrary.md

---

# Layout Strategy

Use consistent layouts.

Follow:

02_DesignSystem.md

Avoid creating different design languages for different pages.

---

# Page Strategy

Follow:

03_PageBlueprints.md

Do not add new sections unless required.

---

# Animation Strategy

Animations should support usability.

Avoid decorative animations.

Follow:

05_Animations.md

---

# Database Strategy

The initial database should remain minimal.

Only include entities required for Version 1.

Future entities should be introduced when the Admin Panel is implemented.

---

# Admin Panel Strategy

The Admin Panel is intentionally postponed.

The public website should not depend on administration features.

---

# Performance Strategy

Optimise assets before introducing advanced optimisation techniques.

Keep the application lightweight.

---

# Security Strategy

Use secure defaults.

Validate all user input.

Avoid exposing sensitive information.

---

# SEO Strategy

SEO is part of the initial implementation.

Every public page should be SEO-ready.

---

# Future Development

Future features should extend the existing architecture.

Avoid major refactoring whenever possible.

---

# Decision Log

Append new architectural decisions below this section.

Format:

Date

Decision

Reason

Impact

---

**2026-07-28**

Decision: CTA Banner and Partner Logos (renamed in practice to "Project
Logo Carousel" — Ançın's own project wordmarks, not third-party
partners) became global shared-layout sections, rendered once in
`_Layout.cshtml` on every page, instead of a page-specific section each
page composes individually.

Reason: A single page-agnostic "Tüm Projeler" CTA and one portfolio trust
strip serve every page equally well; composing them per page would only
duplicate identical markup.

Impact: 03_PageBlueprints.md's per-page "Projects CTA" / "Career CTA"
sections are superseded by the new "Global Sections (Every Page)" note.
04_ComponentLibrary.md's CTA Banner and Partner Logos entries updated to
single global variants.

---

**2026-07-28**

Decision: Home gained a Company History section (composed of Video
Showcase + History Info Panel + History Carousel) and Projects Showcase
was scoped to show every published project via a carousel, not a curated
"Featured Projects" subset.

Reason: Owner request, following Inspirationals/Terzioglu's "ZAMAN
TÜNELİ" / "TAMAMLANAN PROJELER" sections for layout/interaction reference
only (not their copy or branding).

Impact: 03_PageBlueprints.md's Home section list updated; History Info
Panel / History Carousel are intended for reuse on a future About Us
"Our Journey" section.

---

**2026-07-29**

Decision: Per-page SEO (title, description, canonical, Open Graph) is
resolved by `ISeoService` from the existing `SeoMetadata` table, keyed by
a page identifier string (e.g. `"home"`). Canonical/Open-Graph-image
paths are stored relative in the database and resolved to absolute URLs
at request time from `HttpRequest.Scheme`/`Host`, rather than a hardcoded
domain, since no production domain has been assigned yet.

Reason: Keeps seed data portable across dev/staging/production without a
migration or config change once a domain is assigned; avoids a second,
parallel metadata mechanism alongside the existing entity.

Impact: `HomeController.Index` sets `ViewData["Seo"]`; `_Layout.cshtml`
renders meta/OG/Twitter tags only when present, falling back to the
existing `ViewData["Title"]` convention for pages not yet wired (Projects
Details, Error). Future pages integrate by seeding a `SeoMetadata` row
and calling the same service.

---

**2026-07-29**

Decision: Organization and WebSite JSON-LD (schema.org) are sourced from
the existing `SiteSettings` table (via a new `ISiteSettingsService`)
rather than a second hardcoded copy of the company's name/address/social
links alongside Footer's.

Reason: `SiteSettings` already held this exact data, seeded but unused —
introducing a third hardcoded copy (Footer already hardcodes its own,
pending the future Admin Panel) would duplicate business data further.

Impact: `SeoStructuredDataViewComponent`, invoked once from `_Layout`,
renders both schemas on every page. Does not change Footer, which still
hardcodes its own copy pending the Admin Panel milestone.

---

**2026-07-29**

Decision: The Projects Showcase fallback cover image was replaced — a
2.4MB JPEG swapped for a ~3KB self-generated WebP line-art placeholder
(4:3, matching `.project-card-media`'s aspect ratio), reusing the same
minimal-architectural-line-art style already established by
`overview-placeholder.svg` rather than inventing a new illustration
style. `ProjectsShowcaseViewComponent.ResolveCoverImage` already checked
for a real `/images/projects/{slug}/cover.webp` before falling back, so
no code change was needed beyond pointing the fallback constant at the
new file — dropping a real cover image in still overrides it
automatically.

Reason: The oversized JPEG fallback hurt page weight/performance on
every project card without a real photo (currently all but nysa-gold).

Impact: `AncinInsaat/wwwroot/images/projects/all-projects/` (the old
fallback's folder) removed; new fallback lives at
`/images/projects/project-cover-placeholder.webp`.

---

**2026-07-29**

Decision: Cormorant Garamond (the approved decorative font, referenced by
the `--font-decorative` token since Milestone 1 but never loaded) is now
self-hosted at `wwwroot/fonts/cormorant-garamond/`, weight 400 only
(latin + latin-ext subsets) — the only weight/style Company History's
decorative title actually uses.

Reason: The token existed without a matching `@font-face`, so the
decorative heading silently fell back to the browser default serif.

Impact: `site.css` gained two `@font-face` rules following the existing
Playfair Display / Inter pattern. No visual regression — this closes a
gap rather than changing approved typography.

---

**2026-07-29**

Decision: Project gained two nullable, generic fields — `CataloguePath`
(path to a downloadable catalogue file) and `Amenities` (freeform text,
one item per line) — added via EF Core migration
`AddProjectCataloguePathAndAmenities`. Both are plain nullable columns on
the existing `Project` table rather than new related entities, matching
how `ShortDescription`/`Description` are already simple text columns.

Reason: Kicks off the Projects Listing / Project Detail milestone's data
model. Kept intentionally generic — no project-specific logic — so every
project (not just Nysa Gold) can optionally carry a catalogue and an
amenities list without schema changes later.

Impact: `Project.cs`, `AppDbContextModelSnapshot.cs`, and a new migration
pair. `08_DatabasePlan.md`'s Project field list updated. Projects without
a catalogue or amenities leave both fields null; the Project Detail page
must render those sections conditionally (Catalogue / Social Facilities)
rather than showing an empty state — that page composition is a later
phase of this same milestone.

---

**2026-07-29**

Decision: Hero Banner's "Standard" variant (docs/04_ComponentLibrary.md)
is implemented as its own `HeroBannerStandardViewComponent` /
`HeroBannerStandardViewModel`, not an extra mode on the existing
`HeroBannerViewComponent` — matching the placeholder note already left in
that file. It takes `Heading` / `Subheading` / `BackgroundImageUrl`
directly from the calling view rather than resolving them from
`IProjectQueryService`, since the same component must serve both a static
page heading (Projects listing) and per-project content (Project Detail).
It renders no Project CTA and no Scroll Indicator — `03_PageBlueprints.md`
lists only "Hero Banner" for every non-Home page that uses it, unlike Home
which explicitly adds both.

Reason: Keeps the Home hero (data-driven off the latest featured project)
and the Standard hero (caller-driven, page-agnostic) as two components
with a single responsibility each, rather than one component branching on
a mode flag.

Impact: New files under `ViewComponents/` and
`Views/Shared/Components/HeroBannerStandard/`. `site.css` gained a
`.hero--standard` modifier (shorter, vertically centered) that reuses
`.hero`/`.hero-bg`'s existing gradient + zoom background treatment rather
than introducing a second background style. Not yet wired into any page —
Projects Listing and Project Detail compose it in a later phase of this
milestone.

---

**2026-07-29**

Decision: The Projects Listing page (`/projects`) reuses the existing
per-page SEO mechanism for its structured data instead of adding a second
one. `SeoStructuredDataViewComponent` (previously Organization + WebSite
only, invoked unconditionally from `_Layout`) now optionally also renders
one generic page schema — `@type` taken from a new `ViewData["SeoPageType"]`
the controller sets alongside the existing `ViewData["Seo"]`, with
name/description/url read straight from the `SeoModel` `ISeoService`
already resolved. `ProjectsController.Index` sets `SeoPageType =
"CollectionPage"`; Home leaves it unset, so its output is byte-identical to
before this change.

Reason: `03_PageBlueprints.md` calls for CollectionPage on Projects
(RealEstateListing on Project Detail, ContactPage on Contact, etc. later)
— all reduce to the same name/description/url shape `SeoModel` already
carries, so a single generic schema type needs no new per-page data
fetching and no parallel JSON-LD authoring path outside
`SeoStructuredDataViewComponent`.

Impact: `SeoStructuredDataViewComponent`/`SeoStructuredDataViewModel`
gained optional parameters/property; `_Layout.cshtml` forwards them.
Additive and backward-compatible — no existing page's rendered output
changes.

---

**2026-07-29**

Decision: Project Card's "Standard" variant (docs/04_ComponentLibrary.md)
is a new shared partial, `_ProjectCard.cshtml` + `ProjectCardModel`, used
by the Projects Grid. It is **not** wired into Home's Projects Showcase
carousel, which keeps its own inline card markup and
`ProjectShowcaseCardModel` unchanged.

Reason: The carousel belongs to the completed Home milestone, which the
project's working rules say not to modify without an explicit request —
even a zero-visual-difference refactor. The two card markups are
consequently near-duplicates of each other for now; reconciling them into
one shared partial is a fair future cleanup if requested, not assumed
here.

Impact: New `AncinInsaat/Models/ProjectCardModel.cs` and
`Views/Shared/_ProjectCard.cshtml`. `ProjectsController.Index` also
duplicates `ProjectsShowcaseViewComponent`'s `ResolveCoverImage` file-
existence check for the same reason, rather than extracting it to a
shared service.

---

**2026-07-29**

Decision: Category Tabs (All / Ongoing / Completed) filter the Projects
Grid entirely client-side — `site.js` toggles `[hidden]` on
`.projects-grid-item` elements by a `data-project-status` attribute, no
page navigation or re-fetch — matching `01_SiteMap.md`'s "Filtering should
happen without navigating to another page." Implemented as a button group
(`role="group"`, `aria-pressed`) rather than the ARIA tabs pattern, since
this filters one grid rather than switching between separate tabpanels.

Reason: Simplest solution that satisfies the requirement; avoids ARIA
tabs semantics that don't actually match what's happening (no tabpanel
content is being swapped, only shown/hidden).

Impact: New CSS Section 22 in `site.css` (`.project-tabs`, `.project-tab`,
`.projects-grid-item`, `.projects-grid-empty`) and a new IIFE appended to
`site.js`. Both the "no published projects at all" and "no projects in the
selected category" empty states share the `.projects-grid-empty` look.

---

**2026-07-31**

Decision: The Projects listing gained two Filter Dropdowns (Project Type,
Location) alongside the existing Category Tabs — a custom single-select
listbox popup (`FilterDropdownModel` + `_FilterDropdown.cshtml`), not a
native `<select>`, so open/close animation and per-option hover state match
the rest of the design system. It reuses the Navbar dropdown's
button+popup/arrow-key-roving interaction shape (`site.js`) rather than a
new pattern. All three filters (status, type, location) combine with AND
logic against `.projects-grid-item` — a project must match every active
filter to stay visible. Each dropdown's options are computed in
`ProjectsController.Index` from the distinct, non-empty values already
present in the published projects for that field (Project Type sorted by
`08_DatabasePlan.md`'s reference taxonomy, then alphabetically; Location
alphabetically) — never a static list, so an option only appears once real
data uses it.

Reason: `03_PageBlueprints.md`'s Project Categories section only specified
Category Tabs, but the underlying `Project` entity already carries
`ProjectType`/`Location` per project (docs/08_DatabasePlan.md) with no way
to filter by them; a second, page-agnostic filter control was simpler than
extending Category Tabs to a multi-dimension control it wasn't designed
for.

Impact: New `AncinInsaat/Models/FilterDropdownModel.cs`,
`Views/Shared/_FilterDropdown.cshtml`, a `.filter-dropdown*` CSS block, and
a new IIFE in `site.js`. `docs/04_ComponentLibrary.md` gained a Filter
Dropdown entry. Additive to the Projects listing only — no other page's
filtering changes.

---

**2026-07-31**

Decision: The Projects listing (`/projects`) no longer opens with a Hero
Banner (Standard or otherwise). It now composes a Breadcrumb followed by a
plain `<h1 class="projects-page-heading">Projelerimiz</h1>` before the
Project Categories section — a real, visible heading (not visually hidden)
for SEO/accessibility, not a decorative banner.

Reason: A full-height hero added scroll distance in front of the filters
and grid without adding information a page whose whole purpose is "browse
everything" actually needs; a compact heading gets visitors to the grid
faster.

Impact: `03_PageBlueprints.md`'s Projects page "Hero Banner" section is
superseded by this heading for the listing page specifically — Project
Detail's Hero Banner Standard is unaffected. `ProjectsController.Index`
still sets `ViewData["Seo"]`/`ViewData["SeoPageType"]` as before; only the
visible page-top markup changed.

---

**2026-07-31**

Decision: Project Detail (`/projects/{slug}`) is now built from a
`ProjectDetailViewModel` (name, slug, status, hero background URL,
description paragraphs, a `ProjectInformationModel`) assembled entirely in
`ProjectsController.Details`, rather than passing the `Data.Entities.Project`
straight to the view — matching how `ProjectsController.Index` already
shapes `Project` into `ProjectCardModel`. The page composes Breadcrumb
("Projeler" → project name) → Hero Banner Standard (status badge +
project name over `/images/projects/{slug}/banner.webp`, a convention-based
path not existence-checked, since a 404'd background degrades silently to
the Hero's gradient) → one Project Overview section (Section Header + a
two-column grid: description paragraphs left, the Project Information panel
right). `Description` is split into paragraphs on blank lines
(`\n\n`/`\r\n\r\n`) rather than rendered as one block, so multi-paragraph
project copy doesn't collapse into a wall of text; a project with no
`Description` yet renders no paragraphs rather than an empty `<p>`.
`ProjectInformationModel` renders Name and Status always (every project has
both) but skips the Location/Completion Date rows entirely when those
fields are null, per `03_PageBlueprints.md`'s "Only display values that
exist."

Reason: This phase ("Project Detail Foundation") intentionally builds page
structure and the two sections the blueprint lists first (Hero Banner,
Project Overview) only. Gallery, Social Facilities, Project Catalogue,
Floor Plans and Business Partners are separate, later phases of the same
milestone — reserved as HTML comment anchors (in `Details.cshtml`, one per
future section, in blueprint order) so the page never needs reshaping when
they land.

Impact: New `AncinInsaat/Models/ProjectDetailViewModel.cs` and
`ProjectInformationModel.cs`, and `Views/Shared/_ProjectInformation.cshtml`
(a plain partial, not a ViewComponent — purely presentational, driven
entirely by data the calling page already has). `docs/04_ComponentLibrary.md`'s
Project Information entry already anticipated this; no further doc change
needed there.

---

**2026-07-31**

Decision: Project Detail's Gallery and Floor Plans (Project Detail Media
phase) share one full-viewport dialog — `_MediaViewer.cshtml`, one instance
per page — instead of each section getting its own lightbox markup/script.
Every gallery/floor-plan thumbnail is a `<button>` carrying
`data-media-viewer-group` ("gallery" or "floorplans") plus its image's
src/alt; a single `site.js` module reads every thumbnail on the page once,
groups them by that key, and opens the shared dialog positioned at the
clicked item — Previous/Next wrap around rather than disabling at the
list's ends. The dialog's focus-trap / Escape-to-close / body-scroll-lock
shape reuses the existing Video Modal pattern (Section 19) rather than a
new one; swipe navigation (touch) is new. Both `GalleryImageModel` and
`FloorPlanModel` are existence-filtered in `ProjectsController.Details` via
a new shared `FileExistsInWebRoot` helper (refactored out of the existing
`ResolveCoverImage`, now its third caller) — a seeded image path that
doesn't exist on disk yet is dropped before it reaches the view rather than
rendering a broken `<img>`. The Gallery section always renders, showing its
own empty-state message when a project has zero valid photos; the Floor
Plans section instead renders conditionally — Details.cshtml skips it
entirely when a project has no valid floor plan image, since
03_PageBlueprints.md does not call for an empty state there.

Reason: One dialog/module serves any number of image groups on a page
without duplicating lightbox markup or JavaScript per section, directly
satisfying this phase's "reuse as much logic from the Gallery Lightbox as
possible" instruction. Existence-filtering (rather than trusting seed data)
follows the same defensive precedent `ResolveCoverImage` already set for
cover images.

Impact: New `AncinInsaat/Models/GalleryImageModel.cs`,
`FloorPlanModel.cs`, `Views/Shared/_ProjectGallery.cshtml`,
`_FloorPlans.cshtml`, `_MediaViewer.cshtml`; `ProjectDetailViewModel`
gained `GalleryImages`/`FloorPlans`; new CSS Section 25 in `site.css` and a
new IIFE in `site.js`. `docs/04_ComponentLibrary.md`'s Image Gallery,
Lightbox and Floor Plan Viewer entries updated to match (Grid-only Gallery,
one shared Lightbox, Grid-only Floor Plan Viewer — the previously
documented Carousel/Preview variants were never built and are dropped
rather than left inaccurate).

---

**2026-07-31**

Decision: Nysa Gold's Gallery now shows its 10 real exterior/amenity
photos (supplied in `ProjectAssets/Projects/nysa-gold/`, converted to WebP
at `wwwroot/images/projects/nysa-gold/gallery-01.webp` … `gallery-10.webp`)
instead of the 2 placeholder entries seeded previously. Its Floor Plans
remain empty (no `FloorPlan` rows seeded) — the only plan asset supplied,
`nysa-gold-plan.jpg`, is a vaziyet planı (site/amenities plan: pool,
basketball court, playground, pet park, parking) rather than a per-unit
(Type A/B) floor plan, so wiring it up under Floor Plans would mislabel it.
Every other seeded project keeps its placeholder `Images`/`FloorPlans`
entries (still pointing at files that do not exist), which correctly
render each section's existence-filtered fallback (Gallery's empty-state
message; Floor Plans section omitted).

Reason: `03_PageBlueprints.md`/this phase call for real project images
when available — Nysa Gold's are the only real photos supplied so far.
The site plan mismatch is a content question (whether/where to show it,
e.g. a future Social Facilities section), not an implementation one, so it
is left unused rather than guessed at.

Impact: `DbSeeder.SeedProjectsAsync`'s Nysa Gold entry only; no other
seeded project changed. Pending project-owner input: whether
`nysa-gold-plan.jpg` should be used for a future Social Facilities /
amenities section instead of Floor Plans.

---

**2026-08-01**

Decision: `01_SiteMap.md`'s URL Structure listed the About Us route as
`/about` while `03_PageBlueprints.md` and `10_SEO.md` both specified
`/about-us`, and the already-implemented Navbar link used `/about`. Rather
than guess, this was raised to the project owner, who chose `/about-us` as
the canonical route.

Reason: A route conflict between two approved documents is exactly the
"documentation is ambiguous — stop and ask" case CLAUDE.md's Communication
section calls for, and route choice affects the Navbar link, the
`AboutController` route attribute, the `SeoMetadata.Page`/`CanonicalUrl`
seed values and (eventually) any inbound links — expensive to reverse once
built the other way.

Impact: `01_SiteMap.md`'s URL Structure updated to `/about-us` to match;
`Navbar/Default.cshtml`'s "About Us" link updated from `/about` to
`/about-us`; `AboutController.Index` uses `[HttpGet("about-us")]` (like
`ContactController`, rather than relying on the default
`{controller}/{action}` convention, which would have resolved to
`/About`).

---

**2026-08-01**

Decision: About Us Foundation phase — built Hero Banner, Company
Introduction and Our Journey (the first three sections of
`03_PageBlueprints.md`'s "Page — About Us"), following the same phased
approach as "Project Detail Foundation": Mission & Vision and Areas of
Expertise are reserved as HTML comment anchors in `About/Index.cshtml`
rather than built now. Projects CTA / Partner Logos / Footer need no
section of their own here since they are already global (`_Layout.cshtml`,
2026-07-28 decision).

Three specific requirements shaped the implementation:

1. Hero Banner always renders a real banner image
   (`wwwroot/images/hero/about-placeholder.svg`, following
   `07_AssetStructure.md`'s `hero/about.webp` convention with a
   `-placeholder` suffix pending real photography) rather than
   `HeroBannerStandard`'s neutral gradient fallback that Contact's Hero
   uses today — the project owner explicitly did not want the
   gradient-only treatment on this page.
2. Company Introduction (new `CompanyIntroductionViewComponent`) reuses
   the exact Section Header + Image Block pieces Company Overview (Home)
   already established, but with its own identity — Image / Text
   left-to-right on desktop (mirrored from Company Overview's Text /
   Image), a 55/45 image-weighted column split instead of Company
   Overview's 35/65, and an offset accent-coloured frame behind the image
   as its one signature flourish. Column placement is driven by
   `grid-template-areas` + a single `TextFirst` flag
   (`CompanyIntroductionViewModel`), not source order, specifically so a
   future request to swap which side the image/text sit on is a one-line
   flag flip rather than a markup rewrite, per the project owner's
   explicit "must stay reversible" requirement.
3. Company history milestones moved out of
   `CompanyHistorySectionViewComponent` into a new shared
   `AncinInsaat.Data.CompanyHistoryData` static class. Home's Company
   History section and the new `OurJourneyViewComponent` (About Us "Our
   Journey") both read this one list — editing a milestone now updates
   both pages, per the project owner's explicit "must never duplicate
   history data" requirement. `OurJourneyViewComponent` reuses History
   Info Panel and History Carousel unchanged (exactly the reuse
   2026-07-28's Company History decision already flagged as intended),
   under its own `Id` ("our-journey") so `site.js` pairs it independently
   from Home's "company-history" instance; unlike Home it leads with a
   real Section Header rather than an oversized decorative title, and has
   no Video Showcase (that stays Home's own promotional teaser).

A real layout bug was caught during responsive verification: the initial
CSS applied `.company-introduction-media`/`.company-introduction-content`'s
`grid-area: media`/`grid-area: content` unconditionally, but
`grid-template-areas` only defines those names inside the
`min-width: 1024px` query. Below 1024px this made the two named-but-
undefined grid areas collapse into the same auto-placed cell, rendering
the paragraph text directly on top of the image between 768–1024px.
Fixed by moving both `grid-area` declarations inside the same media query
that defines `grid-template-areas`.

Also added: `AboutController` (SEO wired via `ISeoService.GetPageSeoAsync("about-us", ...)`
and `SeoPageType = "AboutPage"`, same pattern as `ContactController`); an
`about-us` row in `DbSeeder.SeedSeoMetadataAsync`; and
`wwwroot/images/company/introduction-placeholder.svg`, a second abstract
placeholder graphic (distinct composition from Company Overview's
`overview-placeholder.svg`) so About Us doesn't visually repeat Home's
image.

Reason: Matches this project's established phased-build and
shared-reusable-component precedents (Project Detail Foundation, Company
History's own "intended for later reuse on About Us" note) while meeting
the project owner's three explicit requirements above.

Impact: New `AncinInsaat/Controllers/AboutController.cs`,
`AncinInsaat/Data/CompanyHistoryData.cs`,
`CompanyIntroductionViewComponent`/`ViewModel`, `OurJourneyViewComponent`/
`ViewModel`, their `Views/Shared/Components/.../Default.cshtml`,
`Views/About/Index.cshtml`; `CompanyHistorySectionViewComponent` now reads
`CompanyHistoryData.Milestones` instead of its own private list; new CSS
Sections 28–29 in `site.css`; `DbSeeder.cs` gained the `about-us`
`SeoMetadata` row. `04_ComponentLibrary.md` gained Company Introduction and
Our Journey entries. Mission & Vision and Areas of Expertise remain a
later phase.

---

**2026-08-03**

Decision: HR Policy (`/hr-policy`) built per `03_PageBlueprints.md`'s "Page
— HR Policy": Breadcrumb → Hero Banner (Standard, dedicated placeholder
image, same "always has its own banner" precedent as About Us/Values) → HR
Philosophy (Section Header + Image Block + copy, `HrPhilosophyViewComponent`,
reusing Company Overview's plain `.grid.grid-cols-2` shape rather than
Company Introduction's accent-frame flourish) → Core Principles (Information
Card Grid, `CorePrinciplesViewComponent`, same precedent as
`WhyJoinAncinViewComponent`/`MissionVisionViewComponent`) → Employee
Development (Section Header + a new **Statistics Card** component + copy,
`EmployeeDevelopmentViewComponent`).

Two scope decisions were made explicit by the project owner before
implementation:

1. **Statistics Card** is a genuinely new reusable component (docs/
   04_ComponentLibrary.md already named it, but it had never been built) —
   `StatisticsCardModel` + `Views/Shared/_StatisticsCard.cshtml`. It is a
   sibling to Information Card, not a replacement: Information Card pairs an
   icon with a heading/description, Statistics Card pairs one large numeric
   value with a short label. It reuses `.card` and the existing
   `.grid/.grid-cols-N` utility exactly as Information Card's Grid layout
   does — only the inner value/label shape (site.css Section 33) is new —
   and is intentionally page-agnostic for future reuse (e.g. About Us, per
   `04_ComponentLibrary.md`'s existing "Used In" note).
2. **Career CTA is intentionally not built.** `03_PageBlueprints.md`'s HR
   Policy blueprint calls for a page-specific CTA → Career, but the global
   CTA Banner (2026-07-28 decision) is hardcoded to a single page-agnostic
   "Tüm Projeler" (→ `/projects`) banner rendered once in `_Layout.cshtml`.
   Reopening it to a second destination/variant would break that
   architecture. The project owner explicitly chose to keep the existing
   single-global-CTA architecture unchanged and rely on it as-is, the same
   way About Us and Our Values already do for their own blueprint-listed
   CTA sections. Social Media is likewise intentionally omitted — the global
   Footer already covers it, consistent with About Us/Our Values.

All HR Philosophy / Core Principles / Employee Development copy is
placeholder — realistic but not client-approved — following
`WhyJoinAncinViewComponent`'s existing "PLACEHOLDER copy" convention.
Employee Development's "53+" statistic reuses the one confirmed real fact
already established elsewhere on the site ("53 Yıllık Tecrübe" from Company
Overview); its other two figures ("150+" team members, "%90+" employee
retention) are fictional placeholders and must be replaced with verified
numbers before launch.

Reason: Matches this project's established phased-build and
shared-reusable-component precedents (Project Detail Foundation, About Us
Foundation) while meeting the project owner's explicit scope decisions
above — no new page-specific CTA architecture, no standalone Social Media
section, and exactly one new reusable UI component.

Impact: New `AncinInsaat/Controllers/HrPolicyController.cs`;
`HrPhilosophyViewComponent`/`ViewModel`, `CorePrinciplesViewComponent`/
`ViewModel`, `EmployeeDevelopmentViewComponent`/`ViewModel` and their
`Views/Shared/Components/.../Default.cshtml`; `StatisticsCardModel` and
`Views/Shared/_StatisticsCard.cshtml`; `Views/HrPolicy/Index.cshtml`; new
CSS Sections 33–34 in `site.css`; `DbSeeder.cs` gained the `hr-policy`
`SeoMetadata` row; two new placeholder SVGs
(`wwwroot/images/hero/hr-policy-placeholder.svg`,
`wwwroot/images/company/hr-philosophy-placeholder.svg`).
`04_ComponentLibrary.md`'s Statistics Card entry updated from planned to
implemented. No existing page's markup, CSS, or behaviour changed.

---

# Current Status

Current project phase:

Version 1 Development

Current objective:

Complete the public website before implementing the Admin Panel.