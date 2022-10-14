using ChatBot;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using ChatBot.Auth;
using ChatBot.Auth.Helpers;
using ChatBot.Services;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSingleton<TicketService>();
builder.Services.AddSingleton<AuthenticationService>(); 
builder.Services.AddScoped<JwtUtils>(); //inject utils
builder.Services.AddCors(); //add cors
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings")); //configure appsettings
//builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();

app.UseEndpoints(endpoint => { endpoint.MapControllers(); });

app.UseWebSockets(new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromSeconds(60),
});

app.UseCors(x => x  //cors settings, can be changed to only allow specific origins
    .WithOrigins("http://localhost:8080") //only localhost
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseMiddleware<ErrorHandlerMiddleware>(); //custom global error handler
app.UseMiddleware<JwtMiddleware>(); //custom jwt auth middleware

app.UseMiddleware<ConversationMiddleware>();

app.Run();
