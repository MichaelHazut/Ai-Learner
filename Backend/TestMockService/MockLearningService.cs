using DataAccessLayer.dbContext;
using DataAccessLayer.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMockService
{
    public interface IMockLearningService
    {
        Task AddMaterialWithQuestionsAndAnswersAsync();
    }
    public interface IMockUserService
    {
        Task AddUserAsync();
    }
    public class MockUserService(AiLearnerDbContext context) : IMockUserService
    {
        public async Task AddUserAsync()
        {
           
            try
            {


            await context.SaveChangesAsync();
            }catch(Exception e)
            {
                await Console.Out.WriteLineAsync("Exeption In Mock: ["+ e.Message+"]");
            }
        }
    }

    public class MockLearningService(AiLearnerDbContext context) : IMockLearningService
    {
        public async Task AddMaterialWithQuestionsAndAnswersAsync()
        {
            try
            {

                // Create a new material
                var material = new Material
                {
                    UserId = "MockUser", // Replace with an actual user ID from your User table
                    Topic = "Introduction to Mock Services",
                    Content = "This is a content placeholder for learning material about mock services.",
                    UploadDate = DateTime.UtcNow,
                    Questions = []
            };
            context.Materials.Add(material);
            Console.WriteLine("material added");


                // Add questions and answers to the material
                var questionsAndAnswers = new List<(string QuestionText, string[] AnswerTexts, int CorrectAnswerIndex)>
        {
            ("What is a mock service?", new string[] {"A service animal", "A fake service for testing", "A type of web service", "A cloud service"}, 1),
            ("Why use mock services?", new string[] {"For fun", "To reduce costs", "For testing without side effects", "To increase network traffic"}, 2),
            // ... Add more questions here
        };

                foreach (var (questionText, answerTexts, correctAnswerIndex) in questionsAndAnswers)
                {
                    var question = new Question
                    {
                        Answers = [],
                        MaterialId = material.MaterialId, // This will be assigned once the material is added to the context
                        Text = questionText,
                        CreateDate = DateTime.UtcNow
                    };

                    for (int j = 0; j < answerTexts.Length; j++)
                    {
                        var answer = new Answer
                        {
                            QuestionId = question.QuestionId, // This will be assigned once the question is added to the context
                            Text = answerTexts[j],
                        };
                        question.Answers.Add(answer);
                    }

                    material.Questions.Add(question);
                }

                context.Materials.Add(material);
                await context.SaveChangesAsync();
            }
            catch(Exception e)
            {
                await Console.Out.WriteLineAsync("expetion on add material: "+ e.Message);
                await Console.Out.WriteLineAsync("inner expetion on add material: "+ e.InnerException?.Message);
            }
        }
    }
}
