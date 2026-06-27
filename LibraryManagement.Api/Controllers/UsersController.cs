using LibraryManagement.Application.Common.Security;
using LibraryManagement.Application.Users.DTOs;
using LibraryManagement.Application.Users.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize(Roles = RoleNames.Administrator)]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<UserResponse>>> GetAll(
            CancellationToken cancellationToken)
        {
            var users = await _userService.GetAllAsync(cancellationToken);

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponse>> GetById(
            string id,
            CancellationToken cancellationToken)
        {
            var user = await _userService.GetByIdAsync(id, cancellationToken);

            if (user is null)
                return NotFound(new { message = "User was not found." });

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserResponse>> Create(
            CreateUserRequest request,
            CancellationToken cancellationToken)
        {
            var user = await _userService.CreateAsync(request, cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = user.Id },
                user);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserResponse>> Update(
            string id,
            UpdateUserRequest request,
            CancellationToken cancellationToken)
        {
            var user = await _userService.UpdateAsync(id, request, cancellationToken);

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(
            string id,
            CancellationToken cancellationToken)
        {
            await _userService.DeleteAsync(id, cancellationToken);

            return NoContent();
        }

        [HttpPut("{id}/role")]
        public async Task<ActionResult<UserResponse>> ChangeRole(
            string id,
            ChangeUserRoleRequest request,
            CancellationToken cancellationToken)
        {
            var user = await _userService.ChangeRoleAsync(
                id,
                request,
                cancellationToken);

            return Ok(user);
        }

        [HttpPut("{id}/activate")]
        public async Task<IActionResult> Activate(
            string id,
            CancellationToken cancellationToken)
        {
            await _userService.ActivateAsync(id, cancellationToken);

            return NoContent();
        }

        [HttpPut("{id}/deactivate")]
        public async Task<IActionResult> Deactivate(
            string id,
            CancellationToken cancellationToken)
        {
            await _userService.DeactivateAsync(id, cancellationToken);

            return NoContent();
        }
    }
}
