using ApiPractice.Enums;
using ApiPractice.Models;
using ApiPractice.ViewModels;
using JWT;
using JWT.Algorithms;
using JWT.Exceptions;
using JWT.Serializers;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace ApiPractice.Shared
{
    public static class JWTShared
    {
        //private const string issuer = "TestAPI";
        private const string Secret ="db3OIsj+BXE9NZDy0t8W3TcNekrF+2d/1sFnWG4HnV8TZY30iTOdtVWJG8abWvB1GlOgJuQZdcF2Luqm/hccMw==";

        #region 太難了以後研究
        //public string TokenCreation(string userId,int expireMinutes = 2)
        //{
        //    // 建立一組對稱式加密的金鑰，主要用於 JWT 簽章之用
        //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
        //    // var symmetricKey = Convert.FromBase64String(Secret); 或者可以用base64

        //    var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        //    // 建立 SecurityTokenDescriptor
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Issuer = issuer,
        //        //Audience = issuer, // 由於你的 API 受眾通常沒有區分特別對象，因此通常不太需要設定，也不太需要驗證
        //        //NotBefore = DateTime.Now, // 預設值就是 DateTime.Now
        //        //IssuedAt = DateTime.Now, // 預設值就是 DateTime.Now
        //        Subject = GetClaimsIdentity(userId),
        //        Expires = DateTime.Now.AddMinutes(expireMinutes),
        //        SigningCredentials = signingCredentials
        //    };

        //    // 產出所需要的 JWT securityToken 物件，並取得序列化後的 Token 結果(字串格式)
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        //    var serializeToken = tokenHandler.WriteToken(securityToken);

        //    return serializeToken;
        //}

        //public ClaimsIdentity GetClaimsIdentity(string userId)
        //{
        //    List<Claim> claims = new List<Claim>();

        //    claims.Add(new Claim(JwtRegisteredClaimNames.Sub, userId));
        //    claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

        //    //以下自行填充
        //    claims.Add(new Claim("roles", "Users"));

        //    //製作成ClaimsIdentity回傳
        //    return new ClaimsIdentity(claims);
        //}
        #endregion

        #region 使用JWT Nuget創建/驗證 Token
        public static string CreateToken(UserModel userModel, int expireMinutes = 20)
        {
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm(); // symmetric
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            Dictionary<string, object> payload = new Dictionary<string, object>()
            {
                { "Id", userModel.UID },
                {"exp", DateTimeOffset.UtcNow.AddMinutes(expireMinutes).ToUnixTimeSeconds() },
                {"role", userModel.Role}
            };

            var token = encoder.Encode(payload, Secret);

            return token;
        }

        /// <summary>
        /// 驗證Token
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        public static ApiResultViewModel TokenVerification(string Token)
        {
            ApiResultViewModel result = new ApiResultViewModel();
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                var provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtAlgorithm algorithm = new HMACSHA256Algorithm(); // symmetric
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);

                var json = decoder.Decode(Token, Secret, verify: true);

                result.Result = true;
                result.Status = ResponseCode.Success.ToString();
                result.ObjectData = json;

                //Console.WriteLine(json);
            }
            catch (TokenExpiredException)//這個Exception代表Token已過期
            {
                //Console.WriteLine("Token has expired");
                result.Result = false;
                result.Status = ResponseCode.Fail.ToString();
                result.Message = "Token已過期，請重新取得Token後再操作!!!";
                return result;
            }
            catch (SignatureVerificationException)
            {
                //Console.WriteLine("Token has invalid signature");
                result.Result = false;
                result.Status = ResponseCode.Fail.ToString();
                result.Message = "驗證Token內容發生錯誤!!!";
                return result;
            }
            catch(Exception)
            {
                result.Result = false;
                result.Status = ResponseCode.Fail.ToString();
                result.Message = "驗證Token內容發生錯誤!!!";
                return result;
            }

            return result;
        }

        #endregion

        #region 取得使用者ID
        public static string GetUserID(string strToken)
        {
            ApiResultViewModel result = TokenVerification(strToken);
            string UID = string.Empty;
            if(result.Result)
            {
                //取出Token資料
                JObject jObject = JObject.Parse(result.ObjectData.ToString());
                UID = jObject["Id"].ToString();
            }

            return UID;
        }

        #endregion
    }
}