﻿using BusinessObjects;
using DataObjects;
using DataObjects.IRepository;
using DataObjects.Repository;
using System;
using System.Collections.Generic;
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
            Article article = articleRepository.GetByID(id);
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
    }
}