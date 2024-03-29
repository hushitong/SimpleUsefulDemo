﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWT.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        readonly IConfiguration configuration;

        public AuthController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        public IActionResult Authenticate(string userName, string pwd)
        {
            //To Do:实际做登陆验证，这里就写死了
            if (userName == "admin" && pwd == "123456")
            {
                var jwtConfig = configuration.GetSection("Jwt");

                //定义签名使用的密钥，以及使用Hmacsha256签名算法
                var securityKey = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.GetValue<string>("Secret"))), SecurityAlgorithms.HmacSha256);

                //有效载荷
                var claims = new Claim[] {
                    new Claim(JwtRegisteredClaimNames.Iss,jwtConfig.GetValue<string>("Iss")),
                    new Claim(JwtRegisteredClaimNames.Aud,jwtConfig.GetValue<string>("Aud")),
                    new Claim(ClaimTypes.Name,"admin"),
                    new Claim(ClaimTypes.NameIdentifier,"1"),
                    new Claim(ClaimTypes.Role,"system"),
                    new Claim(ClaimTypes.Role,"admin")
                };

                SecurityToken securityToken = new JwtSecurityToken(
                    signingCredentials: securityKey,
                    expires: DateTime.Now.AddSeconds(jwtConfig.GetValue<int>("ExpireSeconds")), //过期时间
                    claims: claims
                );
                //生成jwt令牌
                return Content(new JwtSecurityTokenHandler().WriteToken(securityToken));
            }
            else
            {
                return BadRequest("登陆失败");
            }
        }
    }
}
