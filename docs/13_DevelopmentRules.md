# 13_DevelopmentRules.md

# Development Rules

## Purpose

This document defines the development standards for the Ançın İnşaat website.

All code should follow these rules to ensure consistency, maintainability and scalability.

---

# General Principles

- Keep the solution simple.
- Prefer readability over clever code.
- Follow ASP.NET Core best practices.
- Build for maintainability.
- Avoid unnecessary abstractions.

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
- SQLite

Deployment

- IIS

Source Control

- Git
- GitHub

---

# Architecture

Use standard ASP.NET Core MVC architecture.

Separate:

- Models
- Views
- Controllers

Keep business logic outside controllers.

---

# Controllers

Controllers should:

- Stay small
- Validate requests
- Delegate business logic
- Return appropriate responses

Avoid complex logic inside controllers.

---

# Services

Business logic belongs inside Services.

Controllers should communicate with Services.

Services should communicate with the database.

---

# Dependency Injection

Use ASP.NET Core Dependency Injection.

Avoid manual object creation whenever possible.

---

# Entity Framework

Use Entity Framework Core.

Prefer LINQ.

Avoid unnecessary raw SQL.

Use migrations for schema updates.

---

# Asynchronous Programming

Use async/await for:

- Database operations
- File operations
- External services

Avoid blocking calls.

---

# Error Handling

Handle exceptions gracefully.

Return friendly error pages.

Log unexpected errors.

Never expose internal exception details.

---

# Logging

Use ASP.NET Core logging.

Log:

- Errors
- Warnings
- Important application events

Avoid excessive logging.

---

# Validation

Validate all user input.

Client-side validation improves UX.

Server-side validation is mandatory.

---

# Naming Convention

Classes

- PascalCase

Methods

- PascalCase

Properties

- PascalCase

Interfaces

- Prefix with I

Variables

- camelCase

Private fields

- _camelCase

---

# Folder Structure

Keep folders organised by responsibility.

Avoid unnecessary nesting.

Follow the project structure defined by the solution.

---

# Views

Views should remain presentation-focused.

Avoid business logic inside Razor files.

Reuse partial views whenever appropriate.

---

# Components

Reuse existing components whenever possible.

Do not duplicate UI components.

Follow:

- 04_ComponentLibrary.md

---

# Styling

Follow:

- 02_DesignSystem.md

Do not introduce page-specific styling unless required.

---

# JavaScript

Keep JavaScript modular.

Avoid large scripts.

Load only where necessary.

---

# Configuration

Do not hardcode:

- URLs
- File paths
- Secrets

Use configuration files.

---

# Version Control

Commit frequently.

Each commit should represent a meaningful change.

Use descriptive commit messages.

---

# Code Quality

Prefer:

- Clean methods
- Small classes
- Reusable code

Avoid:

- Dead code
- Duplicate code
- Large methods
- Magic values

---

# Testing

Code should be easy to test.

Avoid tightly coupled implementations.

---

# Documentation

When architecture changes:

Update the relevant documentation.

Do not allow documentation to become outdated.

---

# Future Compatibility

Write code that supports future additions without major refactoring.

Avoid assumptions that limit future development.

---

# Single Source of Truth

Development decisions should follow this priority:

1. CLAUDE.md
2. 14_Decisions.md
3. Project Documentation
4. ASP.NET Core Best Practices