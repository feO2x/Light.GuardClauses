# AGENTS.md for AI Plans

Apart from this file, this folder contains only Markdown plans. Plan file names start with the four-digit GitHub issue number they belong to.

## How to Write Plans

1. Every plan contains exactly these three sections, in this order: `## Rationale`, `## Acceptance Criteria`, and `## Technical Details`.
2. The Rationale briefly describes the problem and the overarching goal of addressing it. Keep it to one or two short paragraphs unless additional context is necessary to understand the change.
3. Acceptance Criteria contains observable and verifiable outcomes using Markdown task-list items (`- [ ]`). Describe what must be true when the work is complete, not the individual implementation steps.
4. Technical Details records the important implementation decisions, constraints, affected components, and non-obvious interactions. Include enough information for an implementer to understand the intended design without prescribing the complete implementation. Assume the implementer is a senior software engineer.
5. Use code examples in Technical Details when they define an important contract more clearly and concisely than prose, such as API signatures, central interfaces, or DTO shapes. Keep them minimal, omit implementation bodies and routine context, and identify them as exact or illustrative when this is not obvious. Avoid step-by-step instructions, exhaustive file lists, and background knowledge expected of a senior software engineer.
6. When behavior changes, include an acceptance criterion requiring appropriate automated test coverage. Describe specific test cases in Technical Details only when they are not obvious from the acceptance criteria.
7. Include microbenchmarks only for changes where performance is a relevant risk or requirement. Add them to Acceptance Criteria and describe benchmark scenarios in Technical Details only when those scenarios are not self-evident.
8. Keep plans concise. Prefer decisions, constraints, and outcomes to explanations of routine implementation work.
