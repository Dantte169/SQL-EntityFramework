namespace P03_FootballBetting.Data.ConfigurationModels
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using P03_FootballBetting.Data.Models;

    public class PlayerConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> player)
        {
            player
                .HasKey(k => k.PlayerId);

            player
                .Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired(true);

            player
                .HasOne(t => t.Team)
                .WithMany(p => p.Players)
                .HasForeignKey(t => t.TeamId);

            player
                .HasOne(p => p.Position)
                .WithMany(pp => pp.Players)
                .HasForeignKey(p => p.PositionId);

        }
    }
}
