namespace BookVault.Application.DTOs.Employee.Responses;

public class EmployeeDto
{
    public int Id { get; set; }

    public string Firstname { get; set; } = string.Empty;

    public string Lastname { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Employeeposition { get; set; } = string.Empty;

    public DateOnly Hiredate { get; set; }

    public DateOnly? Enddate { get; set; }
}