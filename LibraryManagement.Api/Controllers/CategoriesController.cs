using LibraryManagement.Application.Categories.DTOs;
using LibraryManagement.Application.Categories.Interfaces;
using LibraryManagement.Application.Common.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers
{

    [ApiController]
    [Route("api/categories")]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<CategoryResponse>>> GetAll(
            CancellationToken cancellationToken)
        {
            var categories = await _categoryService.GetAllAsync(cancellationToken);

            return Ok(categories);
        }

        [HttpGet("tree")]
        public async Task<ActionResult<IReadOnlyList<CategoryTreeResponse>>> GetTree(
            CancellationToken cancellationToken)
        {
            var categories = await _categoryService.GetTreeAsync(cancellationToken);

            return Ok(categories);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoryResponse>> GetById(
            int id,
            CancellationToken cancellationToken)
        {
            var category = await _categoryService.GetByIdAsync(id, cancellationToken);

            if (category is null)
                return NotFound(new { message = "Category was not found." });

            return Ok(category);
        }

        [HttpPost]
        [Authorize(Roles = RoleNames.AdministratorOrLibrarian)]
        public async Task<ActionResult<CategoryResponse>> Create(
            CreateCategoryRequest request,
            CancellationToken cancellationToken)
        {
            var category = await _categoryService.CreateAsync(request, cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = category.Id },
                category);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = RoleNames.AdministratorOrLibrarian)]
        public async Task<ActionResult<CategoryResponse>> Update(
            int id,
            UpdateCategoryRequest request,
            CancellationToken cancellationToken)
        {
            var category = await _categoryService.UpdateAsync(id, request, cancellationToken);

            return Ok(category);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = RoleNames.Administrator)]
        public async Task<IActionResult> Delete(
            int id,
            CancellationToken cancellationToken)
        {
            await _categoryService.DeleteAsync(id, cancellationToken);

            return NoContent();
        }
    }
}
