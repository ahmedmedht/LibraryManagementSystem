using FluentValidation;
using LibraryManagement.Application.ActivityLogs.Interfaces;
using LibraryManagement.Application.Categories.DTOs;
using LibraryManagement.Application.Categories.Interfaces;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Categories.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IActivityLogger _activityLogger;
        private readonly IValidator<CreateCategoryRequest> _createValidator;
        private readonly IValidator<UpdateCategoryRequest> _updateValidator;

        public CategoryService(
            ICategoryRepository categoryRepository,
            IActivityLogger activityLogger,
            IValidator<CreateCategoryRequest> createValidator,
            IValidator<UpdateCategoryRequest> updateValidator)
        {
            _categoryRepository = categoryRepository;
            _activityLogger = activityLogger;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<IReadOnlyList<CategoryResponse>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            var categories = await _categoryRepository.GetAllAsync(cancellationToken);

            return categories
                .Select(MapToResponse)
                .ToList();
        }

        public async Task<IReadOnlyList<CategoryTreeResponse>> GetTreeAsync(
            CancellationToken cancellationToken = default)
        {
            var categories = await _categoryRepository.GetAllAsync(cancellationToken);
            var categoryList = categories.ToList();

            return categoryList
                .Where(category => category.ParentCategoryId is null)
                .OrderBy(category => category.Name)
                .Select(category => BuildTree(category, categoryList))
                .ToList();
        }

        public async Task<CategoryResponse?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);

            return category is null ? null : MapToResponse(category);
        }

        public async Task<CategoryResponse> CreateAsync(
            CreateCategoryRequest request,
            CancellationToken cancellationToken = default)
        {
            await _createValidator.ValidateAndThrowAsync(request, cancellationToken);

            if (request.ParentCategoryId.HasValue)
            {
                var parentExists = await _categoryRepository.ExistsByIdAsync(
                    request.ParentCategoryId.Value,
                    cancellationToken);

                if (!parentExists)
                    throw new InvalidOperationException("Parent category was not found.");
            }

            var nameExists = await _categoryRepository.ExistsByNameAsync(
                request.Name,
                excludedCategoryId: null,
                cancellationToken);

            if (nameExists)
                throw new InvalidOperationException("A category with the same name already exists.");

            var category = new Category
            {
                Name = request.Name.Trim(),
                ParentCategoryId = request.ParentCategoryId,
                CreatedAt = DateTime.UtcNow
            };

            await _categoryRepository.AddAsync(category, cancellationToken);
            await _categoryRepository.SaveChangesAsync(cancellationToken);

            await _activityLogger.LogAsync(
                ActivityAction.CreateCategory,
                nameof(Category),
                category.Id.ToString(),
                $"Category '{category.Name}' was created.",
                cancellationToken);

            return MapToResponse(category);
        }

        public async Task<CategoryResponse> UpdateAsync(
            int id,
            UpdateCategoryRequest request,
            CancellationToken cancellationToken = default)
        {
            await _updateValidator.ValidateAndThrowAsync(request, cancellationToken);

            var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);

            if (category is null)
                throw new InvalidOperationException("Category was not found.");

            if (request.ParentCategoryId == id)
                throw new InvalidOperationException("A category cannot be its own parent.");

            if (request.ParentCategoryId.HasValue)
            {
                var parentExists = await _categoryRepository.ExistsByIdAsync(
                    request.ParentCategoryId.Value,
                    cancellationToken);

                if (!parentExists)
                    throw new InvalidOperationException("Parent category was not found.");
            }

            var nameExists = await _categoryRepository.ExistsByNameAsync(
                request.Name,
                excludedCategoryId: id,
                cancellationToken);

            if (nameExists)
                throw new InvalidOperationException("Another category with the same name already exists.");

            category.Name = request.Name.Trim();
            category.ParentCategoryId = request.ParentCategoryId;
            category.UpdatedAt = DateTime.UtcNow;

            await _categoryRepository.SaveChangesAsync(cancellationToken);

            await _activityLogger.LogAsync(
                ActivityAction.UpdateCategory,
                nameof(Category),
                category.Id.ToString(),
                $"Category '{category.Name}' was updated.",
                cancellationToken);

            return MapToResponse(category);
        }

        public async Task DeleteAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);

            if (category is null)
                throw new InvalidOperationException("Category was not found.");

            var hasChildren = await _categoryRepository.HasChildrenAsync(id, cancellationToken);

            if (hasChildren)
                throw new InvalidOperationException("Cannot delete this category because it has subcategories.");

            var hasBooks = await _categoryRepository.HasBooksAsync(id, cancellationToken);

            if (hasBooks)
                throw new InvalidOperationException("Cannot delete this category because it is assigned to one or more books.");

            _categoryRepository.Remove(category);
            await _categoryRepository.SaveChangesAsync(cancellationToken);

            await _activityLogger.LogAsync(
                ActivityAction.DeleteCategory,
                nameof(Category),
                id.ToString(),
                $"Category '{category.Name}' was deleted.",
                cancellationToken);
        }

        private static CategoryResponse MapToResponse(Category category)
        {
            return new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                ParentCategoryId = category.ParentCategoryId,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt
            };
        }

        private static CategoryTreeResponse BuildTree(
            Category category,
            List<Category> allCategories)
        {
            return new CategoryTreeResponse
            {
                Id = category.Id,
                Name = category.Name,
                Children = allCategories
                    .Where(child => child.ParentCategoryId == category.Id)
                    .OrderBy(child => child.Name)
                    .Select(child => BuildTree(child, allCategories))
                    .ToList()
            };
        }
    }
}
