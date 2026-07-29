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

# Current Status

Current project phase:

Version 1 Development

Current objective:

Complete the public website before implementing the Admin Panel.