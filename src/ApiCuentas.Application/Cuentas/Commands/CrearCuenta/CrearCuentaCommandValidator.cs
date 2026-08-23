using FluentValidation;

namespace ApiCuentas.Application.Cuentas.Commands.CrearCuenta;

public class CrearCuentaCommandValidator : AbstractValidator<CrearCuentaCommand>
{
    public CrearCuentaCommandValidator()
    {
        RuleFor(c => c.NumeroCuenta)
            .NotEmpty().WithMessage("El número de cuenta es obligatorio.")
            .MaximumLength(20).WithMessage("El número de cuenta no puede superar los 20 caracteres.");

        RuleFor(c => c.Titular)
            .NotEmpty().WithMessage("El titular es obligatorio.")
            .MaximumLength(150).WithMessage("El titular no puede superar los 150 caracteres.");

        RuleFor(c => c.TipoCuenta)
            .NotEmpty().WithMessage("El tipo de cuenta es obligatorio.");
    }
}
