using LibraryManagement.Application.Common.Security;
using LibraryManagement.Application.Publishers.DTOs;
using LibraryManagement.Application.Publishers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers
{

    [ApiController]
    [Route("api/publishers")]
    [Authorize]
    public class PublishersController : ControllerBase
    {
        private readonly IPublisherService _publisherService;

        public PublishersController(IPublisherService publisherService)
        {
            _publisherService = publisherService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<PublisherResponse>>> GetAll(
            CancellationToken cancellationToken)
        {
            var publishers = await _publisherService.GetAllAsync(cancellationToken);

            return Ok(publishers);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PublisherResponse>> GetById(
            int id,
            CancellationToken cancellationToken)
        {
            var publisher = await _publisherService.GetByIdAsync(id, cancellationToken);

            if (publisher is null)
                return NotFound(new { message = "Publisher was not found." });

            return Ok(publisher);
        }

        [HttpPost]
        [Authorize(Roles = RoleNames.AdministratorOrLibrarian)]
        public async Task<ActionResult<PublisherResponse>> Create(
            CreatePublisherRequest request,
            CancellationToken cancellationToken)
        {
            var publisher = await _publisherService.CreateAsync(request, cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = publisher.Id },
                publisher);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = RoleNames.AdministratorOrLibrarian)]
        public async Task<ActionResult<PublisherResponse>> Update(
            int id,
            UpdatePublisherRequest request,
            CancellationToken cancellationToken)
        {
            var publisher = await _publisherService.UpdateAsync(id, request, cancellationToken);

            return Ok(publisher);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = RoleNames.Administrator)]
        public async Task<IActionResult> Delete(
            int id,
            CancellationToken cancellationToken)
        {
            await _publisherService.DeleteAsync(id, cancellationToken);

            return NoContent();
        }
    }
}
