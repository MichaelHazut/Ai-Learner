using DataAccessLayer.dbContext;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;


namespace DataAccessLayer.UnitOfWork
{
    // The UnitOfWork class is responsible for managing the repositories and the database context.
    // It implements the IUnitOfWork interface and provides the implementation for the repositories.
    public class UnitOfWork(AiLearnerDbContext context, IUserRepo userRepo, IMaterialRepo materialRepo, IQuestionRepo questionRepo, IAnswerRepo answerRepo, IUsersAnswersRepo usersAnswersRepo) : IUnitOfWork
    {
        // The UnitOfWork class has a constructor that takes the database context and the repositories as parameters.
        public IUserRepo Users { get; private set; } = userRepo;
        public IMaterialRepo Materials { get; private set; } = materialRepo;
        public IQuestionRepo Questions { get; private set; } = questionRepo;
        public IAnswerRepo Answers { get; private set; } = answerRepo;
        public IUsersAnswersRepo UserAnswers { get; private set; } = usersAnswersRepo;

        private readonly AiLearnerDbContext _context = context;

        // The CompleteAsync method saves the changes to the database
        // and returns the number of state enteries wirrten to the database.
        public async Task<int> CompleteAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // The DisposeAsync method disposes the database context.
        public async ValueTask DisposeAsync()
        {
            if (_context != null)
            {
                await _context.DisposeAsync();
            }
        }
        public void Dispose()
        {
            DisposeAsync().GetAwaiter().GetResult();
        }
    }

}

