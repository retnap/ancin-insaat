# 09_AdminPanelPlan.md

# Admin Panel Plan

## Purpose

This document defines the future Admin Panel architecture for the Ançın İnşaat website.

The Admin Panel is **not included in Version 1** but the public website should be developed with future integration in mind.

---

# Principles

- Separate public website from administration.
- Keep modules independent.
- Follow CRUD architecture.
- Support future scalability.
- Keep UI simple and consistent.

---

# Version Roadmap

## Version 1

Public website only.

No Admin Panel.

Database and application architecture should support future integration.

---

## Future Version

Introduce a secure Admin Panel for managing website content.

---

# Authentication

Admin users must authenticate before accessing the panel.

Future support may include:

- Login
- Logout
- Password Reset
- Role Management

---

# Dashboard

## Purpose

Provide a quick overview of website activity.

Possible widgets:

- Total Projects
- Contact Messages
- Job Applications
- Recent Activity

---

# Modules

## Projects

Purpose

Manage construction projects.

Features

- Create
- Edit
- Delete
- Publish
- Unpublish
- Reorder

---

## Project Gallery

Purpose

Manage project images.

Features

- Upload
- Replace
- Delete
- Reorder

---

## Floor Plans

Purpose

Manage project floor plans.

Features

- Upload
- Edit
- Delete

---

## Partners

Purpose

Manage business partners.

Features

- Create
- Edit
- Delete

---

## Contact Messages

Purpose

Review messages submitted through the website.

Features

- Read
- Archive
- Delete

---

## Job Applications

Purpose

Review career applications.

Features

- View
- Download CV
- Archive

---

# Future Modules

These modules are intentionally excluded from Version 1.

Possible future additions:

- Site Settings
- SEO Management
- Hero Management
- News
- Blog
- Team Members
- Testimonials
- Certificates
- Awards
- Multi-language
- User Management

---

# File Management

The Admin Panel should support:

- Image Upload
- PDF Upload
- Replace
- Delete
- Preview

---

# Validation

Forms should validate:

- Required fields
- File types
- File size
- Duplicate slugs

---

# Logging

Future versions may log:

- Login history
- Content updates
- Deleted records
- Published changes

---

# Security

Only authenticated administrators may access the Admin Panel.

Uploaded files should be validated before storage.

Sensitive operations should require confirmation.

---

# Future Compatibility

The public website should never depend on the Admin Panel.

If the Admin Panel is unavailable, the website should continue functioning normally.

---

# Single Source of Truth

This document defines the future administration architecture.

It does not define:

- Database entities
- Public pages
- UI components
- Business logic

Those responsibilities belong to their respective documentation.