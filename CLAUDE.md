# Claude.md

## Claude Code Instructions

## Always Do First

**Invoke the `frontend-design` skill** before writing any frontend code, every session, no exceptions.

## Working Rules

Never continue to the next milestone without user approval.

Never change completed pages unless requested.

Never replace existing architecture.

Always explain your implementation plan before modifying the project.

Commit-sized changes are preferred over large rewrites.

## Purpose

This document defines how Claude Code should understand, interpret and build the Ançın İnşaat website.

All project decisions should follow this document before generating code.

---

Never store:

- API Keys
- Passwords
- JWT Secrets
- SMTP Credentials
- Connection Strings containing credentials
- OAuth Client Secrets

inside:

- appsettings.json
- appsettings.Development.json
- launchSettings.json
- source code
- Git repository

Development secrets must use ASP.NET Core User Secrets.

Production secrets must be provided through the hosting environment (IIS Environment Variables, Azure Key Vault, Railway Variables, Docker Secrets, etc.).

---

# Project Working Agreement

## Decision Making

- Never invent business decisions.
- Never invent branding decisions.
- Never invent design decisions.
- Never make assumptions when documentation is incomplete.
- Always ask for clarification before proceeding.

## Design Rules

Use the frontend-design plugin only to improve:

- layout quality
- spacing
- accessibility
- responsiveness
- consistency

Never use it to invent:

- branding
- company identity
- colour palette
- typography
- marketing content
- illustrations

## Inspiration

The Inspirationals folder exists only to communicate visual direction.

Never copy layouts.

Never recreate another company's website.

Use it only to understand the desired design language.

## Placeholder Content

If real content is unavailable:

- use realistic placeholder content
- clearly mark it as placeholder
- make replacement easy later

## Milestone Workflow

After every milestone:

- explain what was implemented
- list created files
- list modified files
- explain architectural decisions
- explain trade-offs
- identify anything requiring review

Always wait for approval.

Never continue automatically.

## Documentation

Never modify documentation automatically.

Explain proposed documentation changes first.

Wait for approval.

## Architecture

Prefer:

- maintainability
- readability
- scalability
- reusable components

Avoid unnecessary complexity.

Always optimise for future Admin Panel compatibility.

## Communication

If documentation is ambiguous:

Stop.

Explain the issue.

Ask questions.

Never guess.

---

# Primary Objective

Build a premium corporate construction website for Ançın İnşaat.

The final result should feel comparable in quality to leading real estate websites while remaining completely original.

---

# Project Goal

The first release is a client presentation (Version 1).

Priorities:

- Excellent UI
- Clean Architecture
- Responsive Design
- SEO Ready
- Production Ready

The Admin Panel will be implemented in a future version.

Do not generate unnecessary administration features.

---

# Technology Stack

Frontend

- ASP.NET Core MVC
- Razor Views
- HTML
- CSS
- JavaScript

Backend

- ASP.NET Core
- Entity Framework Core

Database

- SQLite

Deployment

- IIS

Source Control

- Git
- GitHub

---

# Project Philosophy

Prefer:

- Simplicity
- Readability
- Maintainability
- Scalability

Avoid unnecessary complexity.

Always choose the simplest solution that satisfies the requirements.

---

# Visual References

The folder:

Inspirationals/

contains visual references.

Use them only to understand:

- Layout quality
- Spacing
- Typography
- Visual hierarchy
- Animation feeling

Never reproduce any design directly.

The implementation must remain original.

---

# Documentation Priority

When multiple documents overlap, follow this order:

1. Claude.md
2. Decisions.md
3. ProjectVision
4. DesignSystem
5. PageBlueprints
6. ComponentLibrary
7. Remaining documentation

---

# Responsibilities of Each Document

ProjectVision

Project goals.

SiteMap

Navigation.

DesignSystem

Visual language.

PageBlueprints

Page hierarchy.

ComponentLibrary

Reusable UI.

Animations

Motion behaviour.

ContentStructure

Content organisation.

AssetStructure

Static assets.

DatabasePlan

Database entities.

AdminPanelPlan

Future administration.

SEO

Search optimisation.

Performance

Performance goals.

Security

Security guidelines.

DevelopmentRules

Coding standards.

Decisions

Latest architectural decisions.

---

# Coding Rules

Generate production-quality code.

Do not generate placeholder implementations unless requested.

Keep controllers small.

Use Services.

Use Dependency Injection.

Follow ASP.NET Core MVC conventions.

---

# UI Rules

The website should feel:

- Premium
- Elegant
- Minimal
- Professional

Use generous whitespace.

Avoid clutter.

Avoid unnecessary visual effects.

---

# Components

Always reuse existing components before creating new ones.

Do not duplicate functionality.

Follow:

04_ComponentLibrary.md

---

# Styling

Follow:

02_DesignSystem.md

Do not create page-specific design systems.

---

# Pages

Follow:

03_PageBlueprints.md

Do not invent additional sections unless necessary.

---

# Animations

Follow:

05_Animations.md

Animations should support usability rather than decoration.

---

# Database

Follow:

08_DatabasePlan.md

Do not introduce additional entities unless required.

---

# Version 1 Scope

Implement:

- Public Website
- Responsive Layout
- Project Pages
- Contact Form
- Career Form
- SQLite Database

Do not implement:

- Admin Panel
- Authentication
- Authorization
- CMS
- Multi-language
- Blog
- News

unless explicitly requested.

---

# Code Quality

Write code that is:

- Clean
- Modular
- Reusable
- Well structured

Avoid duplication.

Avoid over-engineering.

---

# Performance

Follow:

11_Performance.md

Optimise before adding features.

---

# Security

Follow:

12_Security.md

Use secure defaults.

Validate all user input.

---

# SEO

Follow:

10_SEO.md

Every public page should support proper metadata.

---

# Documentation

If implementation changes architecture:

Update the relevant documentation.

Do not allow implementation and documentation to diverge.

---

# Working Method

When implementing new features:

1. Read relevant documentation.
2. Reuse existing components.
3. Implement the simplest solution.
4. Keep code consistent.
5. Verify responsiveness.
6. Verify accessibility.
7. Verify performance.
8. Verify SEO.

---

# When Unsure

If documentation is incomplete:

Prefer existing project conventions.

Do not invent unnecessary architecture.

Keep the implementation simple.

---

# Final Goal

Produce a production-ready ASP.NET Core MVC website that is clean, scalable, maintainable and visually premium while remaining faithful to the project documentation.