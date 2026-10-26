using BookVault.Application.DTOs.BookCase.Requests;
using FluentValidation;

namespace BookVault.Application.Validators.BookCase;

public class UpdateBookCaseValidator : AbstractValidator<UpdateBookCaseDto>
{
	public UpdateBookCaseValidator()
	{
		RuleFor(x => x.Casenumber)
			.GreaterThan(0)
			.WithMessage("Case number must be greater than zero.");

		RuleFor(x => x.Capacity)
			.GreaterThanOrEqualTo(0)
			.WithMessage("Capacity cannot be negative.");

		RuleFor(x => x.Sectionid)
			.GreaterThan(0)
			.WithMessage("Section ID must be greater than zero.");
	}
}