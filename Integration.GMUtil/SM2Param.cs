using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.GMUtil
{
    public class SM2Param
    {
        public static String[] ecc_param = { "FFFFFFFEFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FFFFFFFFFFFFFFFF", "FFFFFFFEFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FFFFFFFFFFFFFFFC", "28E9FA9E9D9F5E344D5A9E4BCF6509A7F39789F515AB8F92DDBCBD414D940E93", "FFFFFFFEFFFFFFFFFFFFFFFFFFFFFFFF7203DF6B21C6052B53BBF40939D54123", "32C4AE2C1F1981195F9904466A39C9948FE30BBFF2660BE1715A4589334C74C7", "BC3736A2F4F6779C59BDCEE36B692153D0A9877CC62A474002DF32E52139F0A0" };
        public BigInteger ecc_p;
        public BigInteger ecc_a;
        public BigInteger ecc_b;
        public BigInteger ecc_n;
        public BigInteger ecc_gx;
        public BigInteger ecc_gy;
        public ECCurve ecc_curve;
        public ECDomainParameters ecc_bc_spec;

        public SM2Param()
        {
            this.ecc_p = new BigInteger(ecc_param[0], 16);
            this.ecc_a = new BigInteger(ecc_param[1], 16);
            this.ecc_b = new BigInteger(ecc_param[2], 16);
            this.ecc_n = new BigInteger(ecc_param[3], 16);
            this.ecc_gx = new BigInteger(ecc_param[4], 16);
            this.ecc_gy = new BigInteger(ecc_param[5], 16);
            this.ecc_curve = new FpCurve(ecc_p, ecc_a, ecc_b, ecc_n, BigInteger.One);
            this.ecc_bc_spec = new ECDomainParameters(this.ecc_curve, this.ecc_curve.CreatePoint(this.ecc_gx, this.ecc_gy), this.ecc_n);
        }
    }
}
