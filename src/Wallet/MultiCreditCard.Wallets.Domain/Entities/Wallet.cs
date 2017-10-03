﻿using MultiCreditCard.CreditCards.Domain.Entities;
using MultiCreditCard.Users.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MultiCreditCard.Wallets.Domain.Entities
{
    public class Wallet
    {
        private IList<CreditCard> _creditCards;

        public Wallet(User user)
        {
            User = user;
            _creditCards = new List<CreditCard>();
        }

        public decimal AvailableCredit { get; set; }

        public decimal MaximumCreditLimit
        {
            get
            {
                return _creditCards.Sum(x => x.CreditLimit);
            }
        }

        public decimal UserCreditLimit { get; private set; }

        public User User { get; private set; }

        public ICollection<CreditCard> CreditCards
        {
            get { return _creditCards; }
            private set { _creditCards = new List<CreditCard>(value); }
        }

        public void AddNewCreditCart(CreditCard newCreditCard)
        {
            if (_creditCards.Where(x => x.CreditCardNumber == newCreditCard.CreditCardNumber).Any())
                throw new InvalidOperationException($"Cartão de Crédito {newCreditCard.CreditCardNumber} já adicionado a carteira.");

            _creditCards.Add(newCreditCard);
        }

        public void RemoveCreditCard(CreditCard creditCard)
        {
            if (!_creditCards.Where(x => x.CreditCardNumber == creditCard.CreditCardNumber).Any())
                throw new InvalidOperationException($"Cartão de Crédito {creditCard.CreditCardNumber} já removido da carteira.");

            _creditCards.Remove(creditCard);
        }

        public void UpdateUserCreditLimit(decimal newUserCreditLimit)
        {
            if (newUserCreditLimit > MaximumCreditLimit)
                throw new InvalidOperationException($"Cartão de Crédito {newUserCreditLimit} já removido da carteira.");

            UserCreditLimit = newUserCreditLimit;
        }

        public void Buy(decimal valueBuy)
        {
            var purchaseDate = DateTime.Now.Day;

            if (valueBuy > MaximumCreditLimit)
                throw new InvalidOperationException($"Cartão de Crédito {valueBuy} já removido da carteira.");

            var credit = CreditCards
                            .Where(cc => cc.PayDay > purchaseDate)
                            .FirstOrDefault();
        }
    }
}