﻿using MultiCreditCard.Users.Domain.Contracts.Repositories;
using MultiCreditCard.Users.Domain.Contracts.Services;
using MultiCreditCard.Users.Domain.Entities;
using System.Threading.Tasks;

namespace MultiCreditCard.Users.Domain.Services
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepository _userRepository;

        public UserServices(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task CreateUserAsync(User user)
        {
            await _userRepository.CreateAsync(user);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _userRepository.GetUserByEmail(email);
        }

        public Task<User> GetUserByUserId(string email)
        {
            throw new System.NotImplementedException();
        }
    }
}
