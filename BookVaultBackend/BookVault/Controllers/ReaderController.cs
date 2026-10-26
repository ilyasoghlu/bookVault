using BookVault.Application.Authorization;
using BookVault.Application.DTOs.Reader.Requests;
using BookVault.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookVault.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReaderController : ControllerBase
{
	private readonly IReaderService _readerService;

	public ReaderController(IReaderService readerService)
	{
		_readerService = readerService;
	}

	[RequirePermission("READER_READ")]
	[Authorize]
	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		var result = await _readerService.GetAllAsync();

		return Ok(result);
	}

	[RequirePermission("READER_READ")]
	[Authorize]
	[HttpGet("{id:int}")]
	public async Task<IActionResult> GetById(int id)
	{
		var result = await _readerService.GetByIdAsync(id);

		if (result == null)
			return NotFound();

		return Ok(result);
	}

	[RequirePermission("READER_CREATE")]
	[Authorize]
	[HttpPost]
	public async Task<IActionResult> Create(
		[FromBody] CreateReaderDto dto)
	{
		var result = await _readerService.CreateAsync(dto);

		return CreatedAtAction(
			nameof(GetById),
			new { id = result.Id },
			result);
	}

	[RequirePermission("READER_UPDATE")]
	[Authorize]
	[HttpPut("{id:int}")]
	public async Task<IActionResult> Update(
		int id,
		[FromBody] UpdateReaderDto dto)
	{
		await _readerService.UpdateAsync(id, dto);

		return NoContent();
	}

	[RequirePermission("READER_DELETE")]
	[Authorize]
	[HttpDelete("{id:int}")]
	public async Task<IActionResult> Delete(int id)
	{
		await _readerService.DeleteAsync(id);

		return NoContent();
	}
}

