using System.IdentityModel.Tokens.Jwt;
using TaskManager.Application.Common.Email.Verifier;
using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Security.Auth.Jwt.Claims;
using TaskManager.Application.Common.Security.Auth.Jwt.Tokens;
using TaskManager.Core.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.Users.Requests.VerifyEmail;

public sealed record VerifyEmailRequest(string Code) : RequestBase<VerifyEmailResponse>;
public sealed record VerifyEmailResponse(string AccessToken, string Username, int UserId, string RoleName, int RoleId) : ResponseBase;

public sealed class VerifyEmailRequestHandler
    : RequestHandlerBase<VerifyEmailRequest, VerifyEmailResponse>
{
    private readonly IJwtSecurityTokenFactory _tokenFactory;
    private readonly IJwtClaimsFactory _claimsFactory;
    private readonly IEmailVerifier _verifier;

    public VerifyEmailRequestHandler(IUnitOfWork unitOfWork,
                                     IJwtSecurityTokenFactory tokenFactory,
                                     IJwtClaimsFactory claimsFactory,
                                     IEmailVerifier verifier) : base(unitOfWork)
    {
        _tokenFactory = tokenFactory;
        _claimsFactory = claimsFactory;
        _verifier = verifier;
    }

    public override async Task<VerifyEmailResponse> Handle(VerifyEmailRequest request, CancellationToken cancellationToken)
    {
        var verifiedUser = await _verifier.Verify(request.Code, cancellationToken);

        var claims = _claimsFactory.CreateDefault(verifiedUser.Id, verifiedUser.Role.Id, verifiedUser.Username, verifiedUser.Role.Name);

        var token = new JwtSecurityTokenHandler()
            .WriteToken(_tokenFactory.CreateSecurityToken(claims));

        return new VerifyEmailResponse(token, verifiedUser.Username, verifiedUser.Id, verifiedUser.Role.Name, verifiedUser.Role.Id);
    }
}
