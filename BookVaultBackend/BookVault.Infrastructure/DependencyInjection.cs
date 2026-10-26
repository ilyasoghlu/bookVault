using BookVault.Application.Interfaces.Auth;
using BookVault.Application.Interfaces.Repositories;
using BookVault.Application.Interfaces.Services;
using BookVault.Application.Services;
using BookVault.Application.Services.Auth;
using BookVault.Domain.Entities;
using BookVault.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;



namespace BookVault.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped<IAuthorService, AuthorService>();

        services.AddScoped<IPublisherRepository, PublisherRepository>();
        services.AddScoped<IPublisherService, PublisherService>();

        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IBookService, BookService>();

        services.AddScoped<ISectionRepository, SectionRepository>();
        services.AddScoped<ISectionService, SectionService>();

        services.AddScoped<IShelfRepository, ShelfRepository>();
        services.AddScoped<IShelfService, ShelfService>();

        services.AddScoped<IBookCaseRepository, BookCaseRepository>();
        services.AddScoped<IBookCaseService, BookCaseService>();

        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IRoomService, RoomService>();


        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IEmployeeService, EmployeeService>();

        services.AddScoped<IReaderRepository, ReaderRepository>();
        services.AddScoped<IReaderService, ReaderService>();

        services.AddScoped<IBorrowingRepository, BorrowingRepository>();
        services.AddScoped<IBorrowingService, BorrowingService>();

        // =========================================================
        // AUTH
        // =========================================================

        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

		services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserRuleRepository, UserRuleRepository>();
        services.AddScoped<IRuleRepository, RuleRepository>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IRuleService, RuleService>();
		

		return services;
    }
}