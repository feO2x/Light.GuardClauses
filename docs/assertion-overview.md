# Assertion overview

[Documentation index](README.md) · [Contributing and building](contributing-and-building.md) · [Structuring checks](structuring-precondition-checks.md) · [Source inclusion](source-code-inclusion.md) · **Assertions** · [Historical performance](historical-performance.md) · [Background](guard-clause-background.md)

This page groups the current public assertion families defined by the `Check.*.cs` files. Exact overloads, generic constraints, return annotations, and exception contracts are documented in the source XML comments and appear in IntelliSense.

Names beginning with `Must` validate or throw and return the successfully validated value. `InvalidArgument`, `InvalidOperation`, `InvalidState`, and `ObjectDisposed` are throwing condition checks. Other names return `bool`. Most throwing families offer a default-exception overload and an exception-factory overload; see [Structuring precondition checks](structuring-precondition-checks.md).

## Target-specific API

The package has .NET Standard 2.0, .NET Standard 2.1, and .NET 10 assets. The common surface includes string, collection, `ImmutableArray<T>`, and many span/memory guards.

The .NET 10 asset additionally provides:

- generic `INumber<T>` overloads for `IsApproximately`, `MustBeApproximately`, `MustNotBeApproximately`, `IsGreaterThanOrApproximately`, `MustBeGreaterThanOrApproximately`, `IsLessThanOrApproximately`, and `MustBeLessThanOrApproximately`;
- generic `INumber<T>` overloads for `MustBePositive`, `MustBeNegative`, `MustNotBePositive`, `MustNotBeNegative`, and `MustNotBeZero`, covering numeric types without concrete overloads (such as `short`, `byte`, or `Half`);
- generic `IFloatingPointIeee754<T>` overloads for `IsFinite` and `MustBeFinite`, including `Half` but excluding `decimal`;
- `Span<char>`, `ReadOnlySpan<char>`, `Memory<char>`, and `ReadOnlyMemory<char>` overloads for `IsEmailAddress` and `MustBeEmailAddress`; and
- trimming annotations on the type-relation helpers where supported by the framework.

The seven `ImmutableArray<T>` guard families are available across all package targets:

| Assertion | Behavior |
| --- | --- |
| `MustNotBeDefaultOrEmpty` | Rejects a default or empty immutable array |
| `MustContain`, `MustNotContain` | Require or reject an item |
| `MustHaveLength` | Requires an exact length |
| `MustHaveLengthIn` | Requires a length within a `Range<int>` |
| `MustHaveMinimumLength`, `MustHaveMaximumLength` | Enforce inclusive lower or upper length bounds |

## Null, default, identity, type, and value assertions

| Assertion | Behavior |
| --- | --- |
| `MustNotBeNull` | Rejects a null reference and returns a non-null value |
| `MustNotBeNullReference` | Rejects null only when generic `T` is a reference type; intended for generic contexts |
| `MustNotBeDefault` | Rejects `default(T)` |
| `MustHaveValue` | Requires a nullable value and returns the underlying value |
| `IsSameAs`, `MustNotBeSameAs` | Test or reject reference identity |
| `MustBe`, `MustNotBe` | Require equality or inequality; overloads support equality comparers and string comparison rules |
| `MustBeOfType` | Requires a value castable to `T` and returns the cast value |
| `IsValidEnumValue`, `MustBeValidEnumValue` | Validate defined enum constants and valid flags combinations |
| `IsEmpty`, `MustNotBeEmpty` | Test or reject `Guid.Empty` |
| `IsUuidVersion7`, `MustBeUuidVersion7` | Structurally test or require an RFC/IETF UUIDv7 |

UUIDv7 validation checks the version-7 nibble and RFC/IETF `10xx` variant bits directly in the GUID layout without allocation. It does not claim to validate timestamp provenance, entropy, randomness, or monotonic generation.

## Condition and state assertions

| Assertion | Behavior |
| --- | --- |
| `InvalidArgument` | Throws `ArgumentException` when the condition is true |
| `InvalidOperation` | Throws `InvalidOperationException` when the condition is true |
| `InvalidState` | Throws `InvalidStateException` when the condition is true |
| `ObjectDisposed` | Throws `ObjectDisposedException` when the condition is true |

## Comparable, range, and approximate assertions

| Assertion family | Behavior |
| --- | --- |
| `MustBeGreaterThan`, `MustBeGreaterThanOrEqualTo` | Require the value to be above a boundary |
| `MustBeLessThan`, `MustBeLessThanOrEqualTo` | Require the value to be below a boundary |
| `MustNotBeGreaterThan`, `MustNotBeGreaterThanOrEqualTo` | Reject values above or at an upper boundary |
| `MustNotBeLessThan`, `MustNotBeLessThanOrEqualTo` | Reject values below or at a lower boundary |
| `MustBePositive`, `MustBeNegative` | Require a value greater than, or less than, zero |
| `MustNotBePositive`, `MustNotBeNegative` | Require a value less than or equal to, or greater than or equal to, zero |
| `MustNotBeZero` | Rejects a value that compares equal to zero |
| `IsIn`, `MustBeIn` | Test or require membership in a `Range<T>` |
| `IsNotIn`, `MustNotBeIn` | Test or require non-membership in a `Range<T>` |
| `IsApproximately`, `MustBeApproximately`, `MustNotBeApproximately` | Compare floating-point values using a tolerance |
| `IsFinite`, `MustBeFinite` | Test or require finite `float` and `double` values; the .NET 10 asset also supports generic IEEE 754 types |
| `IsGreaterThanOrApproximately`, `MustBeGreaterThanOrApproximately` | Accept values greater than or within tolerance of the comparison value |
| `IsLessThanOrApproximately`, `MustBeLessThanOrApproximately` | Accept values less than or within tolerance of the comparison value |

The five sign guard families have concrete overloads for `int`, `long`, `decimal`, `float`, `double`, and `TimeSpan` on all package targets; the .NET 10 asset adds the generic `INumber<T>` overloads listed above. All checks compare the value against zero with the type's comparison operators. Consequently, `NaN` is rejected by the four sign guards and accepted by `MustNotBeZero`, positive and negative infinity satisfy the guards matching their sign (compose with `MustBeFinite` to reject non-finite values), and negative zero — including `decimal`'s signed zero representations — behaves exactly like zero. `MustNotBeZero` uses exact equality; tolerance-based comparisons remain the domain of the approximation guards.

Create ranges with the `Range<T>` fluent API:

```csharp
starRating.MustBeIn(Range.InclusiveBetween(1, 5));
percentage.MustBeIn(Range.FromExclusive(0).ToInclusive(100));
```

## Collections and membership

| Assertion family | Applies to and behavior |
| --- | --- |
| `IsNullOrEmpty` | Tests `IEnumerable` or `string` for null or emptiness |
| `MustNotBeNullOrEmpty` | Requires a non-null, non-empty collection or string |
| `MustHaveCount` | Requires an exact collection count |
| `MustHaveCountIn` | Requires a collection count within an inclusive/exclusive `Range<int>` |
| `MustHaveSameCountAs` | Requires two reference collections implementing `IEnumerable` to have equal counts, even when their concrete and element types differ |
| `MustHaveMinimumCount`, `MustHaveMaximumCount` | Enforce inclusive collection-count bounds |
| `IsOneOf`, `MustBeOneOf`, `MustNotBeOneOf` | Test or enforce membership among supplied values |
| `MustContain`, `MustNotContain` | Require or reject an item in collections and immutable arrays; string overloads operate on substrings |
| `MustNotContainNull` | Reject null items in reference collections implementing non-generic `IEnumerable`, with a dedicated `ImmutableArray<T>` overload |
| `MustNotContainNullOrWhiteSpace` | Reject null, empty, or Unicode-whitespace-only strings in reference collections implementing `IEnumerable<string?>`, with a dedicated immutable-array overload |
| `MustContainKey`, `MustNotContainKey` | Require or reject a dictionary key via `ContainsKey`, never enumerating and honoring the dictionary's key comparer; accept `IReadOnlyDictionary<TKey, TValue>` receivers, with dedicated overloads preserving the `Dictionary<TKey, TValue>` shape |
| `MustNotBeDefaultOrEmpty` | Requires an initialized, non-empty `ImmutableArray<T>` |
| `MustHaveLength`, `MustHaveLengthIn`, `MustHaveMinimumLength`, `MustHaveMaximumLength` | Validate string, span, or immutable-array length as provided by their overloads |

Collection overloads preserve and return the original collection shape where possible. Check the XML documentation before using an `IEnumerable` guard in a hot path; annotations identify guards that do not enumerate.

`MustHaveSameCountAs` validates both collection references before observing either count, returns the original receiver, and compares counts without comparing contents. Null inputs throw `ArgumentNullException`, while differing counts throw `InvalidCollectionCountException`. Existing count-property fast paths avoid enumeration; otherwise, each input is enumerated at most once and its enumerator is disposed. Comparing a collection with itself returns immediately without enumeration.

The two collection-content guards treat empty collections and empty or default immutable arrays as valid. Arrays, lists, and other supported indexable receivers are inspected by index without allocating an enumerator; arbitrary enumerable receivers are enumerated at most once and dispose that enumerator normally. Both paths stop at the first invalid item, use O(n) time, and require constant additional space. `MustNotContainNull` uses non-generic item access for reference collections so it can preserve any receiver shape without knowing the item type; consequently, value-type items are boxed by that overload. Its dedicated immutable-array overload uses generic indexed access and avoids that cost.

The dictionary key guards bind to any dictionary type implementing `IReadOnlyDictionary<TKey, TValue>` (such as `ConcurrentDictionary`, `SortedDictionary`, `ReadOnlyDictionary`, `ImmutableDictionary`, and `FrozenDictionary` on .NET 10). A receiver statically typed as `IDictionary<TKey, TValue>` cannot use them; call `dictionary.Keys.MustContain(key)` as a workaround — the key collections of the BCL dictionaries implement `ICollection<TKey>.Contains` via `ContainsKey`, so this stays O(1).

## Text, character, span, and memory assertions

| Assertion family | Behavior |
| --- | --- |
| `IsNullOrWhiteSpace`, `MustNotBeNullOrWhiteSpace` | Test or reject null, empty, or all-whitespace strings |
| `IsEmptyOrWhiteSpace`, `MustNotBeEmptyOrWhiteSpace` | Test character span/memory values for, or reject, emptiness and all-whitespace content |
| `IsAscii`, `MustBeAscii` | Test or require ASCII characters, bytes, strings, and character/byte span or memory values; empty inputs are valid |
| `ContainsOnlyDigits`, `MustContainOnlyDigits` | Test or require Unicode decimal-digit content |
| `ContainsOnlyLettersOrDigits`, `MustContainOnlyLettersOrDigits` | Test or require Unicode letter-or-decimal-digit content |
| `IsUpperCase`, `MustBeUpperCase` | Test or require the absence of Unicode lowercase characters |
| `IsLowerCase`, `MustBeLowerCase` | Test or require the absence of Unicode uppercase characters |
| `MustNotContainWhiteSpace` | Reject any Unicode whitespace character |
| `IsBase64`, `MustBeBase64` | Test or require structurally valid standard Base64 without decoding |
| `IsHexadecimal`, `MustBeHexadecimal` | Test or require ASCII hexadecimal characters (`0`-`9`, `A`-`F`, and `a`-`f`) |
| `IsWhiteSpace`, `IsLetter`, `IsLetterOrDigit`, `IsDigit` | Character classification |
| `IsNewLine`, `MustBeNewLine` | Recognize or require `"\n"` or `"\r\n"`, independently of the platform newline |
| `IsTrimmed`, `MustBeTrimmed` | Test or require no leading or trailing whitespace |
| `IsTrimmedAtStart`, `MustBeTrimmedAtStart` | Test or require no leading whitespace |
| `IsTrimmedAtEnd`, `MustBeTrimmedAtEnd` | Test or require no trailing whitespace |
| `IsFileExtension`, `MustBeFileExtension` | Test or require a file extension; string and span/memory overloads are available |
| `IsEmailAddress`, `MustBeEmailAddress` | Test or require the default or a supplied email regex; span/memory overloads are .NET 10-only |
| `MustMatch` | Requires a string to match a regular expression |
| `Equals` | Compares strings using `StringComparisonType`, including ignore-whitespace modes |
| `IsSubstringOf`, `MustBeSubstringOf`, `MustNotBeSubstringOf` | Test, require, or reject substring relationships |
| `MustContain`, `MustNotContain` | Require or reject substrings, with comparison overloads |
| `MustStartWith`, `MustNotStartWith`, `MustEndWith`, `MustNotEndWith` | Enforce string boundaries; `MustNotStartWith` also has span overloads |
| `MustHaveLength`, `MustHaveLengthIn` | Enforce exact or ranged lengths for strings, spans, memory, and immutable arrays as provided by their overloads |
| `MustNotBeEmpty` | Reject empty spans and memory while preserving the mutable or read-only input shape |
| `MustBeLongerThan`, `MustBeLongerThanOrEqualTo` | Enforce lower string/span length bounds |
| `MustBeShorterThan`, `MustBeShorterThanOrEqualTo` | Enforce upper string/span length bounds |

The `Light.GuardClauses.FrameworkExtensions.StringExtensions.Contains` companion method supplies `string.Contains(string, StringComparison)` without colliding with the `Check` assertion namespace.

The string-inspection predicates and guards support `string`, `Span<char>`, `ReadOnlySpan<char>`, `Memory<char>`, and `ReadOnlyMemory<char>`. A null string fails predicates and causes default guards to throw `ArgumentNullException`; empty inputs satisfy every inspection. Digit, letter-or-digit, casing, and whitespace checks use .NET's invariant Unicode character classification. “Upper case” means no lowercase character is present, while “lower case” means no uppercase character is present, so uncased letters, digits, punctuation, symbols, whitespace, and empty inputs can satisfy both. Hexadecimal deliberately accepts only ASCII characters and imposes no length, prefix, or numeric-range rule.

Base64 inspection validates the standard `A`-`Z`, `a`-`z`, `0`-`9`, `+`, and `/` alphabet and legal quartet/padding structure. It ignores only space, tab, carriage return, and line feed anywhere in the input; Base64Url-only characters are invalid. No inspection allocates, copies, normalizes, changes case, decodes, invokes a regular expression, or uses LINQ. All scans stop as soon as failure is known and otherwise use O(n) time with constant additional space. .NET 10 delegates Base64 validation to the framework's optimized `Base64.IsValid`; portable targets use the equivalent allocation-free scalar validator. Other Unicode-sensitive scans stay single-pass scalar implementations on every target so their observable classification remains identical.

## Date and time assertions

| Assertion | Behavior |
| --- | --- |
| `MustBeUtc` | Requires `DateTimeKind.Utc` for `DateTime`, or `TimeSpan.Zero` offset for `DateTimeOffset`; values are validated without conversion |
| `MustBeLocal` | Requires `DateTimeKind.Local` |
| `MustBeUnspecified` | Requires `DateTimeKind.Unspecified` |

## Stream assertions

| Assertion | Behavior |
| --- | --- |
| `MustBeReadable` | Requires a non-null stream whose `CanRead` property is `true` |
| `MustBeWritable` | Requires a non-null stream whose `CanWrite` property is `true` |
| `MustBeSeekable` | Requires a non-null stream whose `CanSeek` property is `true` |

Each stream guard reads only its matching capability property: it performs no I/O, does not inspect the other
capabilities, and does not change the stream position or any other state. A null stream throws
`ArgumentNullException`; a missing capability throws `ArgumentException`. Successful guards return the same instance
while preserving its concrete stream type, and all three guards support custom messages and exception factories.

## Type-relation assertions

| Assertion | Behavior |
| --- | --- |
| `MustBeAssignableTo` | Requires CLR assignability from a candidate type to a required base type or interface |
| `IsEquivalentTypeTo` | Treats equal types and constructed-generic/definition pairs as equivalent |
| `Implements`, `IsOrImplements` | Test interface implementation, optionally allowing equality |
| `DerivesFrom`, `IsOrDerivesFrom` | Test base-class derivation, optionally allowing equality |
| `InheritsFrom`, `IsOrInheritsFrom` | Test derivation or interface implementation, optionally allowing equality |
| `IsOpenConstructedGenericType` | Tests for a constructed generic type that still has open parameters |

`MustBeAssignableTo` uses `requiredType.IsAssignableFrom(candidateType)` directly, so it covers interface
assignability, generic variance, and the BCL's open-generic behavior. The other relation methods provide comparer
overloads where applicable.

## URI assertions

| Assertion | Behavior |
| --- | --- |
| `MustBeUri` | Require a string parsable as a relative, absolute, or either kind of URI |
| `MustBeAbsoluteUri`, `MustBeRelativeUri` | Require an absolute or relative URI |
| `MustHaveScheme`, `MustHaveOneSchemeOf` | Require one specific scheme or one of several schemes |
| `MustBeHttpUrl`, `MustBeHttpsUrl`, `MustBeHttpOrHttpsUrl` | Require an absolute HTTP/HTTPS URI with the named allowed scheme |
