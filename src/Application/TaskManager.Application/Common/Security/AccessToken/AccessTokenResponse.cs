﻿using TaskManager.Application.Common.Requests;

namespace TaskManager.Application.Common.Security.AccessToken;

public sealed record AccessTokenResponse(string AccessTokenString, int UserId, string Username, int RoleId, string RoleName) : ResponseBase;
