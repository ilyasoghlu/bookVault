using BookVault.Application.Authorization;
using BookVault.Application.DTOs.Room.Requests;
using BookVault.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookVault.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomController : ControllerBase
{
	private readonly IRoomService _roomService;

	public RoomController(IRoomService roomService)
	{
		_roomService = roomService;
	}

	[RequirePermission("ROOM_READ")]
	[Authorize]
	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		var rooms = await _roomService.GetAllAsync();

		return Ok(rooms);
	}

	[RequirePermission("ROOM_READ")]
	[Authorize]
	[HttpGet("{id:int}")]
	public async Task<IActionResult> GetById(int id)
	{
		var room = await _roomService.GetByIdAsync(id);

		if (room == null)
			return NotFound();

		return Ok(room);
	}

	[RequirePermission("ROOM_CREATE")]
	[Authorize]
	[HttpPost]
	public async Task<IActionResult> Create(
		[FromBody] CreateRoomDto dto)
	{
		var room = await _roomService.CreateAsync(dto);

		return CreatedAtAction(
			nameof(GetById),
			new { id = room.Id },
			room
		);
	}

	[RequirePermission("ROOM_UPDATE")]
	[Authorize]
	[HttpPut("{id:int}")]
	public async Task<IActionResult> Update(
		int id,
		[FromBody] UpdateRoomDto dto)
	{
		await _roomService.UpdateAsync(id, dto);

		return NoContent();
	}

	[RequirePermission("ROOM_DELETE")]
	[Authorize]
	[HttpDelete("{id:int}")]
	public async Task<IActionResult> Delete(int id)
	{
		await _roomService.DeleteAsync(id);

		return NoContent();
	}
}

