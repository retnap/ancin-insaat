# 05_Animations.md

# Animation Guidelines

## Purpose

This document defines the animation philosophy used throughout the Ançın İnşaat website.

Animations should improve user experience, reinforce the premium brand identity and guide attention without becoming distracting.

Detailed visual references are provided in the Inspirationals folder.

---

# Principles

- Smooth over flashy.
- Subtle over dramatic.
- Consistent across all pages.
- Performance first.
- Mobile-friendly.
- Respect reduced motion preferences.

---

# Global Behaviour

Animations should never delay interaction.

Content should remain usable even if animations are disabled.

Avoid multiple simultaneous animations.

Only animate elements that benefit user experience.

---

# Page Load

Animate:

- Hero content
- Section headers
- Main content blocks

Use:

- Fade In
- Fade Up

Avoid:

- Large zoom effects
- Long loading animations

---

# Scroll Animations

Animate elements only when they first enter the viewport.

Suitable components:

- Section Header
- Feature Card
- Value Card
- Project Card
- Statistics Card
- CTA Banner

Avoid repeated animations while scrolling.

---

# Hover Animations

Interactive elements should provide subtle feedback.

Apply to:

- Buttons
- Project Cards
- Navigation Items
- Social Icons
- Gallery Images

Hover effects should feel responsive rather than decorative.

---

# Page Transitions

Navigation between pages should feel seamless.

Use:

- Short fade transition

Avoid:

- Full-screen loaders
- Complex page transition effects

---

# Hero Banner

Hero sections may include:

- Background image fade
- Text reveal
- CTA fade

Hero animations should finish quickly.

---

# Project Gallery

Images should open with a smooth transition.

Gallery navigation should feel lightweight.

Avoid excessive motion.

---

# Forms

Inputs should provide visual feedback for:

- Focus
- Success
- Validation Error

Feedback should be immediate.

---

# Navigation

Navbar should animate only when necessary.

Examples:

- Sticky appearance
- Mobile menu open
- Mobile menu close

Scrolling should remain smooth.

---

# Performance Rules

Prefer CSS animations.

Use JavaScript only when necessary.

Animate:

- opacity
- transform

Avoid animating:

- width
- height
- top
- left

Minimise layout shifts.

---

# Accessibility

Respect the user's reduced motion preference.

Animations must never hide important content.

Users should never be forced to wait for an animation before interacting.

---

# Inspiration

Animation timing, pacing and overall feeling should follow the visual references inside:

- Inspirationals/Folkart
- Inspirationals/Terzioglu

The goal is to achieve the same premium feeling while keeping the implementation original.