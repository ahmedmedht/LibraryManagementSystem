using FluentValidation;
using LibraryManagement.Application.Publishers.DTOs;
using LibraryManagement.Application.Publishers.Interfaces;
using LibraryManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Publishers.Services
{
    public class PublisherService : IPublisherService
    {
        private readonly IPublisherRepository _publisherRepository;
        private readonly IValidator<CreatePublisherRequest> _createValidator;
        private readonly IValidator<UpdatePublisherRequest> _updateValidator;

        public PublisherService(
            IPublisherRepository publisherRepository,
            IValidator<CreatePublisherRequest> createValidator,
            IValidator<UpdatePublisherRequest> updateValidator)
        {
            _publisherRepository = publisherRepository;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<IReadOnlyList<PublisherResponse>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            var publishers = await _publisherRepository.GetAllAsync(cancellationToken);

            return publishers
                .Select(MapToResponse)
                .ToList();
        }

        public async Task<PublisherResponse?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var publisher = await _publisherRepository.GetByIdAsync(id, cancellationToken);

            return publisher is null ? null : MapToResponse(publisher);
        }

        public async Task<PublisherResponse> CreateAsync(
            CreatePublisherRequest request,
            CancellationToken cancellationToken = default)
        {
            await _createValidator.ValidateAndThrowAsync(request, cancellationToken);

            var exists = await _publisherRepository.ExistsByNameAsync(
                request.Name,
                excludedPublisherId: null,
                cancellationToken);

            if (exists)
                throw new InvalidOperationException("A publisher with the same name already exists.");

            var publisher = new Publisher
            {
                Name = request.Name.Trim(),
                Address = request.Address?.Trim(),
                Website = request.Website?.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            await _publisherRepository.AddAsync(publisher, cancellationToken);
            await _publisherRepository.SaveChangesAsync(cancellationToken);

            return MapToResponse(publisher);
        }

        public async Task<PublisherResponse> UpdateAsync(
            int id,
            UpdatePublisherRequest request,
            CancellationToken cancellationToken = default)
        {
            await _updateValidator.ValidateAndThrowAsync(request, cancellationToken);

            var publisher = await _publisherRepository.GetByIdAsync(id, cancellationToken);

            if (publisher is null)
                throw new InvalidOperationException("Publisher was not found.");

            var exists = await _publisherRepository.ExistsByNameAsync(
                request.Name,
                excludedPublisherId: id,
                cancellationToken);

            if (exists)
                throw new InvalidOperationException("Another publisher with the same name already exists.");

            publisher.Name = request.Name.Trim();
            publisher.Address = request.Address?.Trim();
            publisher.Website = request.Website?.Trim();
            publisher.UpdatedAt = DateTime.UtcNow;

            await _publisherRepository.SaveChangesAsync(cancellationToken);

            return MapToResponse(publisher);
        }

        public async Task DeleteAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var publisher = await _publisherRepository.GetByIdAsync(id, cancellationToken);

            if (publisher is null)
                throw new InvalidOperationException("Publisher was not found.");

            var hasBooks = await _publisherRepository.HasBooksAsync(id, cancellationToken);

            if (hasBooks)
                throw new InvalidOperationException("Cannot delete this publisher because it is assigned to one or more books.");

            _publisherRepository.Remove(publisher);
            await _publisherRepository.SaveChangesAsync(cancellationToken);
        }

        private static PublisherResponse MapToResponse(Publisher publisher)
        {
            return new PublisherResponse
            {
                Id = publisher.Id,
                Name = publisher.Name,
                Address = publisher.Address,
                Website = publisher.Website,
                CreatedAt = publisher.CreatedAt,
                UpdatedAt = publisher.UpdatedAt
            };
        }
    }
}
