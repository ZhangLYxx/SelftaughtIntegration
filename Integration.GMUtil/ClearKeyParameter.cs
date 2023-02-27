using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.GMUtil
{
    public class ClearKeyParameter : KeyParameter
    {
        private string _algorithmType;
        private int _keyLengthInBit = 0;
        private bool _isPrivate = false;
        private byte[] _key;
        private byte[] _password;
        private byte[]? _salt;
        private byte[]? _id;

        public string GetAlgorithm()
        {
            return _algorithmType;
        }

        public int getKeySize()
        {
            return _keyLengthInBit;
        }

        public ICipherParameters GetBCkey()
        {
            byte[] var1 = GetPassword(_password, _key.Length * 8);
            byte[] var2 = _key;
            //object var3 = null;
            byte[] var5 = new byte[var2.Length];
            if (("SM2".Equals(_algorithmType, StringComparison.CurrentCultureIgnoreCase)
                || "RSA".Equals(_algorithmType, StringComparison.CurrentCultureIgnoreCase)) && !_isPrivate)
            {
                //var5 = Array.CopyOfRange(var2, 0, var2.Length);
                Array.Copy(var2, var5, var2.Length);
            }
            else
            {
                var5 = UnwapKey(var2, var1);
            }

            ICipherParameters var4;
            if (!"SM2".Equals(_algorithmType) && !"RSA".Equals(_algorithmType))
            {
                var4 = new KeyParameter(var5);
            }
            else
            {
                var4 = Helper.toAsymmetricKey(var5, GetKeyType());
            }

            return var4;
        }

        public static ClearKeyParameter GetInstance(string var0, byte[] var1)
        {
            ClearKeyParameter var2 = GetInstance(var0, false, var1);
            return var2;
        }

        public static ClearKeyParameter GetInstance(string algorithmType, bool var1, byte[] var2)
        {
            ClearKeyParameter var3 = new ClearKeyParameter(algorithmType, var1, var2);
            return var3;

        }


        /*
        protected ClearKeyParameter(byte[] key) :base(key)
        {
            _key = key;
        }

        protected ClearKeyParameter(byte[] key, int keyOff, int keyLen) :base( key,  keyOff,  keyLen)
        {
            _key = key;
           
        }
        */
        protected ClearKeyParameter(string algorithmType, bool isPrivate, byte[] key) : base(key)
        {
            _salt = null;
            _algorithmType = algorithmType;
            _keyLengthInBit = key.Length * 8;
            _isPrivate = isPrivate;

            try
            {
                _password = MessageDigest.Digest("SM3", key);
            }
            catch
            {
                _password = new byte[0];
            }

            if (("SM2".Equals(algorithmType, StringComparison.OrdinalIgnoreCase) || "RSA".Equals(algorithmType, StringComparison.OrdinalIgnoreCase)) && !isPrivate)
            {
                //this.key = Arrays.copyOf(var3, var3.length);
                _key = new byte[key.Length];
                Array.Copy(key, _key, key.Length);
            }
            else
            {
                byte[] var4 = GetPassword(_password, key.Length * 8);
                _key = WapKey(key, var4);
            }
        }

        public void SetId(byte[] var1)
        {
            if (var1 == null)
            {
                _id = null;
            }
            else
            {
                _id = new byte[var1.Length];
                Array.Copy(var1, _id, var1.Length);
            }
        }

        public byte[]? GetId()
        {
            return _id;
        }



        public string? GetPublicKey()
        {
            return _isPrivate ? null : Hex.ToHexString(_key);
        }

        private byte[] GetPassword(byte[] var1, int var2)
        {
            if (_salt == null)
            {
                var var3 = new SecureRandom();
                _salt = new byte[32];
                var3.NextBytes(_salt);
            }
            //PKCS5S2ParametersGenerator
            Pkcs5S2ParametersGenerator var5 = new Pkcs5S2ParametersGenerator(new SM3Digest());
            var5.Init(var1, _salt, 16);
            KeyParameter var4 = (KeyParameter)var5.GenerateDerivedParameters(_algorithmType, var2);
            return var4.GetKey();
        }

        private static byte[] WapKey(byte[] var1, byte[] var2)
        {
            byte[] var3 = new byte[var1.Length];

            for (int var4 = 0; var4 < var3.Length; ++var4)
            {
                var3[var4] = (byte)((var1[var4] ^ var2[var4]) & 255);
            }

            return var3;
        }

        private byte[] UnwapKey(byte[] var1, byte[] var2)
        {
            byte[] var3 = new byte[_key.Length];

            for (int var4 = 0; var4 < var3.Length; ++var4)
            {
                var3[var4] = (byte)((_key[var4] ^ var2[var4]) & 255);
            }

            return var3;
        }

        public string GetKeyType()
        {
            if ("SM2".Equals(this._algorithmType, StringComparison.CurrentCultureIgnoreCase))
            {
                return this._isPrivate ? "SM2private" : "SM2public";
            }
            else if ("RSA".Equals(this._algorithmType, StringComparison.CurrentCultureIgnoreCase))
            {
                return this._isPrivate ? "RSAprivate" : "RSApublic";
            }
            else
            {
                throw new NotSupportedException($"not supported KeyType {_algorithmType} ");
            }
        }

    }
}
