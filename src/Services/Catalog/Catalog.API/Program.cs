using BuildingBlocks.Behaviors;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

//add services to container
var assembly = typeof(Program).Assembly;

//MediatR
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(assembly);

//carter
builder.Services.AddCarter();

//marten
builder.Services.AddMarten(opt =>
{
    opt.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();

var app = builder.Build();

//http pipelines

//carter
app.MapCarter();

app.UseExceptionHandler(ex =>
{
    ex.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        if(exception == null)
            return;

        var problemDetails = new ProblemDetails
        {
            Title = exception.Message,
            Status = StatusCodes.Status500InternalServerError,
            Detail = exception.StackTrace
        };

        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogError(exception, exception.Message);

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/problem+json";

        await context.Response.WriteAsJsonAsync(problemDetails);

    });
});

app.Run();
