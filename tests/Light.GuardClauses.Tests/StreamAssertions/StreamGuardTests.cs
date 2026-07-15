#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.StreamAssertions;

public static class StreamGuardTests
{
    private static readonly Func<TrackingStream?, Exception> AllocationExceptionFactory =
        static _ => new InvalidOperationException();

    [Theory]
    [MemberData(nameof(CapabilityStates))]
    public static void GuardsAcceptExactlyTheirMatchingCapabilityAndInspectOnlyThatProperty(
        Capability capability,
        bool canRead,
        bool canWrite,
        bool canSeek
    )
    {
        var stream = new TrackingStream(canRead, canWrite, canSeek) { StoredPosition = 42 };

        Action act = () => InvokeDefault(stream, capability);

        if (GetExpectedCapability(capability, canRead, canWrite, canSeek))
        {
            act.Should().NotThrow();
        }
        else
        {
            act.Should().Throw<ArgumentException>();
        }

        stream.GetCapabilityReadCount(capability).Should().Be(1);
        stream.GetOtherCapabilityReadCount(capability).Should().Be(0);
        stream.PositionReadCount.Should().Be(0);
        stream.LengthReadCount.Should().Be(0);
        stream.IoOperationCount.Should().Be(0);
        stream.StoredPosition.Should().Be(42);
    }

    public static IEnumerable<object[]> CapabilityStates()
    {
        foreach (var capability in Enum.GetValues<Capability>())
        {
            for (var state = 0; state < 8; state++)
            {
                yield return
                [
                    capability,
                    (state & 1) != 0,
                    (state & 2) != 0,
                    (state & 4) != 0,
                ];
            }
        }
    }

    [Theory]
    [InlineData(Capability.Readable)]
    [InlineData(Capability.Writable)]
    [InlineData(Capability.Seekable)]
    public static void GuardsPreserveConcreteTypeAndOriginalInstance(Capability capability)
    {
        var stream = new TrackingStream(true, true, true);

        InvokeDefault(stream, capability).Should().BeSameAs(stream);
    }

    [Theory]
    [InlineData(Capability.Readable, "readable")]
    [InlineData(Capability.Writable, "writable")]
    [InlineData(Capability.Seekable, "seekable")]
    public static void DefaultFailuresUseCapturedExpressionAndCapabilitySpecificMessage(
        Capability capability,
        string capabilityName
    )
    {
        var stream = CreateUnsupportedStream(capability);

        var act = () => InvokeDefault(stream, capability);

        act.Should().Throw<ArgumentException>()
           .WithParameterName(nameof(stream))
           .WithMessage($"*{nameof(stream)} must be {capabilityName}.*");
    }

    [Theory]
    [InlineData(Capability.Readable)]
    [InlineData(Capability.Writable)]
    [InlineData(Capability.Seekable)]
    public static void NullStreamsThrowArgumentNullExceptionWithCapturedExpression(Capability capability)
    {
        TrackingStream? stream = null;

        var act = () => InvokeDefault(stream, capability);

        act.Should().Throw<ArgumentNullException>()
           .WithParameterName(nameof(stream));
    }

    [Theory]
    [InlineData(Capability.Readable)]
    [InlineData(Capability.Writable)]
    [InlineData(Capability.Seekable)]
    public static void CustomMessagesAreUsedForCapabilityFailures(Capability capability)
    {
        const string message = "The stream has the wrong capability";
        var stream = CreateUnsupportedStream(capability);

        var act = () => InvokeDefault(stream, capability, message: message);

        act.Should().Throw<ArgumentException>().WithMessage($"{message}*");
    }

    [Theory]
    [InlineData(Capability.Readable)]
    [InlineData(Capability.Writable)]
    [InlineData(Capability.Seekable)]
    public static void CustomMessagesAreUsedForNullStreams(Capability capability)
    {
        const string message = "The stream must be supplied";
        TrackingStream? stream = null;

        var act = () => InvokeDefault(stream, capability, message: message);

        act.Should().Throw<ArgumentNullException>().WithMessage($"{message}*");
    }

    [Theory]
    [InlineData(Capability.Readable)]
    [InlineData(Capability.Writable)]
    [InlineData(Capability.Seekable)]
    public static void CustomFactoriesReceiveUnsupportedOriginalStream(Capability capability)
    {
        var stream = CreateUnsupportedStream(capability);
        var expectedException = new InvalidOperationException();
        TrackingStream? capturedStream = null;

        Exception ExceptionFactory(TrackingStream? value)
        {
            capturedStream = value;
            return expectedException;
        }

        var act = () => InvokeFactory(stream, capability, ExceptionFactory);

        act.Should().Throw<InvalidOperationException>().Which.Should().BeSameAs(expectedException);
        capturedStream.Should().BeSameAs(stream);
    }

    [Theory]
    [InlineData(Capability.Readable)]
    [InlineData(Capability.Writable)]
    [InlineData(Capability.Seekable)]
    public static void CustomFactoriesReceiveNullStream(Capability capability)
    {
        var expectedException = new InvalidOperationException();
        var factoryWasCalled = false;

        Exception ExceptionFactory(TrackingStream? value)
        {
            value.Should().BeNull();
            factoryWasCalled = true;
            return expectedException;
        }

        var act = () => InvokeFactory(null, capability, ExceptionFactory);

        act.Should().Throw<InvalidOperationException>().Which.Should().BeSameAs(expectedException);
        factoryWasCalled.Should().BeTrue();
    }

    [Theory]
    [InlineData(Capability.Readable)]
    [InlineData(Capability.Writable)]
    [InlineData(Capability.Seekable)]
    public static void SuccessfulCustomFactoryGuardsDoNotInvokeFactory(Capability capability)
    {
        var stream = new TrackingStream(true, true, true);

        var result = InvokeFactory(stream, capability, _ => throw new InvalidOperationException());

        result.Should().BeSameAs(stream);
    }

    [Theory]
    [InlineData(Capability.Readable)]
    [InlineData(Capability.Writable)]
    [InlineData(Capability.Seekable)]
    public static void DisposedFrameworkStreamsFailAccordingToTheirCapabilityProperties(Capability capability)
    {
        var stream = new MemoryStream();
        stream.Dispose();

        var act = () => InvokeDefault(stream, capability);

        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(Capability.Readable)]
    [InlineData(Capability.Writable)]
    [InlineData(Capability.Seekable)]
    public static void ExceptionsFromMatchingCapabilityGetterArePreserved(Capability capability)
    {
        var expectedException = new NotSupportedException();
        var stream = new TrackingStream(true, true, true)
        {
            ThrowingCapability = capability,
            CapabilityException = expectedException,
        };

        var act = () => InvokeDefault(stream, capability);

        act.Should().Throw<NotSupportedException>().Which.Should().BeSameAs(expectedException);
    }

    [Theory]
    [InlineData(Capability.Readable)]
    [InlineData(Capability.Writable)]
    [InlineData(Capability.Seekable)]
    public static void SuccessfulDefaultAndFactoryGuardsAllocateNoMemory(Capability capability)
    {
        var stream = new TrackingStream(true, true, true);
        InvokeDefault(stream, capability);
        InvokeFactory(stream, capability, AllocationExceptionFactory);

        var allocatedBefore = GC.GetAllocatedBytesForCurrentThread();
        for (var i = 0; i < 10_000; i++)
        {
            InvokeDefault(stream, capability);
            InvokeFactory(stream, capability, AllocationExceptionFactory);
        }

        var allocatedBytes = GC.GetAllocatedBytesForCurrentThread() - allocatedBefore;

        allocatedBytes.Should().Be(0);
    }

    private static TrackingStream InvokeDefault(
        TrackingStream? stream,
        Capability capability,
        string? parameterName = null,
        string? message = null
    ) => parameterName is null ?
        capability switch
        {
            Capability.Readable => stream.MustBeReadable(message: message),
            Capability.Writable => stream.MustBeWritable(message: message),
            Capability.Seekable => stream.MustBeSeekable(message: message),
            _ => throw new ArgumentOutOfRangeException(nameof(capability)),
        } :
        capability switch
        {
            Capability.Readable => stream.MustBeReadable(parameterName, message),
            Capability.Writable => stream.MustBeWritable(parameterName, message),
            Capability.Seekable => stream.MustBeSeekable(parameterName, message),
            _ => throw new ArgumentOutOfRangeException(nameof(capability)),
        };

    private static MemoryStream InvokeDefault(MemoryStream stream, Capability capability) =>
        capability switch
        {
            Capability.Readable => stream.MustBeReadable(),
            Capability.Writable => stream.MustBeWritable(),
            Capability.Seekable => stream.MustBeSeekable(),
            _ => throw new ArgumentOutOfRangeException(nameof(capability)),
        };

    private static TrackingStream InvokeFactory(
        TrackingStream? stream,
        Capability capability,
        Func<TrackingStream?, Exception> exceptionFactory
    ) =>
        capability switch
        {
            Capability.Readable => stream.MustBeReadable(exceptionFactory),
            Capability.Writable => stream.MustBeWritable(exceptionFactory),
            Capability.Seekable => stream.MustBeSeekable(exceptionFactory),
            _ => throw new ArgumentOutOfRangeException(nameof(capability)),
        };

    private static TrackingStream CreateUnsupportedStream(Capability capability) =>
        capability switch
        {
            Capability.Readable => new (false, true, true),
            Capability.Writable => new (true, false, true),
            Capability.Seekable => new (true, true, false),
            _ => throw new ArgumentOutOfRangeException(nameof(capability)),
        };

    private static bool GetExpectedCapability(
        Capability capability,
        bool canRead,
        bool canWrite,
        bool canSeek
    ) =>
        capability switch
        {
            Capability.Readable => canRead,
            Capability.Writable => canWrite,
            Capability.Seekable => canSeek,
            _ => throw new ArgumentOutOfRangeException(nameof(capability)),
        };

    public enum Capability
    {
        Readable,
        Writable,
        Seekable,
    }

    private sealed class TrackingStream(bool canRead, bool canWrite, bool canSeek) : Stream
    {
        public Capability? ThrowingCapability { get; init; }

        public Exception? CapabilityException { get; init; }

        public int CanReadReadCount { get; private set; }

        public int CanWriteReadCount { get; private set; }

        public int CanSeekReadCount { get; private set; }

        public int PositionReadCount { get; private set; }

        public int LengthReadCount { get; private set; }

        public int IoOperationCount { get; private set; }

        public long StoredPosition { get; set; }

        public override bool CanRead
        {
            get
            {
                CanReadReadCount++;
                ThrowIfConfigured(Capability.Readable);
                return canRead;
            }
        }

        public override bool CanWrite
        {
            get
            {
                CanWriteReadCount++;
                ThrowIfConfigured(Capability.Writable);
                return canWrite;
            }
        }

        public override bool CanSeek
        {
            get
            {
                CanSeekReadCount++;
                ThrowIfConfigured(Capability.Seekable);
                return canSeek;
            }
        }

        public override long Length
        {
            get
            {
                LengthReadCount++;
                return 0;
            }
        }

        public override long Position
        {
            get
            {
                PositionReadCount++;
                return StoredPosition;
            }
            set => StoredPosition = value;
        }

        public int GetCapabilityReadCount(Capability capability) =>
            capability switch
            {
                Capability.Readable => CanReadReadCount,
                Capability.Writable => CanWriteReadCount,
                Capability.Seekable => CanSeekReadCount,
                _ => throw new ArgumentOutOfRangeException(nameof(capability)),
            };

        public int GetOtherCapabilityReadCount(Capability capability) =>
            capability switch
            {
                Capability.Readable => CanWriteReadCount + CanSeekReadCount,
                Capability.Writable => CanReadReadCount + CanSeekReadCount,
                Capability.Seekable => CanReadReadCount + CanWriteReadCount,
                _ => throw new ArgumentOutOfRangeException(nameof(capability)),
            };

        public override void Flush()
        {
            IoOperationCount++;
            throw new NotSupportedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            IoOperationCount++;
            throw new NotSupportedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            IoOperationCount++;
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            IoOperationCount++;
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            IoOperationCount++;
            throw new NotSupportedException();
        }

        public override string ToString() => throw new InvalidOperationException("A guard must not format the stream.");

        private void ThrowIfConfigured(Capability capability)
        {
            if (ThrowingCapability == capability)
            {
                throw CapabilityException ?? new InvalidOperationException();
            }
        }
    }
}
