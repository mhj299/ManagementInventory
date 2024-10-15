using WebUi.Components;
using Infrastructure.DependencyInjection;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Application.DependencyInjection;
using WebUi.Components.Layout.Identity;
using Microsoft.AspNetCore.Components.Authorization;
using WebUi.Hubs;
using WebUi.States;
using WebUi.States.User;
using WebUi.States.Administration;
using Syncfusion.Blazor;
using NetcodeHub.Packages.Components.DataGrid;
using MudBlazor.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
builder.Services.AddInfrastructureService(builder.Configuration);
builder.Services.AddScoped<NetcodeHubConnectionService>();
builder.Services.AddScoped<UserActiveOrderCountState>();
builder.Services.AddScoped<AdminActiveOrderCountState>();
builder.Services.AddScoped<GenericHomeHeaderState>();
builder.Services.AddSingleton<ChangePasswordState>();
builder.Services.AddScoped<ICustomAuthorizationService, CustomAuthorizationService>();
builder.Services.AddApplicationService();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthStateProvider>();
builder.Services.AddSyncfusionBlazor();
builder.Services.AddVirtualizationService();
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NCaF5cXmZCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWXZeeXRTRmJdU0Z3XEo=");
builder.Services.AddMudServices();
builder.Services.AddSignalR();
builder.Services.AddDbContext<AppDbContext>
(options =>
 options.UseSqlServer(
 builder.Configuration.
 GetConnectionString("Default")));


 var app = builder.Build();

 // Configure the HTTP request pipeline.
 if (!app.Environment.IsDevelopment())
     {
        app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

 app.UseHttpsRedirection();

 app.UseStaticFiles();
 app.UseAntiforgery();

 app.MapRazorComponents<App>()
      .AddInteractiveServerRenderMode();
app.MapSignOutEndpoint();
app.MapHub<CommunicationHub>("/communicationhub");
 app.Run();
        
   

