using Microsoft.EntityFrameworkCore;
using CMCSApp1.Models;
using CMCSApp1.Services; // Add the namespace for ClaimVerificationService

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register the ClaimVerificationService
builder.Services.AddScoped<ClaimVerificationService>();

// Add In-Memory Database
builder.Services.AddDbContext<CMCSContext>(options =>
    options.UseInMemoryDatabase("CMCSDb"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
