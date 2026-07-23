# Root AGENTS.md

Light.GuardClauses is a lightweight library for writing GuardClauses in C#/.NET

## Implementation rules

Plans typically have acceptance criteria with check boxes. Check each box when you are finished with the corresponding criterion.

## Plan Rules

Read ./ai-plans/AGENTS.md for details on how to write plans.

## Test Rules

Read ./tests/AGENTS.md for details on how to write tests.

## Here is Your Space

If you encounter something worth noting while you are working on this code base, write it down here in this section. Once you are finished, I will discuss it with you, and we can decide where to put your notes.

- Plan 0168 refers to `ArgumentOutOfRangeException.ActualValue`, but the existing generic
  `Throw.MustBePositive<T>` helper uses the two-argument constructor and reports the actual value in the generated
  message rather than populating the `ActualValue` property.
