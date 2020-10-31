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
        private readonly UserManager<ApplicationUser> _userManager;
        public UserRepository(ApplicationDBContext context,
            UserManager<ApplicationUser> userManager) : base(context)
        {
            _userManager = userManager;
        }

    }
}
