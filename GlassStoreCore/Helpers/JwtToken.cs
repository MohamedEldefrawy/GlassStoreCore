using GlassStoreCore.BL.DTOs.UsersDtos;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace GlassStoreCore.Helpers
{
    public static class JwtToken
    {
        private const string secretKey = "BBAIDf4CZEmxZ2TIGdDJ7w==";
        public static readonly SymmetricSecurityKey SigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        public static string GenerateJwtToken(LoggedInUserDto userDto)
        {
            var credentials = new SigningCredentials(SigningKey, SecurityAlgorithms.HmacSha256);

            var header = new JwtHeader(credentials);

            DateTime Expiery = DateTime.UtcNow.AddMinutes(120);
            int ts = (int)(Expiery - new DateTime(1970, 1, 1)).TotalSeconds;

            var payLoad = new JwtPayload()
            {
                {"sub",userDto.UserID },
                {"Name",userDto.UserName },
                {"exp",ts },
                {"iss","https://localhost:44302/" }, // issuer the party generated the jwt
                {"aud","https://localhost:44302/" } // Audiance the address of the resource
            };

            var secToken = new JwtSecurityToken(payload: payLoad, header: header);
            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(secToken);
        }
    }
}
