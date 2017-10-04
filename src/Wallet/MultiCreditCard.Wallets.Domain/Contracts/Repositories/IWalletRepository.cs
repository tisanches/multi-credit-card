﻿using MultiCreditCard.Users.Domain.Entities;
using MultiCreditCard.Wallets.Domain.Entities;
using System.Threading.Tasks;

namespace MultiCreditCard.Wallets.Domain.Contracts.Repositories
{
    public interface IWalletRepository
    {
        Task CreateWalletAsync(Wallet wallet);

        Task<Wallet> GetWalletByUserId(string userId);

        void AddNewCreditCart(Wallet wallet);

        void RemoveCreditCart(Wallet wallet);

        void UpdateUserCreditLimit(Wallet wallet);
    }
}
