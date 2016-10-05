using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AspNetCore.Models;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AspNetCore.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TestPackage> TestPackages { get; set; }
        public DbSet<TestResult> TestResults { get; set; }
        public DbSet<TestItem> TestItems { get; set; }
        public DbSet<TestOption> TestOptions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TestResult>()
            .HasOne(p => p.User)
            .WithMany(b => b.Testresults)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}
