using Hospital.Application;
using Hospital.Infrastructure;
using Hospital.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("engineered-frontend", policy =>
    {
        policy.WithOrigins("http://localhost:3101", "http://127.0.0.1:3101")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddHospitalApplication();
builder.Services.AddHospitalInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("engineered-frontend");
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<HospitalDbContext>();
    await HospitalSeedData.SeedAsync(db);
}

app.Run();

public partial class Program
{
}
