using Services.Interfaces;
using Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Register Services
builder.Services.AddSingleton<IChatGPTService, ChatGPTService>();
builder.Services.AddSingleton<ITestService, TestService>();

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
