using System;
using System.Reflection;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.TypeAssertionsTests
{
    public sealed class MustNotBeStaticClassTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustNotBeStaticClass must throw a TypeException when the specified type is not a static class.")]
        public void TypeStatic()
        {
            var staticType = typeof(Console);

            Action act = () => staticType.MustNotBeStaticClass(nameof(staticType));

            act.Should().Throw<TypeException>()
               .And.Message.Should().Contain($"{nameof(staticType)} \"{staticType}\" must not be a static class, but it is.");
        }

        [Fact(DisplayName = "MustNotBeStaticClass must throw a TypeException when the specified TypeInfo is not a static class.")]
        public void TypeInfoStatic()
        {
            var staticTypeInfo = typeof(Convert);

            Action act = () => staticTypeInfo.GetTypeInfo().MustNotBeStaticClass(nameof(staticTypeInfo));

            act.Should().Throw<TypeException>()
               .And.Message.Should().Contain($"{nameof(staticTypeInfo)} \"{staticTypeInfo}\" must not be a static class, but it is.");
        }

        [Fact(DisplayName = "MustNotBeStaticClass must not throw an exception when the specified type is a static class.")]
        public void TypeNotStatic()
        {
            var structType = typeof(double);

            var result = structType.MustNotBeStaticClass();

            result.Should().BeSameAs(structType);
        }

        [Fact(DisplayName = "MustNotBeStaticClass must not throw an exception when the specified TypeInfo is a static class.")]
        public void TypeInfoNotStatic()
        {
            var objectType = typeof(object).GetTypeInfo();

            var result = objectType.MustNotBeStaticClass();

            result.Should().BeSameAs(objectType);
        }

        [Fact(DisplayName = "MustNotBeStaticClass must throw an ArgumentNullException when the specified type is null.")]
        public void TypeNull()
        {
            Action act = () => ((Type) null).MustNotBeStaticClass();

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact(DisplayName = "MustNotBeStaticClass must throw an ArgumentNullException when the specified TypeInfo is null.")]
        public void TypeInfoNull()
        {
            Action act = () => ((TypeInfo) null).MustNotBeStaticClass();

            act.Should().Throw<ArgumentNullException>();
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.AddExceptionTest(exception => typeof(GC).MustNotBeStaticClass(exception: exception))
                    .AddMessageTest<TypeException>(message => typeof(Console).MustNotBeStaticClass(message: message));

            testData.AddExceptionTest(exception => typeof(Nullable).GetTypeInfo().MustNotBeStaticClass(exception: exception))
                    .AddMessageTest<TypeException>(message => typeof(Convert).GetTypeInfo().MustNotBeStaticClass(message: message));
        }
    }
}