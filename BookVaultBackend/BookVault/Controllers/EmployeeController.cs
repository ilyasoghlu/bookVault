using BookVault.Application.Authorization;
using BookVault.Application.DTOs.Employee.Requests;
using BookVault.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookVault.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
	private readonly IEmployeeService _employeeService;

	public EmployeeController(IEmployeeService employeeService)
	{
		_employeeService = employeeService;
	}

	[RequirePermission("EMPLOYEE_READ")]
	[Authorize]
	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		var employees = await _employeeService.GetAllAsync();

		return Ok(employees);
	}

	[RequirePermission("EMPLOYEE_READ")]
	[Authorize]
	[HttpGet("{id:int}")]
	public async Task<IActionResult> GetById(int id)
	{
		var employee = await _employeeService.GetByIdAsync(id);

		if (employee == null)
			return NotFound();

		return Ok(employee);
	}

	[RequirePermission("EMPLOYEE_CREATE")]
	[Authorize]
	[HttpPost]
	public async Task<IActionResult> Create(
		[FromBody] CreateEmployeeDto dto)
	{
		var employee = await _employeeService.CreateAsync(dto);

		return CreatedAtAction(
			nameof(GetById),
			new { id = employee.Id },
			employee
		);
	}

	[RequirePermission("EMPLOYEE_UPDATE")]
	[Authorize]
	[HttpPut("{id:int}")]
	public async Task<IActionResult> Update(
		int id,
		[FromBody] UpdateEmployeeDto dto)
	{
		await _employeeService.UpdateAsync(id, dto);

		return NoContent();
	}

	[RequirePermission("EMPLOYEE_DELETE")]
	[Authorize]
	[HttpDelete("{id:int}")]
	public async Task<IActionResult> Delete(int id)
	{
		await _employeeService.DeleteAsync(id);

		return NoContent();
	}
}

