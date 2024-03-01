using DataAccessLayer.dbContext;
using Microsoft.EntityFrameworkCore;

namespace TestMockService
{
    internal class Program
    {
        static async Task Main()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AiLearnerDbContext>();
            optionsBuilder.UseSqlServer("Server=MICHAELZENBOOK;Database=AiLearnerDb;Trusted_Connection=True;TrustServerCertificate=true");
            int userInput = 10;
            while(userInput != 0)
            {
                Console.WriteLine("enter input:");
                Console.WriteLine("press 1 for user mock service");
                Console.WriteLine("press 2 for learing mock service");
                Console.WriteLine("press 0 exit");
                userInput = int.Parse(Console.ReadLine()??"10");

                switch (userInput)
                {
                    case 1:
                        await UseUserMock(optionsBuilder);
                        break;
                    case 2: 
                        await UseQuestionAndAnswersMock(optionsBuilder);
                        break;
                }
            }
        }
        public static async Task UseUserMock(DbContextOptionsBuilder<AiLearnerDbContext> optionsBuilder)
        {
            try
            {
                using var context = new AiLearnerDbContext(optionsBuilder.Options);
                Console.WriteLine("Context Init");
                MockUserService mockUser = new(context);
                Console.WriteLine("Mock Service Created");
                await mockUser.AddUserAsync();
                Console.WriteLine("User added succesfully");
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync("Exeption Thrown:["+ e.Message+"]");
            }
        }        
        public static async Task UseQuestionAndAnswersMock(DbContextOptionsBuilder<AiLearnerDbContext> optionsBuilder)
        {
            try
            {
                using var context = new AiLearnerDbContext(optionsBuilder.Options);
                Console.WriteLine("Context Init");
                MockLearningService mockLearning = new(context);
                Console.WriteLine("Mock Service Created");
                await mockLearning.AddMaterialWithQuestionsAndAnswersAsync();
                Console.WriteLine("User added succesfully");
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync("Exeption Thrown:["+ e.Message+ "]");
            }
        }
    }
}
