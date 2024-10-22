﻿using System.IdentityModel.Tokens.Jwt;
using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Security.Auth.Claims;
using TaskManager.Application.Common.Security.Auth.Tokens;
using TaskManager.Application.Modules.Email.Verifier;
using TaskManager.Core.Entities.Common.Exceptions;
using TaskManager.Core.Entities.Users.Exceptions;
using TaskManager.Core.UseCases.Common.UnitOfWorks;
using TaskManager.Core.UseCases.Users.Specifications;

namespace TaskManager.Application.Users.Requests.VerifyEmail;

public sealed record VerifyEmailRequest(string Code) : RequestBase<VerifyEmailResponse>;
public sealed record VerifyEmailResponse(string AccessToken, string Username, int UserId, string RoleName, int RoleId) : ResponseBase;

public sealed class VerifyEmailRequestHandler
    : RequestHandlerBase<VerifyEmailRequest, VerifyEmailResponse>
{
    private readonly IJwtSecurityTokenFactory _tokenFactory;
    private readonly IClaimsFactory _claimsFactory;
    private readonly ICodeVerifier _verifier;

    public VerifyEmailRequestHandler(IUnitOfWork unitOfWork,
                                     IJwtSecurityTokenFactory tokenFactory,
                                     IClaimsFactory claimsFactory,
                                     ICodeVerifier verifier) : base(unitOfWork)
    {
        _tokenFactory = tokenFactory;
        _claimsFactory = claimsFactory;
        _verifier = verifier;
    }

    public override async Task<VerifyEmailResponse> Handle(VerifyEmailRequest request, CancellationToken cancellationToken)
    {
        var isCodeRight = _verifier.Verify(request.Code, out string email);

        if (!isCodeRight)
            throw new CodeNotVerifiedException();

        var verifiedUser = await UnitOfWork.Users.SingleOrDefaultAsync(new ReadUserByEmailSpec(email), cancellationToken)
            ?? throw new UserNotFoundException(email);

        var claims = _claimsFactory.CreateDefault(verifiedUser.Id, verifiedUser.Role.Id, verifiedUser.Username, verifiedUser.Role.Name);

        var token = new JwtSecurityTokenHandler()
            .WriteToken(_tokenFactory.CreateSecurityToken(claims));

        return new VerifyEmailResponse(token, verifiedUser.Username, verifiedUser.Id, verifiedUser.Role.Name, verifiedUser.Role.Id);
    }
}
