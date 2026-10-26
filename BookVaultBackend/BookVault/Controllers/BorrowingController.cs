using BookVault.Application.Authorization;
using BookVault.Application.DTOs.Borrowings.Requests;
using BookVault.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookVault.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BorrowingController : ControllerBase
{
	private readonly IBorrowingService _borrowingService;

	public BorrowingController(IBorrowingService borrowingService)
	{
		_borrowingService = borrowingService;
	}

	[RequirePermission("BORROWING_READ")]
	[Authorize]
	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		var result = await _borrowingService.GetAllAsync();

		return Ok(result);
	}

	[RequirePermission("BORROWING_READ")]
	[Authorize]
	[HttpGet("{id:int}")]
	public async Task<IActionResult> GetById(int id)
	{
		var result = await _borrowingService.GetByIdAsync(id);

		if (result == null)
			return NotFound();

		return Ok(result);
	}

	[RequirePermission("BORROWING_CREATE")]
	[Authorize]
	[HttpPost]
	public async Task<IActionResult> Create(
		[FromBody] CreateBorrowingDto dto)
	{
		var result = await _borrowingService.CreateAsync(dto);

		return CreatedAtAction(
			nameof(GetById),
			new { id = result.Id },
			result);
	}

	[RequirePermission("BORROWING_UPDATE")]
	[Authorize]
	[HttpPut("{id:int}")]
	public async Task<IActionResult> Update(
		int id,
		[FromBody] UpdateBorrowingDto dto)
	{
		await _borrowingService.UpdateAsync(id, dto);

		return NoContent();
	}
}
