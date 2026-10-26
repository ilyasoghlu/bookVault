using BookVault.Application.DTOs.Rules;
using FluentValidation;

namespace BookVault.Application.Validators.Rules;

public class AssignPermissionValidator : AbstractValidator<AssignPermissionRequest>
{
	public AssignPermissionValidator()
	{
		RuleFor(x => x.UserId)
			.NotEqual(Guid.Empty)
			.WithMessage("UserId is required.");

		RuleFor(x => x.RuleCode)
			.NotEmpty()
			.MaximumLength(100);

		RuleFor(x => x.ExpiresAt)
			.Must(expiresAt =>
				!expiresAt.HasValue ||
				expiresAt.Value > DateTimeOffset.UtcNow)
			.WithMessage("Expiration date must be in the future.");
	}
}