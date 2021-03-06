﻿using MediatR;
using Microsoft.Extensions.Logging;
using MultiCreditCard.Users.Command.Commands;
using MultiCreditCard.Users.Command.Reponse;
using MultiCreditCard.Users.Command.Validators;
using MultiCreditCard.Users.Domain.Contracts.Repositories;
using MultiCreditCard.Users.Domain.Entities;
using MultiCreditCard.Users.Domain.ValueObjects;
using MultiCreditCard.Wallets.Domain.Contracts.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MultiCreditCard.Users.Command.Handlers
{
    public class RegisterNewUserHandler : IAsyncRequestHandler<RegisterNewUserCommand, RegisterNewUserReponse>
    {
        private User _newUser = User.DefaultEntity();
        private readonly ILogger _logger;
        private readonly IWalletService _walletService;
        private readonly IUserRepository _userRepository;
        private readonly RegisterNewUserValidator validator = new RegisterNewUserValidator();

        public RegisterNewUserHandler(IUserRepository userRepository, IWalletService walletService, ILoggerFactory loggerFactory)
        {
            _walletService = walletService;
            _userRepository = userRepository;
            _logger = loggerFactory.CreateLogger<RegisterNewUserHandler>();

        }

        public async Task<RegisterNewUserReponse> Handle(RegisterNewUserCommand message)
        {
            var response = message.Response;

            ValidateCommand(message, response);
            if (response.HasError)
                return response;

            RegisterNewUser(message, response);
            if (response.HasError)
                return response;

            GetResponse(message, response);
            if (response.HasError)
                return response;

            return response;
        }

        private void ValidateCommand(RegisterNewUserCommand command, RegisterNewUserReponse response)
        {
            var results = validator.Validate(command);
            var validationSucceeded = results.IsValid;

            if (!results.IsValid)
            {
                results.Errors.ToList().ForEach(x => response.AddError(x.ErrorMessage));
                return;
            }

            var user = _userRepository.GetUserByEmail(command.Email).Result;
            if (!string.IsNullOrEmpty(user.UserId))
                response.AddError($"Usuário ja criado para o email {user.Email}");
        }

        private async void RegisterNewUser(RegisterNewUserCommand command, RegisterNewUserReponse response)
        {
            try
            {
                _newUser = AdapterCommantToEntity(command, response);
                if (response.HasError)
                    return;

                await _userRepository.CreateAsync(_newUser);
                await _walletService.CreateWalletAsync(_newUser);

                _logger.LogInformation($"Usuário {_newUser.UserName}, email {_newUser.Email}, documento {_newUser.DocumentNumber} registrado com sucesso.");
            }
            catch (Exception ex)
            {
                var message = $"Problemas ao salvar o usuário {command.UserName} - {ex.Message}";

                _logger.LogError(ex, message);
                response.AddError(message);
            }
        }

        private static User AdapterCommantToEntity(RegisterNewUserCommand command, RegisterNewUserReponse response)
        {
            var newUser = User.DefaultEntity();

            try
            {
                var email = new Email(command.Email);
                var password = new Password(command.Password);

                newUser = new User(command.UserName, command.DocumentNumber, email.EletronicAddress, password.Encoded);
            }
            catch (Exception ex)
            {
                response.AddError($"Problemas a criar usuário. {ex.Message}");
            }

            return newUser;
        }

        private void GetResponse(RegisterNewUserCommand command, RegisterNewUserReponse response)
        {
            try
            {
                response.UserId = _newUser.UserId;
                response.UserName = _newUser.UserName;
                response.Email = _newUser.Email;
                response.DocumentNumber = _newUser.DocumentNumber;
            }
            catch (Exception ex)
            {
                response.AddError($"Problemas a criar usuário. {ex.Message}");
            }
        }
    }
}
