using BookVault.Application.DTOs.Authors.Requests;
using FluentValidation;

namespace BookVault.Application.Validators.Authors;

public class UpdateAuthorValidator : AbstractValidator<UpdateAuthorDto>
{
	public UpdateAuthorValidator()
	{
		RuleFor(x => x.Firstname)
			.NotEmpty()
			.MaximumLength(30);

		RuleFor(x => x.Lastname)
			.NotEmpty()
			.MaximumLength(30);

		RuleFor(x => x.Nationality)
			.NotEmpty()
			.MaximumLength(30);

		RuleFor(x => x.Biography)
			.NotEmpty();

		RuleFor(x => x.Birthdate)
			.LessThanOrEqualTo(
				DateOnly.FromDateTime(DateTime.UtcNow));

		RuleFor(x => x.Deathdate)
			.Must((dto, deathDate) =>
				!deathDate.HasValue ||
				(deathDate.Value > dto.Birthdate &&
				 deathDate.Value <= DateOnly.FromDateTime(DateTime.UtcNow)))
			.WithMessage(
				"Deathdate must be later than Birthdate and cannot be in the future.");
	}
}