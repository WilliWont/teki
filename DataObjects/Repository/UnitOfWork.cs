using DataObjects.IRepository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Repository
{
    // This class includes all of repositories and declare them with same DBContext
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDBContext _context;

        public UnitOfWork(ApplicationDBContext context)
        {
            _context = context;
            InitRepositories();
        }
        public IArticleRepository ArticleRepository { get; private set; }

        public IStatusRepository StatusRepository { get; private set; }

        // Initialize all repositories with the same DBContext.
        private void InitRepositories()
        {
            ArticleRepository = new ArticleRepository(_context);
            StatusRepository = new StatusRepository(_context);
        }

        // Commit: This function is used to save any change into database when updates, delete , etc 
        public async Task<bool> Commit()
        {
            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }
    }
}
