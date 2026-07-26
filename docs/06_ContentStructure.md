# 06_ContentStructure.md

# Content Structure

## Purpose

This document defines how website content is organised and managed.

It identifies which content is static, which should be manageable through the future Admin Panel and how content should remain consistent across the website.

Content hierarchy is defined in **03_PageBlueprints.md**.

Database relationships are defined in **08_DatabasePlan.md**.

---

# Content Principles

- Keep content clear and concise.
- Avoid duplicated content.
- Use consistent terminology.
- Prefer reusable content.
- Support future localisation.
- Support future CMS integration.

---

# Static Content

The following content is expected to change rarely.

- Company Name
- Logo
- Corporate Identity
- Navigation Structure
- Footer Structure
- HR Policy
- KVKK
- Copyright

---

# Dynamic Content

The following content should be manageable through the Admin Panel.

- Hero Banners
- Company Introduction
- Featured Projects
- Project Information
- Project Status
- Project Images
- Project Gallery
- Floor Plans
- Business Partners
- CTA Content
- Contact Information
- Social Media Links
- Job Listings
- SEO Metadata

---

# Home Content

Content includes:

- Hero
- Company Overview
- Featured Projects
- Company Strengths
- CTA

Featured Projects should automatically display selected projects.

---

# About Us Content

Content includes:

- Company Story
- Mission
- Vision
- Timeline
- Areas of Expertise

---

# Our Values Content

Content includes:

- Introduction
- Core Values
- Quality Commitment

---

# Projects Content

Each project should contain:

- Name
- Slug
- Short Description
- Full Description
- Status
- Cover Image
- Gallery
- Location
- Completion Date
- Floor Plans
- Business Partners
- SEO Information

---

# Contact Content

Content includes:

- Address
- Phone
- Email
- Working Hours
- Google Maps Location
- Social Media Links

---

# Career Content

Content includes:

- Introduction
- Open Positions
- Application Information

---

# HR Policy Content

Content includes:

- Introduction
- HR Philosophy
- Employee Development
- Corporate Principles

---

# KVKK Content

Content includes:

- Privacy Policy
- Contact Information
- Downloadable Documents (Optional)

---

# Images

Each image should include:

- Alt Text
- Caption (Optional)
- Display Order
- Related Content

---

# SEO Content

Each public page should support:

- Meta Title
- Meta Description
- Open Graph Image
- Canonical URL

Each project should have its own SEO metadata.

---

# Content Relationships

One Project may have:

- Multiple Images
- Multiple Floor Plans
- Multiple Partners

One Page may contain:

- Multiple Sections

One CTA may be reused across multiple pages.

---

# Naming Rules

Use consistent naming throughout the project.

Examples:

- Project
- Partner
- Floor Plan
- Gallery
- Career
- Contact

Avoid duplicate terminology for the same concept.

---

# Future CMS Compatibility

The content model should support:

- Create
- Edit
- Delete
- Publish
- Unpublish

Content should remain independent from page layout.

The same content should be reusable across multiple pages whenever possible.

---

# Single Source of Truth

This document defines **what content exists**.

It does not define:

- Database schema
- UI components
- Page layouts
- Visual design

Those responsibilities belong to their respective documentation.