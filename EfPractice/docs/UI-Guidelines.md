# Project Guidelines

## Architecture & Structure
- Prefer Razor Pages for new UI; keep MVC views where already used (`Views/Home/*`).
- Organize by feature: Pages/Areas, Controllers, Models, Services, Data, wwwroot.
- Use partials for reusable UI (`Views/Shared`, `Pages/Shared`).
- Keep JS/CSS in wwwroot; avoid inline scripts unless small and scoped.

## Coding Standards
- Target .NET 8, C# 12. Use `async`/`await` for I/O.
- Name classes and files consistently, PascalCase for types, camelCase for locals.
- Inject dependencies via constructor; avoid service locator.
- Use DTO/ViewModels per page/view to avoid over-posting.

## Razor Pages & MVC
- Razor Pages: place page models in the same folder; use `asp-page`, `asp-page-handler`.
- MVC Views: continue using `asp-action`, `asp-controller` for existing controllers.
- Use tag helpers for forms, validation, and links.

## UI Guidelines

- Use container with card layout for CRUD and list sections.
- CRUD form:
  - Wrap in `div.container > div.card.card-add > div.card-body`.
  - Header row: `d-sm-flex align-items-center mb-4` with `h4.card-title` and optional action button aligned `ms-auto`.
  - Form uses `class="row g-3"`, labels `form-label`, inputs `form-control`, checkboxes `form-check`.
  - Group related fields with a bordered `fieldset` using `class="border p-3 mb-3 rounded"`.
  - Tables use `class="table"`; avoid `table-bordered` unless needed.
  - Primary submit button first; secondary actions (clear/back) as outlined buttons.
- List section:
  - Separate card: `div.card > div.card-body`.
  - Title `h4.card-title`.
  - Table headers: Id, Name, Active, Actions; actions column width constrained and `text-nowrap`.
- Accessibility:
  - Match `label for` with input `id`.
  - Use descriptive placeholder and `aria-required` where appropriate.
- Responsiveness:
  - Prefer `col-12 col-sm-6 col-md-4` patterns for fields.
  - Use `ms-auto`, `text-end`, and grid utilities for alignment.
- Navigation:
  - Use `asp-page` for Razor Pages and `asp-action` for MVC views appropriately.
- Scripts:
  - Load shared scripts via layout; avoid duplicate library imports.
  - Keep autocomplete/select2 initializations unobtrusive and feature-detected.
- Styling:
  - Reuse template CSS: `~/template/css/bootstrap.min.css` and `~/template/css/style.css`.
  - Keep custom inline styles minimal and scoped.
- Pattern example (Brands):
  - Card for add/update form.
  - Card for list with table.
  - Buttons: Edit (warning), Delete (danger, confirm), Clear (secondary).
- Apply same pattern to Sale Invoice (`SInv.cshtml`):
  - CRUD form card with buyer fieldset and items table.
  - Second card showing quick list or link to full `SInvList`.

## Validation & Model Binding
- Use data annotations for server-side validation; enable client validation with `_ValidationScriptsPartial`.
- Bind only required properties. For collections, ensure correct indices in form fields.
- Prefer `asp-for` and `asp-validation-for` tag helpers.

## Security
- Enforce authentication via Identity; protect pages with `[Authorize]`.
- Validate anti-forgery token on POST (`<form asp-antiforgery="true">` enabled by default).
- Encode output by default; avoid `Html.Raw` unless safe.
- Validate and sanitize user inputs and uploaded files.

## Performance
- Bundle and minify static assets; use CDN where safe.
- Avoid N+1 queries; use async EF Core with `AsNoTracking` for read-only.
- Cache commonly used lookups; debounce client-side searches.
- Defer heavy scripts; initialize features on demand.

## Logging & Error Handling
- Use `ILogger<T>` and structured logs.
- Global exception handling via middleware and user-friendly error pages.
- Log sensitive data cautiously.

## Testing
- Unit test services and page handlers.
- Integration tests for critical flows (login, invoice save).
- Use test data builders and in-memory providers where suitable.

## Deployment & Configuration
- Use `appsettings.json` and environment overrides; do not hardcode secrets.
- Enable HTTPS redirection and HSTS in production.
- Run EF Core migrations as part of deployment.

## Accessibility
- Keyboard navigable forms and tables.
- Sufficient color contrast and focus states.
- Proper ARIA attributes for interactive components.
