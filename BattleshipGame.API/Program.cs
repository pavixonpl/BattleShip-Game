using System.Text.Json;
using System.Text.Json.Serialization;
using BattleshipGame.Core.Services.BoardPatternGenerator;
using BattleshipGame.Core.Services.DataAccess;
using BattleshipGame.Core.Services.GameManager;
using BattleshipGame.Core.Services.GameplayGenerator;
using BattleshipGame.Core.Services.ShotProvider;
using BattleshipGame.DAL;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers().AddJsonOptions(opts =>
{
    var enumConverter = new JsonStringEnumConverter();
    opts.JsonSerializerOptions.Converters.Add(enumConverter);
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IDataAccessService, InMemoryDataAccessService>();
builder.Services.AddScoped<IGameManagerService, GameManagerService>();
builder.Services.AddScoped<IBoardGeneratorService, ClassicBoardGeneratorService>();
builder.Services.AddScoped<IGameplayGeneratorService, SimulatedGameplayGeneratorService>();
builder.Services.AddScoped<IShootingService, ShootingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(s => s.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseAuthorization();

app.MapControllers();

app.Run();
