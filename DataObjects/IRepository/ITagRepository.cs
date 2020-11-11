using System;
using System.Collections.Generic;
using System.Text;
using BusinessObjects;

namespace DataObjects.IRepository
{
    public interface ITagRepository : IGenericRepository<Tag>
    {
        public void UpdateTag(Tag tag);
        //public IQueryable<Tag> GetTagsAlphabetically();
        public void AddArticleTag(Guid articleID, List<int> tagList);
        public void UpdateArticleTag(Guid articleID, List<int> tagList);

    }
}
