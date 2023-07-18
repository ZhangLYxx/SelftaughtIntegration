using Integration.GMUtil;
using Integration.ToolKits;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Math;
using System.Configuration;
using System.Web;

namespace UCmember.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DecodeController : ControllerBase
    {
        //密钥分量1
        private static string EncryptWeightOne = "";
        //密钥分量2
        private static string EncryptWeightTwo = "";
        //公钥
        private static string publicKey = "";
        /// <summary>
        /// sm4解密，sm2验签
        /// </summary>
        /// <param name="ciphertext">密文串</param>
        /// <param name="signtext">签名串</param>
        /// <returns></returns>
        /// <exception cref="BizException"></exception>
        public bool Decode(string ciphertext, string signtext)
        {
            var result = string.Empty;
            try
            {
                result = SmUtility.Sm4Decrypt(EncryptWeightOne, EncryptWeightTwo, ciphertext);
            }
            catch
            {
                throw new BizException("解密失败");
            }
            //公钥128位(原130位，去掉开头两位)，拆成2个64位。
            publicKey = publicKey[2..];
            string pa = publicKey.Substring(0, 64);
            string pb = publicKey.Substring(64, 64);
            //加载公钥
            AsymmetricKeyParameter publicKey1 =
                GmUtil.GetPublickeyFromXY(new BigInteger(pa, 16), new BigInteger(pb, 16));
            //明文值
            //result
            //签名串
            var Sign = HttpUtility.UrlDecode(signtext);
            //SM2验签
            bool bRst = Sm2Encryptor.DoVerify(result, string.Empty, publicKey1, Sign);
            if (!bRst)
            {
                throw new BizException("验签失败");
            }
            return bRst;
        }
    }
}
