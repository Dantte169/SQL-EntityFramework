namespace P01_StudentSystem.Data.ConfigModels
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using P01_StudentSystem.Data.Models;

    public class StudentCourseConfiguration : IEntityTypeConfiguration<StudentCourse>
    {
        public void Configure(EntityTypeBuilder<StudentCourse> studentCourse)
        {
            studentCourse
                .HasKey(k => new { k.CourseId, k.StudentId });

            studentCourse
                .HasOne(s => s.Student)
                .WithMany(s => s.CourseEnrollments)
                .HasForeignKey(s => s.StudentId);

            studentCourse
                .HasOne(c => c.Course)
                .WithMany(s => s.StudentsEnrolled)
                .HasForeignKey(c => c.CourseId);
        }
    }
}
