using HeroApi.Entities;
using HeroApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<HeroContext>(opt => 
        opt.UseInMemoryDatabase("HeroList"));
builder.Services.AddScoped<IHeroesService, HeroesService>();
builder.Services.AddCors(options => options.AddPolicy(name: "HeroClient",
        policy =>
        {
                policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
        }));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}
app.UseCors("HeroClient");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
