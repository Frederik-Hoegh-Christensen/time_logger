using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Freelancer> Freelancers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TimeRegistration> TimeRegistrations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Freelancer → Project (1-to-Many)
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Freelancer)
                .WithMany(f => f.Projects)
                .HasForeignKey(p => p.FreelancerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Project → TimeRegistration (1-to-Many)
            modelBuilder.Entity<TimeRegistration>()
                .HasOne(tr => tr.Project)
                .WithMany(p => p.TimeRegistrations)
                .HasForeignKey(tr => tr.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // Freelancer → TimeRegistration (1-to-Many) (Redundant FK for validation)
            modelBuilder.Entity<TimeRegistration>()
                .HasOne(tr => tr.Freelancer)
                .WithMany()
                .HasForeignKey(tr => tr.FreelancerId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
