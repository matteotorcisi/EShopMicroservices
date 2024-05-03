var builder = WebApplication.CreateBuilder(args);

// Services

var app = builder.Build();

// HTTP pipelines

app.Run();
