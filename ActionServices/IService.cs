using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Microsoft.AspNetCore.Http;

namespace ActionServices
{
    public interface IService
    {
        // Misc Services
        List<int> StringToInt(List<string> input);

        // Article Services
        Article GetArticle(Guid id);
        IEnumerable<Article> GetAllArticle();
        void AddArticle(Article article);
        IQueryable<Article> GetArticleWithUser(ApplicationUser user);
        bool UpdateArticle(Article article);
        IQueryable<Article> SearchArticle(string searchValue);
        IQueryable<Article> GetArticleForViewer(ApplicationUser user);
        IQueryable<Article> GetArticleByStatus(string status);
        IQueryable<Article> GetArticleForAdmin();
        IQueryable<Article> GetUserDrafts(ApplicationUser user);
        IQueryable<Article> GetArticleByTag(int tagid);
        void GetImage(out byte[] img, HttpRequest req);
        byte[] ResizeImgageByWidth(byte[] originalBytes, int w, ImageFormat format);
        byte[] CropImage(byte[] originalBytes, Rectangle crop, ImageFormat format);
        //bool DeleteArticlesByAdmin(Guid id);

        // Bookmark Services
        void AddBookmark(Bookmark bookmark);
        void RemoveBookmark(Bookmark bookmark);

        IQueryable<Bookmark> GetBookmarks(Article article, ApplicationUser user);
        IQueryable<Bookmark> GetBookmarks(Article article);
        IQueryable<Bookmark> GetBookmarks(ApplicationUser user);
        IQueryable<Bookmark> GetBookmarks(ApplicationUser user, bool includeArticle);


        // Status Services
        Status GetStatus(string name);
        
        // Tag Services
        IEnumerable<Tag> GetAllTags();
        Tag GetTagByID(int id);
        void CreateTag(Tag tag);
        bool DeleteTag(int id);
        IEnumerable<Tag> GetAllActiveTags();
        bool RestoreTag(int id);
        Task<bool> Commit();
        void AddArticleTag(Guid articleID, List<int> tagList);
        void UpdateArticleTag(Guid articleID, List<int> tagList);


        // Cloud Service
        Task UploadToS3(string k, string sK, string bucketName, byte[] file, string fileName);
    }
}
