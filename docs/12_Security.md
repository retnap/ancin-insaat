# 12_Security.md

# Security Guidelines

## Purpose

This document defines the security principles for the Ançın İnşaat website.

Security should be considered throughout development without introducing unnecessary complexity.

---

# Principles

- Secure by default.
- Validate every user input.
- Never trust client-side data.
- Minimise exposed information.
- Follow ASP.NET Core security best practices.

---

# Authentication

Version 1 does not include an Admin Panel.

Future authentication should support:

- Secure Login
- Password Hashing
- Session Management
- Role-Based Authorization

---

# Authorization

Only authenticated administrators may access future administration features.

Public users should never access administrative endpoints.

---

# Input Validation

Validate all user input.

Including:

- Contact Form
- Career Form
- File Uploads

Validation should exist on both client and server.

---

# File Uploads

Only allow supported file types.

Validate:

- File Extension
- MIME Type
- File Size

Never execute uploaded files.

Store uploads outside executable locations when appropriate.

---

# SQL Injection

Always use Entity Framework Core.

Avoid raw SQL unless absolutely necessary.

Never build SQL queries using string concatenation.

---

# Cross-Site Scripting (XSS)

Encode all user-generated content before rendering.

Avoid rendering raw HTML unless the content is trusted.

---

# Cross-Site Request Forgery (CSRF)

Protect all POST requests.

Use ASP.NET Core anti-forgery protection.

---

# Sensitive Data

Never expose:

- Connection Strings
- API Keys
- Secrets
- Internal Paths

Store sensitive configuration securely.

---

# Error Handling

Display friendly error pages.

Never expose:

- Stack Traces
- Database Errors
- Server Information

Log detailed errors internally.

---

# HTTPS

Use HTTPS in production.

Redirect HTTP requests to HTTPS.

---

# Headers

Enable appropriate security headers.

Examples include protection against:

- Clickjacking
- MIME Sniffing
- Untrusted Content

---

# Dependencies

Keep third-party packages up to date.

Remove unused dependencies.

Only use trusted libraries.

---

# Logging

Log important events.

Examples:

- Application Errors
- File Upload Failures
- Authentication Events (Future)

Do not log sensitive user information.

---

# Contact Form

Protect against:

- Spam
- Invalid Input
- Automated Submissions

Future versions may include CAPTCHA if necessary.

---

# Security Updates

Review dependencies regularly.

Apply security updates during maintenance.

---

# Future Compatibility

The project should support future implementation of:

- Identity
- Multi-Factor Authentication
- Audit Logs
- Role Management

without major architectural changes.

---

# Single Source of Truth

This document defines the security guidelines.

It does not define:

- Business Logic
- Database Schema
- UI Components
- Authentication Implementation

Those responsibilities belong to their respective documentation.