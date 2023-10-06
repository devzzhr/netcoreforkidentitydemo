using Even.Data;
using DemoApp.Data;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.KnownProxies.Add(IPAddress.Parse("5.196.4.238"));
});
// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection") ??
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));

builder.Services.AddDbContext<BaseContactDBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("BaseContactConnection") ??
    throw new InvalidOperationException("Connection string 'BaseContactConnection' not found.")));




builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});
builder.Services.Configure<IdentityOptions>(options =>
{
    options.User.RequireUniqueEmail = true;
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-.@_";
    options.User.RequireUniqueEmail = true;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});
var authservices = builder.Services;
var configuration = builder.Configuration;


authservices.AddAuthentication().AddMicrosoftAccount(microsoftOptions =>
{
    microsoftOptions.ClientId = configuration["Authentication:Microsoft:ClientId"];
    microsoftOptions.ClientSecret = configuration["Authentication:Microsoft:ClientSecret"];
});


var app = builder.Build();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});




app.Use((context, next) =>
{
    // if (!context.Request.Path.StartsWithSegments("/gestionnaire"))
    {
        context.Request.PathBase = "/DemoApp";
    }
    return next();
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseHttpsRedirection();
//app.UseStatusCodePages();
app.UseStatusCodePagesWithRedirects("/even/Error?statuscode={0}");
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<ApplicationDbContext>();
    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }
   

    var contactContext = services.GetRequiredService<BaseContactDBContext>();
    if (contactContext.Database.GetPendingMigrations().Any())
    {
        contactContext.Database.Migrate();
    }

    contactContext.Database.EnsureCreated();
    
    BaseContactDBInitializer.Initialize(contactContext);
}
app.Run();
