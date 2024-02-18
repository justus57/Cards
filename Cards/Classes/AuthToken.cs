using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace Cards.Classes
{
  
        public class JWTAuthService
        {
            public static readonly string USER = "";
            public static readonly string PASSWORD = "";
            private const int TOKEN_EXPIRY_DURATION = 60; // TIME TAKEN FOR TOKEN TO EXPIRE: MINUTES
            private const string SECRET_KEY = "cards";

            /// <summary>
            /// CREATES A TOKEN WITH THE GIVEN SECRET KEY, THE TOKEN EXPIRES AFTER 60_MINUTES
            /// </summary>
            /// <param name="secretKey"></param>
            /// <returns>token</returns>
            public string CreateToken(string user)
            {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user),
           // new Claim(ClaimTypes.Role, role)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET_KEY));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: "Logicea", // Replace with your issuer
                    audience: "Logicea", // Replace with your audience
                    claims: claims,
                    expires: DateTime.Now.AddHours(TOKEN_EXPIRY_DURATION), // Set token expiration time
                    signingCredentials: credentials
                );

                var tokenHandler = new JwtSecurityTokenHandler();
                return tokenHandler.WriteToken(token);
            }


            /// <summary>
            /// VALIDATES THE GIVEN TOKEN, CHECKS EXPIRY
            /// </summary>
            /// <param name="token"></param>
            /// <returns>TRUE | FALSE</returns>


            public bool ValidateToken(string token)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET_KEY));

                try
                {
                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "Logicea", // Replace with your issuer
                        ValidAudience = "Logicea", // Replace with your audience
                        IssuerSigningKey = key
                    }, out SecurityToken validatedToken);

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
    

    }
        
        
    
}