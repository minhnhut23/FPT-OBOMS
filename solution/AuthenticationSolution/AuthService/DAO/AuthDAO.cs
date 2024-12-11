using BusinessObject.DTO;
using Microsoft.AspNetCore.Mvc;
using Supabase;
using Supabase.Gotrue;
using System.IdentityModel.Tokens.Jwt;

namespace AuthService.DAO;

public class AuthDAO
{
    private readonly Supabase.Client _client;

    public AuthDAO(Supabase.Client client)
    {
        _client = client;
    }

    public async Task<LoginResponseDTO> Login(string email, string password)
    {
        var session = await _client.Auth.SignIn(email, password);

        if (session == null)
        {
            throw new Exception("Login Error");
        }

        var decodeToken = ConvertJwtStringToJwtSecurityToken(session.AccessToken);

        Console.WriteLine(DecodeJwt(decodeToken));

        return new LoginResponseDTO
        {
            AccessToken = session!.AccessToken!           
        };

    }
    public static JwtSecurityToken ConvertJwtStringToJwtSecurityToken(string? jwt)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwt);

        return token;
    }

    public static DecodedToken DecodeJwt(JwtSecurityToken token)
    {
        var keyId = token.Header.Kid;
        var audience = token.Audiences.ToList();
        var claims = token.Claims.Select(claim => (claim.Type, claim.Value)).ToList();
        return new DecodedToken(
            keyId,
            token.Issuer,
            audience,
            claims,
            token.ValidTo,
            token.SignatureAlgorithm,
            token.RawData,
            token.Subject,
            token.ValidFrom,
            token.EncodedHeader,
            token.EncodedPayload
        );
    }
}

public class DecodedToken
{
    public string KeyId { get; set; }
    public string Issuer { get; set; }
    public List<string> Audience { get; set; }
    public List<Tuple<string, string>> Claims { get; set; }
    public DateTime ValidTo { get; set; }
    public string SignatureAlgorithm { get; set; }
    public string RawData { get; set; }
    public string Subject { get; set; }
    public DateTime ValidFrom { get; set; }
    public string EncodedHeader { get; set; }
    public string EncodedPayload { get; set; }

    public DecodedToken(string keyId, string issuer, List<string> audience,
                        List<Tuple<string, string>> claims, DateTime validTo,
                        string signatureAlgorithm, string rawData, string subject,
                        DateTime validFrom, string encodedHeader, string encodedPayload)
    {
        KeyId = keyId;
        Issuer = issuer;
        Audience = audience;
        Claims = claims;
        ValidTo = validTo;
        SignatureAlgorithm = signatureAlgorithm;
        RawData = rawData;
        Subject = subject;
        ValidFrom = validFrom;
        EncodedHeader = encodedHeader;
        EncodedPayload = encodedPayload;
    }

    public DecodedToken(string keyId, string issuer, List<string> audience, List<(string Type, string Value)> claims, DateTime validTo, string signatureAlgorithm, string rawData, string subject, DateTime validFrom, string encodedHeader, string encodedPayload)
    {
        KeyId = keyId;
        Issuer = issuer;
        Audience = audience;
        claims = claims;
        ValidTo = validTo;
        SignatureAlgorithm = signatureAlgorithm;
        RawData = rawData;
        Subject = subject;
        ValidFrom = validFrom;
        EncodedHeader = encodedHeader;
        EncodedPayload = encodedPayload;
    }
}
