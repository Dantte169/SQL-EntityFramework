namespace P03_FootballBetting.Data.ConfigurationModels
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using P03_FootballBetting.Data.Models;

    public class BetConfiguration : IEntityTypeConfiguration<Bet>
    {
        public void Configure(EntityTypeBuilder<Bet> bet)
        {
            bet
                .HasKey(k => k.BetId);

            bet
                .HasOne(u => u.User)
                .WithMany(b => b.Bets)
                .HasForeignKey(u => u.UserId);

            bet
                .HasOne(g => g.Game)
                .WithMany(b => b.Bets)
                .HasForeignKey(g => g.GameId);

            bet
                .Property(p => p.Amount)
                .IsRequired();

            bet
                .Property(p => p.Prediction)
                .IsRequired();

            bet
                .Property(p => p.DateTime)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
