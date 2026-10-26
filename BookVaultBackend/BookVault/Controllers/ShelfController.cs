using BookVault.Application.Authorization;
using BookVault.Application.DTOs.Shelf.Requests;
using BookVault.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookVault.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShelfController : ControllerBase
{
	private readonly IShelfService _shelfService;

	public ShelfController(IShelfService shelfService)
	{
		_shelfService = shelfService;
	}

	[RequirePermission("SHELF_READ")]
	[Authorize]
	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		var shelves = await _shelfService.GetAllAsync();

		return Ok(shelves);
	}

	[RequirePermission("SHELF_READ")]
	[Authorize]
	[HttpGet("{id:int}")]
	public async Task<IActionResult> GetById(int id)
	{
		var shelf = await _shelfService.GetByIdAsync(id);

		if (shelf == null)
			return NotFound();

		return Ok(shelf);
	}

	[RequirePermission("SHELF_CREATE")]
	[Authorize]
	[HttpPost]
	public async Task<IActionResult> Create(
		[FromBody] CreateShelfDto dto)
	{
		var shelf = await _shelfService.CreateAsync(dto);

		return CreatedAtAction(
			nameof(GetById),
			new { id = shelf.Id },
			shelf
		);
	}

	[RequirePermission("SHELF_UPDATE")]
	[Authorize]
	[HttpPut("{id:int}")]
	public async Task<IActionResult> Update(
		int id,
		[FromBody] UpdateShelfDto dto)
	{
		await _shelfService.UpdateAsync(id, dto);

		return NoContent();
	}

	[RequirePermission("SHELF_DELETE")]
	[Authorize]
	[HttpDelete("{id:int}")]
	public async Task<IActionResult> Delete(int id)
	{
		await _shelfService.DeleteAsync(id);

		return NoContent();
	}
}

