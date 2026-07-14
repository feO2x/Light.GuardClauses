# Root AGENTS.md

Light.GuardClauses is a lightweight library for writing GuardClauses in C#/.NET

## Implementation rules

Plans typically have acceptance criteria with check boxes. Check each box when you are finished with the corresponding criterion.

## Plan Rules

Read ./ai-plans/AGENTS.md for details on how to write plans.

## Here is Your Space

If you encounter something worth noting while you are working on this code base, write it down here in this section. Once you are finished, I will discuss it with you, and we can decide where to put your notes.

- The source exporter used to merge `Check.*.cs` files in `DirectoryInfo.GetFiles` order, which is filesystem-dependent and unordered on APFS, so adding source files could reshuffle the entire generated single file. `SourceFileMerger` now sorts the file list with `OrderBy(f => f.Name, StringComparer.Ordinal)`, making the output deterministic. The regeneration that shipped with the sign guards contains the resulting one-time reshuffle; future diffs will only show actual content changes.
