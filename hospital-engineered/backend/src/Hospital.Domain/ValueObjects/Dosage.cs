using Hospital.Domain.Common;

namespace Hospital.Domain.ValueObjects;

public readonly record struct Dosage
{
    public string Value { get; }

    public Dosage(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException("Dose e obrigatoria.");
        }

        Value = value.Trim();
    }

    public override string ToString() => Value;
}
