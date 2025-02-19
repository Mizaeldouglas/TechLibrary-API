using FluentValidation;
using TechLibrary.Communication.Requests;

namespace TechLibrary.Application.UseCases.Users.Register;

public class RegisterUserValidator : AbstractValidator<RequestUserJson>
{
    public RegisterUserValidator()
    {
        RuleFor(request => request.Name).NotEmpty().WithMessage("O nome não pode ser vazio.");
        RuleFor(request => request.Email).EmailAddress().WithMessage("O e-mail informado não é válido.");
        RuleFor(request => request.Password).NotEmpty().WithMessage("A senha não pode ser vazia.");
        When(request => string.IsNullOrEmpty(request.Password), () =>
        {
            RuleFor(request => request.Password.Length).GreaterThanOrEqualTo(6)
                .WithMessage("A senha deve ter no mínimo 6 caracteres.");
        });
    }
}