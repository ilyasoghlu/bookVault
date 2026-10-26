using BookVault.Application.DTOs.Rules;
using FluentValidation;

namespace BookVault.Application.Validators.Rules;

public class CreateRuleValidator : AbstractValidator<CreateRuleRequest>
{
	public CreateRuleValidator()
	{
		RuleFor(x => x.Code)
			.NotEmpty()
			.MaximumLength(100);

		RuleFor(x => x.RuleName)
			.NotEmpty()
			.MaximumLength(150);

		RuleFor(x => x.Description)
			.MaximumLength(500);
	}
}

