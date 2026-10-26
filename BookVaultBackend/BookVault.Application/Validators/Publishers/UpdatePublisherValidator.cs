using BookVault.Application.DTOs.Publishers.Requests;
using FluentValidation;

namespace BookVault.Application.Validators.Publishers;

public class UpdatePublisherValidator
	: AbstractValidator<UpdatePublisherDto>
{
	public UpdatePublisherValidator()
	{
		RuleFor(x => x.Publishername)
			.NotEmpty()
			.WithMessage("Publisher name is required.")
			.MaximumLength(30)
			.WithMessage("Publisher name cannot exceed 150 characters.");

		RuleFor(x => x.Website)
			.NotEmpty()
			.WithMessage("Website is required.")
			.MaximumLength(30)
			.WithMessage("Website cannot exceed 30 characters.");

		RuleFor(x => x.Email)
			.NotEmpty()
			.WithMessage("Email is required.")
			.EmailAddress()
			.WithMessage("Email format is invalid.");

		RuleFor(x => x.Address)
			.NotEmpty()
			.WithMessage("Address is required.")
			.MaximumLength(50)
			.WithMessage("Address cannot exceed 50 characters.");

		RuleFor(x => x.City)
			.NotEmpty()
			.WithMessage("City is required.")
			.MaximumLength(30)
			.WithMessage("City cannot exceed 30 characters.");

		RuleFor(x => x.Phone)
			.NotEmpty()
			.WithMessage("Phone is required.")
			.MaximumLength(20)
			.WithMessage("Phone number cannot exceed 20 characters.");
	}
}