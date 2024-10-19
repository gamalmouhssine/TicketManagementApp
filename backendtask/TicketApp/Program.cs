
using CoreTicket.Repositories;
using InfrastructureTicket.Data;
using InfrastructureTicket.Repositories;
using Microsoft.EntityFrameworkCore;

namespace TicketApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Configure Entity Framework     
            builder.Services.AddDbContext<ApplicationDbContext>(options => 
                    options.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));

            // Register repository
            builder.Services.AddScoped<ITicketRepository, TicketRepository>();

            // Swagger
         

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder => builder
                        .AllowAnyOrigin()  // Allow any origin temporarily for testing
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
            var app = builder.Build();
            app.UseCors("AllowAllOrigins");
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
}
