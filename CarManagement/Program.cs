using CarManagement.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    try
    {
        if (!db.Database.CanConnect())
        {
            await Task.Delay(5000);
        }

        await db.Database.ExecuteSqlRawAsync(@"
        CREATE TABLE IF NOT EXISTS CarFactories (
            Id SERIAL PRIMARY KEY,
            Name VARCHAR(100) NOT NULL,
            Country VARCHAR(100),
            CONSTRAINT uq_factory_name_country UNIQUE (Name, Country)
        )");

        await db.Database.ExecuteSqlRawAsync(@"
        CREATE TABLE IF NOT EXISTS Cars (
            Id SERIAL PRIMARY KEY,
            CarFactoryId INTEGER NOT NULL,
            Name VARCHAR(100) NOT NULL,
            Type VARCHAR(50),
            CONSTRAINT fk_carfactory FOREIGN KEY (CarFactoryId) 
            REFERENCES CarFactories(Id) ON DELETE CASCADE
        );
        
        CREATE UNIQUE INDEX idx_car_name_type_factory_ci 
        ON Cars (LOWER(Name), LOWER(Type), CarFactoryId);");

    }
    catch (Exception ex)
    {
        throw;
    }
}
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();