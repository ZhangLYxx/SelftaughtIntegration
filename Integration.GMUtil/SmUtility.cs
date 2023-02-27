using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.Encoders;
using System.ComponentModel.Design;
using System.Net;
using System.Text;

namespace Integration.GMUtil
{
    /// <summary>
    /// 国密工具
    /// </summary>
    public static class SmUtility
    {
        /// <summary>
        /// SM4 加密
        /// </summary>
        /// <param name="keyPart1">16进制</param>
        /// <param name="keyPart2">16进制</param>
        /// <param name="text"></param>
        /// <returns>urlencode(base64)</returns>
        public static string Sm4Encrypt(string keyPart1, string keyPart2, string text)
        {
            var plainBytes = Encoding.UTF8.GetBytes(text);

            byte[] sm4KeyByte = KeyXor(Hex.Decode(keyPart1), Hex.Decode(keyPart2));
            var sm4KeyString = Hex.ToHexString(sm4KeyByte);
            ClearKeyParameter sm4key = LoadClear(sm4KeyString, "SM4");

            var cipher = CipherUtilities.GetCipher("SM4/CBC/PKCS5PADDING");
            cipher.Init(true, sm4key);
            var encryptBytes = cipher.DoFinal(plainBytes);

            return WebUtility.UrlEncode(Convert.ToBase64String(encryptBytes));
            //var encryptResult = WebUtility.UrlEncode(encryptText);
            //Console.WriteLine(encryptText);
            //Console.WriteLine(encryptResult);
            //return encryptResult;
        }

        /// <summary>
        /// Sm4 解密
        /// </summary>
        /// <param name="keyPart1">16进制</param>
        /// <param name="keyPart2">16进制</param>
        /// <param name="urlEncrypt"></param>
        /// <returns></returns>
        public static string Sm4Decrypt(string keyPart1, string keyPart2, string encrypt)
        {
            var encryptBytes = Convert.FromBase64String(WebUtility.UrlDecode(encrypt));

            byte[] sm4KeyByte = KeyXor(Hex.Decode(keyPart1), Hex.Decode(keyPart2));
            var sm4KeyString = Hex.ToHexString(sm4KeyByte);
            ClearKeyParameter sm4key = LoadClear(sm4KeyString, "SM4");
            var cipher = CipherUtilities.GetCipher("SM4/CBC/PKCS5PADDING");
            cipher.Init(false, sm4key);
            var plainBytes = cipher.DoFinal(encryptBytes);
            return Encoding.UTF8.GetString(plainBytes);
            //Console.WriteLine(plainText);
            //return plainText;
        }

        /// <summary>
        /// SM2 验签
        /// </summary>
        /// <param name="pubKeyText"></param>
        /// <param name="signText"></param>
        /// <returns></returns>
        public static bool Sm2Verify(string pubKeyText, string signBase64, string data)
        {
            //var realText = WebUtility.UrlDecode(data);
            var base64Bytes = Convert.FromBase64String(data);
            var signBytes = Convert.FromBase64String(WebUtility.UrlDecode(signBase64));

            ClearKeyParameter pubKey = LoadClear(pubKeyText, "SM2public");
            ISigner signer = SignerUtilities.GetSigner("SM3withSM2");
            signer.Init(false, pubKey);
            signer.BlockUpdate(base64Bytes, 0, base64Bytes.Length);

            return signer.VerifySignature(signBytes);
        }

        /*
        /// <summary>
        /// SM2 签名
        /// </summary>
        /// <param name="pubKeyText"></param>
        /// <param name="content"></param>
        /// <returns>urlencode(base64)</returns>
        public static string Sm2GenerateSignature(string pubKeyText, string content)
        {
            var bytes = Encoding.UTF8.GetBytes(content);

            ICipherParameters pubKey = LoadClear(pubKeyText, "SM2public");
            ISigner signner = SignerUtilities.GetSigner("SM3withSM2");
            signner.Init(true, pubKey);
            signner.BlockUpdate(bytes, 0, bytes.Length);
            var signBytes = signner.GenerateSignature();
            return WebUtility.UrlEncode(Convert.ToBase64String(signBytes));
        }
        */

        private static byte[] KeyXor(byte[] key1Bytes, byte[] key2Bytes)
        {
            if (key1Bytes != null && key2Bytes != null && key1Bytes.Length == key2Bytes.Length)
            {
                var keyBytes = new byte[key1Bytes.Length];
                for (var i = 0; i < key1Bytes.Length; i++)
                {
                    keyBytes[i] = (byte)((key1Bytes[i] ^ key2Bytes[i]) & 255);
                }
                return keyBytes;
            }

            return Array.Empty<byte>();
        }


        internal static ClearKeyParameter LoadClear(string key, string algorithm)
        {
            ClearKeyParameter var2 = null;
            if (!"DES".Equals(algorithm, StringComparison.CurrentCultureIgnoreCase)
                && !"DESede".Equals(algorithm, StringComparison.CurrentCultureIgnoreCase)
                && !"AES".Equals(algorithm, StringComparison.CurrentCultureIgnoreCase)
                && !"SM4".Equals(algorithm, StringComparison.CurrentCultureIgnoreCase))
            {
                if ("SM2private".Equals(algorithm, StringComparison.CurrentCultureIgnoreCase))
                {
                    var2 = ClearKeyParameter.GetInstance("SM2", true, Hex.Decode(key));
                }
                else if ("RSAprivate".Equals(algorithm, StringComparison.CurrentCultureIgnoreCase))
                {
                    var2 = ClearKeyParameter.GetInstance("RSA", true, Hex.Decode(key));
                }
                else if ("SM2public".Equals(algorithm, StringComparison.CurrentCultureIgnoreCase))
                {
                    var2 = ClearKeyParameter.GetInstance("SM2", false, Hex.Decode(key));
                }
                else if ("RSApublic".Equals(algorithm, StringComparison.CurrentCultureIgnoreCase))
                {
                    var2 = ClearKeyParameter.GetInstance("RSA", false, Hex.Decode(key));
                }
            }
            else
            {
                var2 = ClearKeyParameter.GetInstance(algorithm, false, Hex.Decode(key));
            }

            return var2;

        }
    }
}