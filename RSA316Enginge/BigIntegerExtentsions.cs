using System;
using System.Numerics;
using System.Collections;
namespace RSA316Infinito.BigRsaCrypto
{
    public class BigIntegerExtensions
    {
        public static BigInteger2 modPower(BigInteger2 current, BigInteger2 power, BigInteger2 mod)
        {
            var numb1 = BigInteger2.GetBytesTwosCompliment(current);
            var numb2 = BigInteger2.GetBytesTwosCompliment(power);
            var numb3 = BigInteger2.GetBytesTwosCompliment(mod);

            BigInteger n1 = new BigInteger(numb1);
            BigInteger n2 = new BigInteger(numb2);
            BigInteger n3 = new BigInteger(numb3);

            var result = BigInteger.ModPow(n1, n2, n3);
            
            var bitAry = BigInteger2.ReverseBigIntegerBitsToBitArray(result.ToByteArray());
            var whole = new BigInteger2(bitAry.Length, 0);
            BigInteger2.CopyFromBitArrayFromIndex(bitAry, whole, 0);

            return BigInteger2.NegateZeros(whole);
        }

        public static BigInteger2 gcd(BigInteger2 Numb1, BigInteger2 Numb2)
        {

            var numb1 = BigInteger2.GetBytesTwosCompliment(Numb1);
            var numb2 = BigInteger2.GetBytesTwosCompliment(Numb2);

            BigInteger n1 = new BigInteger(numb1);
            BigInteger n2 = new BigInteger(numb2);

            var result = BigInteger.GreatestCommonDivisor(n1, n2);
            
            var bitAry  = BigInteger2.ReverseBigIntegerBitsToBitArray(result.ToByteArray());
            var whole = new BigInteger2(bitAry.Length, 0);
            BigInteger2.CopyFromBitArrayFromIndex(bitAry, whole, 0);
            return BigInteger2.NegateZeros(whole);
        }


        public static BigInteger2 inverseMod(BigInteger2 numbe, BigInteger2 modf)
        {
            //Check to see that gcd is 1 before proceeding: gcd(numbe,modf)=1
            ArrayList factors2 = new ArrayList();
            int count = 0;
            BigInteger2 NUM = (BigInteger2)numbe.Clone();
            BigInteger2 MOD = (BigInteger2)modf.Clone();
            bool foundInverse = true;

            while (true)
            {
                BigInteger2[] temp = BigIntegerExtensions.DivideBy(modf, numbe);

                Factors2 x = new Factors2();
                x.Mod = modf;
                x.fMod = (BigInteger2)BigInteger2.ONE().Clone();
                x.fnumb = BigInteger2.NegateZeros((BigInteger2)temp[0].Clone());
                x.fnumb.bitlength[0] = true;

                if (count == 0) factors2.Add(x);//added only in first instance
                count++;//iteration number
                //Look up for previous remainder for factor substitution in x:
                if (count == 2)
                {
                    Factors2 prevx = (Factors2)factors2[0];

                    //Check for Factors2 to incorporate!!
                    Factors2 tmpResolve = new Factors2();

                    Factors2 resCurr = new Factors2();
                    resCurr.fnumb = BigInteger2.NegateZeros(BigIntegerExtensions.Multiply(x.fnumb, prevx.fnumb));
                    resCurr.fMod = BigInteger2.NegateZeros(BigIntegerExtensions.Multiply(x.fnumb, prevx.fMod));

                    tmpResolve.fnumb = x.fMod + resCurr.fnumb;
                    tmpResolve.fMod = resCurr.fMod;
                    factors2.Add(tmpResolve);
                }
                if (count > 2)
                {
                    Factors2 prev2Back = (Factors2)factors2[factors2.Count - 2];
                    Factors2 prevBack = (Factors2)factors2[factors2.Count - 1];
                    Factors2 resCurr = new Factors2();

                    resCurr.fnumb = BigInteger2.NegateZeros(BigIntegerExtensions.Multiply(x.fnumb, prevBack.fnumb));
                    resCurr.fMod = BigInteger2.NegateZeros(BigIntegerExtensions.Multiply(x.fnumb, prevBack.fMod));

                    resCurr.fnumb = prev2Back.fnumb + resCurr.fnumb;
                    resCurr.fMod = prev2Back.fMod + resCurr.fMod;

                    factors2.Add(resCurr);
                }
                if (temp[1] == BigInteger2.ONE()) break;
                if (temp[1] == new BigInteger2(1, 0))
                {
                    foundInverse = false;
                    break;
                }
                modf = (BigInteger2)numbe.Clone();
                numbe = (BigInteger2)temp[1].Clone();
            }

            if (!foundInverse) return new BigInteger2(1, 0);
            //Calculate inverse mod
            Factors2 result = (Factors2)factors2[count - 1];
            BigInteger2 invres = result.fnumb;
            if (!invres.bitlength[0]) return (BigIntegerExtensions.DivideBy(invres, MOD))[1];//if invres is positive
            else // invres is negative
            {
                invres.bitlength[0] = false;
                if (invres < MOD) return BigInteger2.NegateZeros(MOD - invres);
                else
                {
                    BigInteger2 test = BigIntegerExtensions.DivideBy(invres, MOD)[0] + BigInteger2.ONE();
                    return BigInteger2.NegateZeros(BigIntegerExtensions.Multiply(test, MOD) - MOD);
                }
            }

        }
        //MillerRabinTest
        public static bool MillerRabinIsPrime(BigInteger2 numb)
        {
            //Use Rabin's Test suite a.modPower(d,numb)== 1 : then numb is prime else numb composite

            BigInteger2 n = (BigInteger2)numb.Clone();
            BigInteger2 numbLess1 = n - BigInteger2.ONE();

            BigInteger2[] testSuite = new BigInteger2[3];

            //Fill testSuite with BigInteger2 values:
            //Including 5, 11, and 61

            testSuite[0] = new BigInteger2(1, 0);
            testSuite[0].bitlength = BigInteger2.ConvertToBinary(61, 6);
            testSuite[1] = new BigInteger2(1, 0);
            testSuite[1].bitlength = BigInteger2.ConvertToBinary(31, 6);
            testSuite[2] = new BigInteger2(1, 0);
            testSuite[2].bitlength = BigInteger2.ConvertToBinary(11, 4);

            // Determine two.power(r) factor of q:
            // By: using q is least set bit referenced from zero
            int i = 0, j = 0;
            for (i = numbLess1.bitlength.Length - 1, j = 0; i >= 2; i--, j++)
                if (numbLess1.bitlength[i]) break;
            //Console.Out.WriteLine("j: {0}", j.ToString());
            //Form component two.power(r) = twoPowerR
            BigInteger2 twoPowerS = BigInteger2.TWO().power(j);

            //Calculate d:
            BigInteger2 d = BigIntegerExtensions.DivideBy(numbLess1, twoPowerS)[0];

            //Use testSuite to eliminate Composites:
            bool isPrime = true;
            foreach (BigInteger2 integer in testSuite)
            {
                bool prime = false;
                BigInteger2 result = BigIntegerExtensions.modPower(integer, d, numb);
                if (result == BigInteger2.ONE())
                {
                    prime = true;
                    continue;
                }
                else
                {

                    for (i = 0; i < j; i++)
                    {
                        if ((numb - result) == BigInteger2.ONE())
                        {
                            prime = true;
                            break;
                        }
                        result = BigIntegerExtensions.modPower(result, BigInteger2.TWO(), numb);
                    }
                    if (prime)
                    {
                        continue;
                    }
                    else
                    {
                        isPrime = false;
                        break;
                    }
                }
            }
            return isPrime;
        }
        public static BigInteger2[] DivideBy(BigInteger2 Numb1, BigInteger2 Numb2)
        {
            var numb1 = BigInteger2.GetBytesTwosCompliment(Numb1);
            var numb2 = BigInteger2.GetBytesTwosCompliment(Numb2);

            BigInteger n1 = new BigInteger(numb1);
            BigInteger n2 = new BigInteger(numb2);
            BigInteger quot;
            BigInteger result = BigInteger.DivRem(n1, n2, out quot);

            BigInteger2[] results = new BigInteger2[2];
            
            var bitAry = BigInteger2.ReverseBigIntegerBitsToBitArray(result.ToByteArray());
            var whole = new BigInteger2(bitAry.Length, 0);
            BigInteger2.CopyFromBitArrayFromIndex(bitAry, whole, 0);
            
            var bitAry2 = BigInteger2.ReverseBigIntegerBitsToBitArray(quot.ToByteArray());
            var quotient = new BigInteger2(bitAry2.Length, 0);
            BigInteger2.CopyFromBitArrayFromIndex(bitAry2, quotient, 0);

            results[0] = BigInteger2.NegateZeros(whole);
            results[1] = BigInteger2.NegateZeros(quotient);

            return results;
        }

        public static BigInteger2 Multiply(BigInteger2 Numb1, BigInteger2 Numb2)
        {
            var numb1 = BigInteger2.GetBytesTwosCompliment(Numb1);
            var numb2 = BigInteger2.GetBytesTwosCompliment(Numb2);

            BigInteger n1 = new BigInteger(numb1);
            BigInteger n2 = new BigInteger(numb2);

            var result = BigInteger.Multiply(n1, n2);
            
            var bitAry = BigInteger2.ReverseBigIntegerBitsToBitArray(result.ToByteArray());
            var whole = new BigInteger2(bitAry.Length, 0);
            BigInteger2.CopyFromBitArrayFromIndex(bitAry, whole, 0);
            return BigInteger2.NegateZeros(whole);
        }
        public static int[] mySet = BigInteger2.getnormalPrimes(2048);
        public static BigInteger2 getProbablePrime(int n)
        {
            BigInteger2 num = new BigInteger2(n);
            BigInteger2 zero = BigInteger2.Zero();
            BigInteger2 temp = new BigInteger2(1, 0);
            BigInteger2 nem = ((BigInteger2)num.Clone()) + BigInteger2.T50();
            bool success = true;
            int ni = 0;
            while (true)
            {
                ni++;
                Console.Out.WriteLine(ni);
                if (num < nem || num == nem)
                    foreach (int i in mySet)
                    {
                        bool[] val = BigInteger2.ConvertToBinary(i, 11);
                        bool[] t = new bool[val.Length];
                        for (int x = 0; x < val.Length; x++)
                        {
                            t[x] = (val[x] ? true : false);
                        }
                        temp.bitlength = t;
                        if (BigIntegerExtensions.DivideBy(num, temp)[1] == zero)
                        {
                            success = false;
                            break;
                        }
                    }
                if (success && BigIntegerExtensions.MillerRabinIsPrime(num)) break;
                if (num == nem)
                {
                    success = true;
                    var skip = DateTime.Now.Millisecond % 5;
                    switch (skip)
                    {
                        case 0:

                            num = num + BigInteger2.TWO();
                            nem = nem + BigInteger2.T50();
                            break;
                        case 1:
                            num = num + BigInteger2.FOUR();
                            nem = nem + BigInteger2.T100();
                            break;
                        case 2:
                            num = num + BigInteger2.T16();
                            nem = nem + BigInteger2.T100();
                            break;
                        case 3:
                            num = num + BigInteger2.T32();
                            nem = nem + BigInteger2.T100();
                            break;
                        case 4:
                            num = num + BigInteger2.T50();
                            nem = nem + BigInteger2.T100();
                            break;
                    }
                }
                else
                {
                    success = true;
                    num = num + BigInteger2.TWO();
                }
            }
            return num;
        }
        /*public static BigInteger2 modPower(BigInteger2 current, BigInteger2 power, BigInteger2 mod)
        {
            BigInteger2 Z = (BigInteger2)current.Clone();
            if (Z == new BigInteger2(1, 0)) return new BigInteger2(1, 0);
            if (Z == BigInteger2.ONE()) return ((BigInteger2)BigInteger2.ONE().Clone());
            BigInteger2 result = BigInteger2.ONE();
            for (int i = power.bitlength.Length - 1; i >= 2; i--)
            {
                if (power.bitlength[i])
                    result = (BigIntegerExtensions.DivideBy(BigIntegerExtensions.Multiply(result,Z),mod))[1];
                Z = (BigIntegerExtensions.DivideBy(BigIntegerExtensions.Multiply(Z, Z),mod))[1];
                GC.Collect();
            }
            BigInteger2 temp = BigInteger2.NegateZeros(result);
            result = null;
            GC.Collect();
            return temp;
        }*/
    }

}
