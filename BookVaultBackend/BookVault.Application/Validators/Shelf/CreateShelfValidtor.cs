using BookVault.Application.DTOs.Shelf.Requests;
using FluentValidation;

namespace BookVault.Application.Validators.Shelf;

public class CreateShelfValidator : AbstractValidator<CreateShelfDto>
{
	public CreateShelfValidator()
	{
		RuleFor(x => x.Shelvecode)
			.GreaterThan(0)
			.WithMessage("Shelf code must be greater than zero.");

		RuleFor(x => x.Capacity)
			.GreaterThanOrEqualTo(0)
			.WithMessage("Capacity cannot be negative.");

		RuleFor(x => x.Caseid)
			.GreaterThan(0)
			.WithMessage("Case ID must be greater than zero.");
	}
}