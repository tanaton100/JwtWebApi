using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using WebApiJwt.Model;
using WebApiJwt.Model.InputModel;
using System.Linq;
using System.Security.Claims;

namespace WebApiJwt.Services
{
    public interface IUserServices
    {
        string Login(UserLoginModel loginModel);
        IEnumerable<Users> GetAllUsers();
    }
    public class UserServices : IUserServices
    {
        private IConfiguration _config;

        public UserServices(IConfiguration config)
        {
            _config = config;
        }

        public IEnumerable<Users> GetAllUsers()
        {
            return new List<Users>
            {
                new Users{
                    Id =1,
                    UserName ="Admin",
                    Password = "Admin",
                    Email="admin@gmail.com"
                },
                new Users{
                    Id =2,
                    UserName ="SaDev",
                    Password = "dotnet",
                    Email="sadev@gmail.com"
                },
                 new Users{
                    Id =3,
                    UserName ="BaDev",
                    Password = "Java",
                    Email="badev@gmail.com"
                },
            };
        }

        public string Login(UserLoginModel loginModel)
        {
            var user = GetAllUsers()
                .FirstOrDefault(u => u.UserName.Equals(loginModel.UserName) && u.Password.Equals(loginModel.Password));

            if (user is null)
            {
                return "";
            }
            return GenerateJSONWebToken(user);
        }

        private string GenerateJSONWebToken(Users userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(Convert.ToInt32(_config["Jwt:Expires"]));
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.UserName),
                new Claim(JwtRegisteredClaimNames.Email, userInfo.Email),
                new Claim("Georgeo", "JwtToken"),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),

            };


            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: expires,
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
