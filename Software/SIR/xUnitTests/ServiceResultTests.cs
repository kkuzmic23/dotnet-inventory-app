using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace xUnitTests
{
    public class ServiceResultTests
    {
        [Fact]
        public void Success_ReturnsSuccessfulResult()
        {
            var result = ServiceResult.Success();

            Assert.True(result.IsSuccessful);
            Assert.Null(result.ErrorMessage);
        }

        [Fact]
        public void Failure_ReturnsUnsuccessfulResultWithErrorMessage()
        {
            var errorMessage = "Something went wrong.";

            var result = ServiceResult.Failure(errorMessage);

            Assert.False(result.IsSuccessful);
            Assert.Equal(errorMessage, result.ErrorMessage);
        }

        [Fact]
        public void Failure_ReturnsUnsuccessfulResultWithEmptyErrorMessage()
        {
            var result = ServiceResult.Failure(string.Empty);

            Assert.False(result.IsSuccessful);
            Assert.Equal(string.Empty, result.ErrorMessage);
        }

        [Theory]
        [InlineData("Database connection failed.")]
        [InlineData("Unauthorized access.")]
        [InlineData("Item not found.")]
        public void Failure_DisplaysGivenErrorMessage(string errorMessage)
        {
            var result = ServiceResult.Failure(errorMessage);

            Assert.Equal(errorMessage, result.ErrorMessage);
        }
    }
}
