using FluentValidation;
using LibraryManagement.Application.Authors.DTOs;
using LibraryManagement.Application.Authors.Interfaces;
using LibraryManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Authors.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IValidator<CreateAuthorRequest> _createValidator;
        private readonly IValidator<UpdateAuthorRequest> _updateValidator;

        public AuthorService(
            IAuthorRepository authorRepository,
            IValidator<CreateAuthorRequest> createValidator,
            IValidator<UpdateAuthorRequest> updateValidator)
        {
            _authorRepository = authorRepository;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<IReadOnlyList<AuthorResponse>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            var authors = await _authorRepository.GetAllAsync(cancellationToken);

            return authors
                .Select(MapToResponse)
                .ToList();
        }

        public async Task<AuthorResponse?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var author = await _authorRepository.GetByIdAsync(id, cancellationToken);

            return author is null ? null : MapToResponse(author);
        }

        public async Task<AuthorResponse> CreateAsync(
            CreateAuthorRequest request,
            CancellationToken cancellationToken = default)
        {
            await _createValidator.ValidateAndThrowAsync(request, cancellationToken);

            var exists = await _authorRepository.ExistsByNameAsync(
                request.FullName,
                excludedAuthorId: null,
                cancellationToken);

            if (exists)
                throw new InvalidOperationException("An author with the same name already exists.");

            var author = new Author
            {
                FullName = request.FullName.Trim(),
                Bio = request.Bio?.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            await _authorRepository.AddAsync(author, cancellationToken);
            await _authorRepository.SaveChangesAsync(cancellationToken);

            return MapToResponse(author);
        }

        public async Task<AuthorResponse> UpdateAsync(
            int id,
            UpdateAuthorRequest request,
            CancellationToken cancellationToken = default)
        {
            await _updateValidator.ValidateAndThrowAsync(request, cancellationToken);

            var author = await _authorRepository.GetByIdAsync(id, cancellationToken);

            if (author is null)
                throw new InvalidOperationException("Author was not found.");

            var exists = await _authorRepository.ExistsByNameAsync(
                request.FullName,
                excludedAuthorId: id,
                cancellationToken);

            if (exists)
                throw new InvalidOperationException("Another author with the same name already exists.");

            author.FullName = request.FullName.Trim();
            author.Bio = request.Bio?.Trim();
            author.UpdatedAt = DateTime.UtcNow;

            await _authorRepository.SaveChangesAsync(cancellationToken);

            return MapToResponse(author);
        }

        public async Task DeleteAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var author = await _authorRepository.GetByIdAsync(id, cancellationToken);

            if (author is null)
                throw new InvalidOperationException("Author was not found.");

            var hasBooks = await _authorRepository.HasBooksAsync(id, cancellationToken);

            if (hasBooks)
                throw new InvalidOperationException("Cannot delete this author because they are assigned to one or more books.");

            _authorRepository.Remove(author);
            await _authorRepository.SaveChangesAsync(cancellationToken);
        }

        private static AuthorResponse MapToResponse(Author author)
        {
            return new AuthorResponse
            {
                Id = author.Id,
                FullName = author.FullName,
                Bio = author.Bio,
                CreatedAt = author.CreatedAt,
                UpdatedAt = author.UpdatedAt
            };
        }
    }
}
