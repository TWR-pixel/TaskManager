﻿using System.Security.Claims;

namespace TaskManager.Application.Common.Security.Authentication.JwtClaims;

public interface IJwtClaimsFactory
{
    public IEnumerable<Claim> CreateDefault(int userId, int roleId, string userName, string roleName);
}