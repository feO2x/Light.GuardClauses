# AGENTS.md for AI Plans

*Version 1.1*

This directory contains Markdown plans and plan deviation records, plus this instruction file. These documents follow the [Guided Coding](https://kenny-codes.net/docs/guided-coding/) approach.

File names begin with the four-digit GitHub issue number and a short kebab-case description. When an issue has multiple documents, insert a zero-based sequence after the issue number, for example `0015-0-feature.md`, `0015-1-optimization.md`, and `0015-2-plan-deviations.md`. Do not rename or restructure completed historical plans solely to conform to the current rules.

## How to Write Plans

1. Every plan starts with a `# Title` and contains exactly three sections, in this order: `## Rationale`, `## Acceptance Criteria`, and `## Technical Details`.
2. The Rationale briefly describes the problem and the overarching goal of addressing it. Keep it to one or two short paragraphs unless additional context is necessary to understand the change.
3. Acceptance Criteria contains observable and verifiable outcomes using Markdown task-list items (`- [ ]`). Describe what must be true when the work is complete, not the individual implementation steps.
4. Technical Details records the important implementation decisions, constraints, affected components, and non-obvious interactions. Include enough information for an implementer to understand the intended design without prescribing the complete implementation. Assume the implementer is a senior software engineer.
5. Use code examples in Technical Details when they define an important contract more clearly and concisely than prose, such as API signatures, central interfaces, or DTO shapes. Keep them minimal, omit implementation bodies and routine context, and identify them as exact or illustrative when this is not obvious. Avoid step-by-step instructions, exhaustive file lists, and background knowledge expected of a senior software engineer.
6. When behavior changes, include an acceptance criterion requiring appropriate automated test coverage. Describe specific test cases in Technical Details only when they are not obvious from the acceptance criteria.
7. Include microbenchmarks only for changes where performance is a relevant risk or requirement. Add them to Acceptance Criteria and describe benchmark scenarios in Technical Details only when those scenarios are not self-evident.
8. Keep plans concise. Prefer decisions, constraints, and outcomes to explanations of routine implementation work.

Use these rules for follow-up plans, too.

## How to Write Plan Deviation Files

When users find substantial issues in the Guiding Phase and create new plans to fix these, a single plan deviation file sums up the differences from the original plan and its follow-up plans. They do not use the same structure as plans, you can use any format that clearly communicates the differences. Often, a feature-based approach is appropriate.

While the format is flexible, include the following information:

- Reference every plan included in the comparison.
- Describe what was originally specified, what the codebase actually does, and why the change was made.
