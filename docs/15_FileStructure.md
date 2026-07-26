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

# Current Status

Current project phase:

Version 1 Development

Current objective:

Complete the public website before implementing the Admin Panel.