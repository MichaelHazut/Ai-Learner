using AiLearner_ClassLibrary.OpenAi_Service;
using Azure;
using DataAccessLayer.dbContext;
using DataAccessLayer.Models;
using DataAccessLayer.Models.Entities;
using DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Threading.Channels;
using System.Transactions;

namespace TestMockService
{
    internal class Program
    {
        public static DbContextOptionsBuilder<AiLearnerDbContext> optionsBuilder2 = new DbContextOptionsBuilder<AiLearnerDbContext>().UseSqlServer("Server=MICHAELZENBOOK;Database=AiLearnerDb;Trusted_Connection=True;TrustServerCertificate=true");
        public static AiLearnerDbContext AiLearnerDbContext = new(optionsBuilder2.Options);
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
                Console.WriteLine("press 4 for Clean Json service");
                Console.WriteLine("press 6 for ValidateStudyMaterial");
                Console.WriteLine("press 7 for user testing");
                Console.WriteLine("press 8 for CleanJson");
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
                    case 4:
                        MockTestJsonCleaner();
                        break;
                    case 6:
                        ValidateStudyMaterial();
                        break;
                    case 7:
                        break;
                    case 8:
                        CleanJson();
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
                List<StudyMaterial> stList = [];
                for (int i = 0; i < 50; i++)
                {
                    string response = string.Empty;
                    try
                    {
                        Random random = new();
                        string message = contentArray[random.Next(contentArray.Length)];

                        response = await openAIService.CallChatGPTAsync(message, numberOfQuestions);
                        StudyMaterial? stItem = JsonConvert.DeserializeObject<StudyMaterial>(response);
                        await File.AppendAllTextAsync("../../../logs/material_Log.txt", response + ",\n");
                        stList.Add(stItem!);
                        await Console.Out.WriteLineAsync(i + 1 + " completed");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        await Console.Out.WriteLineAsync("Attemting Json Clean");
                        await File.AppendAllTextAsync("../../../logs/failed_material_Log.txt", $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} \n" + response + "\n\n");
                        StudyMaterial? stItem = new() { Topic = "dasd", Summary = "dsad", Questions = [] };
                        string cleanedJson = JsonService.CleanJson(response);
                        try
                        {
                            await Console.Out.WriteLineAsync("Json Cleaned");
                            stItem = JsonConvert.DeserializeObject<StudyMaterial>(cleanedJson);
                            await File.AppendAllTextAsync("../../../logs/material_Log.txt", cleanedJson + ",\n");
                            await Console.Out.WriteLineAsync("clean Json Added");
                        }
                        catch (Exception ex)
                        {
                            await Console.Out.WriteLineAsync(ex.Message);
                        }


                        await Console.Out.WriteLineAsync("Json Is An Object");
                        stList.Add(stItem!);

                    }
                }
                List<bool> boolist = [];
                stList.ForEach(x => boolist.Add(IsStudyMaterialValid(x)));
                boolist.ForEach(x => Console.WriteLine(x));
                Console.ReadLine();
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync("!!!Exeption Thrown:[" + e.Message + "]");
                await Console.Out.WriteLineAsync("Exeption Thrown:[" + e + "]");
            }
        }
        public static void MockTestJsonCleaner()
        {
            try
            {
                for (var i = 0; i < 1; i++)
                {
                    Console.WriteLine("Json Cleaner");
                    string cleanJson = JsonService.CleanJson(dirtyJson);
                    StudyMaterial? stItem = JsonConvert.DeserializeObject<StudyMaterial>(cleanJson);
                    Console.WriteLine(cleanJson + "\n press any button");
                    Console.ReadLine();

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        public static bool IsStudyMaterialValid(StudyMaterial material)
        {
            if (material == null)
            {
                Console.WriteLine("StudyMaterial is null");
                return false;
            }

            if (string.IsNullOrEmpty(material.Topic))
            {
                Console.WriteLine("Topic is null or empty");
                return false;
            }

            if (string.IsNullOrEmpty(material.Summary))
            {
                Console.WriteLine("Summary is null or empty");
                return false;
            }

            if (material.Questions == null || material.Questions.Count == 0)
            {
                Console.WriteLine("Questions list is null or empty");
                return false;
            }

            foreach (var question in material.Questions)
            {
                if (string.IsNullOrEmpty(question.Question))
                {
                    Console.WriteLine("Question text is null or empty");
                    return false;
                }

                if (question.Options == null || question.Options.Count == 0)
                {
                    Console.WriteLine("Options dictionary is null or empty");
                    return false;
                }

                foreach (var option in question.Options)
                {
                    if (string.IsNullOrEmpty(option.Key) || string.IsNullOrEmpty(option.Value))
                    {
                        Console.WriteLine("An option key or value is null or empty");
                        return false;
                    }
                }

                if (string.IsNullOrEmpty(question.Answer))
                {
                    Console.WriteLine("Answer is null or empty");
                    return false;
                }
            }

            return true;
        }
        public static void ValidateStudyMaterial()
        {
            StudyMaterial? st = JsonConvert.DeserializeObject<StudyMaterial>(studyMaterialString);
            bool ans = st!.ValidateStudyMaterial();
            Console.WriteLine(ans);

        }


        public async static void CleanJson()
        {
            StudyMaterial? st = JsonService.DeserializeJson<StudyMaterial>(studyMaterialString);
            UserRepo userRepo = new(AiLearnerDbContext);
            MaterialRepo materialRepo = new(AiLearnerDbContext);
            QuestionRepo questionRepo = new(AiLearnerDbContext);
            AnswerRepo answerRepo = new(AiLearnerDbContext);
            UsersAnswersRepo usersAnswersRepo = new(AiLearnerDbContext);



            User? user = await userRepo.NewUser("Michael1mic1@gmail.com", "abcd1234");

#pragma warning disable CS8604 // Possible null reference argument.
            Material material = await materialRepo.CreateMaterial(user!.Id, st!.Topic, st.Content, st.Summary);
#pragma warning restore CS8604 // Possible null reference argument.

            List<Question> questions = await questionRepo.CreateQuestion(material.MaterialId, st.Questions);

            await answerRepo.CreateAnswer(questions, st.Questions);

            var res = await usersAnswersRepo.CreateUsersAnswers("71c724b2-5947-459d-98ac-241b08af9085", 3, 11);
            Console.WriteLine(res);
        }

        public static string studyMaterialString = "{ \"topic\": \"The History and Impact of the Internet\", \"summary\": \"The Internet has a rich history that began with ARPANET in the 1960s and expanded in the 1970s and 1980s with the development of TCP/IP. The 1990s saw the introduction of the World Wide Web, leading to massive growth in internet usage. Today, the Internet is a vital part of global infrastructure, shaping various aspects of society.\", \"questions\": [ { \"question\": \"Which U.S. Department funded the project that laid the groundwork for the modern Internet?\", \"options\": { \"A\": \"Department of Defense\", \"B\": \"Department of Energy\", \"C\": \"Department of Commerce\", \"D\": \"Department of Homeland Security\" }, \"answer\": \"A\" }, { \"question\": \"What pivotal set of rules allowed diverse computer networks to communicate seamlessly in the 1970s and 1980s?\", \"options\": { \"A\": \"Transmission Control Protocol/Internet Protocol (TCP/IP)\", \"B\": \"Hypertext Transfer Protocol (HTTP)\", \"C\": \"File Transfer Protocol (FTP)\", \"D\": \"Simple Mail Transfer Protocol (SMTP)\" }, \"answer\": \"A\" }, { \"question\": \"Who conceptualized the World Wide Web in the 1990s?\", \"options\": { \"A\": \"Steve Jobs\", \"B\": \"Bill Gates\", \"C\": \"Tim Berners-Lee\", \"D\": \"Larry Page\" }, \"answer\": \"C\" }, { \"question\": \"What technology introduced in the 1990s led to an explosion in Internet usage by providing a user-friendly interface to access information?\", \"options\": { \"A\": \"Social Media\", \"B\": \"Web Browsers\", \"C\": \"Search Engines\", \"D\": \"Cloud Technology\" }, \"answer\": \"B\" }, { \"question\": \"Which emerging technology promises to redefine our digital future by enabling new possibilities on the Internet?\", \"options\": { \"A\": \"Artificial Intelligence\", \"B\": \"Blockchain\", \"C\": \"Internet of Things (IoT)\", \"D\": \"Virtual Reality (VR)\" }, \"answer\": \"C\" }, { \"question\": \"What was the name of the project in the 1960s that laid the groundwork for the development of the Internet?\", \"options\": { \"A\": \"ARPA-Net\", \"B\": \"ARPNET\", \"C\": \"ARPANET\", \"D\": \"ARPANetwork\" }, \"answer\": \"C\" }, { \"question\": \"Who funded the development of ARPANET in the 1960s?\", \"options\": { \"A\": \"National Science Foundation\", \"B\": \"Central Intelligence Agency\", \"C\": \"Department of Defense\", \"D\": \"National Aeronautics and Space Administration\" }, \"answer\": \"C\" }, { \"question\": \"What significant event occurred in the 1990s that led to massive growth in Internet usage?\", \"options\": { \"A\": \"Launch of the first satellite internet\", \"B\": \"Introduction of the World Wide Web\", \"C\": \"Creation of the first online shopping platform\", \"D\": \"Development of the first search engine\" }, \"answer\": \"B\" }, { \"question\": \"Which technology has reshaped the digital landscape by allowing people to access vast amounts of information online?\", \"options\": { \"A\": \"Artificial Intelligence\", \"B\": \"Cloud Technology\", \"C\": \"Virtual Reality (VR)\", \"D\": \"Search Engines\" }, \"answer\": \"D\" }, { \"question\": \"Which individual played a key role in the development of the World Wide Web?\", \"options\": { \"A\": \"Steve Jobs\", \"B\": \"Bill Gates\", \"C\": \"Tim Berners-Lee\", \"D\": \"Larry Page\" }, \"answer\": \"C\" }, { \"question\": \"What technology established a set of rules in the 1970s and 1980s for different computer networks to communicate effectively?\", \"options\": { \"A\": \"World Wide Web (WWW)\", \"B\": \"Transmission Control Protocol/Internet Protocol (TCP/IP)\", \"C\": \"File Transfer Protocol (FTP)\", \"D\": \"Hypertext Transfer Protocol (HTTP)\" }, \"answer\": \"B\" }, { \"question\": \"Which technology introduced in the 1990s made it easier for users to navigate and access information on the Internet?\", \"options\": { \"A\": \"Web Browsers\", \"B\": \"Social Media\", \"C\": \"Cloud Technology\", \"D\": \"Search Engines\" }, \"answer\": \"A\" }, { \"question\": \"What term is used to describe the platform connecting billions of people worldwide and democratizing information?\", \"options\": { \"A\": \"Global Web\", \"B\": \"Interweb\", \"C\": \"Internet\", \"D\": \"Digital Network\" }, \"answer\": \"C\" }, { \"question\": \"Which technology has the potential to revolutionize the way objects interact and communicate over the Internet?\", \"options\": { \"A\": \"Augmented Reality\", \"B\": \"Virtual Reality\", \"C\": \"Internet of Things (IoT)\", \"D\": \"Blockchain\" }, \"answer\": \"C\" }, { \"question\": \"What key protocol was instrumental in allowing different computer networks to exchange data in the 1970s and 1980s?\", \"options\": { \"A\": \"Hypertext Transfer Protocol (HTTP)\", \"B\": \"Simple Mail Transfer Protocol (SMTP)\", \"C\": \"Transmission Control Protocol/Internet Protocol (TCP/IP)\", \"D\": \"File Transfer Protocol (FTP)\" }, \"answer\": \"C\" } ] }";
        public static string[] contentArray = ["Astronomy (Lifecycle of a Star):\r\nThe lifecycle of a star begins in a nebula, where vast clouds of gas and dust coalesce under gravity, forming a protostar. As the protostar accretes more mass, its core temperature rises until it ignites nuclear fusion, converting hydrogen into helium and releasing immense energy. This marks the birth of a main-sequence star, like our Sun, which can last billions of years. Stars of different masses have varied lifespans and evolutionary paths. Massive stars exhaust their fuel quickly, swelling into red supergiants, and may explode as supernovae, leaving behind neutron stars or black holes. Less massive stars, like the Sun, evolve into red giants before shedding their outer layers to form planetary nebulae, with their cores becoming white dwarfs.", "World History (Industrial Revolution):\r\nThe Industrial Revolution was a transformative period from the late 18th to the early 19th century, starting in Britain and spreading globally. It marked the transition from manual production methods to machine-based manufacturing, introducing steam engines, textile machinery, and iron-making techniques. This revolution catalyzed profound economic, social, and cultural changes. It fostered urbanization as people moved to cities for factory work, altered living conditions, and led to significant developments in transportation and communication, like the railway and telegraph. The Industrial Revolution also had profound implications for social structures, labor laws, and environmental conditions, setting the stage for modern industrial society.", "Biology (Photosynthesis):\r\nPhotosynthesis is a complex process that plants, algae, and certain bacteria use to convert light energy into chemical energy, stored in glucose. It occurs mainly in the chloroplasts of plant cells, where chlorophyll captures sunlight. The process is divided into two main phases: the light-dependent reactions and the Calvin cycle. In the light-dependent reactions, sunlight is converted into ATP and NADPH, which are then used in the Calvin cycle to fix carbon dioxide into glucose. Photosynthesis is fundamental to life on Earth, providing the oxygen we breathe and the basis of the food chain.", "Environmental Science (Climate Change):\r\nClimate change refers to significant, long-term changes in the statistical distribution of weather patterns over periods ranging from decades to millions of years. It can be a change in the average weather conditions or a shift in their distribution. Today, it is often associated with global warming, driven by human activities such as burning fossil fuels, deforestation, and industrial processes, leading to an increase in greenhouse gases in the Earth's atmosphere. This change has widespread impacts on natural systems, leading to more extreme weather events, rising sea levels, changes in wildlife populations and habitats, and significant effects on human health and livelihoods.", "Literature (Shakespeare's Works):\r\nWilliam Shakespeare, often hailed as the greatest playwright in the English language, produced a vast array of works that explore complex themes such as love, betrayal, power, and existential angst. His plays, divided into tragedies, comedies, and histories, delve into the depths of human nature and emotion, often through intricate plots and rich character development. Shakespeare's innovative use of language, his mastery of meter and verse, and his ability to intertwine multiple plotlines have cemented his works as timeless pieces of literature, continuously studied, performed, and revered around the world.", "The Impressionist art movement emerged in the late 19th century, revolutionizing the world of painting with its unique approach to capturing moments and emotions. Originating in France, this movement was characterized by a focus on light, color, and everyday subjects, breaking away from the traditional confines of academic painting.\r\n\r\nImpressionist artists, including Claude Monet, Edgar Degas, and Pierre-Auguste Renoir, sought to capture the fleeting moments of life, emphasizing spontaneous, loose brushwork and vibrant colors. They often painted en plein air (outdoors) to directly capture the effects of light and atmosphere on their subjects.\r\n\r\nThis movement was initially met with resistance from traditional art institutions, as its techniques defied the conventional rules of academic painting. However, over time, Impressionism gained popularity and acceptance, profoundly influencing subsequent art movements and altering the course of art history.\r\n\r\nImpressionism also paved the way for Post-Impressionism, where artists like Vincent van Gogh and Paul Cézanne further explored color, form, and emotional depth. Today, Impressionist works are celebrated for their contribution to the dynamic and diverse landscape of modern art, showcasing the beauty of the ordinary and the interplay of light and color.\r\n\r\n", "One of the most monumental achievements in human history, the Moon landing of 1969, marked the climax of the space race between the United States and the Soviet Union. On July 20, 1969, the Apollo 11 mission successfully landed astronauts Neil Armstrong and Edwin \"Buzz\" Aldrin on the Moon, while Michael Collins orbited above in the command module.\r\n\r\nThe mission was launched by a Saturn V rocket from Kennedy Space Center on July 16, 1969, and it took the astronauts four days to reach the Moon. Armstrong's first steps on the lunar surface, watched by an estimated 600 million people worldwide, were immortalized by his words: \"That's one small step for [a] man, one giant leap for mankind.\"\r\n\r\nThis historic event was not just a technical and scientific milestone but also a symbol of human ingenuity and resilience. It demonstrated the potential for humanity to achieve the seemingly impossible through collaboration, innovation, and determination.\r\n\r\nThe Apollo 11 mission not only provided valuable scientific data and samples from the lunar surface but also inspired generations to dream big and pursue exploration and discovery. The Moon landing continues to be a source of inspiration and a testament to what humanity can achieve when we reach for the stars.\r\n\r\n", "Python is a high-level, interpreted programming language known for its simplicity and readability, making it an excellent choice for beginners and experienced programmers alike. Developed by Guido van Rossum and first released in 1991, Python's design philosophy emphasizes code readability with its use of significant whitespace.\r\n\r\nPython supports multiple programming paradigms, including procedural, object-oriented, and functional programming. Its comprehensive standard library provides tools suited to many tasks, making Python a versatile language for various applications, from web development to data analysis.\r\n\r\nOne of Python's strengths is its community and the vast array of third-party modules and libraries available, allowing for the extension of its capabilities and the facilitation of tasks like machine learning, data visualization, and web development.\r\n\r\nTo get started with Python, one only needs to learn the basic syntax, understand control flow (loops and conditionals), and become familiar with Python's data structures such as lists, tuples, dictionaries, and sets. With these fundamentals, a new programmer can begin to explore the powerful and diverse applications of Python in the world of programming.\r\n\r\n", "The French Revolution, spanning from 1789 to 1799, was a pivotal period in French and world history, marked by radical social and political upheaval. It resulted in the downfall of the monarchy, the rise of Napoleon, and significant changes to the global landscape of power and governance.\r\n\r\nThe revolution began due to widespread discontent with the French monarchy and the poor economic policies of King Louis XVI. The Estates-General was convened in 1789 to address the financial crisis, but it quickly escalated into a broader struggle for political reform. The storming of the Bastille on July 14, 1789, became a symbol of the revolution, signifying the fall of autocratic authority and the rise of the people's power.\r\n\r\nThe subsequent years saw the rise of the French Republic, the execution of Louis XVI, and the Reign of Terror, where thousands were executed under the suspicion of counter-revolutionary activities. This period of radical change was marked by significant achievements, including the establishment of a new legal system, secularization of the state, and the Declaration of the Rights of Man and of the Citizen, which laid the groundwork for modern human rights.\r\n\r\nThe revolution profoundly influenced global politics, inspiring revolutionary movements worldwide and laying the foundations for modern democratic institutions. Its complex legacy continues to be a subject of extensive historical debate, symbolizing the power of collective action in shaping the course of history.\r\n\r\n", "Climate change represents one of the most pressing challenges of our time, with widespread impacts on the environment, human health, and global economies. It refers to significant changes in global temperatures and weather patterns over time. While climate change is a natural phenomenon, scientific evidence shows that human activities, particularly the emission of greenhouse gases like carbon dioxide and methane, are the primary drivers of the accelerated changes observed in recent decades.\r\n\r\nThe effects of climate change are diverse and interlinked. Rising global temperatures have led to more frequent and severe weather events such as hurricanes, floods, droughts, and heatwaves. Melting ice caps and glaciers contribute to rising sea levels, threatening coastal communities and ecosystems. Changes in climate patterns also affect agriculture, reducing food security, and impacting water resources.\r\n\r\nMitigating climate change requires global cooperation and significant efforts to reduce greenhouse gas emissions, transition to renewable energy sources, and enhance sustainability practices. Individuals, communities, governments, and businesses worldwide are called to participate in combating climate change by adopting more sustainable practices, investing in green technologies, and supporting policies aimed at environmental conservation.\r\n\r\nThe fight against climate change is not just about preserving the environment; it's also about ensuring a sustainable future for generations to come, highlighting the interconnectedness of our global community in addressing shared challenges.", "The Internet, a revolutionary technology that has transformed every aspect of our lives, has a fascinating history that dates back to the late 20th century. Its inception was rooted in the desire to create a robust, fault-tolerant communication network. This led to the development of ARPANET in the 1960s, a project funded by the U.S. Department of Defense, which laid the groundwork for the Internet we know today.\r\n\r\nThroughout the 1970s and 1980s, the network expanded beyond military and academic institutions, incorporating a growing number of computers. The introduction of the Transmission Control Protocol/Internet Protocol (TCP/IP) was pivotal, establishing a set of rules that allowed diverse computer networks to communicate seamlessly.\r\n\r\nThe 1990s marked a significant turning point with the advent of the World Wide Web, conceptualized by Tim Berners-Lee. The web provided a user-friendly interface to access information, leading to an explosion in Internet usage. Web browsers, search engines, and the proliferation of websites made the Internet accessible and useful to the general public.\r\n\r\nOver the subsequent decades, the Internet has seen exponential growth, profoundly influencing commerce, communication, education, and entertainment. The rise of social media, the advent of mobile computing, and the development of cloud technology have further reshaped our digital landscape.\r\n\r\nToday, the Internet is an integral part of our global infrastructure, a platform for innovation and a tool for democratizing information, connecting billions of people worldwide. Its evolution continues, with emerging technologies like the Internet of Things (IoT) and 5G promising to unlock new possibilities and redefine our digital future.\r\n\r\n", "Artificial Intelligence (AI) refers to the simulation of human intelligence in machines that are programmed to think like humans and mimic their actions. The term can also be applied to any machine that exhibits traits associated with a human mind such as learning and problem-solving.\r\n\r\nThe roots of AI can be traced back to antiquity, with myths, stories, and speculations about artificial beings endowed with intelligence or consciousness by master craftsmen. However, the field of AI as we know it today began in the mid-20th century, inspired by classical philosophers' attempts to describe human thinking as a symbolic system.\r\n\r\nThe core problems of AI include programming computers for traits such as knowledge, reasoning, problem-solving, perception, learning, planning, and the ability to manipulate and move objects. Long-term goals of AI research include achieving Creativity, Social Intelligence, and General (Human Level) Intelligence.\r\n\r\nAI has been used in various applications, from basic algorithms that predict what type of products consumers buy to self-driving cars. AI technologies, including machine learning, deep learning, and natural language processing, are now an integral part of our everyday lives, embedded in systems that analyze data, make decisions, and predict outcomes.\r\n\r\nDespite its vast potential, AI raises ethical and societal concerns, including privacy issues, employment displacement, and the potential for misuse. As AI continues to evolve, ongoing research and dialogue are crucial to harness its benefits while mitigating its risks.\r\n\r\n", "The Industrial Revolution was a period of major industrialization and innovation that began in Great Britain in the late 18th century and spread to other parts of the world. It marked a significant turning point in history, as almost every aspect of daily life was influenced by some form of technological advancement.\r\n\r\nBefore the revolution, manufacturing was predominantly done in people's homes, using basic hand tools or simple machines. The Industrial Revolution ushered in complex machinery, factory systems, and significant advancements in transportation and communication, fundamentally changing the structure of societies.\r\n\r\nKey innovations during this period included the steam engine, which significantly improved transportation and manufacturing processes, and the spinning jenny, which revolutionized the textile industry. These innovations led to increased production capabilities, reduced manual labor requirements, and enhanced efficiencies in various industries.\r\n\r\nThe Industrial Revolution also had profound social implications. Urbanization increased as people moved to cities to work in factories, leading to the growth of new urban centers. This shift resulted in significant demographic, economic, and social changes, including the rise of a new industrial working class and the expansion of the middle class.\r\n\r\nHowever, the rapid industrialization also brought challenges, such as poor working conditions, child labor, and environmental pollution. These issues eventually led to the development of labor unions and new social policies aimed at improving workers' rights and living conditions.\r\n\r\nOverall, the Industrial Revolution was a catalyst for immense economic growth and technological progress, setting the stage for the modern industrialized world. Its legacy is evident in the continued innovation and technological advancements that drive today's economies.\r\n\r\n", "The Human Genome Project (HGP) was an international scientific research project with the goal of determining the sequence of nucleotide base pairs that make up human DNA and of identifying and mapping all the genes of the human genome from both a physical and functional standpoint. Initiated in 1990 and completed in 2003, the HGP was one of the largest collaborative scientific endeavors of its kind.\r\n\r\nThe project aimed to provide a comprehensive resource on human genetics and to lay the foundation for exploring the molecular basis of human biology and disease. By understanding the genetic blueprint of a human being, scientists hoped to gain insights into the mechanisms of genetic diseases, leading to new strategies for their diagnosis, treatment, and prevention.\r\n\r\nThe HGP had far-reaching impacts on genetics, medicine, and many other fields. It has facilitated the discovery of numerous genes associated with diseases, enabling more precise diagnostic tests and paving the way for personalized medicine, where treatments can be tailored to an individual's genetic profile.\r\n\r\nMoreover, the project spurred advancements in biotechnology and bioinformatics, creating new industries and enhancing our ability to analyze and manipulate genetic information. The ethical, legal, and social implications of the HGP have also been significant, prompting discussions and debates about privacy, genetic discrimination, and the nature of human understanding itself.\r\n\r\nToday, the legacy of the HGP continues to influence science and medicine, driving ongoing research in genomics, genetics, and biology and shaping our approach to health and disease in the 21st century.\r\n\r\n", "Renewable energy sources are essential to the global strategy for reducing reliance on fossil fuels and mitigating climate change. Unlike non-renewable sources like coal, oil, and natural gas, renewable energy sources regenerate naturally and are considered inexhaustible on a human timescale. The primary forms of renewable energy include solar, wind, hydro, geothermal, and biomass.\r\n\r\nSolar Energy: Utilizing the sun's rays, solar power is harnessed using photovoltaic cells that convert sunlight directly into electricity. Solar energy is abundant, sustainable, and available worldwide, making it a key component in the renewable energy mix. Its applications range from small-scale systems for individual homes to large solar power plants.\r\n\r\nWind Energy: Wind power is generated using wind turbines that convert the kinetic energy of wind into electrical energy. It's one of the fastest-growing energy sources globally, with wind farms now a common sight in many countries. Wind energy is clean, cost-effective, and efficient, though its availability can vary based on location and meteorological conditions.\r\n\r\nHydroelectric Energy: This form of energy uses the flow of water to generate power, typically through dams or hydroelectric power plants. It's one of the oldest and most reliable forms of renewable energy. While hydroelectric power is a significant energy source, its environmental impact and feasibility depend on specific geographic and ecological conditions.\r\n\r\nGeothermal Energy: Geothermal power harnesses the Earth's internal heat to produce electricity and heating. This energy source is particularly effective in volcanic regions where the Earth's heat is more accessible. Geothermal plants can provide a consistent energy output, unlike some other renewable sources that may vary with weather conditions.\r\n\r\nBiomass Energy: Biomass energy is derived from organic materials like plants, wood, and waste. When these materials are burned, they release stored energy from the sun, making biomass a natural and renewable energy source. However, its sustainability depends on the careful management and replenishment of the biomass sources.\r\n\r\nThe transition to renewable energy is crucial for reducing greenhouse gas emissions, combating climate change, and promoting global energy security. While challenges remain in terms of storage, transmission, and variability, advancements in technology and increasing investment in renewables are making these energy sources more viable and integral to our energy future.\r\n\r\n"];

        public static string dirtyJson = @"```json { ""topic"": ""Astronomy - Lifecycle of a Star"", ""summary"": ""Stars evolve from gas clouds into protostars, which eventually ignite nuclear fusion to become main-sequence stars. The mass of a star determines its lifespan and its evolutionary path, leading to outcomes such as red supergiants, supernovae, neutron stars, black holes, and white dwarfs."", ""questions"": [ { ""question"": ""What is the initial stage in the lifecycle of a star?"", ""options"": { ""A"": ""Protostar formation in a nebula"", ""B"": ""Main-sequence star birth"", ""C"": ""Supernova explosion"", ""D"": ""White dwarf formation"" }, ""answer"": ""A"" }, { ""question"": ""What process powers a main-sequence star, like our Sun?"", ""options"": { ""A"": ""Hydrogen fusion"", ""B"": ""Helium fusion"", ""C"": ""Carbon fusion"", ""D"": ""Oxygen fusion"" }, ""answer"": ""A"" }, { ""question"": ""Which type of star has a longer lifespan: massive or less massive stars?"", ""options"": { ""A"": ""Massive stars"", ""B"": ""Less massive stars"" }, ""answer"": ""B"" }, { ""question"": ""What do massive stars evolve into after exhausting their fuel?"", ""options"": { ""A"": ""Red dwarfs"", ""B"": ""Red giants"", ""C"": ""Red supergiants"", ""D"": ""Planetary nebulae"" }, ""answer"": ""C"" }, { ""question"": ""What is left behind after a massive star undergoes a supernova explosion?"", ""options"": { ""A"": ""Red giant"", ""B"": ""Neutron star"", ""C"": ""White dwarf"", ""D"": ""Red supergiant"" }, ""answer"": ""B"" }, { ""question"": ""What is the final evolutionary stage of less massive stars like our Sun?"", ""options"": { ""A"": ""Black hole"", ""B"": ""Red giant"", ""C"": ""Planetary nebula"", ""D"": ""Red supergiant"" }, ""answer"": ""C"" }, { ""question"": ""Which stellar remnants have extremely high gravitational pull?"", ""options"": { ""A"": ""Neutron stars"", ""B"": ""Black holes"", ""C"": ""Red giants"", ""D"": ""White dwarfs"" }, ""answer"": ""B"" }, { ""question"": ""What happens to the outer layers of a less massive star before it becomes a white dwarf?"", ""options"": { ""A"": ""They form a black hole"", ""B"": ""They dissipate into space"", ""C"": ""They create a planetary nebula"", ""D"": ""They collapse inward"" }, ""answer"": ""C"" }, { ""question"": ""What marks the birth of a main-sequence star?"", ""options"": { ""A"": ""Protostar formation"", ""B"": ""Supernova explosion"", ""C"": ""Black hole creation"", ""D"": ""Nuclear fusion ignition"" }, ""answer"": ""D"" }, { ""question"": ""Why do stars of different masses have varied lifespans and evolutionary paths?"", ""options"": { ""A"": ""Due to differences in color"", ""B"": ""Because they are further from Earth"", ""C"": ""Based on the rate of nuclear fusion"", ""D"": ""As a result of their gravitational pull"" }, ""answer"": ""C"" } ] } ```";

        public static string jsonWithMissingBracket = "{\n  \"topic\": \"Biology - Photosynthesis\",\n  \"summary\": \"Photosynthesis is a vital process that uses light energy to convert into chemical energy stored in glucose. It takes place in chloroplasts of plant cells and involves two main phases: light-dependent reactions and the Calvin cycle.\",\n  \"questions\": [\n    {\n      \"question\": \"In which organelle of plant cells does photosynthesis mainly occur?\",\n      \"options\": {\n        \"A\": \"Mitochondria\",\n        \"B\": \"Nucleus\",\n        \"C\": \"Chloroplast\",\n        \"D\": \"Endoplasmic Reticulum\"\n      },\n      \"answer\": \"C\"\n    },\n    {\n      \"question\": \"What is the primary function of chlorophyll in photosynthesis?\",\n      \"options\": {\n        \"A\": \"Capture sunlight\",\n        \"B\": \"Store glucose\",\n        \"C\": \"Produce oxygen\",\n        \"D\": \"Produce carbon dioxide\"\n      },\n      \"answer\": \"A\"\n    },\n    {\n      \"question\": \"Which products are produced in the light-dependent reactions of photosynthesis?\",\n      \"options\": {\n        \"A\": \"Glucose and oxygen\",\n        \"B\": \"ATP and NADPH\",\n        \"C\": \"Carbon dioxide\",\n        \"D\": \"Water\"\n      },\n      \"answer\": \"B\"\n    },\n    {\n      \"question\": \"What is the role of the Calvin cycle in photosynthesis?\",\n      \"options\": {\n        \"A\": \"Capture sunlight\",\n        \"B\": \"Produce glucose\",\n        \"C\": \"Produce oxygen\",\n        \"D\": \"Produce carbon dioxide\"\n      },\n      \"answer\": \"B\"\n  ]\n}";
    }
}