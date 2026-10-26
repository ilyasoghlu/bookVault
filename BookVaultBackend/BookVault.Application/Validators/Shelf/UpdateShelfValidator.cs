using BookVault.Application.DTOs.Shelf.Requests;
using FluentValidation;

namespace BookVault.Application.Validators.Shelf;

public class UpdateShelfValidator : AbstractValidator<UpdateShelfDto>
{
	public UpdateShelfValidator()
	{
		RuleFor(x => x.Shelvecode)
			.GreaterThan(0)
			.WithMessage("Shelf code must be greater than zero.");

		RuleFor(x => x.Capacity)
			.GreaterThanOrEqualTo(0)
			.WithMessage("Capacity cannot be negative.");


	}
}