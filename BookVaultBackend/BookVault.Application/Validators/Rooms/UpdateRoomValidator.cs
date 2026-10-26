using BookVault.Application.DTOs.Room.Requests;
using FluentValidation;

namespace BookVault.Application.Validators.Room;

public class UpdateRoomValidator : AbstractValidator<UpdateRoomDto>
{
	public UpdateRoomValidator()
	{
		RuleFor(x => x.Title)
			.NotEmpty()
			.MaximumLength(100);

		RuleFor(x => x.Roomnumber)
			.GreaterThan(0)
			.WithMessage("Room number must be greater than zero.");

		RuleFor(x => x.Capacity)
			.GreaterThanOrEqualTo(0)
			.WithMessage("Capacity cannot be negative.");

	
		RuleFor(x => x.Description)
			.MaximumLength(500);

		RuleFor(x => x.Employeeid)
			.GreaterThan(0)
			.When(x => x.Employeeid.HasValue)
			.WithMessage("Employee ID must be greater than zero.");
	}
}