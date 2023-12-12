using BlazorContainerizedApp.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

string _personInfoURL = Environment.GetEnvironmentVariable("EWS_PERSON_INFO_URL") ?? builder.Configuration.GetValue<string>("ServiceURL");

builder.Services.AddHttpClient<WeatherForecastService>("Client", c =>
{
    c.BaseAddress = new Uri(_personInfoURL);
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    var httpClientHandler = new HttpClientHandler();
    httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
    {
        return true;
    };
    return httpClientHandler;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
