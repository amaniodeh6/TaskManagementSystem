using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddDbContext<ApplicationDbContext>(options =>options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
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
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await DbInitializer.SeedRolesAsync(roleManager);


    var userManager = scope.ServiceProvider
    .GetRequiredService<UserManager<IdentityUser>>();

    var config = scope.ServiceProvider
        .GetRequiredService<IConfiguration>();

    await DbInitializer.SeedUserAsync(
        userManager,
        config["SeedUsers:Admin:Username"]!,
        config["SeedUsers:Admin:Email"]!,
        config["SeedUsers:Admin:Password"]!,
        "Admin");

    await DbInitializer.SeedUserAsync(
        userManager,
        config["SeedUsers:User:Username"]!,
        config["SeedUsers:User:Email"]!,
        config["SeedUsers:User:Password"]!,
        "User");
}


app.Run();
