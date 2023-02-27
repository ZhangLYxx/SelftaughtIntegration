using Org.BouncyCastle.Asn1.GM;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Utilities.Encoders;
using Org.BouncyCastle.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.GMUtil
{
    internal class Helper
    {
        public static AsymmetricKeyParameter toAsymmetricKey(byte[] var0, string var1)
        {
            //ECPoint var4;
            ECDomainParameters var5;
            X9ECParameters var12;
            // ECCurve var14;
            if ("SM2private".Equals(var1, StringComparison.InvariantCultureIgnoreCase))
            {
                var12 = GMNamedCurves.GetByName("sm2p256v1");
                //var14 = var12.GetCurve();
                //var4 = var12.G;
                var5 = new ECDomainParameters(var12.Curve, var12.G, var12.Curve.Order);
                BigInteger var15 = new BigInteger(1, var0);
                ECPrivateKeyParameters var16 = new ECPrivateKeyParameters(var15, var5);
                return var16;
            }
            else if ("SM2public".Equals(var1, StringComparison.CurrentCultureIgnoreCase))
            {
                var12 = GMNamedCurves.GetByName("sm2p256v1");
                //var14 = var12.GetCurve();
                //var4 = var12.GetG();
                var5 = new ECDomainParameters(var12.Curve, var12.G, var12.Curve.Order);
                var var6 = Hex.ToHexString(var0);
                if (!var6.StartsWith("04"))
                {
                    throw new Exception("key data error!");
                }
                else
                {
                    var var7 = var6.Substring(2, 66);
                    var var8 = var6.Substring(66);
                    var var9 = var12.Curve.CreatePoint(new BigInteger(var7, 16), new BigInteger(var8, 16));
                    ECPublicKeyParameters var10 = new ECPublicKeyParameters(var9, var5);
                    return var10;
                }
            }
            else if ("RSAprivate".Equals(var1, StringComparison.CurrentCultureIgnoreCase))
            {
                //RsaPrivateKey
                var var11 = RsaPrivateKeyStructure.GetInstance(var0);
                // RsaPrivateCrtKeyParameters
                var var13 = new RsaPrivateCrtKeyParameters(var11.Modulus, var11.PublicExponent, var11.PrivateExponent, var11.Prime1, var11.Prime2, var11.Exponent1, var11.Exponent2, var11.Coefficient);
                return var13;
            }
            else if ("RSApublic".Equals(var1, StringComparison.CurrentCultureIgnoreCase))
            {
                //RsaPublicKeyStructure
                //RSAPublicKey
                RsaPublicKeyStructure var2 = RsaPublicKeyStructure.GetInstance(var0);
                //RSAKeyParameters
                var var3 = new RsaKeyParameters(false, var2.Modulus, var2.PublicExponent);
                return var3;
            }
            else
            {
                throw new Exception("key type not support! " + var1);
            }
        }

    }
}
