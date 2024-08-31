using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace EXE202_Prep4IELTS.Extensions;

public static class JwtBearerAuthExtension
{
    public static void AddClerkConfigure(this IServiceCollection services)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = "https://primary-jackal-39.clerk.accounts.dev";

                // Configure JWT Bearer token validation
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = "https://primary-jackal-39.clerk.accounts.dev",
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKeyResolver = (token, securityToken, kid, parameters) =>
                    {
                        // Dynamically retrieve Clerk's JWKS and validate the token
                        var client = new HttpClient();
                        var jwksUri = "https://primary-jackal-39.clerk.accounts.dev/.well-known/jwks.json";
                        var jwks = client.GetStringAsync(jwksUri).Result;
                        var keys = JsonDocument.Parse(jwks).RootElement.GetProperty("keys");

                        var matchingKey = keys.EnumerateArray()
                            .FirstOrDefault(key => key.GetProperty("kid").GetString() == kid);
                        if (matchingKey.ValueKind == JsonValueKind.Object)
                        {
                            var rsa = new RSACryptoServiceProvider();
                            rsa.ImportParameters(new RSAParameters
                            {
                                Modulus = Base64UrlEncoder.DecodeBytes(matchingKey.GetProperty("n").GetString()),
                                Exponent = Base64UrlEncoder.DecodeBytes(matchingKey.GetProperty("e").GetString())
                            });
                            return new[] { new RsaSecurityKey(rsa) };
                        }

                        return [];
                    }
                };
            });
    }
}