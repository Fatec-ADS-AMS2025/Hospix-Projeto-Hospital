using Hospital.Domain.Entities;
using Hospital.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Infrastructure.Persistence;

public class HospitalDbContext : DbContext
{
    public HospitalDbContext(DbContextOptions<HospitalDbContext> options) : base(options)
    {
    }

    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Bed> Beds => Set<Bed>();
    public DbSet<Admission> Admissions => Set<Admission>();
    public DbSet<Prescription> Prescriptions => Set<Prescription>();
    public DbSet<ExamRequest> ExamRequests => Set<ExamRequest>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedNever();
            entity.Property(x => x.FullName).HasMaxLength(160).IsRequired();
            entity.Property(x => x.InsuranceName).HasMaxLength(80).IsRequired();
            entity.Property(x => x.Cpf)
                .HasConversion(cpf => cpf.Value, value => new Cpf(value))
                .HasMaxLength(11)
                .IsRequired();
            entity.HasIndex(x => x.Cpf).IsUnique();
        });

        modelBuilder.Entity<Bed>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedNever();
            entity.Property(x => x.Code).HasMaxLength(20).IsRequired();
            entity.Property(x => x.Ward).HasMaxLength(80).IsRequired();
            entity.Property(x => x.Status).HasConversion<string>().HasMaxLength(32).IsRequired();
            entity.HasIndex(x => x.Code).IsUnique();
        });

        modelBuilder.Entity<Admission>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedNever();
            entity.Property(x => x.Status).HasConversion<string>().HasMaxLength(32).IsRequired();
            entity.HasOne(x => x.Patient).WithMany().HasForeignKey(x => x.PatientId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.Bed).WithMany().HasForeignKey(x => x.BedId).OnDelete(DeleteBehavior.Restrict);
            entity.HasMany(x => x.Prescriptions).WithOne().HasForeignKey(x => x.AdmissionId).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(x => x.Exams).WithOne().HasForeignKey(x => x.AdmissionId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Prescription>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedNever();
            entity.Property(x => x.MedicineName).HasMaxLength(120).IsRequired();
            entity.Property(x => x.Dose)
                .HasConversion(dose => dose.Value, value => new Dosage(value))
                .HasMaxLength(40)
                .IsRequired();
            entity.Property(x => x.Status).HasConversion<string>().HasMaxLength(32).IsRequired();
            entity.Ignore(x => x.Period);
        });

        modelBuilder.Entity<ExamRequest>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedNever();
            entity.Property(x => x.ExamType).HasMaxLength(120).IsRequired();
            entity.Property(x => x.Status).HasConversion<string>().HasMaxLength(32).IsRequired();
        });
    }
}
