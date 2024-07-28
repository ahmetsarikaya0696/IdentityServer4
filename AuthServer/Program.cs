using AuthServer;
using AuthServer.Models;
using AuthServer.Repositories;
using AuthServer.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICustomUserRepository, CustomUserRepository>();

string connectionString = builder.Configuration.GetConnectionString("sqlServer");
builder.Services.AddDbContext<CustomDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddIdentityServer()
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryApiScopes(Config.GetApiScopes())
                .AddInMemoryClients(Config.GetClients())
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                //.AddTestUsers(Config.GetTestUsers()) // Test user ile çalýþýlýrken eklenir
                .AddDeveloperSigningCredential()
                .AddProfileService<CustomProfileService>()
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseIdentityServer();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
