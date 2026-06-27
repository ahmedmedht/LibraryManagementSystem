using LibraryManagement.Application.Books.DTOs;
using LibraryManagement.Application.Books.Interfaces;
using LibraryManagement.Application.Common.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers
{

    [ApiController]
    [Route("api/books")]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<BookResponse>>> Search(
            [FromQuery] BookSearchRequest request,
            CancellationToken cancellationToken)
        {
            var books = await _bookService.SearchAsync(request, cancellationToken);

            return Ok(books);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BookResponse>> GetById(
            int id,
            CancellationToken cancellationToken)
        {
            var book = await _bookService.GetByIdAsync(id, cancellationToken);

            if (book is null)
                return NotFound(new { message = "Book was not found." });

            return Ok(book);
        }

        [HttpPost]
        [Authorize(Roles = RoleNames.AdministratorOrLibrarian)]
        public async Task<ActionResult<BookResponse>> Create(
            CreateBookRequest request,
            CancellationToken cancellationToken)
        {
            var book = await _bookService.CreateAsync(request, cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = book.Id },
                book);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = RoleNames.AdministratorOrLibrarian)]
        public async Task<ActionResult<BookResponse>> Update(
            int id,
            UpdateBookRequest request,
            CancellationToken cancellationToken)
        {
            var book = await _bookService.UpdateAsync(id, request, cancellationToken);

            return Ok(book);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = RoleNames.Administrator)]
        public async Task<IActionResult> Delete(
            int id,
            CancellationToken cancellationToken)
        {
            await _bookService.DeleteAsync(id, cancellationToken);

            return NoContent();
        }
    }
}
