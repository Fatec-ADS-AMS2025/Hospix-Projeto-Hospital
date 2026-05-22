using Hospital.Domain.Common;

namespace Hospital.Domain.ValueObjects;

public readonly record struct Cpf
{
    public string Value { get; }

    public Cpf(string value)
    {
        var normalized = new string((value ?? "").Where(char.IsDigit).ToArray());
        if (normalized.Length != 11)
        {
            throw new DomainException("CPF deve conter 11 digitos.");
        }

        Value = normalized;
    }

    public override string ToString() => Value;
}
