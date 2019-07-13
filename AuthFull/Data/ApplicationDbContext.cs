using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AuthFull.Models;

namespace AuthFull.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext() { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Applican> Applicant { get; set; }
        public DbSet<Academic> Academics { get; set; }
        public DbSet<WorkExperience> WorkEx { get; set; }
        public DbSet<Comment> CommentsSection { get; set; }
        public DbSet<FileDetails> FileStorage { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Course>().ToTable("Course");
        }

    }
}
