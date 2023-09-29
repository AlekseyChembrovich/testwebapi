var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer(); // Comment #1
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
