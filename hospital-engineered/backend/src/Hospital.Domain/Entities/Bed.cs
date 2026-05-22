using Hospital.Domain.Common;
using Hospital.Domain.Enums;

namespace Hospital.Domain.Entities;

public class Bed
{
    private Bed()
    {
        Code = "";
        Ward = "";
    }

    public Bed(Guid id, string code, string ward, BedStatus status = BedStatus.Available)
    {
        if (id == Guid.Empty)
        {
            throw new DomainException("Leito exige identificador.");
        }

        if (string.IsNullOrWhiteSpace(code))
        {
            throw new DomainException("Codigo do leito e obrigatorio.");
        }

        Id = id;
        Code = code.Trim();
        Ward = string.IsNullOrWhiteSpace(ward) ? "Geral" : ward.Trim();
        Status = status;
    }

    public Guid Id { get; private set; }
    public string Code { get; private set; }
    public string Ward { get; private set; }
    public BedStatus Status { get; private set; }

    public void Occupy()
    {
        if (Status != BedStatus.Available)
        {
            throw new DomainException("Leito indisponivel para internacao.");
        }

        Status = BedStatus.Occupied;
    }

    public void Release()
    {
        if (Status == BedStatus.Occupied)
        {
            Status = BedStatus.Available;
        }
    }
}
