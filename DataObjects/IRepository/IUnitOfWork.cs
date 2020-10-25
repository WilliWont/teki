﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.IRepository
{
    public interface IUnitOfWork
    {
        IArticleRepository ArticleRepository { get; }
        IStatusRepository StatusRepository { get; }
        Task<bool> Commit();
    }
}