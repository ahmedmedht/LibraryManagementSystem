using FluentValidation;
using LibraryManagement.Application.ActivityLogs.Interfaces.Services;
using LibraryManagement.Application.ActivityLogs.Interfaces;
using LibraryManagement.Application.Authors.Interfaces;
using LibraryManagement.Application.Authors.Services;
using LibraryManagement.Application.Publishers.Interfaces;
using LibraryManagement.Application.Publishers.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Application.Categories.Interfaces;
using LibraryManagement.Application.Categories.Services;
using LibraryManagement.Application.Members.Interfaces;
using LibraryManagement.Application.Members.Services;
using LibraryManagement.Application.Books.Interfaces;
using LibraryManagement.Application.Books.Services;
using LibraryManagement.Application.Borrowings.Interfaces;
using LibraryManagement.Application.Borrowings.Services;

namespace LibraryManagement.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(AssemblyReference.Assembly);

            services.AddScoped<ActivityLogService>();
            services.AddScoped<IActivityLogService>(serviceProvider =>
                serviceProvider.GetRequiredService<ActivityLogService>());
            services.AddScoped<IActivityLogger>(serviceProvider =>
                serviceProvider.GetRequiredService<ActivityLogService>());

            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IPublisherService, PublisherService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IMemberService, MemberService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IBorrowingService, BorrowingService>();

            return services;
        }
    }
}
