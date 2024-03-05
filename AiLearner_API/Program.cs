using AiLearner_API.Services;
using AiLearner_ClassLibrary.OpenAi_Service;
using DataAccessLayer.dbContext;
using DataAccessLayer.models;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace AiLearner_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<AiLearnerDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("AiLearnerConnection")));


            //builder.Services.AddIdentity<User, IdentityRole>()
            //                .AddUserStore<UserRepo>()
            //                .AddDefaultTokenProviders();
            //builder.Services.AddIdentity<User, IdentityRole>(options =>
            //{
                
            //});

            builder.Services.AddSingleton<OpenAIService>();



            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            /**/
            //add my ExceptionMiddleware
            app.UseMiddleware<ExceptionMiddleware>();

            app.MapGet("/OpenAi", async (OpenAIService openAIService) =>
            {
                
                return Results.Ok();
            });

            /**/
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
