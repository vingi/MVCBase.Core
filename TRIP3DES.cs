using System;
using System.Collections.Generic;
using System.Web;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace MVCBase.Core
{
    public class TRIP3DES
    {

        //密钥

        private string sKey = "qJzGEh6hESZDVJeCnFPGuxzaiB7NLQM3";

        //矢量，矢量可以为空

        private string sIV = "vingiDES";

        //构造一个对称算法

        private SymmetricAlgorithm mCSP = new TripleDESCryptoServiceProvider();



        public TRIP3DES(string _sKey, string _sIV)
        {
            sKey = _sKey;
            sIV = _sIV;
        }


        #region public string EncryptString(string Value)

        /// 

        /// 加密字符串

        /// 

        /// 输入的字符串

        /// 加密后的字符串

        public string EncryptString(string Value)
        {

            ICryptoTransform ct;

            MemoryStream ms;

            CryptoStream cs;

            byte[] byt;

            mCSP.Key = Convert.FromBase64String(sKey);

            mCSP.IV = Convert.FromBase64String(sIV);

            //指定加密的运算模式

            mCSP.Mode = System.Security.Cryptography.CipherMode.ECB;

            //获取或设置加密算法的填充模式

            mCSP.Padding = System.Security.Cryptography.PaddingMode.PKCS7;

            ct = mCSP.CreateEncryptor(mCSP.Key, mCSP.IV);

            byt = Encoding.UTF8.GetBytes(Value);

            ms = new MemoryStream();

            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);

            cs.Write(byt, 0, byt.Length);

            cs.FlushFinalBlock();

            cs.Close();

            //return Convert.ToBase64String(ms.ToArray());
            //return Convert.ToChar(ms.ToArray()).ToString();

            string strResult = "";
            byte[] b = ms.ToArray();
            for (int i = 0; i < b.Length; i++)
            {
                strResult += b[i].ToString("x").PadLeft(2, '0');
            }
            return strResult;
        }

        #endregion



        #region public string DecryptString(string Value)

        /// 

        /// 解密字符串

        /// 

        /// 加过密的字符串

        /// 解密后的字符串

        public string DecryptString(string Value)
        {

            ICryptoTransform ct;

            MemoryStream ms;

            CryptoStream cs;

            byte[] byt;

            mCSP.Key = Convert.FromBase64String(sKey);

            mCSP.IV = Convert.FromBase64String(sIV);

            mCSP.Mode = System.Security.Cryptography.CipherMode.ECB;

            mCSP.Padding = System.Security.Cryptography.PaddingMode.PKCS7;

            ct = mCSP.CreateDecryptor(mCSP.Key, mCSP.IV);

            //byt = Convert.FromBase64String(Value);

            byt = new byte[Value.Length / 2];
            int bi = 0;
            for (int i = 0; i < Value.Length; i += 2)
            {
                byt[bi] = (byte)Int32.Parse(Value.Substring(i, 2), System.Globalization.NumberStyles.HexNumber);
                bi++;
            }

            ms = new MemoryStream();

            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);

            cs.Write(byt, 0, byt.Length);

            cs.FlushFinalBlock();

            cs.Close();



            return Encoding.UTF8.GetString(ms.ToArray());

        }

        #endregion

    }

}