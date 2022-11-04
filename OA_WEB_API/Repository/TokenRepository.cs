using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

using OA_WEB_API.Core.Exceptions;
using OA_WEB_API.Models;

using Dapper;
using Newtonsoft.Json;

/// <summary>
/// 權杖資料
/// </summary>
namespace OA_WEB_API.Repository
{
    #region - 權杖加解密 -

    /// <summary>
    /// 權杖(加密/解密)
    /// </summary>
    public static class TokenCrypto
    {
        /// <summary>
        /// 產生 HMACSHA256 雜湊
        /// </summary>
        public static string ComputeHMACSHA256(string data, string key)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);

            using (var hmacSHA = new HMACSHA256(keyBytes))
            {
                var dataBytes = Encoding.UTF8.GetBytes(data);
                var hash = hmacSHA.ComputeHash(dataBytes, 0, dataBytes.Length);

                return BitConverter.ToString(hash).Replace("-", "").ToUpper();
            }
        }

        /// <summary>
        /// AES 加密
        /// </summary>
        public static string AESEncrypt(string data, string key, string iv)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var ivBytes = Encoding.UTF8.GetBytes(iv);
            var dataBytes = Encoding.UTF8.GetBytes(data);

            using (var aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.IV = ivBytes;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                var encryptor = aes.CreateEncryptor();
                var encrypt = encryptor.TransformFinalBlock(dataBytes, 0, dataBytes.Length);

                return Convert.ToBase64String(encrypt);
            }
        }

        /// <summary>
        /// AES 解密
        /// </summary>
        public static string AESDecrypt(string data, string key, string iv)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var ivBytes = Encoding.UTF8.GetBytes(iv);
            var dataBytes = Convert.FromBase64String(data);

            using (var aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.IV = ivBytes;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                var decryptor = aes.CreateDecryptor();
                var decrypt = decryptor.TransformFinalBlock(dataBytes, 0, dataBytes.Length);

                return Encoding.UTF8.GetString(decrypt);
            }
        }
    }

    #endregion

    #region - 權杖管理 -

    public class TokenManager
    {
        #region - 宣告 -

        string privateKey = GlobalParameters.PrivateKey;   //私鑰

        #endregion

        /// <summary>
        /// 產生權杖
        /// </summary>
        public TokenModel Create(Object user)
        {
            var expired = GlobalParameters.Expired;   //過期時間(秒)

            var payload = new PayloadModel
            {
                USER = user,
                EXPIRED = Convert.ToInt32((DateTime.Now.AddSeconds(expired) - new DateTime(1970, 1, 1)).TotalSeconds) //Unix 時間戳
            };

            var json = JsonConvert.SerializeObject(payload);
            var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
            var iv = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 16);

            //使用 AES 加密 Payload
            var encrypt = TokenCrypto.AESEncrypt(base64, privateKey.Substring(0, 16), iv);

            //取得簽章
            var signature = TokenCrypto.ComputeHMACSHA256(iv + "." + encrypt, privateKey.Substring(0, 64));

            return new TokenModel
            {
                //Token 為 iv + encrypt + signature，並用 . 串聯
                ACCESS_TOKEN = iv + "." + encrypt + "." + signature,
                REFRESH_TOKEN = Guid.NewGuid().ToString().Replace("-", ""),
                EXPIRES_IN = expired
            };
        }

        /// <summary>
        /// 驗證權杖
        /// </summary>
        public bool IsAuthenticated(String token)
        {
            bool vResult = false;
            bool IsDebug = true;

            if (!IsDebug)
            {
                if (String.IsNullOrEmpty(token))
                {
                    throw new Exception("權杖不可以空白");
                }
                else
                {
                    var split = token.Split('.');
                    var iv = split[0];
                    var encrypt = split[1];
                    var signature = split[2];

                    //檢查簽章是否正確
                    if (signature != TokenCrypto.ComputeHMACSHA256(iv + "." + encrypt, privateKey.Substring(0, 64)))
                    {
                        throw new Exception("簽章錯誤");
                    }

                    //使用 AES 解密 Payload
                    var base64 = TokenCrypto.AESDecrypt(encrypt, privateKey.Substring(0, 16), iv);
                    var json = Encoding.UTF8.GetString(Convert.FromBase64String(base64));
                    var payload = JsonConvert.DeserializeObject<PayloadModel>(json);

                    //檢查是否過期
                    if (payload.EXPIRED < Convert.ToInt32((DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds))
                    {
                        throw new Exception("權杖過期");
                    }

                    vResult = true;
                }
            }
            else
            {
                vResult = true;
            }

            return vResult;
        }
    }

    #endregion
}