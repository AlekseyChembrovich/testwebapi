using Azure.Storage.Blobs;
using WebApiTest.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(_ =>
    new BlobServiceClient(builder.Configuration.GetConnectionString("AzureBlob")));
builder.Services.AddScoped<IBlobService, BlobService>();
builder.Services.AddControllers();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/time/utc", () => Results.Ok(DateTime.UtcNow));

app.MapControllers();

app.Run();
