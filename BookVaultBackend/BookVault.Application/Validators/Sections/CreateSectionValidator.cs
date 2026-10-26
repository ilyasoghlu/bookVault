using BookVault.Application.DTOs.Section.Requests;
using FluentValidation;

namespace BookVault.Application.Validators.Section;

public class CreateSectionValidator : AbstractValidator<CreateSectionDto>
{
	public CreateSectionValidator()
	{
		RuleFor(x => x.Title)
			.NotEmpty()
			.MaximumLength(30);

		RuleFor(x => x.Sectionnumber)
			.GreaterThan(0)
			.WithMessage("Section number must be greater than zero.");

		RuleFor(x => x.Capacity)
			.GreaterThanOrEqualTo(0)
			.WithMessage("Capacity cannot be negative.");

		RuleFor(x => x.Sectionfloor)
			.GreaterThanOrEqualTo(0)
			.WithMessage("Section floor cannot be negative.");

		RuleFor(x => x.Description)
			.MaximumLength(500);

		RuleFor(x => x.Roomid)
			.GreaterThan(0)
			.WithMessage("Room ID must be greater than zero.");
	}
}