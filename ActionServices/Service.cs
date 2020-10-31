﻿using BusinessObjects;
using DataObjects;
using DataObjects.IRepository;
using DataObjects.Repository;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ActionServices
{
    public class Service : IService
    {
        private readonly ApplicationDBContext _context;
        private readonly UnitOfWork unitOfwork;
        private readonly IArticleRepository articleRepository ;
        private readonly IStatusRepository statusRepository ;

        public Service(ApplicationDBContext context)
        {
            _context = context;
            unitOfwork = new UnitOfWork(_context);
            articleRepository = unitOfwork.ArticleRepository;
            statusRepository = unitOfwork.StatusRepository;
        } 

        // Article Service implemetation
        public IEnumerable<Article> GetAllArticle()
        {
            IEnumerable<Article> list = articleRepository.GetAll();
            return list;
        }
        public void AddArticle(Article article)
        {
            articleRepository.Add(article);
        }
        public Article GetArticle(Guid id)
        {
            Article article = articleRepository.GetArticleInfo(id);
            return article;
        }

        // Status Service Implementation
        public Status GetStatus(string name)
        {
            Status status = statusRepository.GetStatus(name);
            return status;
        }

        public async Task<bool> Commit()
        {
            return await unitOfwork.Commit();
        }

        public IQueryable<Article> GetArticleWithUserID(ApplicationUser user)
        {
            return articleRepository.GetArticlesByID(user);
        }

        public bool UpdateArticle(Article article)
        {
            return articleRepository.UpdateArticle(article);
        }

        public void GetImage(out byte[] img, HttpRequest req)
        {
            foreach (var file in req.Form.Files)
            {
                // TODO: Compress Image if have time
                MemoryStream ms = new MemoryStream();
                file.CopyTo(ms);
                img = ms.ToArray();
                ms.Close();

                ms.Dispose();

                return;
            }
            img = null;
        }

        public IQueryable<Article> SearchArticle(string searchValue)
        {
            return articleRepository.SearchArticle(searchValue);
        }
    }
}
