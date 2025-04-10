﻿namespace Domain.Interfaces.Services;

using System.Threading.Tasks;

using Domain.Entities.User;

public interface ITokenService
{
    Task<string> GenerateTokenAsync(User user);
}