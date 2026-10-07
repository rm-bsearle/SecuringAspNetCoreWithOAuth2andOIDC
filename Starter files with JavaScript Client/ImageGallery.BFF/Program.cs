using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient("IDPClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:5001/");
});

JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();

const string bffCookieScheme = "bffCookieScheme";
const string bffChallengeScheme = "bffChallengeScheme";

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = bffCookieScheme;
    options.DefaultChallengeScheme = bffChallengeScheme;
}).AddCookie(bffCookieScheme)
.AddOpenIdConnect(bffChallengeScheme, options =>
{
    options.SignInScheme = bffCookieScheme;
    options.Authority = "https://localhost:5001";
    options.ClientId = "imagegallerybff";
    options.ClientSecret = "anothersecret";
    options.ResponseType = "code";
    options.SaveTokens = true;
    options.GetClaimsFromUserInfoEndpoint = true;
    options.Scope.Add("roles");
    options.Scope.Add("imagegalleryapi.write");
    options.Scope.Add("imagegalleryapi.read");
    options.Scope.Add("country");
    options.Scope.Add("offline_access");
    options.ClaimActions.MapJsonKey("role",  "role");
    options.ClaimActions.MapUniqueJsonKey("country", "country");
    options.TokenValidationParameters = new()
    {
        NameClaimType = "given_name",
        RoleClaimType = "role",
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();