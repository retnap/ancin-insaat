# 04_ComponentLibrary.md

# Component Library

## Purpose

This document defines the reusable UI components used throughout the Ançın İnşaat website.

It describes each component's purpose, available variants and where it is used.

Visual styling is documented in **02_DesignSystem.md**.

Page hierarchy is documented in **03_PageBlueprints.md**.

---

# Component Standard

Each component includes:

- Purpose
- Variants
- Used In

---

# Layout Components

## Navbar

**Purpose**

Provide global navigation across the website.

**Variants**

- Desktop
- Mobile

**Used In**

All pages

---

## Footer

**Purpose**

Display company information, quick links and social media.

**Variants**

- Default

**Used In**

All pages

---

## Hero Banner

**Purpose**

Introduce the current page with a strong visual identity.

**Variants**

- Home
- Standard

**Used In**

All major pages

---

## Section Header

**Purpose**

Display section titles and optional descriptions.

**Variants**

- Title Only
- Title + Description

**Used In**

Content sections

---

# Navigation Components

## Primary Button

**Purpose**

Navigate users to primary actions.

**Variants**

- Filled
- Outline

**Used In**

All pages

---

## Secondary Button

**Purpose**

Provide secondary actions.

**Variants**

- Outline
- Text

**Used In**

Home
Projects
Project Detail

---

## Category Tabs

**Purpose**

Filter project listings by status.

**Variants**

- All
- Ongoing
- Completed

**Used In**

Projects

---

## Filter Dropdown

**Purpose**

Filter project listings by a single attribute (Project Type, Location) via
a custom single-select listbox popup. Combines with Category Tabs and any
other active Filter Dropdown using AND logic — a project must match every
active filter to remain visible.

**Variants**

- Default (button trigger + listbox popup)

**Used In**

Projects

---

## Breadcrumb

**Purpose**

Show the current page location.

**Variants**

- Default

**Used In**

Projects
Project Detail

---

# Content Components

## Rich Text

**Purpose**

Display formatted page content.

**Variants**

- Default

**Used In**

Corporate pages

---

## Information Card

**Purpose**

Display structured information.

**Variants**

- Default
- Icon

**Used In**

Corporate pages
Contact
HR Policy

---

## Feature Card

**Purpose**

Highlight company strengths and services.

**Variants**

- Icon
- Image

**Used In**

Home
About Us
Career

---

## Value Card

**Purpose**

Present company values.

**Variants**

- Default

**Used In**

Our Values

---

## Statistics Card

**Purpose**

Highlight important numerical information. A sibling to Information Card,
not a replacement: Information Card pairs an icon with a heading/
description, Statistics Card pairs one large numeric value with a short
label. Implemented (2026-08-03) as `StatisticsCardModel` +
`Views/Shared/_StatisticsCard.cshtml`, reusing the existing `.card`
primitive and `.grid`/`.grid-cols-N` utility exactly as Information Card's
Grid layout does.

**Variants**

- Number
- Percentage

**Used In**

HR Policy (Employee Development)
About Us (planned)

---

## Timeline

**Purpose**

Display chronological milestones.

**Variants**

- Vertical

**Used In**

About Us

---

## Company History Section

**Purpose**

Present company milestones as a large decorative heading over a
two-column layout: a compact Video Showcase + History Info Panel on the
left, a History Carousel of milestone cards on the right. Not a vertical
rail timeline — every milestone (year included) is its own independent
card.

**Variants**

- Default

**Used In**

Home

---

## History Info Panel

**Purpose**

Small title + previous/next controls for the History Carousel. Reusable
outside Company History (e.g. a future About Us "Our Journey" section).

**Variants**

- Default

**Used In**

Home (Company History)

---

## History Carousel

**Purpose**

Card-based carousel of milestone entries (year, title, description,
image, link). Reusable alongside History Info Panel.

**Variants**

- Default

**Used In**

Home (Company History)

---

# Project Components

## Projects Showcase

**Purpose**

Section wrapper composing Section Header + a horizontal Project Card
slider (Embla Carousel) over every published project — not a curated
"featured" subset.

**Variants**

- Default

**Used In**

Home

---

## Project Card

**Purpose**

Present a project summary (cover image, name, status badge).

**Variants**

- Carousel (Projects Showcase slide)
- Standard

**Used In**

Home
Projects

---

## Status Badge

**Purpose**

Show project status.

**Variants**

- Ongoing
- Completed

**Used In**

Projects
Project Detail

---

## Project Information

**Purpose**

Display key project details.

**Variants**

- Default

**Used In**

Project Detail

---

## Image Gallery

**Purpose**

Display project images in a responsive grid. Each thumbnail opens the
Lightbox. When a project has no gallery photos yet, shows an empty-state
message instead of an empty or broken layout.

**Variants**

- Grid

**Used In**

Project Detail

---

## Lightbox

**Purpose**

Display images in fullscreen: Previous/Next navigation, keyboard support
(arrow keys, Escape), click-outside-to-close, swipe on touch devices, an
image counter and a focus trap. One shared dialog instance per page,
reused by both the Image Gallery and the Floor Plan Viewer.

**Variants**

- Default

**Used In**

Project Detail

---

## Floor Plan Viewer

**Purpose**

Present project floor plans as thumbnails; opens the same Lightbox as the
Image Gallery for a larger view. Section only appears when a project has
real floor plan artwork.

**Variants**

- Grid

**Used In**

Project Detail

---

## Partner Logos (Project Logo Carousel)

**Purpose**

Marquee/carousel of Ançın's own confirmed project wordmarks — a portfolio
trust strip, not third-party partner logos (component/folder name is
historical). Rendered once by the shared layout, below the CTA Banner.
Content must stay generic (no page-specific project highlighting).

Distinct from Project Detail's future "Business Partners" section (not
yet built), which will read the per-project `Partner` entity for a
project's real third-party partners (contractors, architects) — that
will need its own component name to avoid colliding with this one.

**Variants**

- Marquee (global, shared layout)

**Used In**

Every page (global layout)

---

## Download Card

**Purpose**

Allow users to download project documents.

**Variants**

- PDF

**Used In**

Project Detail

---

# Form Components

## Contact Form

**Purpose**

Allow visitors to send enquiries.

**Variants**

- Default

**Used In**

Contact

---

## Career Form

**Purpose**

Allow candidates to submit job applications.

**Variants**

- Default

**Used In**

Career

---

## File Upload

**Purpose**

Upload CV and supporting documents.

**Variants**

- Single File

**Used In**

Career

---

# CTA Components

## CTA Banner

**Purpose**

Guide visitors toward the complete project portfolio. Rendered once by
the shared layout as a single page-agnostic "Tüm Projeler" (All Projects)
banner — not a different variant per page/goal as originally planned;
copy must stay page-agnostic.

**Variants**

- Default (All Projects)

**Used In**

Every page (global layout)

---

# Media Components

## Social Media Component

**Purpose**

Display official social media links.

**Variants**

- Icon List

**Used In**

Most public pages

---

## Map Component

**Purpose**

Display company office location.

**Variants**

- Interactive Map

**Used In**

Contact

---

# Feedback Components

## Success Message

**Purpose**

Confirm successful form submission.

**Variants**

- Inline

**Used In**

Contact
Career

---

## Error Message

**Purpose**

Display validation or system errors.

**Variants**

- Inline

**Used In**

Forms

---

## Loading Indicator

**Purpose**

Inform users that data is loading.

**Variants**

- Spinner
- Skeleton

**Used In**

Projects
Forms

---

## Company Introduction

**Purpose**

Present the company's story on About Us — the same Section Header + Image
Block pieces Company Overview already uses, composed with its own
image/text ratio and column order so the page reads with its own identity
(see docs/14_Decisions.md, "About Us Foundation"). Reversible: which side
the image/text sit on is a single flag (CompanyIntroductionViewModel.TextFirst),
not a markup change.

**Variants**

- Default

**Used In**

About Us

---

## Our Journey

**Purpose**

Present company milestones on About Us — reuses History Info Panel and
History Carousel exactly as Company History (Home) does, both reading the
same shared `AncinInsaat.Data.CompanyHistoryData` source rather than each
page keeping its own copy. Unlike Company History, leads with a real
Section Header instead of an oversized decorative title and has no Video
Showcase.

**Variants**

- Default

**Used In**

About Us

---

# Shared Rules

- Components should be reusable.
- Components should remain independent from page-specific logic.
- Component names should remain consistent across the project.
- Components should support future Admin Panel integration where applicable.
- Visual appearance must follow **02_DesignSystem.md**.
- Components should prioritise accessibility and responsive behaviour.
- Business logic should remain outside UI components.
- New components should only be created when an existing component cannot be extended.