using App2.Interfaces;
using App2.Models;
using App2.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ExchangeDetailContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DevConnection"))
);
builder.Services.AddScoped<IExchange, ExchangeRepo>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Currency}/{action=Index}/{id?}");

app.Run();
