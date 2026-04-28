using Microsoft.EntityFrameworkCore;
using TechClient.Infrastructure.Data;
using TechClient.Infrastructure.Services;
using TechClient.Domain.Interfaces;
using TechClient.Application.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TechClientDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IChatService, DialogflowService>();
builder.Services.AddScoped<ChatAppService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();