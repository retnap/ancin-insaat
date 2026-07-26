---

Document: 00_ProjectVision.md
Project: Ançın İnşaat Corporate Website
Version: 1.0
Status: Approved
Last Updated: 2026-07-26
Owner: Selman Kayalı
Primary AI: Claude Code
Language: English

---

> This document is the primary vision document of the project.
> Every architectural and design decision must comply with this document.

# Project Purpose

This project is a premium corporate website developed for **Ançın İnşaat**.

The website is intended to strengthen the company's corporate identity, present its completed and ongoing projects, attract potential customers, and provide a modern digital experience that reflects the company's vision.

The website is not an e-commerce platform.

Its primary purpose is to present projects professionally and convert visitors into potential customers through contact forms.

This project will continue to evolve after Version 1. New modules such as an Admin Panel, Analytics Dashboard and Content Management System will be added in future versions.

---

# Project Goals

The website must provide:

- Premium corporate appearance
- High performance
- Excellent mobile experience
- SEO-friendly architecture
- Easy content management
- Easy maintenance
- Long-term scalability
- High accessibility
- Strong security

Every design and development decision must support these goals.

---

# Design Philosophy

The design philosophy is defined by the customer.

> **"The simplicity and freshness of Terzioğlu Mühendislik with the corporate and premium feeling of Folkart."**

This sentence is the main design guideline of the entire project.

The website should never become:

- overly decorative
- overly minimal
- dark themed
- futuristic
- glassmorphism based
- animation heavy

Instead, it should feel:

- clean
- premium
- elegant
- timeless
- trustworthy
- spacious
- modern

The visual language should reflect a professional construction company with more than 50 years of experience.

---

# Brand Identity

The website must represent Ançın İnşaat's own identity.

Reference websites are used only for inspiration.

The project must never become a visual copy of another website.

Components, spacing, typography and interactions should be adapted to Ançın's own corporate identity.

---

# Visual Principles

Background

- White

Accent Color

- Corporate Red

Neutral Colors

- White
- Light Gray
- Dark Gray
- Near Black

The accent color should only be used for:

- CTA buttons
- Active navigation
- Section highlights
- Thin separators
- Small visual details

Avoid using large red backgrounds.

---

# Layout Principles

The layout should be clean and readable.

Sections should have balanced spacing.

Avoid extremely large empty spaces.

Content should breathe naturally while maintaining visual continuity.

Every page should follow the same layout rhythm.

---

# Animation Philosophy

Animations should be subtle.

Target duration:

0.6s – 1.2s

Preferred easing:

ease-in-out

Animations should support the user experience instead of attracting attention.

Preferred animation style:

- Fade
- Soft Slide
- Scale
- Ken Burns
- Smooth Hover

Avoid:

- Bounce
- Elastic
- Flash
- Rotate
- Heavy Parallax

---

# Navigation Philosophy

Navigation must always be predictable.

The navigation bar remains fixed.

The user should always know where they are.

Dropdown menus should be simple and easy to use.

---

# Reusable Design System

The project should be built using reusable components.

Common sections should never be duplicated.

Examples:

- Navbar
- Footer
- Social Media Section
- Partners Section
- CTA Banner
- Buttons
- Cards
- Gallery
- Contact Form

Every reusable part should exist as a shared component.

---

# Technology Stack

Backend

- ASP.NET Core MVC

Language

- C#

Frontend

- Razor Views
- HTML5
- CSS
- JavaScript

Database

- SQLite

Hosting

- IIS

Version Control

- Git
- GitHub

Package Manager

- LibMan / npm (only if required)

---

# Technologies That Should NOT Be Used

Do not migrate the project to another framework.

Do not use:

- React
- Vue
- Angular
- Blazor
- Next.js
- Nuxt
- Tailwind-only architecture
- Docker
- Microservices

The project architecture must remain ASP.NET Core MVC.

---

# Asset Philosophy

Project assets must remain organized.

Images

Videos

PDFs

Logos

Icons

must always follow the predefined folder structure.

Do not rename customer files.

Do not automatically move assets.

Do not generate random folder structures.

---

# Performance Goals

Target:

Google Lighthouse

Performance

95+

Accessibility

95+

Best Practices

100

SEO

100

The website should feel extremely fast.

---

# Mobile First

Desktop is important.

Mobile is mandatory.

Every component must be fully responsive.

No desktop-only layouts are allowed.

---

# SEO

Every page should be optimized.

Support:

- Sitemap
- Robots
- Canonical
- OpenGraph
- Structured Data
- Meta Tags

---

# Accessibility

The website should be accessible.

Support:

- Keyboard navigation
- Screen readers
- Proper contrast
- Semantic HTML

---

# Security

Security is part of the project.

Never sacrifice security for convenience.

Every form

Every upload

Every database operation

must be implemented securely.

Detailed security rules are defined in:

12_Security.md

---

# Future Roadmap

Version 1

Corporate Website

Version 2

Admin Panel

Version 3

Content Management

Version 4

Advanced Analytics

Version 5

Customer Portal (optional)

The architecture should be prepared for future expansion.

---

# Claude Code Mission

Claude Code is responsible for maintaining:

- consistency
- readability
- maintainability
- scalability
- clean architecture

Claude Code should always prioritize project consistency over unnecessary creativity.

If a future request conflicts with this document, the project vision should be updated first before implementing the new feature.

This document is the primary source of truth for the entire project.