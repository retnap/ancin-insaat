# 02_DesignSystem.md

# Design System

## Purpose

This document defines the visual language of the Ançın İnşaat website.

It establishes consistent design rules for layouts, typography, colours, spacing and reusable UI patterns.

Detailed page structures are documented in **03_PageBlueprints.md**.

Reusable components are documented in **04_ComponentLibrary.md**.

---

# Design Principles

The website should feel:

- Premium
- Modern
- Elegant
- Professional
- Minimal

Every page should prioritise readability, consistency and simplicity.

Avoid visual clutter.

---

# Visual Inspiration

The overall design language should follow the references inside:

- Inspirationals/Folkart
- Inspirationals/Terzioglu

These references define the desired visual quality, not the implementation.

The final website must remain original.

---

# Colour Palette

## Primary

Used for:

- Brand
- Primary Buttons
- Active States

---

## Secondary

Used for:

- Background Highlights
- Supporting Elements

---

## Neutral

Used for:

- Backgrounds
- Cards
- Borders

---

## Text

Support:

- Primary Text
- Secondary Text
- Muted Text

---

## Feedback

Support colours for:

- Success
- Warning
- Error
- Information

---

# Typography

Use a maximum of two font families.

Typography should establish a clear visual hierarchy.

Support:

- H1
- H2
- H3
- H4
- Body
- Small Text

Headings should be bold and highly readable.

Body text should prioritise readability over density.

---

# Spacing

Use a consistent spacing scale throughout the project.

Apply spacing consistently to:

- Sections
- Cards
- Forms
- Navigation
- Buttons

Avoid arbitrary spacing values.

---

# Layout

The website should use a consistent content width.

Support:

- Full Width Sections
- Standard Content Container

All layouts should be mobile-first.

---

# Grid

Use a responsive grid.

Layouts should adapt naturally across:

- Mobile
- Tablet
- Desktop

Avoid fixed-width layouts.

---

# Border Radius

Use a consistent border radius across:

- Cards
- Buttons
- Forms
- Images

Avoid mixing multiple corner styles.

---

# Shadows

Use subtle shadows.

Shadows should separate elements rather than attract attention.

Avoid heavy shadow effects.

---

# Borders

Borders should be minimal and consistent.

Use borders only where they improve readability.

---

# Buttons

Support:

- Primary
- Secondary
- Text Button

Buttons should clearly communicate hierarchy.

---

# Cards

Cards should follow a consistent structure.

Typical card content:

- Image
- Title
- Description
- CTA

---

# Forms

Forms should use consistent:

- Labels
- Inputs
- Validation
- Error Messages

Required fields should be clearly indicated.

---

# Icons

Use a single icon library throughout the project.

Icons should remain simple and consistent.

---

# Images

Images should:

- Be high quality
- Match the premium identity
- Use consistent aspect ratios

Avoid low-resolution images.

---

# Sections

Each section should include:

- Clear heading
- Logical spacing
- Consistent alignment

Optional:

- Supporting description

---

# White Space

Use generous white space.

Content should never feel crowded.

---

# Responsive Design

Design mobile-first.

Support:

- Mobile
- Tablet
- Desktop

Layouts should adapt without changing content hierarchy.

---

# Accessibility

Support:

- Keyboard Navigation
- Screen Readers
- Visible Focus States
- Sufficient Colour Contrast

Accessibility should never conflict with the visual design.

---

# Motion

Animation behaviour is defined in:

- 05_Animations.md

---

# Consistency Rules

Maintain consistency across:

- Colours
- Typography
- Buttons
- Cards
- Forms
- Icons
- Navigation
- Spacing

Avoid creating page-specific design variations unless necessary.

---

# Single Source of Truth

This document defines the visual language of the website.

It does not define:

- Page layouts
- Component behaviour
- Database structure
- Business logic

Those responsibilities belong to their respective documentation.

---

# Approved Design Decisions

The following decisions have been explicitly approved by the project owner.

These are not suggestions.

They are mandatory and override any future assumptions.

Claude must always follow these decisions unless they are explicitly changed.

---

## Colour Palette

### Primary Accent

Deep Brick Red

HEX: #9E2B25

Usage:

- Primary CTA buttons
- Active navigation state
- Links
- Highlights
- Small decorative accents

Never use this colour for large backgrounds.

---

### Neutrals

Near Black

#1C1C1C

Dark Gray

#55504D

Light Gray

#F0EEEB

White

#FFFFFF

---

## Typography

Heading Font

Playfair Display

Body Font

Inter

Rules:

- Playfair Display is used only for headings.
- Inter is used for all UI and body text.
- Do not introduce additional font families.

---

## Visual Identity

The visual identity should communicate:

- Trust
- Longevity
- Craftsmanship
- Premium Quality
- Simplicity
- Modern Elegance

Avoid:

- Startup aesthetics
- Tech-product styling
- Glassmorphism
- Neon colours
- Oversized gradients
- Decorative animations

---

## Future Changes

Any future change to the approved Design System requires explicit approval from the project owner.

---

## Single Source of Truth

This document is the single source of truth for all visual design decisions.

Claude must never invent or replace any approved design decision.

If a required design decision is missing, implementation must stop until the project owner approves it.

Any conflicting information in prompts or future conversations must defer to this document unless the project owner explicitly approves a change.