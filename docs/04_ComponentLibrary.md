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

Filter project listings.

**Variants**

- All
- Ongoing
- Completed

**Used In**

Projects

---

## Breadcrumb

**Purpose**

Show the current page location.

**Variants**

- Default

**Used In**

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

Highlight important numerical information.

**Variants**

- Number
- Percentage

**Used In**

HR Policy
About Us

---

## Timeline

**Purpose**

Display chronological milestones.

**Variants**

- Vertical

**Used In**

About Us

---

# Project Components

## Project Card

**Purpose**

Present a project summary.

**Variants**

- Featured
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

Display project images.

**Variants**

- Grid
- Carousel

**Used In**

Project Detail

---

## Lightbox

**Purpose**

Display images in fullscreen.

**Variants**

- Default

**Used In**

Project Detail

---

## Floor Plan Viewer

**Purpose**

Present project floor plans.

**Variants**

- Grid
- Preview

**Used In**

Project Detail

---

## Partner Logos

**Purpose**

Display project partners.

**Variants**

- Logo Grid

**Used In**

Project Detail

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

Guide visitors toward the next important action.

**Variants**

- Projects
- Contact
- Career

**Used In**

Multiple pages

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

# Shared Rules

- Components should be reusable.
- Components should remain independent from page-specific logic.
- Component names should remain consistent across the project.
- Components should support future Admin Panel integration where applicable.
- Visual appearance must follow **02_DesignSystem.md**.
- Components should prioritise accessibility and responsive behaviour.
- Business logic should remain outside UI components.
- New components should only be created when an existing component cannot be extended.