using Bloomia.Application.Modules.Reviews.Commands.Create;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Tests.Reviews
{
    public class CreateReviewCommandValidatorTests
    {
        [Fact]
        public void Validate_ShouldFail_WhenRatingIsGreaterThan5()
        {
            var validator = new CreateReviewCommandValidator();
            var command = new CreateReviewCommand
            {
                TherapistId = 1,
                Rating = 6,
                Comment = "Test komentar"
            };

            var result = validator.Validate(command);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage == "Rating must be between 1 and 5.");
        }
    }
}
