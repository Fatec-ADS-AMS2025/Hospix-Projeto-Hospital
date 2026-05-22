using HospitalLegacy.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalLegacy.Api.Data;

public class HospitalDbContext : DbContext
{
    public HospitalDbContext(DbContextOptions<HospitalDbContext> options) : base(options)
    {
    }

    public DbSet<PatientRecord> Patients => Set<PatientRecord>();
    public DbSet<BedRecord> Beds => Set<BedRecord>();
    public DbSet<AdmissionRecord> Admissions => Set<AdmissionRecord>();
    public DbSet<PrescriptionRecord> Prescriptions => Set<PrescriptionRecord>();
    public DbSet<ExamRequestRecord> ExamRequests => Set<ExamRequestRecord>();
    public DbSet<InvoiceRecord> Invoices => Set<InvoiceRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PatientRecord>().HasKey(x => x.Id);
        modelBuilder.Entity<BedRecord>().HasKey(x => x.Id);
        modelBuilder.Entity<AdmissionRecord>().HasKey(x => x.Id);
        modelBuilder.Entity<PrescriptionRecord>().HasKey(x => x.Id);
        modelBuilder.Entity<ExamRequestRecord>().HasKey(x => x.Id);
        modelBuilder.Entity<InvoiceRecord>().HasKey(x => x.Id);

        modelBuilder.Entity<AdmissionRecord>()
            .HasOne(x => x.Patient)
            .WithMany()
            .HasForeignKey(x => x.PatientId);

        modelBuilder.Entity<AdmissionRecord>()
            .HasOne(x => x.Bed)
            .WithMany()
            .HasForeignKey(x => x.BedId);

        modelBuilder.Entity<PrescriptionRecord>()
            .HasOne(x => x.Admission)
            .WithMany(x => x.Prescriptions)
            .HasForeignKey(x => x.AdmissionId);

        modelBuilder.Entity<ExamRequestRecord>()
            .HasOne(x => x.Admission)
            .WithMany(x => x.Exams)
            .HasForeignKey(x => x.AdmissionId);
    }
}
