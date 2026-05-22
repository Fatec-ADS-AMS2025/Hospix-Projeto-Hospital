using Hospital.Domain.Common;
using Hospital.Domain.ValueObjects;

namespace Hospital.Domain.Entities;

public class Patient
{
    private Patient()
    {
        FullName = "";
        Cpf = new Cpf("00000000000");
        InsuranceName = "";
    }

    public Patient(Guid id, string fullName, Cpf cpf, DateTime birthDate, string insuranceName)
    {
        if (id == Guid.Empty)
        {
            throw new DomainException("Paciente exige identificador.");
        }

        if (string.IsNullOrWhiteSpace(fullName) || fullName.Trim().Length < 3)
        {
            throw new DomainException("Nome do paciente deve ter ao menos 3 caracteres.");
        }

        if (birthDate.Date > DateTime.UtcNow.Date)
        {
            throw new DomainException("Nascimento nao pode estar no futuro.");
        }

        Id = id;
        FullName = fullName.Trim();
        Cpf = cpf;
        BirthDate = birthDate.Date;
        InsuranceName = string.IsNullOrWhiteSpace(insuranceName) ? "Particular" : insuranceName.Trim();
    }

    public Guid Id { get; private set; }
    public string FullName { get; private set; }
    public Cpf Cpf { get; private set; }
    public DateTime BirthDate { get; private set; }
    public string InsuranceName { get; private set; }
}
