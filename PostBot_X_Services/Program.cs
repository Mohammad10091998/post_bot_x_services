using Microsoft.Extensions.Options;
using PostBot_X_Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<OpenAISettings>(builder.Configuration.GetSection("OpenAI"));
builder.Services.AddControllers();
// Add the ChatGPT service
builder.Services.AddSingleton<ChatGPTService>(sp =>
{
    var options = sp.GetRequiredService<IOptions<OpenAISettings>>().Value;
    return new ChatGPTService(options.ApiKey);
});

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
