using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using SampleToDo.Application.Features.TodoItem.Command.Create;
using SampleToDo.Application.Profiles;
using SampleToDo.Domain.Contracts.Persistence;
using SampleToDo.Infrastructure.Persistence;
using SampleToDo.Infrastructure.Persistence.Repositories;

namespace SampleToDo.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        //Sql server
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("sqlserver")));
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        //Mediatr
        builder.Services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblies(typeof(CreateTodoItemCommandHandler).Assembly));

        //AutoMapper
        builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

        //JsonStringEnumConverter 
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        var app = builder.Build();

        //Ef Core
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var pendingMigrations = dbContext.Database.GetPendingMigrations();
            if (pendingMigrations.Any()) dbContext.Database.Migrate();
        }

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
    }
}