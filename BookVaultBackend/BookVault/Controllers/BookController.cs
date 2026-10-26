using BookVault.Application.Authorization;
using BookVault.Application.DTOs.Books.Requests;
using BookVault.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace BookVault.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;

    public BookController(IBookService bookService)
    {
        _bookService = bookService;
    }

	[RequirePermission("BOOK_READ")]
	[Authorize]
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var books = await _bookService.GetAllAsync();

        return Ok(books);
    }

	[RequirePermission("BOOK_READ")]
	[Authorize]
	[HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var book = await _bookService.GetByIdAsync(id);

		if (book == null)
			return NotFound();

		return Ok(book);
    }


	[RequirePermission("BOOK_CREATE")]
	[Authorize]
	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateBookDto dto)
    {
        var id = await _bookService.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id },
            new { id }
        );
    }


	[RequirePermission("BOOK_UPDATE")]
	[Authorize]
	[HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateBookDto dto)
    {
        await _bookService.UpdateAsync(id, dto);

        return NoContent();
    }


	[RequirePermission("BOOK_DELETE")]
	[Authorize]
	[HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status409Conflict)]
	public async Task<IActionResult> Delete(Guid id)
    {
        await _bookService.DeleteAsync(id);

        return NoContent();
    }
}