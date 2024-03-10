using AiLearner_API.Services;
using AiLearner_ClassLibrary.OpenAi_Service;
using DataAccessLayer.dbContext;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using DataAccessLayer.UnitOfWork;
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
            //Add DbContext
            builder.Services.AddDbContext<AiLearnerDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("AiLearnerConnection")));
            
            
            //Add interfaces and their implementations
            builder.Services.AddScoped<IUserRepo, UserRepo>();
            builder.Services.AddScoped<IMaterialRepo, MaterialRepo>();
            builder.Services.AddScoped<IQuestionRepo, QuestionRepo>();
            builder.Services.AddScoped<IAnswerRepo, AnswerRepo>();
            builder.Services.AddScoped<IUsersAnswersRepo, UsersAnswersRepo>();
            
            //Add UnitOfWork 
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            

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
            //Add my ExceptionMiddleware
            app.UseMiddleware<ExceptionMiddleware>();

            app.MapGet("/OpenAi", (OpenAIService openAIService) =>
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
