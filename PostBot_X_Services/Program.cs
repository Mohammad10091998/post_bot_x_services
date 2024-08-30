using Services.Interfaces;
using Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Register HttpClientService with HttpClient dependency
builder.Services.AddHttpClient<IHttpClientService, HttpClientService>();
// Register Service;
builder.Services.AddSingleton<IChatGPTService, ChatGPTService>();
builder.Services.AddSingleton<IChatBotService, ChatBotService>();
builder.Services.AddScoped<ITestService, TestService>();
builder.Services.AddTransient<IHelperService, HelperService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors();

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
