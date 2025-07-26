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
using static Org.BouncyCastle.Math.EC.ECCurve;

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
            AgencyDB agdb=new AgencyDB(_configuration);
            Agency a = agdb.Get_Agency_Data_For_Login(null, U.agencyid, 1, 1, null).FirstOrDefault();
            

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
                          new Claim("salutation",a.ag_salutation),
                          new Claim("f_name",a.ag_first_name),
                          new Claim("m_name",a.ag_m_name),
                          new Claim("l_name",a.ag_l_name)
                    }),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
          
            return new UserToken { AuthToken = tokenHandler.WriteToken(token), userdetails= U, agencydetail= a };
        }

     

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



        public ClaimsPrincipal ValidateJwtToken(string token)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"])); // Wrap byte[] in SymmetricSecurityKey
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                // Validate the token and extract claims
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true, // Ensure the token is not expired
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey, // Use SymmetricSecurityKey here
                                                    // ValidIssuer = "your_issuer",
                                                    // ValidAudience = "your_audience"
                }, out SecurityToken validatedToken);

                return principal;  // Returns the validated claims
            }
            catch (Exception ex)
            {
                // Handle invalid token (e.g., expired or tampered)
                throw new SecurityTokenException("Invalid token", ex);
            }
        }


        public async Task<UserToken> Activity_Token(string userid,string ttpai_id,string ttsam_id,string baseUrl)
        {
            // var user = await _userrepository.ValidateUserExitAsync(userlogin.Mobileno, userlogin.Password);

            //if (userlogin == null)
            //    throw new Exception("Invalid Input received!");

            //            user = await _context.Users.FindAsync(userlogin.Username);

            // User name and password are valid. 
            // Generate JSON Web Token

            AuthDB ADB = new AuthDB(_configuration);
        


            //AuthDB ADB = new AuthDB(_configuration);
            //bool s = ADB.Make_Login_Entry(U.userid, "0", "IP");

            string claimname = null;
            claimname = userid;
            //var claims = await _parmissionservice.GetClaimsAsync((Guid)userlogin.userid);
            var claims = "";
            var tokenHandler = new JwtSecurityTokenHandler();
            // var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
            var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
            var validapiKey = _configuration.GetSection("ApiKey").Value;



            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //Issuer = "",
                //Audience ="",
                Subject = new ClaimsIdentity(new List<Claim>
                    {
                          new Claim(ClaimTypes.Name, claimname),
                          new Claim("userid",userid),
                          new Claim("ttpai_id", ttpai_id),
                          new Claim("ttsam_id",ttsam_id),
                          new Claim("baseUrl",baseUrl),
                          new Claim("Key",validapiKey.ToString())

                    }),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new UserToken { AuthToken = tokenHandler.WriteToken(token)};
        }


    }
}
