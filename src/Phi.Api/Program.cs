using Phi.Client;
using Phi.Model.Api;
using Phi.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ICacheService<int,Story?>, InMemoryCacheService<int,Story?>>();
builder.Services.AddScoped<IStoryService, StoryService>();
builder.Services.AddHttpClient<IDataClient, HackerNewsDataClient>();


var hackerNewsClientConfig = builder.Configuration.GetSection("HackerNewsClientConfig").Get<HackerNewsClientConfig>();
builder.Services.AddScoped<IDataClient, HackerNewsDataClient>(x => 
    new HackerNewsDataClient(x.GetRequiredService<HttpClient>(), hackerNewsClientConfig));

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
