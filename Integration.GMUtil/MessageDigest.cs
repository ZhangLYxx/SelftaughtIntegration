using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.GMUtil
{
    internal static class MessageDigest
    {
        public static byte[] Digest(string var0, byte[] var1)
        {
            IDigest var2;
            int var3;
            if ("MD5".Equals(var0, StringComparison.CurrentCultureIgnoreCase))
            {
                var2 = new MD5Digest();
            }
            else if ("SHA1".Equals(var0, StringComparison.CurrentCultureIgnoreCase))
            {
                var2 = new Sha1Digest(); //new SHA1Digest();
            }
            else if ("SHA224".Equals(var0, StringComparison.CurrentCultureIgnoreCase))
            {
                var2 = new Sha224Digest(); //SHA224Digest();
            }
            else if ("SHA256".Equals(var0, StringComparison.CurrentCultureIgnoreCase))
            {
                var2 = new Sha256Digest();
            }
            else if ("SHA384".Equals(var0, StringComparison.CurrentCultureIgnoreCase))
            {
                var2 = new Sha384Digest(); //SHA384Digest();
            }
            else if ("SHA512".Equals(var0, StringComparison.CurrentCultureIgnoreCase))
            {
                var2 = new Sha512Digest(); //SHA512Digest();
            }
            else if (var0 != null && var0.StartsWith("SHA3-"))
            {
                var3 = int.Parse(var0.Substring("SHA3-".Length));
                var2 = new Sha3Digest(var3);
            }
            else
            {
                if (!"SM3".Equals(var0, StringComparison.CurrentCultureIgnoreCase))
                {
                    throw new Exception("Digest Algorithm Name error:" + var0);
                }

                var2 = new SM3Digest();
            }

            //var3 = ((Digest)var2).getDigestSize();
            var3 = var2.GetDigestSize();
            byte[] var4 = new byte[var3];
            if (var1 != null)
            {
                //((Digest)var2).update(var1, 0, var1.length);
                var2.BlockUpdate(var1, 0, var1.Length);
            }

            //((Digest)var2).doFinal(var4, 0);
            //((Digest)var2).reset();
            var2.DoFinal(var4, 0);
            var2.Reset();
            return var4;
        }
    }
}
