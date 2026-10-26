using BookVault.Application.DTOs.Employee.Requests;
using FluentValidation;

namespace BookVault.Application.Validators.Employee;

public class CreateEmployeeValidator : AbstractValidator<CreateEmployeeDto>
{
	public CreateEmployeeValidator()
	{
		RuleFor(x => x.Firstname)
			.NotEmpty()
			.MaximumLength(30);

		RuleFor(x => x.Lastname)
			.NotEmpty()
			.MaximumLength(30);

		RuleFor(x => x.Email)
			.NotEmpty()
			.EmailAddress()
			.MaximumLength(50);

		RuleFor(x => x.Phone)
			.NotEmpty()
			.MaximumLength(20);

		RuleFor(x => x.Username)
			.NotEmpty()
			.MaximumLength(30);

		RuleFor(x => x.Password)
			.NotEmpty()
			.MinimumLength(6);

		RuleFor(x => x.Employeeposition)
			.NotEmpty()
			.MaximumLength(30);

		RuleFor(x => x.Hiredate)
			.LessThanOrEqualTo(
				DateOnly.FromDateTime(DateTime.UtcNow))
			.WithMessage("Hire date cannot be in the future.");

		RuleFor(x => x.Enddate)
			.Must((dto, endDate) =>
				!endDate.HasValue ||
				endDate.Value >= dto.Hiredate)
			.WithMessage("End date cannot be earlier than hire date.");
	}
}