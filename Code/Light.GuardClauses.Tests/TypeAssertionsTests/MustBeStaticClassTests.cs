using System;
using System.Reflection;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.TypeAssertionsTests
{
    public sealed class MustBeStaticClassTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustBeStaticClass must throw a TypeException when the specified type is not a static class.")]
        public void TypeNotStatic()
        {
            var classType = typeof(string);

            Action act = () => classType.MustBeStaticClass(nameof(classType));

            act.Should().Throw<TypeException>()
               .And.Message.Should().Contain($"{nameof(classType)} \"{classType}\" must be a static class, but it is not.");
        }

        [Fact(DisplayName = "MustBeStaticClass must throw a TypeException when the specified TypeInfo is not a static class.")]
        public void TypeInfoNotStatic()
        {
            var structType = typeof(char);

            Action act = () => structType.GetTypeInfo().MustBeStaticClass(nameof(structType));

            act.Should().Throw<TypeException>()
               .And.Message.Should().Contain($"{nameof(structType)} \"{structType}\" must be a static class, but it is not.");
        }

        [Fact(DisplayName = "MustBeStaticClass must not throw an exception when the specified type is a static class.")]
        public void TypeStatic()
        {
            var staticClassType = typeof(CommonAssertions);

            var result = staticClassType.MustBeStaticClass();

            result.Should().BeSameAs(staticClassType);
        }

        [Fact(DisplayName = "MustBeStaticClass must not throw an exception when the specified TypeInfo is a static class.")]
        public void TypeInfoStatic()
        {
            var staticClassTypeInfo = typeof(Convert).GetTypeInfo();

            var result = staticClassTypeInfo.MustBeStaticClass();

            result.Should().BeSameAs(staticClassTypeInfo);
        }

        [Fact(DisplayName = "MustBeStaticClass must throw an ArgumentNullException when the specified type is null.")]
        public void TypeNull()
        {
            Action act = () => ((Type) null).MustBeStaticClass();

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact(DisplayName = "MustBeStaticClass must throw an ArgumentNullException when the specified TypeInfo is null.")]
        public void TypeInfoNull()
        {
            Action act = () => ((TypeInfo) null).MustBeStaticClass();

            act.Should().Throw<ArgumentNullException>();
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.AddExceptionTest(exception => typeof(IFormattable).MustBeStaticClass(exception: exception))
                    .AddMessageTest<TypeException>(message => typeof(Action).MustBeStaticClass(message: message));

            testData.AddExceptionTest(exception => typeof(decimal).GetTypeInfo().MustBeStaticClass(exception: exception))
                    .AddMessageTest<TypeException>(message => typeof(FieldInfo).GetTypeInfo().MustBeStaticClass(message: message));
        }
    }
}