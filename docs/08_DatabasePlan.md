# 08_DatabasePlan.md

# Database Plan

## Purpose

This document defines the database structure for the Ançın İnşaat website.

The database should support current website requirements while remaining flexible for future Admin Panel features.

The project uses **SQLite** with **Entity Framework Core**.

---

# Design Principles

- Keep the schema simple.
- Avoid duplicated data.
- Normalise where appropriate.
- Use foreign keys.
- Support future scalability.
- Support soft deletion where appropriate.

---

# Naming Convention

Tables use singular names.

Examples:

- Project
- ProjectImage
- Partner

Primary Key

- Id

Foreign Keys

- ProjectId
- PartnerId

---

# Entity Overview

The project consists of the following entities:

- Project
- ProjectImage
- FloorPlan
- Partner
- CareerPosition
- ContactMessage
- JobApplication
- SeoMetadata
- SiteSettings

---

# Project

## Purpose

Stores construction project information.

## Fields

- Id
- Name
- Slug
- ShortDescription
- Description
- Status
- Location
- ProjectType (nullable)
- CompletionDate
- CoverImage
- CataloguePath (nullable)
- Amenities (nullable)
- DisplayOrder
- IsFeatured
- IsPublished
- CreatedAt
- UpdatedAt

## Relationships

One Project

- Many ProjectImages
- Many FloorPlans
- Many Partners

## ProjectType

Free-text field (not an enum) so the taxonomy can grow without a migration.

Initial reference values:

- Residence
- Villa
- Commercial
- Office
- Mixed Use

The Projects listing's Project Type filter only ever shows values that are
actually in use — a project with no ProjectType assigned simply does not
appear in that filter until one is assigned.

---

# ProjectImage

## Purpose

Stores project gallery images.

## Fields

- Id
- ProjectId
- ImagePath
- AltText
- DisplayOrder

## Relationships

Many Images

→ One Project

---

# FloorPlan

## Purpose

Stores project floor plans.

## Fields

- Id
- ProjectId
- Title
- ImagePath
- DisplayOrder

## Relationships

Many FloorPlans

→ One Project

---

# Partner

## Purpose

Stores companies participating in a project.

## Fields

- Id
- ProjectId
- Name
- Logo
- Website
- DisplayOrder

## Relationships

Many Partners

→ One Project

---

# CareerPosition

## Purpose

Stores available job listings.

## Fields

- Id
- Title
- Department
- Location
- Description
- IsPublished
- CreatedAt

---

# ContactMessage

## Purpose

Stores messages submitted through the contact form.

## Fields

- Id
- FullName
- Email
- Phone
- Subject
- Message
- IsRead
- CreatedAt

---

# JobApplication

## Purpose

Stores career applications.

## Fields

- Id
- CareerPositionId
- FullName
- Email
- Phone
- CVPath
- Message
- CreatedAt

## Relationships

Many Applications

→ One CareerPosition

---

# SeoMetadata

## Purpose

Stores SEO information for public pages.

## Fields

- Id
- Page
- MetaTitle
- MetaDescription
- CanonicalUrl
- OpenGraphImage

---

# SiteSettings

## Purpose

Stores global website settings.

## Fields

- Id
- CompanyName
- Address
- Phone
- Email
- WorkingHours
- Facebook
- Instagram
- LinkedIn
- YouTube
- GoogleMaps
- Logo
- FooterText

---

# Relationships

```text
Project
│
├── ProjectImage
│
├── FloorPlan
│
└── Partner

CareerPosition
│
└── JobApplication
```

---

# Enumerations

## ProjectStatus

- Ongoing
- Completed

---

# Indexes

Create indexes for:

- Project.Slug
- Project.Status
- Project.IsFeatured
- CareerPosition.IsPublished

---

# Validation Rules

Project

- Name is required.
- Slug must be unique.

ProjectImage

- ImagePath is required.

CareerPosition

- Title is required.

ContactMessage

- Name
- Email
- Message

are required.

JobApplication

- Name
- Email
- CV

are required.

---

# Soft Delete

Future Admin Panel may support soft deletion.

Suggested field:

- IsDeleted

Do not physically delete important records unless necessary.

---

# Seed Data

Initial seed data should include:

- Company Information
- Site Settings
- Social Media Links
- Sample Projects
- Sample Partners

---

# Future Expansion

The schema should support adding:

- News
- Blog
- Team Members
- Testimonials
- Awards
- Certificates
- Multi-language Content

without major structural changes.

---

# Single Source of Truth

This document defines the database entities and relationships.

It does not define:

- Admin Panel screens
- Entity Framework implementation
- Repository pattern
- Business logic

Those responsibilities belong to their respective documentation.