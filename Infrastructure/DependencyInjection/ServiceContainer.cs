﻿using Application.Extension.Identity;
using Infrastructure.DataAccess;
using Infrastructure.Repository; 
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Application.Interface.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using MediatR;
using Infrastructure.Repository.Products.Handlers.Products; // Pour MediatR
//using Infrastructure.DataAccess.Factories;
namespace Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(o => o.UseSqlServer(config.GetConnectionString("Default")), ServiceLifetime.Scoped);

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            }).AddIdentityCookies();

            services.AddIdentityCore<ApplicationUser>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();


            services.AddAuthorizationBuilder()
                .AddPolicy("AdministrationPolicy", adp =>
                {
                    adp.RequireAuthenticatedUser();
                    adp.RequireRole("Admin", "Manager");
                })
                .AddPolicy("UserPolicy", adp =>
                {
                    adp.RequireAuthenticatedUser();
                    adp.RequireRole("User");
                });
            services.AddCascadingAuthenticationState();
            services.AddScoped<IAccount, Account>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(CreateProductHandler).Assembly));
            services.AddScoped<DataAccess.IDbContextFactory<AppDbContext>, DataAccess.DbContextFactory<AppDbContext>>();


            return services;
        }
    }
}