namespace P03_FootballBetting.Data.ConfigurationModels
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using P03_FootballBetting.Data.Models;

    public class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> game)
        {
            game
                .HasKey(k => k.GameId);

            game
                .HasOne(ht => ht.HomeTeam)
                .WithMany(g => g.HomeGames)
                .HasForeignKey(ht => ht.HomeTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            game
                .HasOne(ht => ht.AwayTeam)
                .WithMany(g => g.AwayGames)
                .HasForeignKey(ht => ht.AwayTeamId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
