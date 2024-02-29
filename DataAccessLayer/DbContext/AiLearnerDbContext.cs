﻿using DataAccessLayer.models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace DataAccessLayer.DbContext
{
    public class AiLearnerDbContext(DbContextOptions<AiLearnerDbContext> options) : IdentityDbContext<User>(options)
    {  
        public DbSet<Material> Materials { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserAnswer>()
                .HasOne(ua => ua.Question)
                .WithMany()
                .HasForeignKey(ua => ua.QuestionId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserAnswer>()
                .HasOne(ua => ua.User)
                .WithMany()
                .HasForeignKey(ua => ua.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserAnswer>()
                .HasOne(ua => ua.Answer)
                .WithMany()
                .HasForeignKey(ua => ua.AnswerId)
                .OnDelete(DeleteBehavior.NoAction);
        }

    }
}

/*

To Create Migration Type In Packet Manager Console
EntityFrameworkCore\Add-Migration InitialCreate -Project DataAccessLayer -StartupProject ExpenSage

To Update Database Type In Packet Manager Console
EntityFrameworkCore\Update-Database -Project DataAccessLayer -StartupProject ExpenSage

*/

