using DistributedSystem.RabbitMQ;
using DistributedSystem.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(policy => policy.AddPolicy("default", opt =>
{
    opt.AllowAnyHeader();
    opt.AllowCredentials();
    opt.AllowAnyMethod();
    opt.SetIsOriginAllowed(_ => true);
}));
builder.Services.AddControllers();
// Add services to the container.
builder.Services.AddDbContext<LinkDBContext>(/*opt => opt.UseNpgsql("Host=postgres;Database=link;Username=stbmte;Password=123456")*/);
builder.Services.AddScoped<IRabbitMqService, RabbitMqService>();
builder.Services.AddSingleton<ICacheRepository, CacheRepository>();
builder.Services.AddStack
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped(typeof(LinkRepository));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
