using FourTails.Core.DomainModels;
using FourTails.DataAccess;
using FourTails.DTOs.Payload.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FourTails.Services.Container;

public class TokenService
{
    private readonly FTDBContext _context;
    private readonly int TokenExpirationMinutes = 30;
    private readonly IConfiguration _configuration;

    public TokenService() {}

    public TokenService(IConfiguration configuration, FTDBContext context)
    {
        
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Dictionary<string, AuthResponseDTO>? CreateToken(AuthRequestDTO request)
    {
        var userInDb = _context.Users.FirstOrDefault(x => x.Email == request.Email);

        if (userInDb is null)
        {
            return null;
        }

        var expiration = DateTime.UtcNow.AddMinutes(TokenExpirationMinutes);
        var token = CreateJwtToken
        (
            CreateClaims(userInDb),
            CreateSigningCredentials(),
            expiration
        );

        _context.SaveChangesAsync();

        var tokenHandler = new JwtSecurityTokenHandler();
        var accessToken = tokenHandler.WriteToken(token);

        var response = new AuthResponseDTO
        (
            UserName: userInDb.UserName,
            Email: userInDb.Email,
            Token: accessToken
        );

        var userAndAccessToken = new Dictionary<string, AuthResponseDTO>() 
        { 
            { accessToken, response }
        };

        return userAndAccessToken;
    }

    private JwtSecurityToken CreateJwtToken
    (
        List<Claim> claims, 
        SigningCredentials credentials,
        DateTime expiration
    )
    {
        return new
        (
            "apiWithAuthBackend",
            "apiWithAuthBackend",
            claims,
            expires: expiration,
            signingCredentials: credentials
        );
    }

    private List<Claim> CreateClaims(User user)
    {
        try 
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, "TokenForTheApiWithAuth"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)

            };

            return claims;
        }
        catch (Exception e) 
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private SigningCredentials CreateSigningCredentials()
    {
        var x = new SigningCredentials
        (
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JwtOptions:0:IssuerSigningKey").Value)),
            SecurityAlgorithms.HmacSha256
        );
        return x; 
    }
}