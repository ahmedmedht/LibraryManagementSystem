using LibraryManagement.Application.Borrowings.DTOs;
using LibraryManagement.Application.Borrowings.Interfaces;
using LibraryManagement.Application.Common.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers
{
    [ApiController]
    [Route("api/borrowings")]
    [Authorize(Roles = RoleNames.AllSystemUsers)]
    public class BorrowingsController : ControllerBase
    {
        private readonly IBorrowingService _borrowingService;

        public BorrowingsController(IBorrowingService borrowingService)
        {
            _borrowingService = borrowingService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<BorrowingResponse>>> GetAll(
            CancellationToken cancellationToken)
        {
            var borrowings = await _borrowingService.GetAllAsync(cancellationToken);

            return Ok(borrowings);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BorrowingResponse>> GetById(
            int id,
            CancellationToken cancellationToken)
        {
            var borrowing = await _borrowingService.GetByIdAsync(id, cancellationToken);

            if (borrowing is null)
                return NotFound(new { message = "Borrowing transaction was not found." });

            return Ok(borrowing);
        }

        [HttpPost]
        public async Task<ActionResult<BorrowingResponse>> BorrowBook(
            BorrowBookRequest request,
            CancellationToken cancellationToken)
        {
            var borrowing = await _borrowingService.BorrowBookAsync(
                request,
                cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = borrowing.Id },
                borrowing);
        }

        [HttpPost("{id:int}/return")]
        public async Task<ActionResult<BorrowingResponse>> ReturnBook(
            int id,
            CancellationToken cancellationToken)
        {
            var borrowing = await _borrowingService.ReturnBookAsync(
                id,
                cancellationToken);

            return Ok(borrowing);
        }
    }
}
