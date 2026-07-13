# Documentation Restructuring

## Rationale

The project documentation is split between the repository and seven GitHub Wiki pages, which prevents documentation changes from being reviewed with the code and has allowed framework, repository-layout, assertion, and source-export guidance to become stale. Move the Wiki's useful content into a navigable `docs` directory, update it against the current .NET 10 codebase, and make the repository the single source of truth.

GitHub cannot redirect a disabled Wiki's `/wiki/...` URLs to repository files. Preserve legacy links during a short transition by replacing the Wiki pages with moved notices, while updating every project-controlled link immediately; after the transition, remove the Wiki pages and disable the Wiki feature as the required final state.

## Acceptance Criteria

- [ ] A `docs` directory contains a landing page and maintained equivalents of all seven current Wiki topics, with consistent navigation and no loss of useful project, usage, contribution, source-inclusion, assertion, performance-history, or background information.
- [ ] All migrated documentation is reviewed against the current repository and public API, accurately describes the .NET 10 and .NET Standard 2.0/2.1 support matrix, uses the current repository layout and build entry points, and contains no stale version, SDK, solution, branch, or project-path guidance.
- [ ] The assertion overview covers the current assertion families and framework-specific overloads, including the ImmutableArray assertions introduced after the last Wiki update, and uses the source files and XML documentation as its authority.
- [ ] The source-code inclusion guide documents the committed .NET Standard 2.0 single-file distribution and the custom source-export tool, including configuration precedence, target-framework selection, conditional-code flattening, assertion whitelisting, exception-factory overload controls, dependency retention, generated-source validation, and failure behavior.
- [ ] The performance page is explicitly presented as a historical 2018 benchmark snapshot, does not imply that its results describe the current release or runtime, and points readers to the current benchmark project for reproducible measurements.
- [ ] `README.md`, `CONTRIBUTING.md`, badges, documentation navigation, and all other repository-controlled references point to the repository-hosted documentation using relative links where possible; no active repository content links to the Wiki or to obsolete `master`-branch paths.
- [ ] All relative documentation links, headings, code fences, tables, and image references render correctly on GitHub, including links followed from the repository root and from every page under `docs`.
- [ ] Each legacy Wiki page temporarily contains only a moved notice with its exact replacement link, then all Wiki pages are removed and the repository Wiki feature is disabled after the agreed transition period.
- [ ] The final documentation and repository contain no duplicate source of truth: the root README remains the concise project entry point, `CONTRIBUTING.md` remains the contributor entry point, and detailed material is maintained under `docs`.

## Technical Details

Use the following migration map. Prefer lowercase, descriptive filenames and relative Markdown links so navigation also works in forks and non-default branches.

| Wiki page | Repository destination | Editorial treatment |
| --- | --- | --- |
| `Home` | `docs/README.md` | Provide a concise documentation index; link to the root README for the project introduction and installation rather than copying it. |
| `How-to-Contribute-and-Build` | `docs/contributing-and-building.md` | Reconcile with `CONTRIBUTING.md`, the root solution files, the current directory layout, the pinned SDK, and current test/benchmark expectations. |
| `How-to-Structure-Your-Precondition-Checks` | `docs/structuring-precondition-checks.md` | Retain the usage guidance and examples, correct syntax and terminology, and describe current caller-argument-expression behavior. |
| `Including-Light.GuardClauses-as-source-code` | `docs/source-code-inclusion.md` | Rewrite against the current source-export implementation and configuration. |
| `Overview-of-All-Assertions` | `docs/assertion-overview.md` | Reconcile categories and names with the active `Check.*.cs` files under both supported source shapes, including ImmutableArray and modern-only APIs. |
| `Performance-Overview-of-All-Assertions` | `docs/historical-performance.md` | Preserve the benchmark data as an explicitly dated archive and link to `benchmarks/Light.GuardClauses.Performance`. |
| `Trivia` | `docs/guard-clause-background.md` | Retain the design-by-contract background and use a relative reference to the tracked Eiffel image. |

Treat the current code, project files, XML comments, committed `settings.json`, and tests as authoritative; use the Wiki only as migration input. The README is newer than most Wiki pages but must also be corrected where it still uses Wiki links, explicit `master` URLs, or inconsistent package versions. Avoid repeating volatile release numbers in prose when a versionless installation example is sufficient.

The source-code inclusion guide should distinguish the ready-made `Light.GuardClauses.SingleFile.cs` from custom generation with `tools/source-export/Light.GuardClauses.SourceCodeTransformation`. Document `settings.json`, optional ignored `settings.local.json`, and command-line overrides in their actual precedence order, using a minimal configuration example and linking to the committed settings file for the complete assertion catalog instead of copying that large catalog into the guide.

Describe `TargetFramework` using its current values, `NetStandard2_0` and `Net10_0`. Generated output is flattened to exactly the selected source shape and contains no conditional-compilation directives; .NET Standard is the default committed distribution, while .NET 10 includes the modern generic-math, span, and memory APIs and suppresses framework-provided attribute polyfills. Explain that generation validates the result through `Light.GuardClauses.SourceValidation` for the matching target, returns a non-zero exit code on build failure, and leaves the generated file available for diagnosis.

When `AssertionWhitelist.IsEnabled` is false, entry settings have no effect and the full selected-framework surface is exported. When enabled, included assertions are reachability roots and required `Check`, `Throw`, exception, attribute, and helper dependencies are retained while unrelated API can be omitted. Document both the global `RemoveOverloadsWithExceptionFactory` switch and each entry's `IncludeExceptionFactoryOverload`, as well as the fail-loud behavior when a `Check.<Name>.cs` file has no corresponding whitelist entry. Include one compact command-line or local-settings example that selects a target framework and a small assertion subset.

GitHub's Wiki setting only hides content and restores it if re-enabled; it does not erase history or provide cross-location redirects. Consequently, permanent compatibility for old Wiki URLs conflicts with the final requirement to disable the Wiki. Use a two-stage operational migration: merge and publish the repository docs, replace all seven Wiki pages with destination-specific moved notices for one release cycle, then remove those pages and disable Wikis under repository settings. Links outside the maintainers' control will stop resolving after that final step; record this limitation in the migration pull request and update all known links under project control before starting the transition.

No automated product tests or benchmarks are required because this change does not alter runtime behavior. Validate the migration with a repository-wide stale-link search and a Markdown link/rendering check that covers anchors, relative paths, and case-sensitive filenames.
