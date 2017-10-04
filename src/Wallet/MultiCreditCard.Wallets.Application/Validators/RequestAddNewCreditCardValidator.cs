﻿using FluentValidation;
using MultiCreditCard.Users.Application.Commands;

namespace MultiCreditCard.Wallets.Application.Validators
{
    public class RequestAddNewCreditCardValidator : AbstractValidator<RequestAddNewCreditCardCommand>
    {
        public RequestAddNewCreditCardValidator()
        {
            RuleFor(card => card.CreditCardNumber)
                .NotEmpty()
                .WithMessage("Número do cartão de credito deve ser preenchido.");

            RuleFor(card => card.PrintedName)
                .NotEmpty()
                .WithMessage("Nome impresso no cartão deve ser preenchido.");

            RuleFor(card => card.PayDay)
                .GreaterThan(0)
                .WithMessage("Dia do pagamento da fatura deve não deve ser zero.");

            RuleFor(card => card.ExpirationDate)
                .NotEmpty()
                .WithMessage("Dia do pagamento da fatura deve não deve ser zero.");

            RuleFor(card => card.CreditLimit)
                .GreaterThan(0)
                .WithMessage("Limite de crédito do cartão não deve ser zero.");

            RuleFor(card => card.CVV)
                .NotEmpty()
                .WithMessage("Código de segurança deve ser preenchido.");
        }
    }
}