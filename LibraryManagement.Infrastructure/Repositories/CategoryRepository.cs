using LibraryManagement.Application.Categories.Interfaces;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Category>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            return await _context.Categories
                .AsNoTracking()
                .OrderBy(category => category.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<Category?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(category => category.Id == id, cancellationToken);
        }

        public async Task<bool> ExistsByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            return await _context.Categories
                .AnyAsync(category => category.Id == id, cancellationToken);
        }

        public async Task<bool> ExistsByNameAsync(
            string name,
            int? excludedCategoryId = null,
            CancellationToken cancellationToken = default)
        {
            var normalizedName = name.Trim().ToLower();

            return await _context.Categories
                .AnyAsync(category =>
                    category.Name.ToLower() == normalizedName &&
                    (!excludedCategoryId.HasValue || category.Id != excludedCategoryId.Value),
                    cancellationToken);
        }

        public async Task<bool> HasChildrenAsync(
            int categoryId,
            CancellationToken cancellationToken = default)
        {
            return await _context.Categories
                .AnyAsync(category => category.ParentCategoryId == categoryId, cancellationToken);
        }

        public async Task<bool> HasBooksAsync(
            int categoryId,
            CancellationToken cancellationToken = default)
        {
            return await _context.BookCategories
                .AnyAsync(bookCategory => bookCategory.CategoryId == categoryId, cancellationToken);
        }

        public async Task AddAsync(
            Category category,
            CancellationToken cancellationToken = default)
        {
            await _context.Categories.AddAsync(category, cancellationToken);
        }

        public void Remove(Category category)
        {
            _context.Categories.Remove(category);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
