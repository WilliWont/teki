using System;
using System.IO;
using BusinessObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DataObjects
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
          : base(options)
        {

        }

        public DbSet<Status> Statuses { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Bookmark> Bookmarks { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ArticleTag> ArticleTags { get; set; }
        // This funtion is used to create identity tables to database when migration.
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Bookmark>().HasKey(x => new { x.UserID, x.ArticleID });
            builder.Entity<Bookmark>().HasOne(x => x.User).WithMany(x => x.BookmarkList).HasForeignKey(x => x.UserID);
            builder.Entity<Bookmark>().HasOne(x => x.Article).WithMany(x => x.BookmarkedUsers).HasForeignKey(x => x.ArticleID);


            base.OnModelCreating(builder);
            builder.HasDefaultSchema("Teki");
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "User");
            });
            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Role");
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });
            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });
            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });
            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });
            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });
            builder.Entity<Status>(entity =>
            {
                entity.ToTable("Status");
            });
            builder.Entity<Article>(entity =>
            {
                entity.ToTable("Article");
            });

            builder.Entity<Tag>(entity =>
            {
                entity.ToTable("Tag");
            });
            builder.Entity<ArticleTag>(entity =>
            {
                entity.ToTable("ArticleTag");
                entity.HasKey(articleTag => new { articleTag.ArticleId, articleTag.TagId });
                entity.HasOne(articleTag => articleTag.Tag)
                    .WithMany(tag => tag.ArticleTags).HasForeignKey(articleTag => articleTag.TagId);
                entity.HasOne(articleTag => articleTag.Article)
                .WithMany(article => article.ArticleTags)
                .HasForeignKey(articleTag => articleTag.ArticleId);
            });
            //builder.Entity<A>
        }
    }

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDBContext>
    {
        public ApplicationDBContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(@Directory.GetCurrentDirectory() + "/../TekiBlog/appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<ApplicationDBContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connectionString);
            return new ApplicationDBContext(builder.Options);
        }
    }
}
