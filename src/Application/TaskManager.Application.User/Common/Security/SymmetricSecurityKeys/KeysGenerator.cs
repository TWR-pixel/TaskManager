﻿using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TaskManager.Application.User.Common.Security.SymmetricSecurityKeys;

public sealed class KeysGenerator : IKeysGenerator
{
    public SymmetricSecurityKey Create(string securityKey)
    {
        if (string.IsNullOrWhiteSpace(securityKey))
            throw new ArgumentException($"\"{nameof(securityKey)}\" не может быть пустым или содержать только пробел.", nameof(securityKey));

        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
    }
}