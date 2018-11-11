using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using Light.GuardClauses.FrameworkExtensions;

namespace Light.GuardClauses.Performance.FrameworkExtensions
{
    public class HashFunctionStringBenchmark
    {
        public string Bar = "Bar";
        public string Baz = "Baz";
        public string Foo = "Foo";
        public string Qux = "Qux";

        [Benchmark]
        public int ReSharperHash2Parameters() => ReSharperHash.CalculateHashCode(Foo, Bar);

        [Benchmark]
        public int ReSharperHash3Parameters() => ReSharperHash.CalculateHashCode(Foo, Bar, Baz);

        [Benchmark]
        public int ReSharperHash4Parameters() => ReSharperHash.CalculateHashCode(Foo, Bar, Baz, Qux);

        [Benchmark]
        public int Fnv1A2Parameters() => Fnv1AHash.CalculateHashCode(Foo, Bar);

        [Benchmark]
        public int Fnv1A3Parameters() => Fnv1AHash.CalculateHashCode(Foo, Bar, Baz);

        [Benchmark]
        public int Fnv1A4Parameters() => Fnv1AHash.CalculateHashCode(Foo, Bar, Baz, Qux);

        [Benchmark]
        public int XxHash2Parameters() => HashCode.Combine(Foo, Bar);

        [Benchmark]
        public int XxHash3Parameters() => HashCode.Combine(Foo, Bar, Baz);

        [Benchmark]
        public int XxHash4Parameters() => HashCode.Combine(Foo, Bar, Baz, Qux);

        [Benchmark]
        public int MultiplyAdd2Parameters() => MultiplyAddHash.CreateHashCode(Foo, Bar);

        [Benchmark]
        public int MultiplyAdd3Parameters() => MultiplyAddHash.CreateHashCode(Foo, Bar, Baz);

        [Benchmark]
        public int MultiplyAdd4Parameters() => MultiplyAddHash.CreateHashCode(Foo, Bar, Baz, Qux);
    }

    public class HashFunctionIntBenchmark
    {
        public int First = 475940556;
        public int Fourth = 2041509450;
        public int Second = 436669774;
        public int Third = 2103655226;

        [Benchmark]
        public int ReSharperHash2Parameters() => ReSharperHash.CalculateHashCode(First, Second);

        [Benchmark]
        public int ReSharperHash3Parameters() => ReSharperHash.CalculateHashCode(First, Second, Third);

        [Benchmark]
        public int ReSharperHash4Parameters() => ReSharperHash.CalculateHashCode(First, Second, Third, Fourth);

        [Benchmark]
        public int Fnv1A2Parameters() => Fnv1AHash.CalculateHashCode(First, Second);

        [Benchmark]
        public int Fnv1A3Parameters() => Fnv1AHash.CalculateHashCode(First, Second, Third);

        [Benchmark]
        public int Fnv1A4Parameters() => Fnv1AHash.CalculateHashCode(First, Second, Third, Fourth);

        [Benchmark]
        public int XxHash2Parameters() => HashCode.Combine(First, Second);

        [Benchmark]
        public int XxHash3Parameters() => HashCode.Combine(First, Second, Third);

        [Benchmark]
        public int XxHash4Parameters() => HashCode.Combine(First, Second, Third, Fourth);

        [Benchmark]
        public int MultiplyAdd2Parameters() => MultiplyAddHash.CreateHashCode(First, Second);

        [Benchmark]
        public int MultiplyAdd3Parameters() => MultiplyAddHash.CreateHashCode(First, Second, Third);

        [Benchmark]
        public int MultiplyAdd4Parameters() => MultiplyAddHash.CreateHashCode(First, Second, Third, Fourth);
    }

    public static class ReSharperHash
    {
        public const int Prime = 397;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CalculateHashCode<T1, T2>(T1 value1, T2 value2)
        {
            unchecked
            {
                var hashCode = value1.GetHashCode();
                hashCode = (hashCode * Prime) ^ value2.GetHashCode();
                return hashCode;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CalculateHashCode<T1, T2, T3>(T1 value1, T2 value2, T3 value3)
        {
            unchecked
            {
                var hashCode = value1.GetHashCode();
                hashCode = (hashCode * Prime) ^ value2.GetHashCode();
                hashCode = (hashCode * Prime) ^ value3.GetHashCode();
                return hashCode;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CalculateHashCode<T1, T2, T3, T4>(T1 value1, T2 value2, T3 value3, T4 value4)
        {
            unchecked
            {
                var hashCode = value1.GetHashCode();
                hashCode = (hashCode * Prime) ^ value2.GetHashCode();
                hashCode = (hashCode * Prime) ^ value3.GetHashCode();
                hashCode = (hashCode * Prime) ^ value4.GetHashCode();
                return hashCode;
            }
        }
    }

    public static class Fnv1AHash
    {
        public const int OffsetBasis = -2128831035;
        public const int FnvPrime = 16777619;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CalculateHashCode<T1, T2>(T1 value1, T2 value2)
        {
            unchecked
            {
                var hashCode = OffsetBasis;
                hashCode = (hashCode ^ value1.GetHashCode()) * FnvPrime;
                hashCode = (hashCode ^ value2.GetHashCode()) * FnvPrime;
                return hashCode;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CalculateHashCode<T1, T2, T3>(T1 value1, T2 value2, T3 value3)
        {
            unchecked
            {
                var hashCode = OffsetBasis;
                hashCode = (hashCode ^ value1.GetHashCode()) * FnvPrime;
                hashCode = (hashCode ^ value2.GetHashCode()) * FnvPrime;
                hashCode = (hashCode ^ value3.GetHashCode()) * FnvPrime;
                return hashCode;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CalculateHashCode<T1, T2, T3, T4>(T1 value1, T2 value2, T3 value3, T4 value4)
        {
            unchecked
            {
                var hashCode = OffsetBasis;
                hashCode = (hashCode ^ value1.GetHashCode()) * FnvPrime;
                hashCode = (hashCode ^ value2.GetHashCode()) * FnvPrime;
                hashCode = (hashCode ^ value3.GetHashCode()) * FnvPrime;
                hashCode = (hashCode ^ value4.GetHashCode()) * FnvPrime;
                return hashCode;
            }
        }
    }

    // xxHash32 is used for the hash code.
    // https://github.com/Cyan4973/xxHash

    public struct HashCode
    {
        private static readonly uint s_seed = GenerateGlobalSeed();

        private const uint Prime1 = 2654435761U;
        private const uint Prime2 = 2246822519U;
        private const uint Prime3 = 3266489917U;
        private const uint Prime4 = 668265263U;
        private const uint Prime5 = 374761393U;

        private uint _v1, _v2, _v3, _v4;
        private uint _queue1, _queue2, _queue3;
        private uint _length;

        private static uint GenerateGlobalSeed()
        {
            unchecked
            {
                return (uint) new Random().Next();
            }
        }

        public static int Combine<T1>(T1 value1)
        {
            // Provide a way of diffusing bits from something with a limited
            // input hash space. For example, many enums only have a few
            // possible hashes, only using the bottom few bits of the code. Some
            // collections are built on the assumption that hashes are spread
            // over a larger space, so diffusing the bits may help the
            // collection work more efficiently.

            var hc1 = (uint) (value1?.GetHashCode() ?? 0);

            uint hash = MixEmptyState();
            hash += 4;

            hash = QueueRound(hash, hc1);

            hash = MixFinal(hash);
            return (int) hash;
        }

        public static int Combine<T1, T2>(T1 value1, T2 value2)
        {
            var hc1 = (uint) (value1?.GetHashCode() ?? 0);
            var hc2 = (uint) (value2?.GetHashCode() ?? 0);

            uint hash = MixEmptyState();
            hash += 8;

            hash = QueueRound(hash, hc1);
            hash = QueueRound(hash, hc2);

            hash = MixFinal(hash);
            return (int) hash;
        }

        public static int Combine<T1, T2, T3>(T1 value1, T2 value2, T3 value3)
        {
            var hc1 = (uint) (value1?.GetHashCode() ?? 0);
            var hc2 = (uint) (value2?.GetHashCode() ?? 0);
            var hc3 = (uint) (value3?.GetHashCode() ?? 0);

            uint hash = MixEmptyState();
            hash += 12;

            hash = QueueRound(hash, hc1);
            hash = QueueRound(hash, hc2);
            hash = QueueRound(hash, hc3);

            hash = MixFinal(hash);
            return (int) hash;
        }

        public static int Combine<T1, T2, T3, T4>(T1 value1, T2 value2, T3 value3, T4 value4)
        {
            var hc1 = (uint) (value1?.GetHashCode() ?? 0);
            var hc2 = (uint) (value2?.GetHashCode() ?? 0);
            var hc3 = (uint) (value3?.GetHashCode() ?? 0);
            var hc4 = (uint) (value4?.GetHashCode() ?? 0);

            Initialize(out uint v1, out uint v2, out uint v3, out uint v4);

            v1 = Round(v1, hc1);
            v2 = Round(v2, hc2);
            v3 = Round(v3, hc3);
            v4 = Round(v4, hc4);

            uint hash = MixState(v1, v2, v3, v4);
            hash += 16;

            hash = MixFinal(hash);
            return (int) hash;
        }

        public static int Combine<T1, T2, T3, T4, T5>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
        {
            var hc1 = (uint) (value1?.GetHashCode() ?? 0);
            var hc2 = (uint) (value2?.GetHashCode() ?? 0);
            var hc3 = (uint) (value3?.GetHashCode() ?? 0);
            var hc4 = (uint) (value4?.GetHashCode() ?? 0);
            var hc5 = (uint) (value5?.GetHashCode() ?? 0);

            Initialize(out uint v1, out uint v2, out uint v3, out uint v4);

            v1 = Round(v1, hc1);
            v2 = Round(v2, hc2);
            v3 = Round(v3, hc3);
            v4 = Round(v4, hc4);

            uint hash = MixState(v1, v2, v3, v4);
            hash += 20;

            hash = QueueRound(hash, hc5);

            hash = MixFinal(hash);
            return (int) hash;
        }

        public static int Combine<T1, T2, T3, T4, T5, T6>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6)
        {
            var hc1 = (uint) (value1?.GetHashCode() ?? 0);
            var hc2 = (uint) (value2?.GetHashCode() ?? 0);
            var hc3 = (uint) (value3?.GetHashCode() ?? 0);
            var hc4 = (uint) (value4?.GetHashCode() ?? 0);
            var hc5 = (uint) (value5?.GetHashCode() ?? 0);
            var hc6 = (uint) (value6?.GetHashCode() ?? 0);

            Initialize(out uint v1, out uint v2, out uint v3, out uint v4);

            v1 = Round(v1, hc1);
            v2 = Round(v2, hc2);
            v3 = Round(v3, hc3);
            v4 = Round(v4, hc4);

            uint hash = MixState(v1, v2, v3, v4);
            hash += 24;

            hash = QueueRound(hash, hc5);
            hash = QueueRound(hash, hc6);

            hash = MixFinal(hash);
            return (int) hash;
        }

        public static int Combine<T1, T2, T3, T4, T5, T6, T7>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7)
        {
            var hc1 = (uint) (value1?.GetHashCode() ?? 0);
            var hc2 = (uint) (value2?.GetHashCode() ?? 0);
            var hc3 = (uint) (value3?.GetHashCode() ?? 0);
            var hc4 = (uint) (value4?.GetHashCode() ?? 0);
            var hc5 = (uint) (value5?.GetHashCode() ?? 0);
            var hc6 = (uint) (value6?.GetHashCode() ?? 0);
            var hc7 = (uint) (value7?.GetHashCode() ?? 0);

            Initialize(out uint v1, out uint v2, out uint v3, out uint v4);

            v1 = Round(v1, hc1);
            v2 = Round(v2, hc2);
            v3 = Round(v3, hc3);
            v4 = Round(v4, hc4);

            uint hash = MixState(v1, v2, v3, v4);
            hash += 28;

            hash = QueueRound(hash, hc5);
            hash = QueueRound(hash, hc6);
            hash = QueueRound(hash, hc7);

            hash = MixFinal(hash);
            return (int) hash;
        }

        public static int Combine<T1, T2, T3, T4, T5, T6, T7, T8>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8)
        {
            var hc1 = (uint) (value1?.GetHashCode() ?? 0);
            var hc2 = (uint) (value2?.GetHashCode() ?? 0);
            var hc3 = (uint) (value3?.GetHashCode() ?? 0);
            var hc4 = (uint) (value4?.GetHashCode() ?? 0);
            var hc5 = (uint) (value5?.GetHashCode() ?? 0);
            var hc6 = (uint) (value6?.GetHashCode() ?? 0);
            var hc7 = (uint) (value7?.GetHashCode() ?? 0);
            var hc8 = (uint) (value8?.GetHashCode() ?? 0);

            Initialize(out uint v1, out uint v2, out uint v3, out uint v4);

            v1 = Round(v1, hc1);
            v2 = Round(v2, hc2);
            v3 = Round(v3, hc3);
            v4 = Round(v4, hc4);

            v1 = Round(v1, hc5);
            v2 = Round(v2, hc6);
            v3 = Round(v3, hc7);
            v4 = Round(v4, hc8);

            uint hash = MixState(v1, v2, v3, v4);
            hash += 32;

            hash = MixFinal(hash);
            return (int) hash;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint Rol(uint value, int count)
            => (value << count) | (value >> (32 - count));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Initialize(out uint v1, out uint v2, out uint v3, out uint v4)
        {
            v1 = s_seed + Prime1 + Prime2;
            v2 = s_seed + Prime2;
            v3 = s_seed;
            v4 = s_seed - Prime1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint Round(uint hash, uint input)
        {
            hash += input * Prime2;
            hash = Rol(hash, 13);
            hash *= Prime1;
            return hash;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint QueueRound(uint hash, uint queuedValue)
        {
            hash += queuedValue * Prime3;
            return Rol(hash, 17) * Prime4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint MixState(uint v1, uint v2, uint v3, uint v4)
        {
            return Rol(v1, 1) + Rol(v2, 7) + Rol(v3, 12) + Rol(v4, 18);
        }

        private static uint MixEmptyState()
        {
            return s_seed + Prime5;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint MixFinal(uint hash)
        {
            hash ^= hash >> 15;
            hash *= Prime2;
            hash ^= hash >> 13;
            hash *= Prime3;
            hash ^= hash >> 16;
            return hash;
        }

        public void Add<T>(T value)
        {
            Add(value?.GetHashCode() ?? 0);
        }

        public void Add<T>(T value, IEqualityComparer<T> comparer)
        {
            Add(comparer != null ? comparer.GetHashCode(value) : (value?.GetHashCode() ?? 0));
        }

        private void Add(int value)
        {
            // The original xxHash works as follows:
            // 0. Initialize immediately. We can't do this in a struct (no
            //    default ctor).
            // 1. Accumulate blocks of length 16 (4 uints) into 4 accumulators.
            // 2. Accumulate remaining blocks of length 4 (1 uint) into the
            //    hash.
            // 3. Accumulate remaining blocks of length 1 into the hash.

            // There is no need for #3 as this type only accepts ints. _queue1,
            // _queue2 and _queue3 are basically a buffer so that when
            // ToHashCode is called we can execute #2 correctly.

            // We need to initialize the xxHash32 state (_v1 to _v4) lazily (see
            // #0) nd the last place that can be done if you look at the
            // original code is just before the first block of 16 bytes is mixed
            // in. The xxHash32 state is never used for streams containing fewer
            // than 16 bytes.

            // To see what's really going on here, have a look at the Combine
            // methods.

            var val = (uint) value;

            // Storing the value of _length locally shaves of quite a few bytes
            // in the resulting machine code.
            uint previousLength = _length++;
            uint position = previousLength % 4;

            // Switch can't be inlined.

            if (position == 0)
                _queue1 = val;
            else if (position == 1)
                _queue2 = val;
            else if (position == 2)
                _queue3 = val;
            else // position == 3
            {
                if (previousLength == 3)
                    Initialize(out _v1, out _v2, out _v3, out _v4);

                _v1 = Round(_v1, _queue1);
                _v2 = Round(_v2, _queue2);
                _v3 = Round(_v3, _queue3);
                _v4 = Round(_v4, val);
            }
        }

        public int ToHashCode()
        {
            // Storing the value of _length locally shaves of quite a few bytes
            // in the resulting machine code.
            uint length = _length;

            // position refers to the *next* queue position in this method, so
            // position == 1 means that _queue1 is populated; _queue2 would have
            // been populated on the next call to Add.
            uint position = length % 4;

            // If the length is less than 4, _v1 to _v4 don't contain anything
            // yet. xxHash32 treats this differently.

            uint hash = length < 4 ? MixEmptyState() : MixState(_v1, _v2, _v3, _v4);

            // _length is incremented once per Add(Int32) and is therefore 4
            // times too small (xxHash length is in bytes, not ints).

            hash += length * 4;

            // Mix what remains in the queue

            // Switch can't be inlined right now, so use as few branches as
            // possible by manually excluding impossible scenarios (position > 1
            // is always false if position is not > 0).
            if (position > 0)
            {
                hash = QueueRound(hash, _queue1);
                if (position > 1)
                {
                    hash = QueueRound(hash, _queue2);
                    if (position > 2)
                        hash = QueueRound(hash, _queue3);
                }
            }

            hash = MixFinal(hash);
            return (int) hash;
        }

        public override int GetHashCode() => throw new NotSupportedException();

        public override bool Equals(object obj) => throw new NotSupportedException();
    }
}