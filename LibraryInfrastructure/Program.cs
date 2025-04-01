using LibraryDomain.Models;
using LibraryInfrastructure;
using LibraryInfrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Додати контролери з представленнями
builder.Services.AddControllersWithViews();

// Зареєструвати контекст бізнес-даних
builder.Services.AddDbContext<DblibraryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Зареєструвати Identity контекст (якщо використовуєте окрему базу для Identity)
builder.Services.AddDbContext<IdentityContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));

// Зареєструвати фабрику сервісів для ResearchWork
builder.Services.AddTransient<IDataPortServiceFactory<ResearchWork>, ResearchWorkDataPortServiceFactory>();

// Налаштування Identity
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<IdentityContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Account/AccessDenied";
});


// Зареєструвати сервіс EmailSender
builder.Services.AddTransient<IEmailSender, LibraryInfrastructure.Services.EmailSender>();

// Вимкнути вимогу підтвердження пошти (якщо потрібно)
builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;
});

var app = builder.Build();

// Ініціалізація ролей та адміністратора
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<User>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await RoleInitializer.InitializeAsync(userManager, roleManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ResearchWorks}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
