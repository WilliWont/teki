using BusinessObjects;
using DataObjects.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataObjects.Repository
{
    public class StatusRepository : GenericRepository<Status>, IStatusRepository
    {
        // Inheritance from GenericRepository with applicationDBContext
        public StatusRepository(ApplicationDBContext context) : base(context)
        {
            
        }
        // Implement all additional  methods in terface IStatusRepository
        public Status GetStatus(string name)
        {
            Status result = _context.Statuses.First(x => x.Name.Equals(name));
            return result;
        }
    }
}
