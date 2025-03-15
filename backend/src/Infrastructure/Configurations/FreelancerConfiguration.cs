using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructure.Configurations
{
    public class FreelancerConfiguration : IEntityTypeConfiguration<Freelancer>
    {
        public void Configure(EntityTypeBuilder<Freelancer> builder)
        {
            builder.HasKey(f => f.Id);

            builder.Property(f => f.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(f => f.FirstName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(f => f.LastName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(f => f.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasMany(f => f.Projects)
                .WithOne(p => p.Freelancer)
                .HasForeignKey(p => p.FreelancerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Freelancers");
        }
    }
}
