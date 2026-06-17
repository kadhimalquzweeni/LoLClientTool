using LoLClientTool.Mvc.Services;
using LoLClientTool.Services;
using System.Diagnostics;

const string appUrl = "http://localhost:5000";

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls(appUrl);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ILeagueClientDetector, LeagueClientDetector>();
builder.Services.AddScoped<ILeagueClientService, LeagueClientService>();
builder.Services.AddHttpClient<IProfileBackgroundAssetService, ProfileBackgroundAssetService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

// For this local desktop-style web app, use HTTP on localhost.
// Do not use HTTPS redirection here, otherwise the published exe may open
// http://localhost:5000 and then redirect to HTTPS, which may not be running.

// app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Lifetime.ApplicationStarted.Register(() =>
{
    try
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = appUrl,
            UseShellExecute = true
        });
    }
    catch
    {
        // Ignore browser launch errors.
        // The user can still manually open http://localhost:5000
    }
});

app.Run();