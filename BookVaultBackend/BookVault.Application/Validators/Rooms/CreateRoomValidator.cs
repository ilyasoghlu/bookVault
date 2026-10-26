using BookVault.Application.DTOs.Room.Requests;
using FluentValidation;

namespace BookVault.Application.Validators.Room;

public class CreateRoomValidator : AbstractValidator<CreateRoomDto>
{
	public CreateRoomValidator()
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

		RuleFor(x => x.Roomfloor)
			.GreaterThanOrEqualTo(0)
			.WithMessage("Room floor cannot be negative.");

		RuleFor(x => x.Description)
			.MaximumLength(500);

	
	}
}