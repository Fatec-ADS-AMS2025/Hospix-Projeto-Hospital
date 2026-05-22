using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BillingDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("Billing") ?? "Data Source=hospital-billing.db");
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BillingDbContext>();
    await db.Database.EnsureCreatedAsync();
}

app.MapPost("/api/billing/admission-discharged", async (AdmissionDischargedEvent request, BillingDbContext db, CancellationToken cancellationToken) =>
{
    var existing = await db.Invoices.FirstOrDefaultAsync(x => x.AdmissionId == request.AdmissionId, cancellationToken);
    if (existing is not null)
    {
        return Results.Ok(ToDto(existing));
    }

    var hours = Math.Max(1, (int)Math.Ceiling((request.DischargedAt - request.AdmittedAt).TotalHours));
    var amount = 350m + (hours * 45m) + (request.ExamCount * 180m) + (request.PrescriptionCount * 90m);

    var invoice = new BillingInvoice
    {
        Id = Guid.NewGuid(),
        AdmissionId = request.AdmissionId,
        Amount = amount,
        Status = "GERADA",
        CreatedAt = DateTime.UtcNow
    };

    db.Invoices.Add(invoice);
    await db.SaveChangesAsync(cancellationToken);
    return Results.Ok(ToDto(invoice));
});

app.MapGet("/api/invoices", async (BillingDbContext db, CancellationToken cancellationToken) =>
{
    var invoices = await db.Invoices.OrderByDescending(x => x.CreatedAt).ToListAsync(cancellationToken);
    return invoices.Select(ToDto);
});

app.Run();

static InvoiceDto ToDto(BillingInvoice invoice)
{
    return new InvoiceDto(invoice.Id, invoice.AdmissionId, invoice.Amount, invoice.Status, invoice.CreatedAt);
}

public class BillingDbContext : DbContext
{
    public BillingDbContext(DbContextOptions<BillingDbContext> options) : base(options)
    {
    }

    public DbSet<BillingInvoice> Invoices => Set<BillingInvoice>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BillingInvoice>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => x.AdmissionId).IsUnique();
            entity.Property(x => x.Status).HasMaxLength(32).IsRequired();
            entity.Property(x => x.Amount).HasPrecision(12, 2);
        });
    }
}

public class BillingInvoice
{
    public Guid Id { get; set; }
    public Guid AdmissionId { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; } = "";
    public DateTime CreatedAt { get; set; }
}

public record AdmissionDischargedEvent(Guid AdmissionId, string PatientName, DateTime AdmittedAt, DateTime DischargedAt, int ExamCount, int PrescriptionCount);
public record InvoiceDto(Guid Id, Guid AdmissionId, decimal Amount, string Status, DateTime CreatedAt);
