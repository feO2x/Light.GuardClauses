# AGENTS.md for AI Plans

In this folder, we only keep Markdown files for AI plans. Each file has a four-digit prefix which corresponds to the GitHub issue number.

## How to Write Plans

1. Each plan consists of exactly the three following elements: a rationale, acceptance criteria, and technical details.
2. A rationale describes the overarching goal of the plan. Keep this concise, only elaborate when special circumstances require it. Use free text in this section of the plan.
3. Acceptance criteria is a list of requirements that must be met for the plan to be considered complete. Use check marks `- [ ]` for these in Markdown.
4. Technical details describes the changes on a technical level. Which classes and members should be extended, which design patterns should be used? How do we ensure performance? Avoid going into too much detail (do not write the finished implementation), but focus on the high-level design and how parts of the codebase are affected. It should give the implementer a clear picture of what needs to be done.
5. Regarding automated tests: it is usually enough to note that 'automated tests need to be written' as one check mark in the acceptance criteria list. If there are special requirements for the tests, these can be elaborated in the technical details section. But normally, you can simply leave these technical details out if the automated tests can be written in a straightforward way.
6. When a plan should involve micro benchmarks, these need to be included in the acceptance criteria list and optionally in the technical details section. As with automated tests, if the BenchmarkDotNet benchmarks can be written in a straightforward way, leave out the technical details.
7. In general, keep the plan concise and to the point. Do not include general information that the implementer already knows.
