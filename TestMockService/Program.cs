using AiLearner_ClassLibrary.OpenAi_Service;
using DataAccessLayer.dbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace TestMockService
{
    internal class Program
    {
        static async Task Main()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AiLearnerDbContext>();
            optionsBuilder.UseSqlServer("Server=MICHAELZENBOOK;Database=AiLearnerDb;Trusted_Connection=True;TrustServerCertificate=true");
            int userInput = 10;
            while (userInput != 0)
            {
                Console.WriteLine("enter input:");
                Console.WriteLine("press 1 for user mock service");
                Console.WriteLine("press 2 for learing mock service");
                Console.WriteLine("press 3 for learing OpenAi service");
                Console.WriteLine("press 0 exit");
                userInput = int.Parse(Console.ReadLine() ?? "10");

                switch (userInput)
                {
                    case 1:
                        await UseUserMock(optionsBuilder);
                        break;
                    case 2:
                        await UseQuestionAndAnswersMock(optionsBuilder);
                        break;
                    case 3:
                        await UseOpenAiMock();
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
                await Console.Out.WriteLineAsync("Exeption Thrown:[" + e.Message + "]");
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
                await Console.Out.WriteLineAsync("Exeption Thrown:[" + e.Message + "]");
            }
        }
        public static async Task UseOpenAiMock()
        {
            try
            {
                IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                OpenAIService openAIService = new(configuration);
                await Console.Out.WriteLineAsync("type number of questions");
                int numberOfQuestions = int.Parse(Console.ReadLine() ?? "5");
                for (int i = 0; i < 5; i++)
                {
                    Random random = new();
                    string message = contentArray[random.Next(contentArray.Length)];

                    string response = await openAIService.CallChatGPTAsync(message, numberOfQuestions);
                    
                    Console.WriteLine("response :\n" + response);
                }
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync("Exeption Thrown:[" + e.Message + "]");
            }
        }
        public static string[] contentArray = ["Astronomy (Lifecycle of a Star):\r\nThe lifecycle of a star begins in a nebula, where vast clouds of gas and dust coalesce under gravity, forming a protostar. As the protostar accretes more mass, its core temperature rises until it ignites nuclear fusion, converting hydrogen into helium and releasing immense energy. This marks the birth of a main-sequence star, like our Sun, which can last billions of years. Stars of different masses have varied lifespans and evolutionary paths. Massive stars exhaust their fuel quickly, swelling into red supergiants, and may explode as supernovae, leaving behind neutron stars or black holes. Less massive stars, like the Sun, evolve into red giants before shedding their outer layers to form planetary nebulae, with their cores becoming white dwarfs.", "World History (Industrial Revolution):\r\nThe Industrial Revolution was a transformative period from the late 18th to the early 19th century, starting in Britain and spreading globally. It marked the transition from manual production methods to machine-based manufacturing, introducing steam engines, textile machinery, and iron-making techniques. This revolution catalyzed profound economic, social, and cultural changes. It fostered urbanization as people moved to cities for factory work, altered living conditions, and led to significant developments in transportation and communication, like the railway and telegraph. The Industrial Revolution also had profound implications for social structures, labor laws, and environmental conditions, setting the stage for modern industrial society.", "Biology (Photosynthesis):\r\nPhotosynthesis is a complex process that plants, algae, and certain bacteria use to convert light energy into chemical energy, stored in glucose. It occurs mainly in the chloroplasts of plant cells, where chlorophyll captures sunlight. The process is divided into two main phases: the light-dependent reactions and the Calvin cycle. In the light-dependent reactions, sunlight is converted into ATP and NADPH, which are then used in the Calvin cycle to fix carbon dioxide into glucose. Photosynthesis is fundamental to life on Earth, providing the oxygen we breathe and the basis of the food chain.", "Environmental Science (Climate Change):\r\nClimate change refers to significant, long-term changes in the statistical distribution of weather patterns over periods ranging from decades to millions of years. It can be a change in the average weather conditions or a shift in their distribution. Today, it is often associated with global warming, driven by human activities such as burning fossil fuels, deforestation, and industrial processes, leading to an increase in greenhouse gases in the Earth's atmosphere. This change has widespread impacts on natural systems, leading to more extreme weather events, rising sea levels, changes in wildlife populations and habitats, and significant effects on human health and livelihoods.", "Literature (Shakespeare's Works):\r\nWilliam Shakespeare, often hailed as the greatest playwright in the English language, produced a vast array of works that explore complex themes such as love, betrayal, power, and existential angst. His plays, divided into tragedies, comedies, and histories, delve into the depths of human nature and emotion, often through intricate plots and rich character development. Shakespeare's innovative use of language, his mastery of meter and verse, and his ability to intertwine multiple plotlines have cemented his works as timeless pieces of literature, continuously studied, performed, and revered around the world."];

    }
}