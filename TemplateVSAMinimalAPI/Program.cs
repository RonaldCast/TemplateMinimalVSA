using Carter;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TemplateVSAMinimalAPI.Common.Behaviors;
using TemplateVSAMinimalAPI.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCarter();

var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(assembly));

builder.Services.AddAutoMapper(assembly);
builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//register Behavivor
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

//Configuration Serilog
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom
    .Configuration(context.Configuration));

//cors 
var _policyCors = "VSApolicy";
builder.Services.AddCors(options => options.AddPolicy(
    _policyCors, 
    build => build.WithOrigins(builder.Configuration["App:Origins"].Split(",", StringSplitOptions.RemoveEmptyEntries)
    .ToArray()
    )
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Manimal API SVA V1");
    });
}

app.UseCors(_policyCors);

//Log each httpRequest 
app.UseSerilogRequestLogging();

app.MapCarter();
app.Run();
