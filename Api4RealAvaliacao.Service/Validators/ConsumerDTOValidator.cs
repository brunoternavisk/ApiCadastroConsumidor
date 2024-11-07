using Api4RealAvaliacao.Domain.DTOs;
using FluentValidation;

namespace Api4RealAvaliacao.Service.Validators;

public class ConsumerDTOValidator : AbstractValidator<ConsumerDTO>
{
    public ConsumerDTOValidator()
    {
        // Validação de Pessoa
        RuleFor(c => c.Pessoa.Nome)
            .NotEmpty().WithMessage("Por favor, entre com o nome da pessoa.")
            .MaximumLength(100).WithMessage("Nome não pode ter mais de 100 caracteres.");
        RuleFor(c => c.Pessoa.Cpf)
            .NotEmpty().WithMessage("Por favor, entre com o CPF.")
            .MaximumLength(200).WithMessage("CPF deve ter no maximo 200 dígitos.");
        RuleFor(c => c.Pessoa.FLAssociado)
            .NotEmpty().WithMessage("Fl_Associado não pode ser NULL.")
            .MaximumLength(1).WithMessage("Fl_Associado não pode ter mais de 1 caractere.");
        RuleFor(c => c.Pessoa.FLAutorizaDados)
            .NotEmpty().WithMessage("Fl_Autoriza_Dados não pode ser NULL.")
            .MaximumLength(2).WithMessage("Fl_Autoriza_Dados não pode ter mais de 2 caracteres.");

        // Validação de Telefone
        RuleFor(c => c.Telefone.DDD)
            .NotEmpty().WithMessage("DDD é obrigatório.")
            .Length(2).WithMessage("DDD deve ter 2 dígitos.");
        RuleFor(c => c.Telefone.Numero)
            .NotEmpty().WithMessage("Número de telefone é obrigatório.")
            .MaximumLength(10).WithMessage("Número de telefone deve ter no máximo 10 dígitos.");

        // Validação de Estabelecimento
        RuleFor(c => c.Estabelecimento.Nome)
            .NotEmpty().WithMessage("Nome do estabelecimento é obrigatório.")
            .MaximumLength(200).WithMessage("Nome não pode ter mais de 200 caracteres.");
        RuleFor(c => c.Estabelecimento.NomeFantasia)
            .NotEmpty().WithMessage("NomeFantasia do estabelecimento é obrigatório.")
            .MaximumLength(200).WithMessage("NomeFantasia não pode ter mais de 200 caracteres.");
        RuleFor(c => c.Estabelecimento.CnpjGrupo)
            .NotEmpty().WithMessage("CNPJ do grupo é obrigatório.")
            .MaximumLength(18).WithMessage("CNPJ do grupo deve ter no máximo 18 dígitos.");
        RuleFor(c => c.Estabelecimento.CnpjFranquia)
            .NotEmpty().WithMessage("CNPJ da franquia é obrigatório.")
            .MaximumLength(18).WithMessage("CNPJ da franquia deve ter no máximo 18 dígitos.");
        RuleFor(c => c.Estabelecimento.Status)
            .NotNull().WithMessage("Status é obrigatório.")
            .InclusiveBetween(0, 9999).WithMessage("Status deve ter no máximo 4 dígitos.");

        // Validação de RedeOptin
        RuleFor(c => c.RedeOptin.Optin)
            .NotNull().WithMessage("Optin é obrigatório. 1 para Optin e 2 para Optout")
            .Must(optin => optin == 1 || optin == 2).WithMessage("Optin deve ser 1 (Optin) ou 2 (Optout).");
    }   
}