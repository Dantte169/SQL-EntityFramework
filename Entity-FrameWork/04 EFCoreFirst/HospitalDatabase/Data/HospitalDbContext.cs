namespace P01_HospitalDatabase.Data.Models
{
    using Microsoft.EntityFrameworkCore;

    public class HospitalDbContext : DbContext
    {
        public DbSet<Diagnose> Diagnoses { get; set; }

        public DbSet<Medicament> Medicaments { get; set; }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<PatientMedicament> PatientMedicaments { get; set; }

        public DbSet<Visitation> Visitations { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer
                (@"Server=DESKTOP-MHJ8P43\SQLEXPRESS;Database=HospitalDatabase;Integrated Security=true;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Diagnose>()
                .HasKey(k => k.DiagnoseId);

            modelBuilder.Entity<Diagnose>()
                .Property(p => p.Name)
                .HasMaxLength(50)
                .IsUnicode(false);

            modelBuilder.Entity<Diagnose>()
                .Property(p => p.Comments)
                .HasMaxLength(250)
                .IsUnicode(false);

            modelBuilder.Entity<Diagnose>()
                .HasOne(p => p.Patient)
                .WithMany(pd => pd.Diagnoses)
                .HasForeignKey(k => k.PatientId);

            modelBuilder.Entity<Medicament>()
                .HasKey(k => k.MedicamentId);

            modelBuilder.Entity<Medicament>()
                .Property(p => p.Name)
                .HasMaxLength(50)
                .IsUnicode(false);

            modelBuilder.Entity<Medicament>()
                .HasMany(p => p.Prescriptions)
                .WithOne(m => m.Medicament)
                .HasForeignKey(k => k.MedicamentId);

            modelBuilder
               .Entity<PatientMedicament>()
               .HasKey(k => new { k.MedicamentId, k.PatientId });

            modelBuilder
                .Entity<PatientMedicament>()
                .HasOne(p => p.Patient)
                .WithMany(pm => pm.Prescriptions)
                .HasForeignKey(k => k.PatientId);

            modelBuilder
                .Entity<PatientMedicament>()
                .HasOne(p => p.Medicament)
                .WithMany(pm => pm.Prescriptions)
                .HasForeignKey(k => k.MedicamentId);

            modelBuilder.Entity<Visitation>()
                .HasKey(k => k.VisitationId);

            modelBuilder.Entity<Visitation>()
                .Property(p => p.Comments)
                .HasMaxLength(250)
                .IsUnicode(false);

            modelBuilder.Entity<Visitation>()
                .HasOne(p => p.Patient)
                .WithMany(v => v.Visitations)
                .HasForeignKey(k => k.PatientId);

            modelBuilder.Entity<Patient>()
                .HasKey(k => k.PatientId);

            modelBuilder.Entity<Patient>()
                .Property(p => p.FirstName)
                .HasMaxLength(50)
                .IsUnicode();

            modelBuilder.Entity<Patient>()
               .Property(p => p.LastName)
               .HasMaxLength(50)
               .IsUnicode();

            modelBuilder.Entity<Patient>()
               .Property(p => p.Address)
               .HasMaxLength(250)
               .IsUnicode();

            modelBuilder.Entity<Patient>()
               .Property(p => p.Email)
               .HasMaxLength(80);

            modelBuilder.Entity<Patient>()
               .Property(p => p.HasInsurance);


            modelBuilder.Entity<Patient>()
                .HasMany(p => p.Prescriptions)
                .WithOne(p => p.Patient)
                .HasForeignKey(k => k.PatientId);

            modelBuilder.Entity<Patient>()
                .HasMany(p => p.Diagnoses)
                .WithOne(p => p.Patient)
                .HasForeignKey(k => k.DiagnoseId);

            modelBuilder.Entity<Patient>()
                .HasMany(p => p.Visitations)
                .WithOne(p => p.Patient)
                .HasForeignKey(k => k.VisitationId);

            modelBuilder.Entity<Doctor>()
                .HasKey(k => k.DoctorId);

            modelBuilder.Entity<Doctor>()
                .Property(p => p.Name)
                .HasMaxLength(100)
                .IsUnicode();

            modelBuilder.Entity<Doctor>()
                .Property(p => p.Specialty)
                .HasMaxLength(100)
                .IsUnicode();
        }
    }
}
