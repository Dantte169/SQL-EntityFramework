namespace P03_FootballBetting.Data.ConfigurationModels
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using P03_FootballBetting.Data.Models;

    public class PositionConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> position)
        {
            position
                .HasKey(k => k.PositionId);

            position
                .Property(p => p.Name)
                .HasMaxLength(25)
                .IsRequired(true);
        }
    }
}
