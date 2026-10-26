using BookVault.Application.DTOs.Books.Requests;
using FluentValidation;

namespace BookVault.Application.Validators.Books;

public class CreateBookValidator : AbstractValidator<CreateBookDto>
{
	public CreateBookValidator()
	{
		RuleFor(x => x.Isbn)
			.NotEmpty()
			.MaximumLength(36);

		RuleFor(x => x.Title)
			.NotEmpty()
			.MaximumLength(150);

		RuleFor(x => x.Category)
			.NotEmpty()
			.MaximumLength(50);

		RuleFor(x => x.Genre)
			.NotEmpty()
			.MaximumLength(50);

		RuleFor(x => x.Publicationdate)
			.LessThanOrEqualTo(
				DateOnly.FromDateTime(DateTime.UtcNow))
			.WithMessage("Publication date cannot be in the future.");

		RuleFor(x => x.Pagecount)
			.GreaterThan(0);

		RuleFor(x => x.Edition)
			.NotEmpty()
			.MaximumLength(50);

		RuleFor(x => x.Totalcopies)
			.GreaterThan(0);

		RuleFor(x => x.Language)
			.NotEmpty()
			.MaximumLength(50);

		RuleFor(x => x.Authorid)
			.NotNull()
			.NotEqual(Guid.Empty);

		RuleFor(x => x.Publisherid)
			.NotNull()
			.NotEqual(Guid.Empty);

		RuleFor(x => x.Shelfid)
			.NotNull()
			.GreaterThan(0);
	}
}