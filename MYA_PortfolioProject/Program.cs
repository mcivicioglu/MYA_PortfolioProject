using MYA_PortfolioProject.Context;
using MYA_PortfolioProject.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PortfolioContext>();
builder.Services.AddScoped<IFileService, FileService>();
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

// MapStaticAssets yalnızca derleme anında var olan dosyaları sunar;
// admin panelinden çalışma anında yüklenen wwwroot/uploads dosyaları için UseStaticFiles gerekir.
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
