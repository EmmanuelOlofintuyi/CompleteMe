# CompleteMe

Task & Goal Management Application

## Overview
CompleteMe is a personal productivity application for managing goals, projects, tasks, categories, recurrence, and scheduling. The current MVP focuses on a clean, layered .NET architecture where goals are the primary planning object and tasks provide the execution detail underneath them.

## Current Status
The project has moved beyond the domain-only foundation and is now operating in a working MVP flow for the core goal and task lifecycle.

### Verified implementation
- Domain entities and enums are in place
- EF Core + LocalDB infrastructure is configured and working
- Goal and task services are implemented
- API controllers are active for goals and tasks
- Goal completion workflow is enforced by business rules
- API integration tests are passing for the main completion rules

### Verified test results
Fresh verification command:

```powershell
dotnet test .\tests\CompleteMe.Api.Tests\CompleteMe.Api.Tests.csproj --nologo
```

Current result:
- Passed: 3
- Failed: 0
- Skipped: 0

## Core domain model
The project models the following entities:

- Goal
  - main planning object
  - optional project association
  - includes dates, status, metadata, and task collection
- Project
  - optional grouping container for related goals
- TaskItem
  - concrete unit of work for a goal
  - supports parent/child hierarchy
  - includes status, dates, and optional recurrence metadata
- Category
  - used to classify goals, projects, and tasks
- RecurrenceRule
  - represents repeating planning rules

## Architecture
The project follows a layered .NET 8 design:

- Domain
- Infrastructure
- Application
- API
- Tests

## Current business rules
The app currently enforces the following key rules:

- a goal cannot be completed while it still has incomplete tasks
- task names cannot be empty or whitespace-only
- a task cannot be its own parent
- a task cannot be moved into a circular parent chain
- updates use patch-style semantics so omitted fields do not overwrite existing data

## Current project structure

```text
CompleteMe/
├── Documentation/
│   ├── Notes/
│   └── README.md
├── src/
│   ├── Domain/
│   ├── Infrastructure/
│   ├── Application/
│   └── Api/
├── tests/
│   ├── CompleteMe.Domain.Tests/
│   └── CompleteMe.Api.Tests/
├── README.md
└── .gitignore
```

## Testing strategy
The project uses both:

- domain-level tests for business rules and data validation
- API integration tests for real HTTP-level workflow verification

This gives coverage for both logic and endpoint behavior.

## Next major work
The project is now ready for the next layer of product rules:

1. Finish the final validation pass for edge cases
2. Add regression tests for invalid date ranges and invalid parent updates
3. Decide recurrence behavior
4. Decide dismissal and overdue handling
5. Define the scheduling model and user-visible rules
6. Move into UI work only after the API is stable and tested

## Notes and documentation
Project notes and testing guidance are in:

- [Documentation/Notes/NextSteps.md](Documentation/Notes/NextSteps.md)
- [Documentation/Notes/2026-09-16/SessionNotes.md](Documentation/Notes/2026-09-16/SessionNotes.md)
- [Documentation/Notes/2026-09-16/LocalDB-and-Testing.md](Documentation/Notes/2026-09-16/LocalDB-and-Testing.md)

## Summary
CompleteMe is now in a working MVP stage for goal and task planning with real API validation. The next focus is not generic scaffolding — it is the remaining validation and product-rule decisions around recurrence, dismissal, and scheduling.
