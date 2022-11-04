using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Data;
using System.Data.SqlClient;
using System.Text;

/// <summary>
/// md5Function 的摘要描述
/// </summary>
public class md5Function
{
	public md5Function()
	{
	}

    public string GetMD5(string original)
    {
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] b = md5.ComputeHash(Encoding.UTF8.GetBytes(original));
        return BitConverter.ToString(b).Replace("-", string.Empty);
    }

    /// <summary>
    /// MD5雜湊
    /// </summary>
    public static string GetHashBytes(string str)
    {
        using (var cryptoMD5 = System.Security.Cryptography.MD5.Create())
        {
            //將字串編碼成 UTF8 位元組陣列
            var bytes = System.Text.Encoding.UTF8.GetBytes(str);

            //取得雜湊值位元組陣列
            var hash = cryptoMD5.ComputeHash(bytes);

            //取得 MD5
            var md5 = BitConverter.ToString(hash).Replace("-", String.Empty).ToUpper();

            return md5;
        }
    }

    public string GetTimestemap()
    {
        int unixTimeSec = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        return unixTimeSec.ToString();
    }
}