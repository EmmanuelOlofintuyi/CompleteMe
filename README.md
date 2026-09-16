# CompleteMe

Task & Goal Management Application

## Overview
CompleteMe is a personal productivity and planning application designed to help users organize goals, projects, tasks, categories, and recurring work in one place. The product is built around a clean domain-first model where a goal is the primary planning object and a project is an optional grouping mechanism.

## Core Domain Model
The project currently models the following entities:

- Goal
  - Represents the main planning unit
  - Can exist without a project
  - Includes metadata such as status, dates, category, and recurrence
- Project
  - Optional grouping container for multiple goals
  - Tracks project-level status and metadata
- TaskItem
  - Concrete work item that belongs to a goal
  - Supports parent/child task relationships
  - Includes status and recurrence information
- Category
  - Used to group and classify goals, projects, and tasks
- RecurrenceRule
  - Represents recurring planning rules for goals and tasks

## Current Status
This repository has reached the domain-model foundation stage. The app has not yet moved into full API or UI implementation, but the core entities and their business relationships are now in place.

### Included so far
- Domain layer with entities and enums
- Goal, project, and task-level status enums
- Domain tests covering core behavior
- Project structure prepared for infrastructure and application layers

## Technology Stack
- C# / .NET 8
- xUnit for domain testing
- Layered architecture approach:
  - Domain
  - Infrastructure
  - Application
  - API
  - MAUI UI (planned)

## Project Structure

```text
CompleteMe/
├── Documentation/
│   └── README.md
├── src/
│   ├── Domain/
│   │   ├── Entities/
│   │   ├── Enums/
│   │   └── CompleteMe.Domain.csproj
│   ├── Infrastructure/
│   ├── Application/
│   ├── Api/
│   └── MAUI/
└── tests/
    └── CompleteMe.Domain.Tests/
```

## Domain Rules Established
- A goal is the primary planning entity.
- A project is optional and may group multiple goals.
- A task requires a goal reference.
- Tasks can support a parent-child relationship.
- Goals and tasks can carry category and recurrence metadata.
- Status is tracked explicitly using domain enums.

## Testing
The project includes a domain test suite covering the core entities and expected behaviors.

Verified test result:
- 5 tests passed
- 0 failed
- 0 skipped

## Next Planned Work
The next step is to move from the domain layer into the persistence and application layers:

1. Create EF Core DbContext and entity configuration
2. Add repositories for goals, projects, and tasks
3. Add application services for use cases
4. Build API controllers
5. Later add the MAUI front end

## Note
This README reflects the current state of the project after the domain model and initial tests were established. It is intended to document the work completed before the next phase of infrastructure and API development.
