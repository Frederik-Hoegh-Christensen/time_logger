using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configurations
{
    public class TimeRegistrationConfiguration : IEntityTypeConfiguration<TimeRegistration>
    {
        public void Configure(EntityTypeBuilder<TimeRegistration> builder)
        {
            // Set primary key
            builder.HasKey(tr => tr.Id);

            builder.Property(tr => tr.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(tr => tr.WorkDate)
                .IsRequired();

            builder.Property(tr => tr.HoursWorked)
                .IsRequired()
                .HasColumnType("decimal(5,2)"); 

            builder.Property(tr => tr.Description)
                .HasMaxLength(500);

            builder.HasOne(tr => tr.Project)  // Specify the navigation property
                .WithMany(p => p.TimeRegistrations)
                .HasForeignKey(tr => tr.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Freelancer>()
                .WithMany()
                .HasForeignKey(tr => tr.FreelancerId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deletion of freelancers if there are time registrations

    
            builder.ToTable("TimeRegistrations");
        }
    }
}
