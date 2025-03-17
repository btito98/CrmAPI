using CRM.API.Configurations;
using CRM.API.Handlers;
using CRM.Shared.IoC;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddControllers().AddJsonOptions(option => option.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
services.AddHttpContextAccessor();

SwaggerConfig.SwaggerConfigurations(services);

DependencyInjection.RegisterServices(services, builder.Configuration);
AuthorizationConfig.AddAuthentication(builder.Configuration, services);
AuthorizationConfig.AddAuthorization(builder.Configuration, services);
AuthorizationConfig.AddAdmin(builder.Configuration, services);

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Host.ConfigureLogging();

builder.Services.AddScoped<TokenHandler>();

builder.Services.AddOcelot(builder.Configuration)
    .AddDelegatingHandler<TokenHandler>();

builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapWhen(context => context.Request.Path.StartsWithSegments("/identity"), appBuilder =>
{
    appBuilder.UseOcelot().Wait();
});


app.Run();
