namespace P03_FootballBetting.Data.ConfigurationModels
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using P03_FootballBetting.Data.Models;

    public class UserConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> user)
        {
            user
                .HasKey(k => k.UserId);

            user
                .Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired(true)
                .IsUnicode(true);

            user
                .Property(p => p.Email)
                .IsRequired(true);

            user
                .Property(p => p.Password)
                .IsRequired();

            user
                .Property(p => p.Balance)
                .IsRequired();

        }
    }
}
