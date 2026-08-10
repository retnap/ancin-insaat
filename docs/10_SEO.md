# 10_SEO.md

# SEO Guidelines

## Purpose

This document defines the SEO strategy for the Ançın İnşaat website.

The goal is to maximise search engine visibility while maintaining clean, semantic and maintainable code.

SEO implementation should be simple, consistent and scalable.

---

# Principles

- SEO should be built into the project from the beginning.
- Every public page should be indexable unless explicitly excluded.
- Generate semantic HTML.
- Prioritise content quality over keyword stuffing.
- Follow Google's best practices.

---

# URL Structure

Use clean, readable URLs.

Examples:

```text
/

about-us

values

projects

projects/{project-slug}

career

contact

kvkk
```

Avoid:

```text
/page?id=15

/projects/123

/project-detail?id=7
```

---

# Meta Tags

Every public page should include:

- Meta Title
- Meta Description
- Canonical URL

Each page should have unique metadata.

---

# Open Graph

Every public page should support:

- Title
- Description
- Image
- URL

This improves link previews on social media.

---

# Structured Data

Implement structured data where appropriate.

Recommended schemas:

- Organization
- WebSite
- AboutPage
- CollectionPage
- ContactPage
- JobPosting
- RealEstateListing

---

# Headings

Each page should contain:

- One H1
- Logical H2 hierarchy
- Logical H3 hierarchy

Avoid skipping heading levels.

---

# Images

Every image should include:

- Alt Text

Optional:

- Title

Use descriptive filenames.

Example:

```text
luxury-apartment-building.webp
```

Avoid:

```text
image001.jpg
```

---

# Internal Linking

Pages should link naturally to related content.

Examples:

Home

→ Projects

About Us

→ Our Values

Projects

→ Project Detail

Career

→ Contact

---

# Project SEO

Each project should have:

- Unique URL
- Unique Meta Title
- Unique Meta Description
- Cover Image
- Open Graph Image

---

# Performance

SEO benefits from good performance.

Prioritise:

- Fast loading
- Responsive images
- Lazy loading
- Optimised assets

---

# Mobile SEO

The website should be mobile-first.

All pages should provide the same content on desktop and mobile.

---

# Sitemap

Generate:

```text
sitemap.xml
```

Include all public pages.

Exclude:

- Error Pages
- Admin Panel

---

# Robots

Provide:

```text
robots.txt
```

Allow indexing of public pages.

Disallow:

- Admin Panel
- Temporary files

---

# Canonical URLs

Each public page should define its canonical URL.

Avoid duplicate content.

---

# Breadcrumbs

Where appropriate, implement breadcrumb navigation.

Recommended for:

- Project Detail

---

# 404 Page

Provide a custom 404 page.

Include links to:

- Home
- Projects
- Contact

---

# Redirects

When URLs change:

Use permanent redirects (301).

Avoid broken links.

---

# Content Quality

Content should be:

- Original
- Clear
- Helpful
- Easy to read

Avoid duplicated content.

---

# Future Compatibility

The future Admin Panel may allow administrators to edit:

- Meta Title
- Meta Description
- Open Graph Image

without changing application code.

---

# Single Source of Truth

This document defines the SEO strategy.

It does not define:

- Page layouts
- Database entities
- UI components
- Visual design

Those responsibilities belong to their respective documentation.