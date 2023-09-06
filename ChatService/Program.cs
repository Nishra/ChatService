using Application.Interface;
using Application.Services;
using ChatService.Interface;
using Common.Utils;
using Infrastructure.Models;
using Infrastructure.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MessageBrokerSettings>(
    builder.Configuration.GetSection("MessageBrokerSettings"));

builder.Services.AddSingleton(
    serviceProvider => serviceProvider.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);

builder.Services.AddScoped<IChatService, ChatService.Services.ChatService>();
builder.Services.AddScoped<IMessageProducer, MessageProducer>();
builder.Services.AddScoped<IMessageValidation, MessageValidation>();
builder.Services.AddScoped<MessageBrokerService>();
builder.Services.AddScoped<TeamCapacityCalculator>();
builder.Services.AddScoped<DistributeMessages>();

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
