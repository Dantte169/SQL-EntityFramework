namespace P03_FootballBetting.Data.ConfigurationModels
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using P03_FootballBetting.Data.Models;

    public class PlayerStatisticConfiguration : IEntityTypeConfiguration<PlayerStatistic>
    {
        public void Configure(EntityTypeBuilder<PlayerStatistic> ps)
        {
            ps
                .HasKey(k => new { k.GameId, k.PlayerId });

            ps
                .HasOne(p => p.Player)
                .WithMany(ps => ps.PlayerStatistics)
                .HasForeignKey(p => p.PlayerId);

            ps
                .HasOne(p => p.Game)
                .WithMany(ps => ps.PlayerStatistics)
                .HasForeignKey(p => p.GameId);
        }
    }
}
