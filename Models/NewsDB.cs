using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace NewsWebSite.Models
{
    public partial class NewsDB : DbContext
    {
        public NewsDB()
            : base("name=NewsDB")
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<user> users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .Property(e => e.categoryName)
                .IsUnicode(false);

            modelBuilder.Entity<Category>()
                .HasMany(e => e.News)
                .WithRequired(e => e.Category)
                .HasForeignKey(e => e.Category_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<News>()
                .Property(e => e.title)
                .IsUnicode(false);

            //modelBuilder.Entity<News>()
            //    .Property(e => e.brief)
            //    .IsUnicode(false);

            modelBuilder.Entity<News>()
                .Property(e => e.decription)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.firstName)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.lastName)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.pass_word)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.photo)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.brief)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .HasMany(e => e.News)
                .WithRequired(e => e.user)
                .WillCascadeOnDelete(false);
        }
    }
}
