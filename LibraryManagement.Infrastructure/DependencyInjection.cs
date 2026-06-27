using LibraryManagement.Application.ActivityLogs.Interfaces;
using LibraryManagement.Application.Auth.Interfaces;
using LibraryManagement.Application.Authors.Interfaces;
using LibraryManagement.Application.Books.Interfaces;
using LibraryManagement.Application.Borrowings.Interfaces;
using LibraryManagement.Application.Categories.Interfaces;
using LibraryManagement.Application.Members.Interfaces;
using LibraryManagement.Application.Publishers.Interfaces;
using LibraryManagement.Application.Users.Interfaces;
using LibraryManagement.Infrastructure.Authentication;
using LibraryManagement.Infrastructure.Identity;
using LibraryManagement.Infrastructure.Persistence;
using LibraryManagement.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("DefaultConnection is missing from configuration.");

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services
            .AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;

                options.User.RequireUniqueEmail = true;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();

        services.AddScoped<IActivityLogRepository, ActivityLogRepository>();
        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped<IPublisherRepository, PublisherRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IMemberRepository, MemberRepository>();
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IBorrowingRepository, BorrowingRepository>();
        return services;
    }
}