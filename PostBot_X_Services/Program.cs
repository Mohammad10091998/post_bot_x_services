using Microsoft.Extensions.Options;
using PostBot_X_Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Add the ChatGPT service
builder.Services.AddSingleton<ChatGPTService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
