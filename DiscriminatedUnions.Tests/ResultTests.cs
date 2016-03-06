using FluentAssertions;
using System;
using System.IO;
using Xunit;

namespace Geosnap.DiscriminatedUnions.Tests
{
    public class ResultTests
    {
        [Fact]
        public void CreateValueTest()
        {
            var v = Result<string>.Of("test");
            v.Should().NotBeNull();
            v.GetType().Should().Be(typeof(Result<string>.Success));
        }

        [Fact]
        public void CreateErrorTest()
        {
            var e = Result<object>.Error(new Exception());
            e.Should().NotBeNull();
            e.GetType().Should().Be(typeof(Result<object>.Failure));
        }

        [Theory]
        [InlineData("test", typeof(Result<string>.Success))]
        [InlineData(null, typeof(Result<string>.Failure))]
        public void CreateFromOptionalTest(string value, Type resultType)
        {
            var result = Result<string>.Of(value, () => new Exception());
            result.GetType().Should().Be(resultType);
        }

        [Theory]
        [InlineData("test", typeof(Result<string>.Success))]
        [InlineData(null, typeof(Result<string>.Failure))]
        public void CreateFromLambdaTest(string val, Type resultType)
        {
            Func<string> fun = () =>
            {
                if (val == null) { throw new Exception(); }                    
                return val;
            };
            var result = Result<string>.Of(fun);
            result.GetType().Should().Be(resultType);
        }

        [Fact]
        public void OrTest()
        {
            const string fallback = "test";
            var result = Result<string>.Of((string)null).Or(fallback);
            result.GetType().Should().Be(typeof(Result<string>.Success));
            ((string)result).Should().Be(fallback);
        }

        [Fact]
        public void OrElseTest()
        {
            const string fallback = "test";
            var result = Result<string>.Of((string)null).GetOrElse(fallback);
            result.Should().Be(fallback);
        }

        [Fact]
        public void SuccessTest()
        {
            var result = Result<object>.Of(true);

            var beingCalled = false;
            result.OnSuccess((_) => beingCalled = true);

            var notBeingCalled = true;
            result.OnFailure((_) => notBeingCalled = false);

            beingCalled.Should().BeTrue();
            notBeingCalled.Should().BeTrue();
        }

        [Fact]
        public void FailureTest()
        {
            var result = Result<string>.Of(() => File.ReadAllText("file_not_found"));

            var beingCalled = false;
            result.OnFailure((_) => beingCalled = true);

            var notBeingCalled = true;
            result.OnSuccess((_) => notBeingCalled = false);

            beingCalled.Should().BeTrue();
            notBeingCalled.Should().BeTrue();
        }

        [Theory]
        [InlineData("test", typeof(Result<string>.Success))]
        [InlineData(null, typeof(Result<string>.Failure))]
        public void GetTest(string val, Type resultType)
        {
            Func<string> fun = () =>
            {
                if (val == null) { throw new Exception(); }
                return val;
            };
            var result = Result<string>.Of(fun);
            string shouldntBeNull = null;
            try
            {
                shouldntBeNull = result.Get();
            }
            catch (Exception)
            {
                shouldntBeNull = "test";
            }

            shouldntBeNull.Should().NotBeNullOrWhiteSpace();
        }
    }
}
