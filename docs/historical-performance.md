# Historical performance snapshot (August 2018)

[Documentation index](README.md) · [Contributing and building](contributing-and-building.md) · [Structuring checks](structuring-precondition-checks.md) · [Source inclusion](source-code-inclusion.md) · [Assertions](assertion-overview.md) · **Historical performance** · [Background](guard-clause-background.md)

> **Archive notice:** These results were measured in early August 2018 against Light.GuardClauses 5.0 and 3.5, BenchmarkDotNet 0.11.0, .NET Core 2.1.2, and .NET Framework 4.7.2. They are preserved as project history. They do not describe the current release, .NET 10, current JITs, or current hardware.

For reproducible measurements of the current code, run the [current BenchmarkDotNet project](../benchmarks/Light.GuardClauses.Performance/). It targets .NET 10; benchmark source and configuration, rather than this archive, are the authority for current scenarios.

The archived tables compare:

- an imperative `if` implementation;
- Light.GuardClauses 5.0 with a custom parameter name;
- Light.GuardClauses 5.0 with a custom exception factory; and
- Light.GuardClauses 3.5, the earlier unoptimized release.

Only successful checks were measured. A reported `0.0000 ns` means the operation was below the benchmark's useful measurement resolution, not that it had literally zero cost.

The measurements ran on two Windows 10 systems:

- Asus Zephyrus M GM501GS, Intel Core i7-8750H, .NET Core SDK 2.1.302, .NET Core 2.1.2, and .NET Framework 4.7.2.
- Microsoft Surface Pro 4, Intel Core i5-6300U, .NET Core SDK 2.1.302, .NET Core 2.1.2, and .NET Framework 4.7.2.

The Wiki page was intentionally incomplete; the following is its complete recorded result set.

## Common assertions

### MustNotBeNull

Ensures that the specified object reference is not null, or otherwise throws an `ArgumentNullException`.

| Runtime | Ran On        | Imperative | v5.0 Custom Parameter | v5.0 Custom Exception | v3.5 Custom Parameter |
| ------  | -----         | ---------: | --------------------: | --------------------: | --------------------: |
| Clr     | Asus Zephyrus | 0.0000ns   | 0.0000ns              | 0.4586ns              | 7.1406ns              |
| Clr     | Surface Pro 4 | 0.0000ns   | 0.0000ns              | 0.6062ns              | 9.6970ns              |
| Core    | Asus Zephyrus | 0.3017ns   | 0.0000ns              | 0.9839ns              | 7.7788ns              |
| Core    | Surface Pro 4 | 0.3524ns   | 0.0000ns              | 1.3231ns              | 10.5766ns             |

### MustNotBeDefault

Ensures that the specified parameter is not the default value, or otherwise throws an `ArgumentNullException` for reference types, or an `ArgumentDefaultException` for value types.

#### Reference types

| Runtime | Ran On        | Imperative | v5.0 Custom Parameter | v5.0 Custom Exception | v3.5 Custom Parameter |
| ------  | -----         | ---------: | --------------------: | --------------------: | --------------------: |
| Clr     | Asus Zephyrus | 0.0000ns   | 0.0000ns              | 0.1428ns              | 9.7358ns              |
| Clr     | Surface Pro 4 | 0.0000ns   | 0.0000ns              | 0.1967ns              | 12.6108ns             |
| Core    | Asus Zephyrus | 0.3271ns   | 0.3564ns              | 1.3281ns              | 10.8932ns             |
| Core    | Surface Pro 4 | 0.3787ns   | 0.4960ns              | 1.7124ns              | 14.0789ns             |

#### Value types

| Runtime | Ran On        | Imperative | v5.0 Custom Parameter | v5.0 Custom Exception | v3.5 Custom Parameter |
| ------  | -----         | ---------: | --------------------: | --------------------: | --------------------: |
| Clr     | Asus Zephyrus | 0.0668ns   | 5.2399ns              | 5.9860ns              | 12.3157ns (24B)       |
| Clr     | Surface Pro 4 | 0.0769ns   | 6.8190ns              | 7.3288ns              | 16.3819ns (24B)       |
| Core    | Asus Zephyrus | 0.0311ns   | 1.1403ns              | 2.3104ns              | 12.8575ns (24B)       |
| Core    | Surface Pro 4 | 0.0330ns   | 1.5056ns              | 2.9813ns              | 17.2990ns (24B)       |

Please note that the performance values for Value Types are only true for version 5.0.1 and above.

### MustNotBeNullReference

Ensures that the specified parameter is not null when `T` is a reference type, or otherwise throws an `ArgumentNullException`. PLEASE NOTICE: you should only use this assertion in generic contexts, use `MustNotBeNull` by default.

**Performance result for reference types:**

| Runtime | Ran On        | Imperative | v5.0 Custom Parameter | v5.0 Custom Exception | v3.5 Custom Parameter |
| ------  | -----         | ---------: | --------------------: | --------------------: | --------------------: |
| Clr     | Asus Zephyrus | 0.0019ns   | 0.3549ns              | 0.8020ns              | N/A                   |
| Clr     | Surface Pro 4 | 0.0069ns   | 0.4004ns              | N/A                   | N/A                   |
| Core    | Asus Zephyrus | 0.0560ns   | 0.0000ns              | 0.7922ns              | N/A                   |
| Core    | Surface Pro 4 | 0.0949ns   | 0.0000ns              | N/A                   | N/A                   |

**Performance result for value types:**

| Runtime | Ran On        | Imperative | v5.0 Custom Parameter | v5.0 Custom Exception | v3.5 Custom Parameter |
| ------  | -----         | ---------: | --------------------: | --------------------: | --------------------: |
| Clr     | Asus Zephyrus | 0.0005ns   | 0.0268ns              | 0.7581ns              | N/A                   |
| Clr     | Surface Pro 4 | 0.0241ns   | 0.4004ns              | N/A                   | N/A                   |
| Core    | Asus Zephyrus | 0.0286s    | 0.0000ns              | 1.4136ns              | N/A                   |
| Core    | Surface Pro 4 | 0.0021s    | 0.0000ns              | N/A                   | N/A                   |

### MustBeOfType

Ensures that `parameter` can be casted to `T` and returns the casted value, or otherwise throws a `TypeCastException`.

| Runtime | Ran On        | Imperative | v5.0 Custom Parameter | v5.0 Custom Exception | v3.5 Custom Parameter |
| ------  | -----         | ---------: | --------------------: | --------------------: | --------------------: |
| Clr     | Asus Zephyrus | 0.0000ns   | 0.4593ns              | 0.8822ns              | 14.0487ns             |
| Clr     | Surface Pro 4 | 0.0001ns   | 0.6893ns              | 1.2326ns              | 19.1403ns             |
| Core    | Asus Zephyrus | 0.6249ns   | 1.0564ns              | 2.0830ns              | 12.4618ns             |
| Core    | Surface Pro 4 | 0.6637ns   | 1.3386ns              | 2.8203ns              | 16.9133ns             |

### IsValidEnumValue

Checks if the specified value is a valid enum value of its type. This is true when the specified value is one of the constants defined in the enum, or a valid flags combination when the enum type is marked with the `FlagsAttribute`.

See `MustBeValidEnumValue` for performance results.

### MustBeValidEnumValue

Ensures that the specified enum value is valid, or otherwise throws an `EnumValueNotDefinedException`. An enum value is valid when the specified value is one of the constants defined in the enum, or a valid flags combination when the enum type is marked with the `FlagsAttribute`.

**Performance results for enum without `FlagsAttribute`**

| Runtime | Ran On        | Imperative | v5.0 Custom Parameter | v5.0 Custom Exception | v3.5 Custom Parameter |
| ------  | -----         | ---------: | --------------------: | --------------------: | --------------------: |
| Clr     | Asus Zephyrus | 204.03ns   | 39.17ns               | 40.74ns               | 56.45ns               |
| Clr     | Surface Pro 4 | 281.12ns   | 58.79ns               | 55.84ns               | 69.10ns               |
| Core    | Asus Zephyrus | 136.55ns   | 37.30ns               | 40.59ns               | 47.20ns               |
| Core    | Surface Pro 4 | 183.99ns   | 50.77ns               | 55.69ns               | 64.74ns               |

**Performance results for enum with `FlagsAttribute`**

| Runtime | Ran On        | Imperative | v5.0 Custom Parameter | v5.0 Custom Exception | v3.5 Custom Parameter |
| ------  | -----         | ---------: | --------------------: | --------------------: | --------------------: |
| Clr     | Asus Zephyrus | N/A        | 50.39ns               | 54.37ns               | N/A                   |
| Clr     | Surface Pro 4 | N/A        | 73.94ns               | 75.62ns               | N/A                   |
| Core    | Asus Zephyrus | N/A        | 51.76ns               | 58.69ns               | N/A                   |
| Core    | Surface Pro 4 | N/A        | 86.31ns               | 83.32ns               | N/A                   |

### MustNotBeEmpty (for GUIDs)

Ensures that the specified GUID is not empty, or otherwise throws an `EmptyGuidException`.

| Runtime | Ran On        | Imperative | v5.0 Custom Parameter | v5.0 Custom Exception | v3.5 Custom Parameter |
| ------  | -----         | ---------: | --------------------: | --------------------: | --------------------: |
| Clr     | Asus Zephyrus | 1.801ns    | 1.945ns               | 2.912ns               | 10.903ns              |
| Clr     | Surface Pro 4 | 2.192ns    | 2.218ns               | 3.815ns               | 14.877ns              |
| Core    | Asus Zephyrus | 2.453ns    | 2.647ns               | 4.396ns               | 11.258ns              |
| Core    | Surface Pro 4 | 3.375ns    | 3.606ns               | 6.097ns               | 15.360ns              |

### InvalidOperation

Checks if the specified `condition` is true and throws an `InvalidOperationException` in this case.

| Runtime | Ran On        | Imperative | v5.0 Custom Parameter | v5.0 Custom Exception | v3.5 Custom Parameter |
| ------  | -----         | ---------: | --------------------: | --------------------: | --------------------: |
| Clr     | Asus Zephyrus | 0.2458ns   | 0.0000ns              | N/A                   | N/A                   |
| Clr     | Surface Pro 4 | 0.3338ns   | 0.0000ns              | N/A                   | N/A                   |
| Core    | Asus Zephyrus | 0.0952ns   | 0.0555ns              | N/A                   | N/A                   |
| Core    | Surface Pro 4 | 0.1076ns   | 0.0728ns              | N/A                   | N/A                   |

### InvalidState

Checks if the specified `condition` is true and throws an `InvalidStateException` in this case.

| Runtime | Ran On        | Imperative | v5.0 Custom Parameter | v5.0 Custom Exception | v3.5 Custom Parameter |
| ------  | -----         | ---------: | --------------------: | --------------------: | --------------------: |
| Clr     | Asus Zephyrus | 0.2438ns   | 0.0000ns              | N/A                   | N/A                   |
| Clr     | Surface Pro 4 | 0.3283ns   | 0.0000ns              | N/A                   | N/A                   |
| Core    | Asus Zephyrus | 0.0961ns   | 0.0574ns              | N/A                   | N/A                   |
| Core    | Surface Pro 4 | 0.1021ns   | 0.0650ns              | N/A                   | N/A                   |

### MustHaveValue (for `Nullable<T>`)

Ensures that the specified nullable has a value and returns it, or otherwise throws a `NullableHasNoValueException`.

| Runtime | Ran On        | Imperative | v5.0 Custom Parameter | v5.0 Custom Exception | v3.5 Custom Parameter |
| ------  | -----         | ---------: | --------------------: | --------------------: | --------------------: |
| Clr     | Asus Zephyrus | 0.1945ns   | 0.2448ns              | 0.9804ns              | 1.9334ns              |
| Clr     | Surface Pro 4 | 0.2363ns   | 0.3151ns              | 1.3194ns              | 2.6079ns              |
| Core    | Asus Zephyrus | 0.2005ns   | 0.1791ns              | 1.4512ns              | 1.7156ns              |
| Core    | Surface Pro 4 | 0.2439ns   | 0.2222ns              | 1.9665ns              | 2.2955ns              |

### IsSameAs

Checks if `parameter` and `other` point to the same object.

| Runtime | Ran On        | Imperative | v5.0     |
| ------  | -----         | ---------: | -------: |
| Clr     | Asus Zephyrus | 0.0000ns   | 0.2446ns |
| Clr     | Surface Pro 4 | 0.0000ns   | 0.3364ns |
| Core    | Asus Zephyrus | 0.0000ns   | 0.0000ns |
| Core    | Surface Pro 4 | 0.0000ns   | 0.0000ns |

### MustNotBeSameAs

Ensures that `parameter` and `other` do not point to the same object instance, or otherwise throws a `SameObjectReferenceException`.

| Runtime | Ran On        | Imperative | v5.0 Custom Parameter | v5.0 Custom Exception | v3.5 Custom Parameter |
| ------  | -----         | ---------: | --------------------: | --------------------: | --------------------: |
| Clr     | Asus Zephyrus | 0.0000ns   | 0.0000ns              | 0.0000ns              | 7.5836ns              |
| Clr     | Surface Pro 4 | 0.0000ns   | 0.0000ns              | 0.0013ns              | 9.8948ns              |
| Core    | Asus Zephyrus | 0.5529ns   | 0.3086ns              | 1.6367ns              | 8.1591ns              |
| Core    | Surface Pro 4 | 0.7221ns   | 0.4062ns              | 2.1138ns              | 10.6896ns             |

### MustBe

Ensures that `parameter` is equal to `other`, or otherwise throws a `ValuesNotEqualException`.

Performance Benchmark TBD.

### MustNotBe

Ensures that `parameter` is not equal to `other`, or otherwise throws a `ValuesEqualException`.

Performance Benchmark TBD.

## Comparable assertions

### MustNotBeLessThan

Ensures that the specified `parameter` is not less than the given `other` value, or otherwise throws an `ArgumentOutOfRangeException`.

| Runtime | Ran On        | Imperative | v5.0 Custom Parameter | v5.0 Custom Exception | v3.5 Custom Parameter |
| ------  | -----         | ---------: | --------------------: | --------------------: | --------------------: |
| Clr     | Asus Zephyrus | 0.1251ns   | 0.7352ns              | 1.0434ns              | 9.7321ns              |
| Clr     | Surface Pro 4 | 0.1754ns   | 1.0086ns              | 1.4326ns              | 13.2368ns             |
| Core    | Asus Zephyrus | 0.0744ns   | 0.4908ns              | 1.9751ns              | 9.7202ns              |
| Core    | Surface Pro 4 | 0.0949ns   | 0.6688ns              | 2.7045ns              | 13.2217ns             |

### MustBeGreaterThanOrEqualTo

Ensures that the specified `parameter` is not less than the given `other` value, or otherwise throws an `ArgumentOutOfRangeException`.

| Runtime | Ran On        | Imperative | v5.0 Custom Parameter | v5.0 Custom Exception | v3.5 Custom Parameter |
| ------  | -----         | ---------: | --------------------: | --------------------: | --------------------: |
| Clr     | Asus Zephyrus | 0.1273ns   | 0.7373ns              | 1.1719ns              | 9.7338ns              |
| Clr     | Surface Pro 4 | 0.1648ns   | 1.0135ns              | 1.6116ns              | 13.3089ns             |
| Core    | Asus Zephyrus | 0.0731ns   | 0.4858ns              | 1.9060ns              | 9.7241ns              |
| Core    | Surface Pro 4 | 0.0949ns   | 0.6731ns              | 2.5526ns              | 13.2522ns             |

### MustBeLessThan

Ensures that the specified `parameter` is less than the given `other` value, or otherwise throws an `ArgumentOutOfRangeException`.

| Runtime | Ran On        | Imperative | v5.0 Custom Parameter | v5.0 Custom Exception | v3.5 Custom Parameter |
| ------  | -----         | ---------: | --------------------: | --------------------: | --------------------: |
| Clr     | Asus Zephyrus | 0.1783ns   | 1.4302ns              | 1.9416ns              | 9.8810ns              |
| Clr     | Surface Pro 4 | 0.1754ns   | 1.0086ns              | 1.4326ns              | 13.2368ns             |
| Core    | Asus Zephyrus | 0.2023ns   | 1.4034ns              | 2.6629ns              | 9.7827ns              |
| Core    | Surface Pro 4 | 0.0949ns   | 0.6688ns              | 2.7045ns              | 13.2217ns             |

### MustNotBeGreaterThanOrEqualTo

Ensures that the specified `parameter` is less than the given `other` value, or otherwise throws an `ArgumentOutOfRangeException`.

See `MustBeLessThan` for benchmark results.

### MustBeGreaterThan

Ensures that the specified `parameter` is greater than the given `other` value, or otherwise throws an `ArgumentOutOfRangeException`.

| Runtime | Ran On        | Imperative | v5.0 Custom Parameter | v5.0 Custom Exception | v3.5 Custom Parameter |
| ------  | -----         | ---------: | --------------------: | --------------------: | --------------------: |
| Clr     | Asus Zephyrus | 0.1196ns   | 0.7637ns              | 1.0408ns              | 9.6945ns              |
| Clr     | Surface Pro 4 | 0.1721ns   | 1.0163ns              | 1.4225ns              | 13.2294ns             |
| Core    | Asus Zephyrus | 0.0739ns   | 0.4898ns              | 1.9818ns              | 9.5656ns              |
| Core    | Surface Pro 4 | 0.1021ns   | 0.6653ns              | 2.7106ns              | 13.0015ns             |

### MustNotBeLessThanOrEqualTo

Ensures that the specified `parameter` is greater than the given `other` value, or otherwise throws an `ArgumentOutOfRangeException`.

| Runtime | Ran On        | Imperative | v5.0 Custom Parameter | v5.0 Custom Exception | v3.5 Custom Parameter |
| ------  | -----         | ---------: | --------------------: | --------------------: | --------------------: |
| Clr     | Asus Zephyrus | 0.0944ns   | 0.0001ns              | 0.4884ns              | 9.3354ns              |
| Clr     | Surface Pro 4 | 0.1141ns   | 0.0000ns              | 0.6618ns              | 12.7290ns             |
| Core    | Asus Zephyrus | 0.1139ns   | 0.0442ns              | 1.3405ns              | 9.2107ns              |
| Core    | Surface Pro 4 | 0.1480ns   | 0.0512ns              | 1.8095ns              | 12.5551ns             |

### MustNotBeGreaterThan

Ensures that the specified `parameter` is not greater than the given `other` value, or otherwise throws an `ArgumentOutOfRangeException`.

| Runtime | Ran On        | Imperative | v5.0 Custom Parameter | v5.0 Custom Exception | v3.5 Custom Parameter |
| ------  | -----         | ---------: | --------------------: | --------------------: | --------------------: |
| Clr     | Asus Zephyrus | 0.0792ns   | 0.2347ns              | 0.5280ns              | 8.6150ns              |
| Clr     | Surface Pro 4 | 0.0889ns   | 0.3245ns              | 0.7169ns              | 11.7063ns             |
| Core    | Asus Zephyrus | 0.1066ns   | 0.0000ns              | 1.0926ns              | 9.2686ns              |
| Core    | Surface Pro 4 | 0.1350ns   | 0.0000ns              | 1.4919ns              | 12.5746ns             |

### MustBeLessThanOrEqualTo

Ensures that the specified `parameter` is not greater than the given `other` value, or otherwise throws an `ArgumentOutOfRangeException`.

| Runtime | Ran On        | Imperative | v5.0 Custom Parameter | v5.0 Custom Exception | v3.5 Custom Parameter |
| ------  | -----         | ---------: | --------------------: | --------------------: | --------------------: |
| Clr     | Asus Zephyrus | 0.0969ns   | 1.3712ns              | 1.8124ns              | 9.7899ns              |
| Clr     | Surface Pro 4 | 0.1267ns   | 1.8457ns              | 2.5297ns              | 13.2975ns             |
| Core    | Asus Zephyrus | 0.0965ns   | 1.3677ns              | 2.5359ns              | 9.8026ns              |
| Core    | Surface Pro 4 | 0.1090ns   | 1.8597ns              | 3.2995ns              | 13.3685ns             |

### IsIn

Checks if the value is within the specified range.

See `MustBeIn` for performance results.

### IsNotIn

Checks if the value is not within the specified range.

See `MustNotBeIn` for performance results.

### MustBeIn

Ensures that `parameter` is within the specified range, or otherwise throws an `ArgumentOutOfRangeException`.

| Runtime | Ran On        | Imperative | v5.0 Custom Parameter | v5.0 Custom Exception | v3.5 Custom Parameter |
| ------  | -----         | ---------: | --------------------: | --------------------: | --------------------: |
| Clr     | Asus Zephyrus | 0.2530ns   | 2.8551ns              | 3.3372ns              | 4.1381ns              |
| Clr     | Surface Pro 4 | 0.3384ns   | 3.9004ns              | 4.5227ns              | 5.6373ns              |
| Core    | Asus Zephyrus | 0.1267ns   | 2.2790ns              | 4.0267ns              | 4.3610ns              |
| Core    | Surface Pro 4 | 0.1647ns   | 3.1076ns              | 5.6882ns              | 5.9395ns              |

### MustNotBeIn

Ensures that `parameter` is not within the specified range, or otherwise throws an `ArgumentOutOfRangeException`.

| Runtime | Ran On        | Imperative | v5.0 Custom Parameter | v5.0 Custom Exception | v3.5 Custom Parameter |
| ------  | -----         | ---------: | --------------------: | --------------------: | --------------------: |
| Clr     | Asus Zephyrus | 0.2508ns   | 2.0246ns              | 2.6800ns              | 4.1193ns              |
| Clr     | Surface Pro 4 | 0.3373ns   | 2.7542ns              | 3.6915ns              | 5.5427ns              |
| Core    | Asus Zephyrus | 0.2770ns   | 2.0246ns              | 2.6800ns              | 4.1193ns              |
| Core    | Surface Pro 4 | 0.3746ns   | 2.7856ns              | 5.3471ns              | 5.3919ns              |
