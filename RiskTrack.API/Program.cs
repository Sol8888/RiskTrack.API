using HealthChecks.SqlServer;
using Microsoft.EntityFrameworkCore;
using RiskTrack.Infrastructure.Data;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks()
    .AddCheck("sql-server-db", new SqlServerHealthCheck(
        new SqlServerHealthCheckOptions
        {
            ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        }),
        tags: new[] { "database" });


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<RiskTrackDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapHealthChecks("/health");

app.UseAuthorization();

app.MapControllers();

app.Run();