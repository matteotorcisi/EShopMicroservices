var builder = WebApplication.CreateBuilder(args);

//add services to container

//carter
builder.Services.AddCarter();

//MediatR
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(typeof(Program).Assembly);
});

//marten
builder.Services.AddMarten(opt =>
{
    opt.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();

var app = builder.Build();

//http pipelines

//carter
app.MapCarter();


app.Run();
