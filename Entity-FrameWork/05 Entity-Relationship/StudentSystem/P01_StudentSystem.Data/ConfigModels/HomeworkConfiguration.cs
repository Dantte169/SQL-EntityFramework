namespace P01_StudentSystem.Data.ConfigModels
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using P01_StudentSystem.Data.Models;

    public class HomeworkConfiguration : IEntityTypeConfiguration<Homework>
    {
        public void Configure(EntityTypeBuilder<Homework> homeworkEntity)
        {
            homeworkEntity
                .HasKey(k => k.HomeworkId);

            homeworkEntity
                .Property(p => p.Content)
                .IsUnicode(false);

            homeworkEntity
                .HasOne(s => s.Student)
                .WithMany(sh => sh.HomeworkSubmissions)
                .HasForeignKey(s => s.StudentId);

            homeworkEntity
                 .HasOne(c => c.Course)
                 .WithMany(ch => ch.HomeworkSubmissions)
                 .HasForeignKey(c => c.CourseId);
        }
    }
}
