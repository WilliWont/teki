using DataObjects.IRepository;
using System;
using System.Collections.Generic;
using System.Text;
using BusinessObjects;

namespace DataObjects.Repository
{
    public class TagRepository : GenericRepository<Tag>, ITagRepository
    {
        public TagRepository(ApplicationDBContext context) : base(context)
        {

        }

       
    }
}
