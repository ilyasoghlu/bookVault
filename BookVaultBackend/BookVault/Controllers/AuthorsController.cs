using BookVault.Application.Authorization;
using BookVault.Application.DTOs.Authors.Requests;
using BookVault.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookVault.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
	private readonly IAuthorService _authorService;

	public AuthorsController(IAuthorService authorService)
	{
		_authorService = authorService;
	}

	[RequirePermission("AUTHOR_READ")]
	[Authorize]
	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		var authors = await _authorService.GetAllAsync();

		return Ok(authors);
	}

	[RequirePermission("AUTHOR_READ")]
	[Authorize]
	[HttpGet("{id:guid}")]
	public async Task<IActionResult> GetById(Guid id)
	{
		var author = await _authorService.GetByIdAsync(id);

		return Ok(author);
	}

	[RequirePermission("AUTHOR_CREATE")]
	[Authorize]
	[HttpPost]
	public async Task<IActionResult> Create(
		[FromBody] CreateAuthorDto dto)
	{
		var id = await _authorService.CreateAsync(dto);

		return CreatedAtAction(
			nameof(GetById),
			new { id },
			id);
	}

	[RequirePermission("AUTHOR_UPDATE")]
	[Authorize]
	[HttpPut("{id:guid}")]
	public async Task<IActionResult> Update(
		Guid id,
		[FromBody] UpdateAuthorDto dto)
	{
		await _authorService.UpdateAsync(id, dto);

		return NoContent();
	}

	[RequirePermission("AUTHOR_DELETE")]
	[Authorize]
	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> Delete(Guid id)
	{
		await _authorService.DeleteAsync(id);

		return NoContent();
	}
}

