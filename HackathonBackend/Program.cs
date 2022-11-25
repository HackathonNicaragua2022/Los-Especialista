using HackathonBackend;

using Microsoft.Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var Builder = WebApplication.CreateBuilder(args);

// Add services to the container.

Builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
Builder.Services.AddEndpointsApiExplorer();
Builder.Services.AddSwaggerGen();
Builder.Services.AddDbContext<DestinyContext>();

var App = Builder.Build();

// Configure the HTTP request pipeline.
if (App.Environment.IsDevelopment())
{
    App.UseSwagger();
    App.UseSwaggerUI();
}

App.UseHttpsRedirection();

App.UseAuthorization();

App.MapControllers();

App.Run();
