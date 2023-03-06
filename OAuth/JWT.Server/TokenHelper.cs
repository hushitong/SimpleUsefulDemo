using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OAuth.Server
{
    public class TokenHelper
    {
        public static string GetToken(string tokenType, IConfigurationSection section = null)
        {
            string access_token = string.Empty;
            if (tokenType == "Jwt" || tokenType == "Bearer")
            {
                access_token = GetJwtToken(section);
            }
            else
            {
                //To Do: 动态生成Token
                access_token = "access_token121212121212";
            }
            var respObject = new
            {
                access_token = access_token,
                token_type = tokenType
            };
            return JsonConvert.SerializeObject(respObject);
        }

        public static string GetJwtToken(IConfigurationSection section)
        {
            //定义签名使用的密钥，以及使用Hmacsha256签名算法
            var securityKey = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(section.GetValue<string>("Secret"))), SecurityAlgorithms.HmacSha256);

            //有效载荷
            //To Do: 动态获得内容
            var claims = new Claim[] {
                    new Claim(JwtRegisteredClaimNames.Iss,section.GetValue<string>("Iss")),
                    new Claim(JwtRegisteredClaimNames.Aud,section.GetValue<string>("Aud")),
                    new Claim(ClaimTypes.Name,"singo"),
                    new Claim(ClaimTypes.NameIdentifier,"4433"),
                    new Claim(ClaimTypes.Role,"system"),
                    new Claim(ClaimTypes.Role,"admin")
                };

            SecurityToken securityToken = new JwtSecurityToken(
                signingCredentials: securityKey,
                expires: DateTime.Now.AddSeconds(section.GetValue<int>("ExpireSeconds")), //过期时间
                claims: claims
            );
            //生成jwt令牌
            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }
    }
}
