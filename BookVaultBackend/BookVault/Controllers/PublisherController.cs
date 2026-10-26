using BookVault.Application.Authorization;
using BookVault.Application.DTOs.Publishers.Requests;
using BookVault.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookVault.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PublishersController : ControllerBase
{
	private readonly IPublisherService _service;

	public PublishersController(IPublisherService service)
	{
		_service = service;
	}

	[RequirePermission("PUBLISHER_READ")]
	[Authorize]
	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		var publishers = await _service.GetAllAsync();

		return Ok(publishers);
	}

	[RequirePermission("PUBLISHER_READ")]
	[Authorize]
	[HttpGet("{id:guid}")]
	public async Task<IActionResult> GetById(Guid id)
	{
		var publisher = await _service.GetByIdAsync(id);

		return Ok(publisher);
	}

	[RequirePermission("PUBLISHER_CREATE")]
	[Authorize]
	[HttpPost]
	public async Task<IActionResult> Create(
		[FromBody] CreatePublisherDto dto)
	{
		var id = await _service.CreateAsync(dto);

		return CreatedAtAction(
			nameof(GetById),
			new { id },
			id
		);
	}

	[RequirePermission("PUBLISHER_UPDATE")]
	[Authorize]
	[HttpPut("{id:guid}")]
	public async Task<IActionResult> Update(
		Guid id,
		[FromBody] UpdatePublisherDto dto)
	{
		await _service.UpdateAsync(id, dto);

		return NoContent();
	}

	[RequirePermission("PUBLISHER_DELETE")]
	[Authorize]
	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> Delete(Guid id)
	{
		await _service.DeleteAsync(id);

		return NoContent();
	}
}

