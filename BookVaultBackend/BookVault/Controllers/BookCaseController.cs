using BookVault.Application.Authorization;
using BookVault.Application.DTOs.BookCase.Requests;
using BookVault.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookVault.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookCaseController : ControllerBase
{
	private readonly IBookCaseService _bookCaseService;

	public BookCaseController(
		IBookCaseService bookCaseService)
	{
		_bookCaseService = bookCaseService;
	}

	[RequirePermission("BOOKCASE_READ")]
	[Authorize]
	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		var bookCases = await _bookCaseService.GetAllAsync();

		return Ok(bookCases);
	}

	[RequirePermission("BOOKCASE_READ")]
	[Authorize]
	[HttpGet("{id:int}")]
	public async Task<IActionResult> GetById(int id)
	{
		var bookCase = await _bookCaseService.GetByIdAsync(id);

		if (bookCase == null)
			return NotFound();

		return Ok(bookCase);
	}

	[RequirePermission("BOOKCASE_CREATE")]
	[Authorize]
	[HttpPost]
	public async Task<IActionResult> Create(
		[FromBody] CreateBookCaseDto dto)
	{
		var result = await _bookCaseService.CreateAsync(dto);

		return CreatedAtAction(
			nameof(GetById),
			new { id = result.Id },
			result
		);
	}

	[RequirePermission("BOOKCASE_UPDATE")]
	[Authorize]
	[HttpPut("{id:int}")]
	public async Task<IActionResult> Update(
		int id,
		[FromBody] UpdateBookCaseDto dto)
	{
		await _bookCaseService.UpdateAsync(id, dto);

		return NoContent();
	}

	[RequirePermission("BOOKCASE_DELETE")]
	[Authorize]
	[HttpDelete("{id:int}")]
	public async Task<IActionResult> Delete(int id)
	{
		await _bookCaseService.DeleteAsync(id);

		return NoContent();
	}
}

