using Client1.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IHttpClientService, HttpClientService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
}).AddCookie("Cookies", options =>
{
    options.AccessDeniedPath = "/Home/AccessDenied";
})
  .AddOpenIdConnect("oidc", options =>
  {
      options.SignInScheme = "Cookies";
      options.Authority = "https://localhost:7061";
      options.ClientId = "Client1-Mvc";
      options.ClientSecret = "secret";
      options.ResponseType = "code id_token";
      options.GetClaimsFromUserInfoEndpoint = true;
      options.SaveTokens = true;
      options.Scope.Add("api1.read");
      options.Scope.Add("offline_access");

      options.Scope.Add("CountryAndCity");
      options.ClaimActions.MapUniqueJsonKey("country", "country");
      options.ClaimActions.MapUniqueJsonKey("city", "city");

      options.Scope.Add("Roles");
      options.ClaimActions.MapUniqueJsonKey("role", "role");
      options.TokenValidationParameters = new TokenValidationParameters()
      {
          RoleClaimType = "role"
      };
  });

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
