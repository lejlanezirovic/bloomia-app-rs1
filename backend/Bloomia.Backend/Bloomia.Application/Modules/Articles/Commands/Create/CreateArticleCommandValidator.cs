using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Articles.Commands.Create
{
    public sealed class CreateArticleCommandValidator : AbstractValidator<CreateArticleCommand>
    {
        public CreateArticleCommandValidator()
        {
            RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required.")
            .MinimumLength(5)
            .WithMessage("Title must contain at least 5 characters.")
            .MaximumLength(150)
            .WithMessage("Title cannot exceed 150 characters.");

            RuleFor(x => x.Content)
                .NotEmpty()
                .WithMessage("Content is required.")
                .MinimumLength(20)
                .WithMessage("Content must contain at least 20 characters.")
                .MaximumLength(5000)
                .WithMessage("Content cannot exceed 5000 characters.");
        }
    }
}
