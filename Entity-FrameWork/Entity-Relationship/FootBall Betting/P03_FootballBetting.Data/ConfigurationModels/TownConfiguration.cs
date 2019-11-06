namespace P03_FootballBetting.Data.ConfigurationModels
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using P03_FootballBetting.Data.Models;

    public class TownConfiguration : IEntityTypeConfiguration<Town>
    {
        public void Configure(EntityTypeBuilder<Town> town)
        {
            town
                .HasKey(k => k.TownId);

            town
                .HasMany(t => t.Teams)
                .WithOne(tt => tt.Town)
                .HasForeignKey(t => t.TeamId)
                .OnDelete(DeleteBehavior.Restrict);

            town
                .Property(p => p.Name)
                .HasMaxLength(30)
                .IsRequired();
        }
    }
}
