---
Document: 01_SiteMap.md
Project: Ançın İnşaat Corporate Website
Version: 1.0
Status: Approved
Last Updated: 2026-07-26
Owner: Selman Kayalı
Primary AI: Claude Code
Language: English
---

# Site Map

## Purpose

This document defines the complete website structure, navigation hierarchy and user flows.

No new page should be created before updating this document.

---

# Website Structure

Home

├── Corporate
│   ├── About Us
│   ├── Our Values
│   └── KVKK
│
├── Projects
│   ├── All Projects
│   ├── Ongoing Projects
│   └── Completed Projects
│
├── People First
│   ├── Career
│   └── HR Policy
│
└── Contact

---

# Dynamic Pages

These pages are not part of the navigation menu but are generated dynamically.

Project Detail

/projects/{project-slug}

Examples

/projects/nysa-gold

/projects/le-jardin

/projects/tralles-gold

Each project uses the same page template while displaying different content.

---

# Navigation

Sticky Navigation Bar

Logo

↓

Home

Corporate

↓

About Us

↓

Our Values

↓

KVKK

Projects

↓

Projects Landing Page

People First

↓

Career

↓

HR Policy

Contact

↓

Contact Page

---

# Projects Structure

Projects is a single page.

Route

/projects

Inside this page users can switch between:

• All Projects

• Ongoing Projects

• Completed Projects

Filtering should happen without navigating to another page.

Project Cards

↓

Project Detail Page

---

# URL Structure

/

/about-us

/values

/kvkk

/projects

/projects/{project-slug}

/career

/hr-policy

/contact

Always use readable slugs.

Never expose database IDs in URLs.

Correct

/projects/nysa-gold

Wrong

/projects?id=15

---

# Page Definitions

## Home

Landing page introducing the company and its featured project.

---

## About Us

Company presentation.

---

## Our Values

Corporate values.

---

## KVKK

Static legal page.

---

## Projects

Project listing page.

Contains

• Tabs

• Filters

• Cards

• Search-ready architecture

---

## Project Detail

Dynamic project page.

One template serves every project.

Content changes according to the selected project.

---

## Career

Career information.

CV upload form.

---

## HR Policy

Company HR principles.

---

## Contact

Contact form

Company information

Map

Department emails

Phone numbers

---

# Shared Components

The following components are shared across the entire website.

Navbar

Footer

Breadcrumb

CTA Banner

Partners Section

Social Media Section

Floating Contact Button

Buttons

Cards

Gallery

Section Title

These components must never be duplicated.

---

# User Flows

Visitor

↓

Home

↓

Projects

↓

Project Detail

↓

Contact

↓

Lead Created

---

Visitor

↓

Home

↓

Career

↓

CV Upload

↓

Application Submitted

---

Visitor

↓

Home

↓

Corporate

↓

About Us

↓

Projects

↓

Project Detail

---

# Breadcrumb Structure

Examples

Home

↓

Corporate

↓

About Us

---

Home

↓

Projects

↓

Nysa Gold

---

Home

↓

People First

↓

Career

---

# Footer

Footer appears on every page.

Contains

Company Information

Navigation

Social Media

Contact Information

Copyright

---

# Floating Contact Button

Visible on every page.

Middle Right

Click

↓

Navigate to Contact Page

---

# Administration Ready

Future Admin Panel should manage

Projects

Project Details

Homepage Banner

Timeline

Partners

Company Information

Career Applications

Contact Messages

The remaining pages can remain static.

---

# Claude Code Rules

Never create new pages without updating this document.

Never change navigation hierarchy unless requested.

Always reuse page templates.

Maintain navigation consistency.

This document is the primary navigation reference for the entire project.