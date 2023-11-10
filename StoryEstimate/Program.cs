
using StoryEstimate;
using Microsoft.AspNetCore.ResponseCompression;
using StoryEstimate.Context;
using StoryEstimate.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<BallotManager>();
builder.Services.AddSingleton<ChatManager>();
builder.Services.AddSingleton<ClientManager>();
builder.Services.AddSingleton<SessionManager>();
builder.Services.AddResponseCompression(options =>
{
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat( new[] { "application/octet-stream" }); 
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

app.MapPost("/create-session", async (HttpContext context) =>
{
    var result = await context.Request.ReadFromJsonAsync<Session>();
    
    var sessionId = Guid.NewGuid().ToString();
    var sessions = app.Services.GetRequiredService<SessionManager>();
    
    if (sessions.TryAdd(sessionId, result))
    {
        return Results.Ok(sessionId);
    }

    return Results.BadRequest();
});

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
app.MapHub<VotingHub>("/vote");
app.MapFallbackToPage("/_Host");

app.Run();