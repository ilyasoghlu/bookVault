using BookVault.Application.Authorization;
using BookVault.Application.DTOs.Section.Requests;
using BookVault.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookVault.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SectionController : ControllerBase
{
	private readonly ISectionService _sectionService;

	public SectionController(ISectionService sectionService)
	{
		_sectionService = sectionService;
	}

	[RequirePermission("SECTION_READ")]
	[Authorize]
	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		var sections = await _sectionService.GetAllAsync();

		return Ok(sections);
	}

	[RequirePermission("SECTION_READ")]
	[Authorize]
	[HttpGet("{id:int}")]
	public async Task<IActionResult> GetById(int id)
	{
		var section = await _sectionService.GetByIdAsync(id);


		return Ok(section);
	}

	[RequirePermission("SECTION_CREATE")]
	[Authorize]
	[HttpPost]
	public async Task<IActionResult> Create(
		[FromBody] CreateSectionDto dto)
	{
		var id = await _sectionService.CreateAsync(dto);

		return CreatedAtAction(
			nameof(GetById),
			new { id },
			id
		);
	}

	[RequirePermission("SECTION_UPDATE")]
	[Authorize]
	[HttpPut("{id:int}")]
	public async Task<IActionResult> Update(
		int id,
		[FromBody] UpdateSectionDto dto)
	{
		await _sectionService.UpdateAsync(id, dto);

		return NoContent();
	}

	[RequirePermission("SECTION_DELETE")]
	[Authorize]
	[HttpDelete("{id:int}")]
	public async Task<IActionResult> Delete(int id)
	{
		await _sectionService.DeleteAsync(id);

		return NoContent();
	}
}

