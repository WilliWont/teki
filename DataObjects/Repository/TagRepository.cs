using DataObjects.IRepository;
using System;
using System.Collections.Generic;
using System.Text;
using BusinessObjects;
using System.Linq;

namespace DataObjects.Repository
{
    public class TagRepository : GenericRepository<Tag>, ITagRepository
    {
        public TagRepository(ApplicationDBContext context) : base(context)
        {

        }

        public void AddArticleTag(Guid articleID, List<int> tagList)
        {
            if(tagList == null || tagList.Count == 0)
                return;

            foreach(int t in tagList){
                ArticleTag tag = new ArticleTag
                {
                    ArticleId = articleID,
                    TagId = t
                };

                _context.Add(tag);
            }
        }

        public void UpdateArticleTag(Guid articleID, List<int> tagList)
        {
            if (tagList == null || tagList.Count == 0)
                return;

            IQueryable<ArticleTag> pastTags = _context.ArticleTags.Select(a => a).Where(a => a.ArticleId == articleID);

            _context.ArticleTags.RemoveRange(pastTags);

            AddArticleTag(articleID, tagList);
        }

        public void UpdateTag(Tag tag)
        {
            _context.Update(tag);
        }

    
    }
}
