﻿using TaskManager.Application.Users.Requests.Common.Security.Hashers;
using Hash = BCrypt.Net;

namespace TaskManager.Application.Users.Requests.Common.Security.Hashers.BCrypt;

public sealed class BCryptPasswordHasher : IPasswordHasher
{
    public string HashPassword(string password, string salt)
    {
        var result = Hash.BCrypt.HashPassword(password, salt);

        return result;
    }

    public string GenerateSalt(int workFactor = 11)
    {
        return Hash.BCrypt.GenerateSalt(workFactor);
    }

    public bool Verify(string text, string hash)
    {
        return Hash.BCrypt.Verify(text, hash);
    }
}
