using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Linq;
using Newtonsoft.Json;
using LitteraCore.DBContext;
using LitteraCore.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using Azure;

namespace LitteraCore.Common.Token
{
    
    public class AppAuthService : IAppAuthService
    {
        private readonly IConfiguration _configuration;
        public AppAuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<UserToken> Authenticate(string username)
        {
            // var user = await _userrepository.ValidateUserExitAsync(userlogin.Mobileno, userlogin.Password);

            //if (userlogin == null)
            //    throw new Exception("Invalid Input received!");

            //            user = await _context.Users.FindAsync(userlogin.Username);

            // User name and password are valid. 
            // Generate JSON Web Token

            AuthDB ADB = new AuthDB(_configuration);
            UserInfo U=ADB.GetUserInfo(username);


            //AuthDB ADB = new AuthDB(_configuration);
            //bool s = ADB.Make_Login_Entry(U.userid, "0", "IP");

            string claimname = null;
            if (U.Mobileno !=null)
            { claimname = U.Mobileno.ToString(); }
            else
            {
                claimname = U.emailid;
            }
            //var claims = await _parmissionservice.GetClaimsAsync((Guid)userlogin.userid);
            var claims = "";
            var tokenHandler = new JwtSecurityTokenHandler();
            // var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
            var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);

           

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //Issuer = "",
                //Audience ="",
                Subject = new ClaimsIdentity(new List<Claim>
                    {
                          new Claim(ClaimTypes.Name, claimname),
                          new Claim("userid", U.userid),
                          new Claim("agencyid", U.agencyid),
                          new Claim("username",U.Username),
                          new Claim("mobileno",U.Mobileno),
                          new Claim("emailid",U.emailid),
                            new Claim("usertype",JsonConvert.SerializeObject(U.usertype)),
                          new Claim("usertype_roles",JsonConvert.SerializeObject(U.userrole)),
                          new Claim("branchid",U.branchid),
                    }),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            return new UserToken { AuthToken = tokenHandler.WriteToken(token), userdetails= U };
        }

        //public static string GetLoginIPAddress(this HttpContext context)
        //{
        //    string sIPAddress = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();

        //    if (string.IsNullOrEmpty(sIPAddress))
        //    {
        //        sIPAddress = context.Connection.RemoteIpAddress?.ToString();
        //    }
        //    else
        //    {
        //        // X-Forwarded-For header may contain multiple IP addresses separated by ","
        //        // We'll take the first one, which is the client's IP address
        //        sIPAddress = sIPAddress.Split(',')[0].Trim();
        //    }

        //    return sIPAddress;
        //}

        public async Task<UserInfo> GetUserRole(string UserId)
        {
            UserInfo u = new UserInfo
            {
                userid = "user",
                agencyid="agency",
                Mobileno = "788747474",
                emailid = "test@gmail.com",
                password = "",
                Username = "test",
                branchid = "branch",
                usertype = new List<UserInfo_usertype>
                       {
                          new  UserInfo_usertype { usertypeid="1" },
                          new  UserInfo_usertype {  usertypeid="3" }
                       }.ToArray(),
                userrole = new List<UserInfo_usertype_roles>
                        {
                            new  UserInfo_usertype_roles { usertypeid="1", roleid="2052" },
                          new  UserInfo_usertype_roles { usertypeid="3", roleid="2053"}

                        }.ToArray(),
            };


            //if (roleName == null)
            //{
            //    throw new Exception("User Role Not Found ");
            //}

            return u;

        }


        public Boolean VerifyPassword(string DBpass, string salt, string password)
        {
            string decryptedpass = YEncryptDecryptData.YEncryptDecryptData.Decrypt(DBpass, true);
#pragma warning disable CS0618 // 'FormsAuthentication.HashPasswordForStoringInConfigFile(string, string)' is obsolete: 'The recommended alternative is to use the Membership APIs, such as Membership.CreateUser. For more information, see http://go.microsoft.com/fwlink/?LinkId=252463.'
            string hassedpassword = HashPasswordForStoringInConfigFile(decryptedpass.Trim(),salt);
#pragma warning restore CS0618 // 'FormsAuthentication.HashPasswordForStoringInConfigFile(string, string)' is obsolete: 'The recommended alternative is to use the Membership APIs, such as Membership.CreateUser. For more information, see http://go.microsoft.com/fwlink/?LinkId=252463.'
            if (hassedpassword.ToLower().Equals(password))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static string HashPasswordForStoringInConfigFile(string password,string salt)
        {
            using (var md5 = MD5.Create())
            {
                var combined = Encoding.UTF8.GetBytes(salt + password);
                var hashbytes= md5.ComputeHash(combined);
                var hash = BitConverter.ToString(hashbytes).Replace("-", "").ToLower();
                return hash;
          }
        }


    }
}
