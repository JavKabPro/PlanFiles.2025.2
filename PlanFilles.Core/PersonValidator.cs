using FluentValidation;

namespace PlainFiles.Core;

public class PersonValidator : AbstractValidator<Person>
{
    public PersonValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .Must(HaveFirstAndLastName)
            .WithMessage("Debe ingresar nombre y apellido.");

        RuleFor(p => p.Phone)
            .NotEmpty()
            .Matches(@"^[0-9+\-\s]{7,}$")
            .WithMessage("El teléfono no es válido.");

        RuleFor(p => p.City)
            .NotEmpty();

        RuleFor(p => p.Balance)
            .GreaterThan(0)
            .WithMessage("El balance debe ser positivo.");
    }

    private bool HaveFirstAndLastName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return false;
        var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return parts.Length >= 2;
    }
}

