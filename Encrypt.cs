using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace MVCBase.Core
{
    public class Encrypt
    {
        /// <summary>
        /// 当前程序加密所使用的密钥

        /// </summary>
        public static readonly string sKey = "vingi3DESLDHzt/pRr/TOGSJPXANLG51";

        #region 加密方法
        /// <summary>
        /// 加密方法
        /// </summary>
        /// <param name="pToEncrypt">需要加密字符串</param>
        /// <param name="sKey">密钥</param>
        /// <returns>加密后的字符串</returns>
        public static string DESEncrypt(string pToEncrypt)
        {
            try
            {
                TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
                des.Key = Convert.FromBase64String(sKey);
                des.Mode = CipherMode.ECB;

                byte[] valBytes = Encoding.Unicode.GetBytes(pToEncrypt);
                ICryptoTransform transform = des.CreateEncryptor();

                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, transform, CryptoStreamMode.Write);
                cs.Write(valBytes, 0, valBytes.Length);
                cs.FlushFinalBlock();
                byte[] returnBytes = ms.ToArray();
                cs.Close();

                return Convert.ToBase64String(returnBytes);
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Response.Write("写入配置信息失败，详细信息：" + ex.Message.Replace("\r\n", "").Replace("'", ""));
            }

            return "";
        }
        #endregion

        #region 解密方法
        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="pToDecrypt">需要解密的字符串</param>
        /// <param name="sKey">密匙</param>
        /// <returns>解密后的字符串</returns>
        public static string DESDecrypt(string pToDecrypt)
        {
            try
            {
                TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
                des.Key = Convert.FromBase64String(sKey);
                des.Mode = CipherMode.ECB;

                byte[] valBytes = Convert.FromBase64String(pToDecrypt);
                ICryptoTransform transform = des.CreateDecryptor();

                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, transform, CryptoStreamMode.Write);
                cs.Write(valBytes, 0, valBytes.Length);
                cs.FlushFinalBlock();
                byte[] returnBytes = ms.ToArray();
                cs.Close();

                return Encoding.Unicode.GetString(returnBytes);
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Response.Write("读取配置信息失败，详细信息：" + ex.Message.Replace("\r\n", "").Replace("'", ""));
            }
            return "";
        }
        #endregion


        #region MD5编码方法
        /// <summary>
        /// MD5编码方法
        /// </summary>
        /// <param name="pToDecrypt">需要编码的字符串</param>
        /// <returns>编码后的字符串</returns>
        public static string MD5Encrypt(string pToEncrypt)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(pToEncrypt, "MD5");
        }
        #endregion

        #region SHA1编码方法
        /// <summary>
        /// MD5编码方法
        /// </summary>
        /// <param name="pToDecrypt">需要编码的字符串</param>
        /// <returns>编码后的字符串</returns>
        public static string SHA1Encrypt(string pToEncrypt)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(pToEncrypt, "SHA1");
        }
        #endregion

    }

}
