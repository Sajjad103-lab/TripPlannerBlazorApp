using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TripPlannerApp.Components;
using TripPlannerApp.Data;
using TripPlannerApp.Models;
using TripPlannerApp.Services;

var builder = WebApplication.CreateBuilder(args);

// ── Database ──────────────────────────────────────────────────────────────────
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Data Source=tripplanner.db"));

// ── Identity ──────────────────────────────────────────────────────────────────
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login";
    options.LogoutPath = "/logout";
    options.AccessDeniedPath = "/access-denied";
});

// ── Blazor ────────────────────────────────────────────────────────────────────
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider,
    Microsoft.AspNetCore.Components.Server.ServerAuthenticationStateProvider>();

// ── Application Services ──────────────────────────────────────────────────────
builder.Services.AddScoped<TripService>();
builder.Services.AddScoped<ExpenseService>();
builder.Services.AddScoped<FileUploadService>();
builder.Services.AddScoped<BookingService>();
builder.Services.AddHttpClient<WeatherService>();
builder.Services.AddHttpClient<LocationService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

var app = builder.Build();

// ── Middleware ────────────────────────────────────────────────────────────────
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

// ── Auth Endpoints ────────────────────────────────────────────────────────────

// LOGIN — FIX: rememberMe must be string? because unchecked checkboxes send no value
app.MapPost("/account/login", async (
    HttpContext context,
    SignInManager<ApplicationUser> signInManager) =>
{
    var form = await context.Request.ReadFormAsync();
    var email       = form["email"].ToString().Trim();
    var password    = form["password"].ToString();
    var rememberMe  = form["rememberMe"].ToString() == "true";

    if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        return Results.Redirect("/login?error=Please+fill+in+all+fields");

    var result = await signInManager.PasswordSignInAsync(email, password, rememberMe, lockoutOnFailure: false);
    if (result.Succeeded)
        return Results.Redirect("/dashboard");

    return Results.Redirect("/login?error=Invalid+email+or+password");
});

// REGISTER — FIX: read form manually to avoid missing-field binding errors
app.MapPost("/account/register", async (
    HttpContext context,
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager) =>
{
    var form           = await context.Request.ReadFormAsync();
    var email          = form["email"].ToString().Trim();
    var password       = form["password"].ToString();
    var confirmPassword = form["confirmPassword"].ToString();
    var firstName      = form["firstName"].ToString().Trim();
    var lastName       = form["lastName"].ToString().Trim();

    if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        return Results.Redirect("/register?error=Email+and+password+are+required");

    if (password != confirmPassword)
        return Results.Redirect("/register?error=Passwords+do+not+match");

    var existing = await userManager.FindByEmailAsync(email);
    if (existing != null)
        return Results.Redirect("/register?error=An+account+with+this+email+already+exists");

    var user = new ApplicationUser
    {
        UserName  = email,
        Email     = email,
        FirstName = firstName,
        LastName  = lastName,
        EmailConfirmed = true
    };

    var result = await userManager.CreateAsync(user, password);
    if (result.Succeeded)
    {
        await signInManager.SignInAsync(user, isPersistent: false);
        return Results.Redirect("/dashboard");
    }

    var errors = string.Join(" | ", result.Errors.Select(e => e.Description));
    return Results.Redirect($"/register?error={Uri.EscapeDataString(errors)}");
});

// LOGOUT
app.MapPost("/account/logout", async (SignInManager<ApplicationUser> signInManager) =>
{
    await signInManager.SignOutAsync();
    return Results.Redirect("/");
});

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// ── Seed Database ─────────────────────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    try
    {
        await SeedData.InitializeAsync(scope.ServiceProvider);
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during database initialization.");
    }
}

app.Run();
