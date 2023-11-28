
using StoryEstimate;
using Microsoft.AspNetCore.ResponseCompression;
using StoryEstimate.Context;
using StoryEstimate.Services;
using StoryEstimate.Services.Abstract;
using Microsoft.Extensions.Configuration;
using StoryEstimate.Models.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<SessionContext>();
builder.Services.AddTransient<ISessionService, SessionService>();
builder.Services.Configure<SessionConfiguration>(builder.Configuration.GetSection("Session"));
builder.Services.AddResponseCompression(options =>
{
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat( new[] { "application/octet-stream" }); 
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseResponseCompression();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowAll");

app.MapBlazorHub();
app.MapHub<SessionHub>("/session");
app.MapFallbackToPage("/_Host");

app.Run();