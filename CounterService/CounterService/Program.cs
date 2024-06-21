using CounterService.Abstraction;
using CounterService.Data;
using CounterService.Mapping;
using CounterService.Repository;
using FluentValidation.AspNetCore;
using CounterService.Service;
using Microsoft.EntityFrameworkCore;
using CounterService.Validation;
using Shared.ErrorHandling;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddControllers()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<MeterReadingDTOValidator>());
builder.Services.AddDbContext<MeterContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseSettings")));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IMeterService, MeterService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
              .SetIsOriginAllowed(x => true);
    });
});
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{


    try
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<MeterContext>();
        await context.Database.MigrateAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
    }
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseAuthorization();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<CounterService.Service.CounterGrpcService>(); 
    endpoints.MapControllers();
});

app.Run();
