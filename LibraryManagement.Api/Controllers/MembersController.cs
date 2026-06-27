using LibraryManagement.Application.Common.Security;
using LibraryManagement.Application.Members.DTOs;
using LibraryManagement.Application.Members.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers
{

    [ApiController]
    [Route("api/members")]
    [Authorize]
    public class MembersController : ControllerBase
    {
        private readonly IMemberService _memberService;

        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<MemberResponse>>> GetAll(
            CancellationToken cancellationToken)
        {
            var members = await _memberService.GetAllAsync(cancellationToken);

            return Ok(members);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MemberResponse>> GetById(
            int id,
            CancellationToken cancellationToken)
        {
            var member = await _memberService.GetByIdAsync(id, cancellationToken);

            if (member is null)
                return NotFound(new { message = "Member was not found." });

            return Ok(member);
        }

        [HttpPost]
        [Authorize(Roles = RoleNames.AdministratorOrLibrarian)]
        public async Task<ActionResult<MemberResponse>> Create(
            CreateMemberRequest request,
            CancellationToken cancellationToken)
        {
            var member = await _memberService.CreateAsync(request, cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = member.Id },
                member);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = RoleNames.AdministratorOrLibrarian)]
        public async Task<ActionResult<MemberResponse>> Update(
            int id,
            UpdateMemberRequest request,
            CancellationToken cancellationToken)
        {
            var member = await _memberService.UpdateAsync(id, request, cancellationToken);

            return Ok(member);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = RoleNames.Administrator)]
        public async Task<IActionResult> Delete(
            int id,
            CancellationToken cancellationToken)
        {
            await _memberService.DeleteAsync(id, cancellationToken);

            return NoContent();
        }

        [HttpPut("{id:int}/activate")]
        [Authorize(Roles = RoleNames.AdministratorOrLibrarian)]
        public async Task<IActionResult> Activate(
            int id,
            CancellationToken cancellationToken)
        {
            await _memberService.ActivateAsync(id, cancellationToken);

            return NoContent();
        }

        [HttpPut("{id:int}/deactivate")]
        [Authorize(Roles = RoleNames.AdministratorOrLibrarian)]
        public async Task<IActionResult> Deactivate(
            int id,
            CancellationToken cancellationToken)
        {
            await _memberService.DeactivateAsync(id, cancellationToken);

            return NoContent();
        }
    }
}
