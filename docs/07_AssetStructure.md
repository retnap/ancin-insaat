# 07_AssetStructure.md

# Asset Structure

## Purpose

This document defines how static assets are organised within the Ancın İnşaat website.

A consistent asset structure improves maintainability, scalability and collaboration.

---

# Principles

- Keep folder names simple.
- Group assets by type.
- Use lowercase file names.
- Use kebab-case naming.
- Avoid duplicate assets.
- Optimise assets before deployment.

---

# Folder Structure

```text
wwwroot/
│
├── images/
│   ├── company/
│   ├── projects/
│   ├── hero/
│   ├── icons/
│   ├── logos/
│   ├── partners/
│   ├── team/
│   └── seo/
│
├── videos/
│
├── documents/
│
├── fonts/
│
├── icons/
│
├── css/
│
├── js/
│
└── uploads/
```

---

# Images

Images should be organised by purpose rather than by page.

Example:

- company/
- projects/
- hero/
- partners/

Avoid creating folders such as:

- home/
- about/
- contact/

---

# Project Images

Each project should have its own folder.

Example:

```text
projects/

green-valley/

    cover.webp

    gallery-01.webp

    gallery-02.webp

    gallery-03.webp

    floorplan-a.webp

    floorplan-b.webp
```

---

# Hero Images

Hero banners should be stored separately.

Example:

```text
hero/

home.webp

about.webp

projects.webp

contact.webp
```

---

# Company Assets

Store reusable corporate assets together.

Examples:

- Logo
- Brand Marks
- Office Photos
- Company Images

---

# Partner Assets

Partner logos should be stored independently.

Example:

```text
partners/

partner-a.webp

partner-b.webp

partner-c.webp
```

---

# Documents

Documents include:

- Project Catalogues
- Brochures
- PDF Files
- KVKK Documents

Example:

```text
documents/

catalogues/

kvkk/

brochures/
```

---

# Videos

Videos include:

- Hero Backgrounds
- Project Videos
- Promotional Videos

Videos should be compressed for web delivery.

---

# Fonts

Store locally hosted fonts.

Example:

```text
fonts/

inter/

playfair-display/
```

---

# Icons

Icons should be reusable.

Prefer SVG whenever possible.

Avoid duplicated icon files.

---

# SEO Assets

Store SEO-related images separately.

Examples:

- Open Graph Images
- Social Sharing Images
- Favicons

---

# Uploads

The uploads directory stores files added through the future Admin Panel.

Examples:

- New Project Images
- Gallery Images
- Documents

Uploads should remain independent from bundled project assets.

---

# File Naming

Use:

```text
company-building.webp

featured-project.webp

hero-home.webp

project-gallery-01.webp
```

Avoid:

```text
IMG001.jpg

photo-final2.png

newimage.jpeg
```

---

# Image Formats

Preferred:

- WebP

Supported:

- PNG
- SVG
- JPG

Use SVG for:

- Logos
- Icons
- Simple Graphics

---

# Document Formats

Preferred:

- PDF

Supported:

- DOCX (Admin Only)

---

# Asset Optimisation

Images should be:

- Compressed
- Responsive
- Lazy Loaded where appropriate

Videos should be:

- Optimised
- Compressed
- Stream-friendly

---

# Display Images

Original photos are the archival source and are never modified.

Every lightbox image, banner and in-page image without a thumbnail is served as a web-sized "display" WebP stored in a `display/` folder next to the original.

Path rule:

- If the original path contains `/originals/`, that segment is replaced with `/display/`.
- Otherwise a `display/` folder is added in the same directory as the original.
- The extension is always changed to `.webp`.

Example:

```text
projects/le-jardin/konum/konum.jpg
→ projects/le-jardin/konum/display/konum.webp

projects/x/gallery/originals/photo-01.jpg
→ projects/x/gallery/display/photo-01.webp
```

Settings (AncinInsaat.ImagePipeline/DisplayImageGenerator.cs):

| Usage | Long edge | WebP quality |
| --- | --- | --- |
| Lightbox and in-page images | 1920px | 80 |
| Banners, hero and full-width images | 2560px | 85 |

- Lossy WebP is always used, also for PNG sources.
- Images are never upscaled.
- EXIF metadata is stripped; the ICC profile is kept.

If a display file would be larger than its original (for example a small, already optimised JPEG), no display file is kept and the site falls back to the original.

Display files are stored in normal Git (not Git LFS).

Generate a display version with:

```text
dotnet run --project tools/ThumbnailTool -- --display <source> <dest> [--wide]
```

---

# Future Admin Compatibility

The Admin Panel should support:

- Upload
- Replace
- Delete
- Preview

Each uploaded asset should store:

- File Name
- File Type
- File Size
- Upload Date
- Related Content

---

# Single Source of Truth

This document defines where assets are stored.

It does not define:

- Database relationships
- UI Components
- Page Layouts
- Styling

Those responsibilities belong to their respective documentation.