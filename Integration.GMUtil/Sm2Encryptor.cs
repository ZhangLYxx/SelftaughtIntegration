using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Math;

namespace Integration.GMUtil
{
    /// 
    ///     国密SM2算法（ECC算法）加密器
    ///     签名部分采用SM3算法进行摘要计算
    /// 
    public class Sm2Encryptor
    {
        /// 
        ///     SM2算法默认用户ID，目前开放平台不会使用非默认用户ID
        /// 
        public const string DefaultUserId = "1234567812345678";

        public string GetAsymmetricType()
        {
            return "SM2";
        }

        public (string privatePem, string publicPem) DoKeyPairGenerator()
        {
            var SM2_ECC_P = new BigInteger("FFFFFFFEFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FFFFFFFFFFFFFFFF", 16);
            var SM2_ECC_A = new BigInteger("FFFFFFFEFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FFFFFFFFFFFFFFFC", 16);
            var SM2_ECC_B = new BigInteger("28E9FA9E9D9F5E344D5A9E4BCF6509A7F39789F515AB8F92DDBCBD414D940E93", 16);
            var SM2_ECC_N = new BigInteger("FFFFFFFEFFFFFFFFFFFFFFFFFFFFFFFF7203DF6B21C6052B53BBF40939D54123", 16);
            var SM2_ECC_H = BigInteger.One;
            var SM2_ECC_GX = new BigInteger("32C4AE2C1F1981195F9904466A39C9948FE30BBFF2660BE1715A4589334C74C7", 16);
            var SM2_ECC_GY = new BigInteger("BC3736A2F4F6779C59BDCEE36B692153D0A9877CC62A474002DF32E52139F0A0", 16);
            var SM2_ECC_Random = new SecureRandom();

            ECCurve curve = new FpCurve(SM2_ECC_P, SM2_ECC_A, SM2_ECC_B, SM2_ECC_N, SM2_ECC_H);

            var g = curve.CreatePoint(SM2_ECC_GX, SM2_ECC_GY);
            var domainParams = new ECDomainParameters(curve, g, SM2_ECC_N);

            var keyPairGenerator = new ECKeyPairGenerator();


            var aKeyGenParams = new ECKeyGenerationParameters(domainParams, SM2_ECC_Random);

            keyPairGenerator.Init(aKeyGenParams);

            var aKp = keyPairGenerator.GenerateKeyPair();

            var aPub = (ECPublicKeyParameters)aKp.Public;
            var aPriv = (ECPrivateKeyParameters)aKp.Private;

            var pkinfoPri = PrivateKeyInfoFactory.CreatePrivateKeyInfo(aPriv);
            var priPem = Convert.ToBase64String(pkinfoPri.GetDerEncoded());

            var pkinfoPub = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(aPub);
            var pubPem = Convert.ToBase64String(pkinfoPub.GetDerEncoded());
            return (priPem, pubPem);
        }

        public string DoDecrypt(string cipherTextBase64, string charset, string privateKey)
        {
            //加载私钥参数
            var cipherParams = BuildPrivateKeyParams(privateKey).Parameters;

            //初始化SM2算法引擎
            var sm2Engine = new SM2Engine();
            sm2Engine.Init(false, cipherParams);

            //对输入密文进行解密
            var input = Convert.FromBase64String(cipherTextBase64);
            var output = sm2Engine.ProcessBlock(input, 0, input.Length);

            //将解密后的明文按指定字符集编码后返回
            return Encoding.GetEncoding(charset).GetString(output);
        }

        public string DoEncrypt(string plainText, string charset, string publicKey)
        {
            //加载公钥参数
            var cipherParams = BuildPublicKeyParams(publicKey).Parameters;
            var parametersWithRandom = new ParametersWithRandom(cipherParams);

            //初始化SM2算法引擎
            var sm2Engine = new SM2Engine();
            sm2Engine.Init(true, parametersWithRandom);

            //对输入明文进行加密
            var input = Encoding.GetEncoding(charset).GetBytes(plainText);
            var output = sm2Engine.ProcessBlock(input, 0, input.Length);

            //将密文Base64编码后返回
            return Convert.ToBase64String(output);
        }

        public string DoSign(string content, string charset, string privateKey)
        {
            //加载私钥参数
            var parametersWithId = BuildPrivateKeyParams(privateKey);

            //加载签名器
            var signer = new SM2Signer();
            signer.Init(true, parametersWithId);

            //向签名器中输入原文
            var input = Encoding.GetEncoding(charset).GetBytes(content);
            signer.BlockUpdate(input, 0, input.Length);

            //将签名结果转换为Base64
            return Convert.ToBase64String(signer.GenerateSignature());
        }

        public static bool DoVerify(string content, string charset, AsymmetricKeyParameter publicKey, string sign)
        {
            //加载公钥参数
            //var parametersWithId = BuildPublicKeyParams(publicKey);

            //加载签名器
            var signer = new SM2Signer();
            signer.Init(false, publicKey);

            //向签名器中输入原文
            var input = Encoding.Default.GetBytes(content);
            signer.BlockUpdate(input, 0, input.Length);

            //传入指定签名串进行验签并返回结果
            return signer.VerifySignature(Convert.FromBase64String(sign));
        }

        private ParametersWithID BuildPrivateKeyParams(string privateKey)
        {
            var key = PrivateKeyFactory.CreateKey(Convert.FromBase64String(privateKey));
            var parametersWithId = new ParametersWithID(key, Encoding.UTF8.GetBytes(DefaultUserId));
            return parametersWithId;
        }

        private static ParametersWithID BuildPublicKeyParams(string publicKey)
        {
            var s = Convert.FromBase64String(publicKey);

            var key = PublicKeyFactory.CreateKey(s);
            var parametersWithId = new ParametersWithID(key, Encoding.UTF8.GetBytes(DefaultUserId));
            return parametersWithId;
        }
    }
}
