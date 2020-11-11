using BusinessObjects;
using DataObjects.IRepository;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataObjects.Repository
{
    class UserRepository : GenericRepository<ApplicationUser> , IUserRepository
    {
        public UserRepository(ApplicationDBContext context) : base(context)
        {

        }

    }
}
