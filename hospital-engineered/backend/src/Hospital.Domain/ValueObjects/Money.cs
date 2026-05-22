using Hospital.Domain.Common;

namespace Hospital.Domain.ValueObjects;

public readonly record struct Money
{
    public decimal Amount { get; }

    public Money(decimal amount)
    {
        if (amount < 0)
        {
            throw new DomainException("Valor monetario nao pode ser negativo.");
        }

        Amount = amount;
    }

    public override string ToString() => Amount.ToString("0.00");
}
