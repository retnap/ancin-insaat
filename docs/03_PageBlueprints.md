# 03_PageBlueprints.md

# Page Blueprints

## Purpose

This document defines the structure and purpose of every public page within the Ançın İnşaat website.

Rather than describing implementation details, it focuses on page hierarchy, user flow and the reusable components required to build each page.

Visual design, animations, responsive behaviour and component implementation are documented separately in the Design System and Component Library.

---

## Page Standard

Every page blueprint follows the same structure:

- Purpose
- Route
- Sections
- SEO
- Future Admin Compatibility

Each section defines:

- Purpose
- Components
- Navigation

---

## Global Sections (Every Page)

Some sections are not composed per-page — they render once from the shared
layout (`_Layout.cshtml`) and appear identically on every page, Home
included. Page blueprints below no longer list these individually under
their own "Projects CTA" / "Career CTA" wording; treat them as already
provided:

- **Navbar** — global navigation.
- **CTA Banner** — a single page-agnostic "Tüm Projeler" (All Projects)
  banner, not a different variant per page.
- **Project Logo Carousel** — a marquee of Ançın's own confirmed project
  wordmarks (component name `PartnerLogos` for historical reasons; these
  are not third-party partners — see 04_ComponentLibrary.md).
- **Footer** (includes the Social Media icon list).

A page only needs its own blueprint entry for a section if that section's
content is specific to that page (e.g. Hero Banner copy, Project Detail's
own Business Partners logos).

---

# Page — Home

## Purpose

The Home page is the primary entry point of the website.

It introduces Ançın İnşaat, highlights featured projects, builds trust and directs visitors toward the most important sections of the website.

---

## Route

/

---

## Sections

### Hero Banner

**Purpose**

Create a strong first impression and communicate the company's premium identity.

**Components**

- Hero Banner
- Primary Button
- Secondary Button
- Scroll Indicator

**Navigation**

Primary CTA → Projects

Secondary CTA → Contact

---

### Company Overview

**Purpose**

Provide a brief introduction to Ançın İnşaat.

**Components**

- Section Header
- Image Block
- Primary Button

**Navigation**

Read More → About Us

---

### Company History

**Purpose**

Build trust by presenting the company's history as a browsable timeline of
milestones, reinforcing the "53 years of experience" claim made in Company
Overview.

**Components**

- Video Showcase (compact variant)
- History Info Panel
- History Carousel (card-per-milestone, not a rail timeline)

Rendered as a large decorative heading (Cormorant Garamond, e.g. "Zaman
Tüneli") above a two-column layout: video + info panel on the left,
carousel on the right.

---

### Projects Showcase

**Purpose**

Present the complete portfolio (every published project, not a curated
subset) in a browsable carousel — replaces the originally planned
"Featured Projects" curated-subset approach.

**Components**

- Section Header
- Project Card (Carousel variant)
- Status Badge

**Navigation**

Project Card → Project Detail

---

### Why Choose Ançın

**Purpose**

Present the company's strengths and values.

**Components**

- Section Header
- Feature Card

---

### CTA Banner, Project Logo Carousel, Footer

Rendered globally by the shared layout — see "Global Sections (Every
Page)" above. Not composed as Home-specific sections.

---

## SEO

- WebSite Schema
- Organization Schema
- Optimised meta title
- Optimised meta description
- Open Graph metadata

---

## Future Admin Compatibility

- Hero Content
- Featured Projects
- Company Overview
- Why Choose Ançın
- CTA Content
- Social Media Links
- SEO Metadata

---

# Page — About Us

## Purpose

Introduce Ançın İnşaat, its history, mission and areas of expertise.

---

## Route

/about-us

---

## Sections

### Hero Banner

**Purpose**

Introduce the About Us page.

**Components**

- Hero Banner

---

### Company Introduction

**Purpose**

Present the company's story.

**Components**

- Section Header
- Image Block
- Rich Text

---

### Mission & Vision

**Purpose**

Present the company's long-term goals.

**Components**

- Section Header
- Mission Card
- Vision Card

---

### Our Journey

**Purpose**

Present important milestones in chronological order.

**Components**

- Section Header
- Timeline

---

### Areas of Expertise

**Purpose**

Introduce the company's core construction services.

**Components**

- Section Header
- Expertise Card

---

### Projects CTA

**Purpose**

Direct visitors to the Projects page.

**Components**

- CTA Banner
- Primary Button

**Navigation**

Projects

---

### Social Media

**Purpose**

Promote official social media accounts.

**Components**

- Social Media Component

---

### Footer

**Purpose**

Provide global navigation.

**Components**

- Footer

---

## SEO

- AboutPage Schema
- Meta Title
- Meta Description

---

## Future Admin Compatibility

- Hero
- Company Introduction
- Mission
- Vision
- Timeline
- Areas of Expertise
- CTA
- SEO Metadata

---

# Page — Our Values

## Purpose

Present the principles that define Ançın İnşaat's corporate culture and approach to construction.

---

## Route

/values

---

## Sections

### Hero Banner

**Purpose**

Introduce the page.

**Components**

- Hero Banner

---

### Introduction

**Purpose**

Briefly explain the company's philosophy.

**Components**

- Section Header
- Rich Text

---

### Core Values

**Purpose**

Present the company's fundamental values.

**Components**

- Section Header
- Value Card

---

### Quality Commitment

**Purpose**

Explain how the company's values are reflected in every project.

**Components**

- Section Header
- Rich Text

---

### Projects CTA

**Purpose**

Encourage visitors to explore completed projects.

**Components**

- CTA Banner
- Primary Button

**Navigation**

Projects

---

### Social Media

**Purpose**

Promote official channels.

**Components**

- Social Media Component

---

### Footer

**Purpose**

Provide global navigation.

**Components**

- Footer

---

## SEO

- AboutPage Schema
- Meta Title
- Meta Description

---

## Future Admin Compatibility

- Hero
- Introduction
- Core Values
- Quality Commitment
- CTA
- SEO Metadata

---

# Page — Projects

## Purpose

Present the complete portfolio of Ançın İnşaat and allow visitors to browse projects by status.

---

## Route

/projects

---

## Sections

### Hero Banner

**Purpose**

Introduce the Projects page.

**Components**

- Hero Banner

---

### Project Categories

**Purpose**

Allow visitors to filter projects.

**Components**

- Section Header
- Category Tabs

**Navigation**

All Projects

Ongoing Projects

Completed Projects

---

### Projects Grid

**Purpose**

Display all available projects.

**Components**

- Project Card
- Status Badge

**Navigation**

Project Card → Project Detail

---

### Projects CTA

**Purpose**

Encourage visitors to contact the company for future projects.

**Components**

- CTA Banner
- Primary Button

**Navigation**

Contact

---

### Social Media

**Purpose**

Promote official social media channels.

**Components**

- Social Media Component

---

### Footer

**Purpose**

Provide global navigation.

**Components**

- Footer

---

## SEO

- CollectionPage Schema
- Meta Title
- Meta Description

---

## Future Admin Compatibility

- Hero
- Categories
- Projects
- CTA
- SEO Metadata

---

# Page — Project Detail

## Purpose

Provide detailed information about an individual project.

---

## Route

/projects/{project-slug}

---

## Sections

### Hero Banner

**Purpose**

Introduce the selected project.

**Components**

- Hero Banner

---

### Project Overview

**Purpose**

Present the project's concept and key information.

**Components**

- Section Header
- Project Information
- Rich Text

---

### Gallery

**Purpose**

Showcase project images.

**Components**

- Image Gallery
- Lightbox

---

### Social Facilities

**Purpose**

Present amenities and shared spaces.

**Components**

- Section Header
- Information Card

---

### Project Catalogue

**Purpose**

Allow visitors to download the project catalogue.

**Components**

- Download Card
- Primary Button

---

### Floor Plans

**Purpose**

Display available floor plans.

**Components**

- Floor Plan Viewer

---

### Business Partners

**Purpose**

Present companies involved in the project.

**Components**

- Partner Logos

---

### Projects CTA

**Purpose**

Encourage visitors to explore other projects.

**Components**

- CTA Banner
- Primary Button

**Navigation**

Projects

---

### Social Media

**Purpose**

Promote official social media channels.

**Components**

- Social Media Component

---

### Footer

**Purpose**

Provide global navigation.

**Components**

- Footer

---

## SEO

- RealEstateListing Schema
- Meta Title
- Meta Description

---

## Future Admin Compatibility

- Hero
- Project Information
- Gallery
- Facilities
- Catalogue
- Floor Plans
- Partners
- CTA
- SEO Metadata

---

# Page — Contact

## Purpose

Allow visitors to contact Ançın İnşaat through multiple communication channels.

---

## Route

/contact

---

## Sections

### Hero Banner

**Purpose**

Introduce the Contact page.

**Components**

- Hero Banner

---

### Contact Information

**Purpose**

Present company contact details.

**Components**

- Information Card

---

### Contact Form

**Purpose**

Allow visitors to send enquiries.

**Components**

- Contact Form
- Primary Button

---

### Office Location

**Purpose**

Display office location.

**Components**

- Map Component

---

### Social Media

**Purpose**

Promote official social media channels.

**Components**

- Social Media Component

---

### Footer

**Purpose**

Provide global navigation.

**Components**

- Footer

---

## SEO

- ContactPage Schema
- Meta Title
- Meta Description

---

## Future Admin Compatibility

- Hero
- Contact Information
- Contact Form
- Map
- Social Media
- SEO Metadata

---

# Page — Career

## Purpose

Encourage talented professionals to join Ançın İnşaat and submit job applications.

---

## Route

/career

---

## Sections

### Hero Banner

**Purpose**

Introduce the Career page.

**Components**

- Hero Banner

---

### Why Join Ançın

**Purpose**

Present the company's working culture.

**Components**

- Section Header
- Feature Card

---

### Open Positions

**Purpose**

Display current job opportunities.

**Components**

- Job Card

**Navigation**

Job Detail (Optional)

---

### Application Form

**Purpose**

Allow visitors to submit applications.

**Components**

- Career Form
- File Upload
- Primary Button

---

### Social Media

**Purpose**

Promote official social media channels.

**Components**

- Social Media Component

---

### Footer

**Purpose**

Provide global navigation.

**Components**

- Footer

---

## SEO

- JobPosting Schema (when applicable)
- Meta Title
- Meta Description

---

## Future Admin Compatibility

- Hero
- Why Join
- Job Listings
- Application Form
- SEO Metadata

---

# Page — HR Policy

## Purpose

Present Ançın İnşaat's approach to employee development, workplace culture and human resources.

---

## Route

/hr-policy

---

## Sections

### Hero Banner

**Purpose**

Introduce the HR Policy page.

**Components**

- Hero Banner

---

### HR Philosophy

**Purpose**

Explain the company's approach to people and organisational culture.

**Components**

- Section Header
- Image Block
- Rich Text

---

### Core Principles

**Purpose**

Present the fundamental principles of the HR policy.

**Components**

- Section Header
- Information Card

---

### Employee Development

**Purpose**

Describe career growth, learning and professional development opportunities.

**Components**

- Section Header
- Statistics Card
- Rich Text

---

### Career CTA

**Purpose**

Encourage visitors to explore career opportunities.

**Components**

- CTA Banner
- Primary Button

**Navigation**

Career

---

### Social Media

**Purpose**

Promote official social media channels.

**Components**

- Social Media Component

---

### Footer

**Purpose**

Provide global navigation.

**Components**

- Footer

---

## SEO

- AboutPage Schema
- Meta Title
- Meta Description

---

## Future Admin Compatibility

- Hero
- HR Philosophy
- Core Principles
- Employee Development
- CTA
- SEO Metadata

---

# Page — KVKK

## Purpose

Provide information regarding the processing and protection of personal data in accordance with Turkish Personal Data Protection Law (KVKK).

---

## Route

/kvkk

---

## Sections

### Hero Banner

**Purpose**

Introduce the KVKK page.

**Components**

- Hero Banner

---

### Introduction

**Purpose**

Briefly explain the purpose of the privacy policy.

**Components**

- Section Header
- Rich Text

---

### Privacy Policy

**Purpose**

Present the complete legal policy.

**Components**

- Rich Text
- Accordion (Optional)
- Download Button (Optional)

---

### Contact Information

**Purpose**

Provide communication details for privacy-related enquiries.

**Components**

- Information Card

---

### Footer

**Purpose**

Provide global navigation.

**Components**

- Footer

---

## SEO

- Meta Title
- Meta Description

---

## Future Admin Compatibility

- Hero
- Privacy Policy
- Contact Information
- Downloadable PDF
- SEO Metadata

---

# Global SEO Rules

Every public page should include:

- Unique page title
- Unique meta description
- Canonical URL
- Open Graph metadata
- Semantic heading hierarchy
- Optimised images
- Search-friendly URLs

Structured Data should be implemented where applicable:

- WebSite
- Organization
- AboutPage
- CollectionPage
- ContactPage
- JobPosting
- RealEstateListing

---

# Global Admin Panel Compatibility

The future Admin Panel should allow administrators to manage:

- Hero banners
- Rich text content
- Images
- Projects
- Categories
- Project galleries
- Floor plans
- Partners
- Timeline
- Company information
- Contact information
- Job listings
- Forms
- Social media links
- SEO metadata

The public website must remain fully functional even if optional content is unavailable.

---

# Single Source of Truth

This document defines the structure and user flow of every public page.

It does not define visual appearance or component behaviour.

Visual language is documented in:

- 02_DesignSystem.md

Reusable UI components are documented in:

- 04_ComponentLibrary.md

All page implementations should follow this document as the primary reference for content hierarchy and navigation.


