using System;
using System.Collections.Generic;
using System.Text;
using BusinessObjects;

namespace DataObjects.IRepository
{
    public interface ITagRepository : IGenericRepository<Tag>
    {
        public void UpdateTag(Tag tag);
    }
}
