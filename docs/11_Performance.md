# 11_Performance.md

# Performance Guidelines

## Purpose

This document defines the performance goals for the Ançın İnşaat website.

The objective is to provide a fast, responsive and smooth user experience across all supported devices.

---

# Principles

- Performance is a feature.
- Optimise before adding complexity.
- Mobile performance is a priority.
- Minimise unnecessary requests.
- Load only what is needed.

---

# Performance Goals

Prioritise:

- Fast initial load
- Smooth scrolling
- Responsive interactions
- Minimal layout shifts

---

# Images

Images should:

- Use WebP where possible.
- Be appropriately sized.
- Be compressed before deployment.
- Include responsive versions when beneficial.

Lazy load non-critical images.

---

# Videos

Videos should:

- Be compressed.
- Avoid autoplay with sound.
- Load only when necessary.

Large background videos should be used sparingly.

---

# CSS

- Minify production CSS.
- Remove unused styles.
- Keep selectors simple.
- Avoid duplicated rules.

---

# JavaScript

- Load scripts only when required.
- Defer non-critical scripts.
- Avoid blocking page rendering.
- Keep client-side logic lightweight.

---

# Fonts

Use a maximum of two font families.

Load only required font weights.

Prefer self-hosted fonts when practical.

---

# Animations

Animation should never reduce usability.

Prefer:

- opacity
- transform

Avoid animating layout-related properties.

Animation rules are defined in:

- 05_Animations.md

---

# HTTP Requests

Reduce unnecessary requests.

Reuse assets whenever possible.

Avoid duplicate downloads.

---

# Caching

Enable caching for:

- Images
- Fonts
- CSS
- JavaScript

Use appropriate cache headers in production.

---

# Compression

Enable response compression.

Compress:

- HTML
- CSS
- JavaScript

---

# Database

Keep queries efficient.

Retrieve only required data.

Avoid unnecessary database operations.

---

# Lighthouse

Target:

- Performance ≥ 90
- Accessibility ≥ 90
- Best Practices ≥ 90
- SEO ≥ 90

These values are targets, not strict requirements.

---

# Monitoring

Before deployment, verify:

- Page load speed
- Broken assets
- Console errors
- Network requests

---

# Future Compatibility

Future optimisation may include:

- CDN
- Image optimisation service
- Output caching
- Response caching

The project structure should allow these improvements without major refactoring.

---

# Single Source of Truth

This document defines the performance goals.

It does not define:

- SEO strategy
- Security
- UI design
- Business logic

Those responsibilities belong to their respective documentation.