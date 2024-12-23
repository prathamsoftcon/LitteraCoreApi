using LitteraCore.Common.Token;
using LitteraCore.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using static System.Net.WebRequestMethods;

namespace LitteraCore.Common.OTP
{
    public class OtpManager
    {
        private readonly IDistributedCache _cache;

        public OtpManager(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<string> GenerateOtpAsync(string mobile)
        {
            // Generate a 6-digit random OTP
            var otp = new Random().Next(100000, 999999).ToString();
            // or Call and API from Telcome provider

            // Store the OTP in the cache with a timestamp
            var otpTimestamp = DateTime.UtcNow;
            await _cache.SetStringAsync($"otp:{mobile}", otp);
            await _cache.SetStringAsync($"otpTimestamp:{mobile}", otpTimestamp.ToString());

            return otp;
        }

        public async Task<string> GenerateOtpID()
        {
            // Generate a 6-digit random OTP
            var otpID = new Random().Next(1000, 9999).ToString();
            // or Call and API from Telcome provider

            return otpID;
        }

        public async Task<bool> VerifyOtpAsync(string mobile, string otp)
        {
            // Get the OTP and timestamp from the cache
            var cachedOtp = await _cache.GetStringAsync($"otp:{mobile}");
            var cachedOtpTimestamp = await _cache.GetStringAsync($"otpTimestamp:{mobile}");

            if (cachedOtp != otp)
            {
                return false;
            }

            // Check if the OTP has expired
            var otpTimestamp = DateTime.Parse(cachedOtpTimestamp);
            if (DateTime.UtcNow - otpTimestamp > TimeSpan.FromMinutes(15))
            {
                return false;
            }

            // Delete the OTP from the cache
            await _cache.RemoveAsync($"otp:{mobile}");
            await _cache.RemoveAsync($"otpTimestamp:{mobile}");

            return true;
        }

        public async Task<string> SetOauthToken(string username,string token)
        {
          
            var otpTimestamp = DateTime.UtcNow;
            await _cache.SetStringAsync($"OauthToken:{username}", token.ToString());
            await _cache.SetStringAsync($"OauthTokenTimestamp:{username}", otpTimestamp.ToString());

            return token.ToString();
        }

        public async Task<bool> CheckOauthToken(string username)
        {

            var cachedOtp = await _cache.GetStringAsync($"OauthToken:{username}");
            var cachedOtpTimestamp = await _cache.GetStringAsync($"OauthTokenTimestamp:{username}");
            if (cachedOtp ==null)
            {
                return false;
            }
            var otpTimestamp = DateTime.Parse(cachedOtpTimestamp);
            if (DateTime.UtcNow - otpTimestamp > TimeSpan.FromMinutes(15))
            {
                return false;
            }

            await _cache.RemoveAsync($"OauthToken:{username}");
            await _cache.RemoveAsync($"OauthTokenTimestamp:{username}");

            return true;
        }


        public async Task<string> Set_Print_Data(string id, PrintData pData)
        {
            // Generate a 6-digit random OTP
          
            await _cache.SetStringAsync($"PrintData:{id}", pData.printdata);
            
            return id;
        }


        public async Task<string> Get_Print_Data(string id)
        {
            // Get the OTP and timestamp from the cache
            var cachedOtp = await _cache.GetStringAsync($"PrintData:{id}");
           

            
            await _cache.RemoveAsync($"PrintData:{id}");
        

            return cachedOtp;
        }

    }
}
