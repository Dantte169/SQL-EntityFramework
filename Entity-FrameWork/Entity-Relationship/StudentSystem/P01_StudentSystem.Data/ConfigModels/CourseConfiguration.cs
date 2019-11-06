namespace P01_StudentSystem.Data.ConfigModels
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using P01_StudentSystem.Data.Models;

    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> courseEntity)
        {
            courseEntity
                .HasKey(k => k.CourseId);

            courseEntity
                .Property(p => p.Name)
                .HasMaxLength(80)
                .IsUnicode(true)
                .IsRequired(true);

            courseEntity
                .Property(p => p.Description)
                .IsUnicode(true)
                .IsRequired(false);

            courseEntity
                .Property(p => p.StartDate)
                .IsRequired(true)
                .HasColumnType("datetime2");

            courseEntity
                .Property(p => p.EndDate)
                .IsRequired(true)
                .HasColumnType("datetime2");

            courseEntity
                .Property(p => p.Price)
                .IsRequired(true);


        }
    }
}
