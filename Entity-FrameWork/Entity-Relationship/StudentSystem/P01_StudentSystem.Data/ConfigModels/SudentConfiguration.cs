
namespace P01_StudentSystem.Data.ConfigModels
{
    using P01_StudentSystem.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class SudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> studentEntity)
        {
            studentEntity
                .HasKey(k => k.StudentId);

            studentEntity
                .Property(p => p.Name)
                .HasMaxLength(100)
                .IsUnicode(true);

            studentEntity
                .Property(p => p.PhoneNumber)
                .HasColumnType("CHAR(10)")
                .IsUnicode(false)
                .IsRequired(false);

            studentEntity
                .Property(p => p.RegisteredOn)
                .HasColumnType("DATETIME2")
                .HasDefaultValueSql("GETDATE()");

            studentEntity
                .Property(p => p.Birthday)
                .HasColumnType("DateTime2")
                .IsRequired(false);
                

        }
    }
}
