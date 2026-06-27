using LibraryManagement.Application.Authors.DTOs;
using LibraryManagement.Application.Authors.Interfaces;
using LibraryManagement.Application.Common.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers
{

    [ApiController]
    [Route("api/authors")]
    [Authorize]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AuthorResponse>>> GetAll(
            CancellationToken cancellationToken)
        {
            var authors = await _authorService.GetAllAsync(cancellationToken);

            return Ok(authors);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AuthorResponse>> GetById(
            int id,
            CancellationToken cancellationToken)
        {
            var author = await _authorService.GetByIdAsync(id, cancellationToken);

            if (author is null)
                return NotFound(new { message = "Author was not found." });

            return Ok(author);
        }

        [HttpPost]
        [Authorize(Roles = RoleNames.AdministratorOrLibrarian)]
        public async Task<ActionResult<AuthorResponse>> Create(
            CreateAuthorRequest request,
            CancellationToken cancellationToken)
        {
            var author = await _authorService.CreateAsync(request, cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = author.Id },
                author);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = RoleNames.AdministratorOrLibrarian)]
        public async Task<ActionResult<AuthorResponse>> Update(
            int id,
            UpdateAuthorRequest request,
            CancellationToken cancellationToken)
        {
            var author = await _authorService.UpdateAsync(id, request, cancellationToken);

            return Ok(author);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = RoleNames.Administrator)]
        public async Task<IActionResult> Delete(
            int id,
            CancellationToken cancellationToken)
        {
            await _authorService.DeleteAsync(id, cancellationToken);

            return NoContent();
        }
    }
}
