namespace P03_FootballBetting.Data.ConfigurationModels
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using P03_FootballBetting.Data.Models;

    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> country)
        {
            country
                .HasKey(k => k.CountryId);

            country
                .Property(p => p.Name)
                .HasMaxLength(80)
                .IsRequired();

            country
                .HasMany(t => t.Towns)
                .WithOne(c => c.Country)
                .HasForeignKey(t => t.TownId);
        }
    }
}
