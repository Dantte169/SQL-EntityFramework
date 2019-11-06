namespace P01_StudentSystem.Data.ConfigModels
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using P01_StudentSystem.Data.Models;

    public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
    {
        public void Configure(EntityTypeBuilder<Resource> resourceEntity)
        {
            resourceEntity
                .HasKey(k => k.ResourceId);

            resourceEntity
                .Property(p => p.Name)
                .HasMaxLength(50)
                .IsUnicode(true)
                .IsRequired(true);

            resourceEntity
                .Property(p => p.Url)
                .IsUnicode(false);

            resourceEntity
                .HasOne(r => r.Course)
                .WithMany(c => c.Resources)
                .HasForeignKey(r => r.CourseId);

        }
    }
}
