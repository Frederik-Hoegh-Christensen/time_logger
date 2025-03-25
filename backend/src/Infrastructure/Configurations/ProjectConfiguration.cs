using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.HasOne(p => p.Customer)
                .WithMany(c => c.Projects)
                .HasForeignKey(p => p.CustomerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.Deadline)
                .IsRequired();

            builder.HasOne(p => p.Freelancer)
                .WithMany(f => f.Projects)
                .HasForeignKey(p => p.FreelancerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade); 
            builder.Property(p => p.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
            builder.Property(p => p.IsCompleted)
                .IsRequired();

            builder.ToTable("Projects");
        }
    }
}
