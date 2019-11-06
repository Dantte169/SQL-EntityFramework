namespace P03_FootballBetting.Data.ConfigurationModels
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using P03_FootballBetting.Data.Models;

    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> team)
        {
            team
                .HasKey(k => k.TeamId);


            team
            .HasOne(e => e.PrimaryKitColor)
            .WithMany(pc => pc.PrimaryKitTeams)
            .HasForeignKey(k => k.PrimaryKitColorId)
            .OnDelete(DeleteBehavior.Restrict);

            team
            .HasOne(e => e.SecondaryKitColor)
            .WithMany(pc => pc.SecondaryKitTeams)
            .HasForeignKey(k => k.SecondaryKitColorId)
            .OnDelete(DeleteBehavior.Restrict);

            team
                .HasOne(t => t.Town)
                .WithMany(tt => tt.Teams)
                .HasForeignKey(t => t.TownId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
