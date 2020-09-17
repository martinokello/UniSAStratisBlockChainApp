using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;

namespace RSA316Infinito.BigRsaCrypto
{
    /// <summary>
    /// Summary description for BigInteger2.
    /// </summary>
    public class BigInteger2 : ICloneable
    {
        public bool[] bitlength;
        public static int[] mySet = BigInteger2.getnormalPrimes(2048);
        public static int[] nonPrimes = BigInteger2.getNonePrimes(9);
        private static Random rand = new Random(DateTime.Now.Millisecond);

        public BigInteger2(int bits, int zero)//Constructor with zero value
        {
            bitlength = new bool[bits + 2];
        }

        public BigInteger2(int bits)
        {
            //
            // TODO: Add constructor logic here
            //
            bitlength = new bool[bits + 2];//2 extra bits: sign and carry

            bitlength[bits + 1] = true;//last bit for odd number set
            bitlength[2] = true;//first bit set

            //set random bits to initialize odd number
            int xfact = (int)(bitlength.Length - bitlength.Length / 4);
            if (xfact >= 3)
                for (int i = xfact; i < bitlength.Length - 1; i++)
                {
                    bool val = ((int)(rand.NextDouble() * 10 / 6) == 1) ? true : false;
                    bitlength[i] = val;
                }
        }


        public static BigInteger2 T100()
        {
            BigInteger2 t100 = new BigInteger2(1);
            t100.bitlength = ConvertToBinary(100, 7);
            return t100;
        }

        public static BigInteger2 T50()
        {
            BigInteger2 t50 = new BigInteger2(1);
            t50.bitlength = ConvertToBinary(50, 6);
            return t50;
        }
        public static BigInteger2 T32()
        {
            BigInteger2 T32 = new BigInteger2(1);
            T32.bitlength = ConvertToBinary(32, 6);
            return T32;
        }

        public static BigInteger2 T16()
        {
            BigInteger2 t16 = new BigInteger2(1);
            t16.bitlength = ConvertToBinary(16, 5);
            return t16;
        }

        public static BigInteger2 T8()
        {
            BigInteger2 t8 = new BigInteger2(1);
            t8.bitlength = ConvertToBinary(8, 4);
            return t8;
        }

        public static BigInteger2 T5()
        {
            BigInteger2 t5 = new BigInteger2(1);
            t5.bitlength = ConvertToBinary(5, 3);
            return t5;
        }
        public static BigInteger2 T7()
        {
            BigInteger2 t7 = new BigInteger2(1);
            t7.bitlength = ConvertToBinary(7, 4);
            return t7;
        }
        public static BigInteger2 T9()
        {
            BigInteger2 t9 = new BigInteger2(1);
            t9.bitlength = ConvertToBinary(9, 4);
            return t9;
        }
        public static BigInteger2 T11()
        {
            BigInteger2 t11 = new BigInteger2(4, 0);
            t11.bitlength = ConvertToBinary(11, 4);
            return t11;
        }
        public static BigInteger2 FOUR()
        {
            BigInteger2 four = new BigInteger2(3, 0);
            four.bitlength[2] = true;
            return four;
        }

        public static BigInteger2 THREE()
        {
            BigInteger2 three = new BigInteger2(2, 0);
            three.bitlength[2] = true;
            three.bitlength[3] = true;
            return three;
        }

        public static BigInteger2 TWO()
        {
            BigInteger2 two = new BigInteger2(2, 0);
            two.bitlength[2] = true;
            return two;
        }

        public static BigInteger2 ONE()
        {
            BigInteger2 one = new BigInteger2(1, 0);
            one.bitlength[2] = true;
            return one;
        }

        public static BigInteger2 Zero()
        {
            BigInteger2 zero = new BigInteger2(1, 0);
            return zero;

        }
        private static BigInteger2[] Compare(BigInteger2 Numb1, BigInteger2 Numb2)
        {
            //comp[0] is smaller; comp[1] is bigger
            BigInteger2[] comp = new BigInteger2[2];
            if (Numb1.bitlength.Length >= Numb2.bitlength.Length)
            {
                comp[0] = Numb2;
                comp[1] = Numb1;
            }
            else
            {
                comp[0] = Numb1;
                comp[1] = Numb2;
            }

            return comp;
        }
        private static bool[] getBoolAdd(bool one, bool two, bool carry)
        {
            //result of sum is index 0, and carry index 1
            bool[] res = { false, false };
            if (one && two && carry)
            {
                res[0] = true;
                res[1] = true;
                return res;
            }
            else if ((one && two && !carry) || (!one && two && carry) || (one && !two && carry))
            {
                res[0] = false;
                res[1] = true;
                return res;
            }
            else if ((one && !two && !carry) || (!one && two && !carry) || (!one && !two && carry))
            {
                res[0] = true;
                res[1] = false;
                return res;
            }
            else return res;
        }

        private static bool[] getBoolSub(bool one, bool two, bool borrow)
        {
            //result of subtraction is index 0, and borrow index 1
            bool[] res = { false, false };
            if (!borrow)
            {
                if (one && two)
                {
                    res[0] = false;
                    res[1] = false;
                    return res;
                }
                else if (one && !two)
                {
                    res[0] = true;
                    res[1] = false;
                    return res;
                }
                else if (!one && two)
                {
                    res[0] = true;
                    res[1] = true;
                    return res;
                }
                else return res;
            }
            else//borrow is set
            {
                if (!one && !two)
                {
                    res[0] = true;
                    res[1] = true;
                    return res;
                }
                else if (!one && two)
                {
                    res[0] = false;
                    res[1] = true;
                    return res;
                }
                else if (one && two)
                {
                    res[0] = true;
                    res[1] = true;
                    return res;
                }
                else return res;
            }
        }
        public static BigInteger2 operator +(BigInteger2 NumbA, BigInteger2 NumbB)
        {
            BigInteger2 Numb1 = (BigInteger2)NumbA.Clone();
            BigInteger2 Numb2 = (BigInteger2)NumbB.Clone();
            BigInteger2[] comp = Compare(Numb1, Numb2);
            BigInteger2 result = new BigInteger2(comp[1].bitlength.Length - 2, 0);
            if (Numb1.bitlength[0] && Numb2.bitlength[0])
            {
                Numb1.bitlength[0] = false;
                Numb2.bitlength[0] = false;
                BigInteger2 res = Numb1 + Numb2;
                res.bitlength[0] = true;
                if (res.bitlength[1])
                {
                    BigInteger2 finRes = new BigInteger2(res.bitlength.Length - 1, 0);
                    res.CopyFromIndex(finRes, 1);
                    res = finRes;
                }
                return res;
            }
            else if (Numb1.bitlength[0] && !Numb2.bitlength[0])
            {
                Numb1.bitlength[0] = false;
                BigInteger2 res = Numb2 - Numb1;
                if (res.bitlength[1])
                {
                    BigInteger2 finRes = new BigInteger2(res.bitlength.Length - 1, 0);
                    res.CopyFromIndex(finRes, 1);
                    res = finRes;
                }
                return res;
            }
            else if (!Numb1.bitlength[0] && Numb2.bitlength[0])
            {
                Numb2.bitlength[0] = false;
                BigInteger2 res = Numb1 - Numb2;
                if (res.bitlength[1])
                {
                    BigInteger2 finRes = new BigInteger2(res.bitlength.Length - 1, 0);
                    res.CopyFromIndex(finRes, 1);
                    res = finRes;
                }
                return res;
            }
            //Compute Sum: All numbers are positive
            int i = 0, j = 0;
            bool cry = false;

            //if bitlengths are equal This will solve
            for (i = comp[1].bitlength.Length - 1, j = comp[0].bitlength.Length - 1; j >= 1; i--, j--)
            {
                bool[] sum = BigInteger2.getBoolAdd(comp[1].bitlength[i], comp[0].bitlength[j], cry);
                result.bitlength[i] = sum[0];
                cry = sum[1];
            }
            //if bitlengths are unequal this will continue solving
            for (int rem = i; rem >= 1; rem--)
            {
                bool[] sum = null;
                sum = BigInteger2.getBoolAdd(comp[1].bitlength[rem], false, cry);
                result.bitlength[rem] = sum[0];
                cry = sum[1];
            }
            if (Numb1.bitlength[0] && Numb2.bitlength[0])
            {
                result.bitlength[0] = true;
            }

            comp = null;
            GC.Collect();
            if (result.bitlength[1])
            {
                BigInteger2 finRes = new BigInteger2(result.bitlength.Length - 1, 0);
                result.CopyFromIndex(finRes, 1);
                result = finRes;
            }
            return result;
        }

        public static BigInteger2 operator -(BigInteger2 NumbA, BigInteger2 NumbB)
        {
            BigInteger2 Numb1 = (BigInteger2)NumbA.Clone();
            BigInteger2 Numb2 = (BigInteger2)NumbB.Clone();
            BigInteger2[] comp = Compare(Numb1, Numb2);
            BigInteger2 result = new BigInteger2(comp[1].bitlength.Length - 2, 0);

            //Compute Difference

            if (Numb1.bitlength[0] && !Numb2.bitlength[0])
            {
                Numb1.bitlength[0] = false;
                result = Numb2 + Numb1;
                result.bitlength[0] = true;
            }
            else if (!Numb1.bitlength[0] && Numb2.bitlength[0])
            {
                Numb2.bitlength[0] = false;
                result = Numb1 + Numb2;
            }
            else if (Numb1.bitlength[0] && Numb2.bitlength[0])
            {
                Numb1.bitlength[0] = false;
                Numb2.bitlength[0] = false;
                result = Numb2 - Numb1;
            }
            else //Compute difference for both positive BigInteger2s
            {
                bool borrow = false;
                if (Numb1.bitlength.Length >= Numb2.bitlength.Length)
                {
                    int i, j;
                    for (i = Numb1.bitlength.Length - 1, j = Numb2.bitlength.Length - 1; j >= 2; i--, j--)
                    {
                        bool[] dif = BigInteger2.getBoolSub(Numb1.bitlength[i], Numb2.bitlength[j], borrow);
                        result.bitlength[i] = dif[0];
                        borrow = dif[1];
                    }
                    for (int n = i; n >= 1; n--)
                    {
                        bool[] dif = BigInteger2.getBoolSub(Numb1.bitlength[n], false, borrow);
                        result.bitlength[n] = dif[0];
                        borrow = dif[1];
                    }
                }
                else//Numb2 bit length is greater
                {
                    int i, j;
                    for (i = Numb2.bitlength.Length - 1, j = Numb1.bitlength.Length - 1; j >= 2; i--, j--)
                    {
                        bool[] dif = BigInteger2.getBoolSub(Numb1.bitlength[j], Numb2.bitlength[i], borrow);
                        result.bitlength[i] = dif[0];
                        borrow = dif[1];
                    }
                    for (int n = i; n >= 1; n--)
                    {
                        bool[] dif = BigInteger2.getBoolSub(false, Numb2.bitlength[n], borrow);
                        result.bitlength[n] = dif[0];
                        borrow = dif[1];
                    }
                }
            }

            if (result.bitlength[1])//if carry bit set: then set sign bit and reset carry!!
            {
                result = new BigInteger2(2, 0) - result;
                result.bitlength[0] = true;
            }

            comp = null;

            GC.Collect();
            return result;
        }

        public static BigInteger2 operator *(BigInteger2 Numb1, BigInteger2 Numb2)
        {
            BigInteger2 Numb1R = (BigInteger2)Numb1.Clone();
            BigInteger2 Numb2R = (BigInteger2)Numb2.Clone();

            BigInteger2[] comp = Compare(Numb1R, Numb2R);
            //Compute Product
            //result bit size is sum of bit length - 2 i.e. carry & sign bit
            int bits = comp[0].bitlength.Length + comp[1].bitlength.Length - 2 + 1;
            BigInteger2 partRes = new BigInteger2(bits - 2, 0);
            Numb1R.bitlength[0] = false;
            Numb2R.bitlength[0] = false;

            for (int i = comp[0].bitlength.Length - 1, count = 1; i >= 2; i--, count++)
            {
                if (comp[0].bitlength[i])
                {
                    if (count == 1)
                    {
                        BigInteger2 tmp = partRes;
                        partRes = partRes + comp[1];
                        tmp = null;
                    }
                    else
                    {
                        //fill part with comp[1] bits
                        BigInteger2 part = new BigInteger2(comp[1].bitlength.Length - 2 + count - 1, 0);
                        for (int pn = 2; pn < comp[1].bitlength.Length; pn++)
                            part.bitlength[pn] = comp[1].bitlength[pn];
                        BigInteger2 tmp = partRes;
                        partRes = partRes + part;
                        tmp = null;
                    }
                }
            }
            //test case:: if (Numb2.bitlength[0]) return (BigInteger2)BigInteger2.T50().Clone();


            comp = null;

            GC.Collect();
            partRes = BigInteger2.NegateZeros(partRes);
            if ((Numb1.bitlength[0] && !Numb2.bitlength[0]) || (!Numb1.bitlength[0] && Numb2.bitlength[0])) partRes.bitlength[0] = true;
            return partRes;
        }

        public static bool operator <(BigInteger2 Numb1, BigInteger2 Numb2)
        {
            Numb1 = BigInteger2.NegateZeros(Numb1);
            Numb2 = BigInteger2.NegateZeros(Numb2);

            return Numb2 > Numb1;
        }

        public static bool operator >(BigInteger2 Numb1, BigInteger2 Numb2)
        {
            Numb1 = BigInteger2.NegateZeros(Numb1);
            Numb2 = BigInteger2.NegateZeros(Numb2);

            if (Numb1.bitlength.Length > Numb2.bitlength.Length) return true;
            else if (Numb2.bitlength.Length > Numb1.bitlength.Length) return false;
            else
            {
                var result = false;
                var previousSets = false;
                //same length: compare bit wise:
                for (var n = 2; n < Numb1.bitlength.Length; n++)
                {
                    if (Numb2.bitlength[n])
                    {
                        previousSets = true;
                    }
                    if (Numb1.bitlength[n] && !Numb2.bitlength[n] && !previousSets)
                    {
                        result = true;
                        break;
                    }
                    else if (!Numb1.bitlength[n] && Numb2.bitlength[n])
                    {
                        result = false;
                        break;
                    }

                    if (Numb1.bitlength[n] && Numb2.bitlength[n] && previousSets)
                    {
                        previousSets = false;
                    }
                }

                return result;
            }
        }


        public static bool EqualsMagnitude(BigInteger2 NumbA, BigInteger2 NumbB)
        {
            bool test = true;

            BigInteger2 Numb1 = (BigInteger2)NumbA.Clone();
            BigInteger2 Numb2 = (BigInteger2)NumbB.Clone();

            BigInteger2[] comp = Compare(Numb1, Numb2);
            int bitdif = comp[1].bitlength.Length - comp[0].bitlength.Length;
            for (int i = comp[1].bitlength.Length - 1, j = comp[0].bitlength.Length - 1; i >= 1; i--, j--)
            {
                if (i >= bitdif + 1)
                {
                    if (comp[1].bitlength[i] != comp[0].bitlength[j])
                    {
                        test = false;
                        break;
                    }
                }
                else
                {
                    if (comp[1].bitlength[i])
                    {
                        test = false;
                        break;
                    }
                }

            }

            comp = null;
            return test;
        }
        //implementation requires subtraction of Numb2 a number of times n from Divisor
        //when value gets less than divisor; then n is the result.
        //:not a requirement for RSA
        public static BigInteger2[] operator /(BigInteger2 NumbA, BigInteger2 NumbB)
        {
            BigInteger2[] results = new BigInteger2[2];
            BigInteger2 Numb1 = (BigInteger2)NumbA.Clone();
            BigInteger2 Numb2 = (BigInteger2)NumbB.Clone();

            Numb1 = BigInteger2.NegateZeros(Numb1);
            Numb2 = BigInteger2.NegateZeros(Numb2);

            if (Numb1 == new BigInteger2(1, 0))
            {
                results[0] = new BigInteger2(1, 0);
                results[1] = Numb2;
                return results;
            }

            if (BigInteger2.EqualsMagnitude(Numb1, Numb2))
            {
                results[0] = (BigInteger2)BigInteger2.ONE().Clone();
                results[1] = new BigInteger2(1, 0);
                return results;
            }
            if (Numb1 > Numb2)
            {
                BigInteger2 PValue = new BigInteger2(2 * Numb1.bitlength.Length - 4, 0);
                BigInteger2 D = new BigInteger2(2 * Numb1.bitlength.Length - 4, 0);
                BigInteger2 divres = new BigInteger2(Numb1.bitlength.Length - 2, 0);
                BigInteger2 tmp = new BigInteger2(Numb1.bitlength.Length - 2, 0);
                //Set up the PValue and result values
                Numb1.CopyTo(PValue, Numb1.bitlength.Length);
                //Prepare D:
                if (Numb1.bitlength.Length > Numb2.bitlength.Length)
                    Numb2.CopyTo(tmp, Numb1.bitlength.Length - Numb2.bitlength.Length + 2);
                else if (Numb1.bitlength.Length == Numb2.bitlength.Length) Numb2.CopyTo(tmp, 2);
                tmp.CopyTo(D, 2);

                //Compute the result and mod values
                for (int i = 2; i < Numb1.bitlength.Length; i++)
                {
                    PValue.ShiftLeft(false);
                    PValue = PValue - D;
                    if (PValue.bitlength[0])
                    {
                        PValue = PValue + D;
                        divres.ShiftLeft(false);
                    }
                    else
                    {
                        divres.ShiftLeft(true);
                    }
                }

                results[0] = divres;
                BigInteger2 temp = new BigInteger2(Numb1.bitlength.Length - 2, 0);
                PValue.CopyTo(temp, 2);
                results[1] = temp;
            }
            else
            {
                results[0] = new BigInteger2(1, 0);
                results[1] = Numb1;
            }

            //return values set
            results[0] = BigInteger2.NegateZeros(results[0]);
            results[1] = BigInteger2.NegateZeros(results[1]);
            return results;
        }

        public void CopyTo(BigInteger2 dest, int indexbeg)
        {
            for (int i = 2, j = indexbeg; i < this.bitlength.Length && j < dest.bitlength.Length; i++, j++)
                dest.bitlength[j] = this.bitlength[i];
        }

        public void CopyFromIndex(BigInteger2 dest, int indexbeg)
        {
            for (int i = indexbeg, j = 2; i < this.bitlength.Length && j < dest.bitlength.Length; i++, j++)
                dest.bitlength[j] = this.bitlength[i];
        }
        public static void CopyFromBitArrayFromIndex(BitArray origin, BigInteger2 dest, int indexbeg)
        {
            for (int i = indexbeg, j = 2; i < origin.Length && j < dest.bitlength.Length; i++, j++)
                dest.bitlength[j] = origin[i];
        }
        public static BitArray BigIntegerToBitArrayNoShrinkSize(BigInteger2 Number)
        {
            bool[] tmp = Number.bitlength;

            BitArray ary = new BitArray(Number.bitlength.Length - 2, false);

            for (int i = 2, j = 0; i < tmp.Length; i++, j++)
            {
                ary[j] = tmp[i];
            }
            Console.WriteLine("BitArryToString: " + BitArrayToString(ary));
            return ary;
        }
        public BigInteger2 ShiftLeft(bool value)
        {
            int length = this.bitlength.Length;
            for (int i = 3; i < length; i++)
            {
                this.bitlength[i - 1] = this.bitlength[i];
            }
            this.bitlength[length - 1] = value;
            return this;
        }
        public BigInteger2 ShiftLeftToFill(int size, ref int numberOfShifts)
        {
            var result = new BigInteger2(size, 0);
            numberOfShifts = result.bitlength.Length - this.bitlength.Length;
            this.CopyFromIndex(result, (this.bitlength.Length - 2));
            return result;
        }
        public static bool operator !=(BigInteger2 Numb1, BigInteger2 Numb2)
        {
            return !(Numb1 == Numb2);
        }

        public override bool Equals(object obj)
        {
            return this == ((BigInteger2)obj);
        }

        public static bool operator ==(BigInteger2 Numb1, BigInteger2 Numb2)
        {
            bool test = true;
            BigInteger2[] comp = Compare(Numb1, Numb2);
            int bitdif = comp[1].bitlength.Length - comp[0].bitlength.Length;
            if (Numb1.bitlength[0] != Numb2.bitlength[0]) return false;
            for (int i = comp[1].bitlength.Length - 1, j = comp[0].bitlength.Length - 1; i >= 1; i--, j--)
            {
                if (i >= bitdif + 1)
                {
                    if (comp[1].bitlength[i] != comp[0].bitlength[j])
                    {
                        test = false;
                        break;
                    }
                }
                else
                {
                    if (comp[1].bitlength[i])
                    {
                        test = false;
                        break;
                    }
                }

            }

            comp = null;
            return test;
        }

        public BigInteger2 modPower(BigInteger2 power, BigInteger2 mod)
        {
            BigInteger2 Z = (BigInteger2)this.Clone();
            if (Z == new BigInteger2(1, 0)) return new BigInteger2(1, 0);
            if (Z == BigInteger2.ONE()) return ((BigInteger2)BigInteger2.ONE().Clone());
            BigInteger2 result = BigInteger2.ONE();
            for (int i = power.bitlength.Length - 1; i >= 2; i--)
            {
                if (power.bitlength[i])
                    result = ((result * Z) / mod)[1];
                Z = ((Z * Z) / mod)[1];
                GC.Collect();
            }
            BigInteger2 temp = BigInteger2.NegateZeros(result);
            result = null;
            GC.Collect();
            return temp;
        }

        public BigInteger2 gcd(BigInteger2 NumbA, BigInteger2 NumbB)
        {
            BigInteger2 Numb1 = (BigInteger2)BigInteger2.NegateZeros(NumbA).Clone();
            BigInteger2 Numb2 = (BigInteger2)BigInteger2.NegateZeros(NumbB).Clone();
            BigInteger2 zero = new BigInteger2(1, 0);//3 bit: 1 sign, 1 carry; 1 zero bit;
            BigInteger2 temp = null;
            while (true)
            {
                Numb1 = (BigInteger2)BigInteger2.NegateZeros(Numb1).Clone();
                Numb2 = (BigInteger2)BigInteger2.NegateZeros(Numb2).Clone();
                if (Numb2 != zero)
                {
                    if (Numb2 < Numb1)
                    {
                        temp = Numb2;
                        Numb2 = (Numb1 / Numb2)[1];
                        Numb1 = temp;
                    }
                    else Numb2 = (Numb2 / Numb1)[1];
                }
                else break;
            }
            GC.Collect();
            return Numb1;
        }

        public BigInteger2 inverseMod(BigInteger2 numbe, BigInteger2 modf)
        {
            //Check to see that gcd is 1 before proceeding: gcd(numbe,modf)=1
            ArrayList factors2 = new ArrayList();
            int count = 0;
            BigInteger2 NUM = (BigInteger2)numbe.Clone();
            BigInteger2 MOD = (BigInteger2)modf.Clone();
            bool foundInverse = true;

            while (true)
            {
                BigInteger2[] temp = modf / numbe;

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
                    resCurr.fnumb = BigInteger2.NegateZeros(x.fnumb * prevx.fnumb);
                    resCurr.fMod = BigInteger2.NegateZeros(x.fnumb * prevx.fMod);

                    tmpResolve.fnumb = x.fMod + resCurr.fnumb;
                    tmpResolve.fMod = resCurr.fMod;
                    factors2.Add(tmpResolve);
                }
                if (count > 2)
                {
                    Factors2 prev2Back = (Factors2)factors2[factors2.Count - 2];
                    Factors2 prevBack = (Factors2)factors2[factors2.Count - 1];
                    Factors2 resCurr = new Factors2();
                    resCurr.fnumb = BigInteger2.NegateZeros(x.fnumb * prevBack.fnumb);
                    resCurr.fMod = BigInteger2.NegateZeros(x.fnumb * prevBack.fMod);

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
            if (!invres.bitlength[0]) return (invres / MOD)[1];//if invres is positive
            else // invres is negative
            {
                invres.bitlength[0] = false;
                if (invres < MOD) return NegateZeros(MOD - invres);
                else
                {
                    BigInteger2 test = (invres / MOD)[0] + BigInteger2.ONE();
                    return NegateZeros((test * MOD) - MOD);
                }
            }

        }

        //MillerRabinTest
        public bool MillerRabinIsPrime(BigInteger2 numb)
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
            BigInteger2 d = (numbLess1 / twoPowerS)[0];

            //Use testSuite to eliminate Composites:
            bool isPrime = true;
            foreach (BigInteger2 integer in testSuite)
            {
                bool prime = false;
                BigInteger2 result = integer.modPower(d, numb);
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
                        result = result.modPower(BigInteger2.TWO(), numb);
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

        //Ferment's Test
        public bool FermatWitness(BigInteger2 numb)
        {
            //Stricter Ferment Witness Tests:
            BigInteger2 power = numb - BigInteger2.ONE();
            bool passed = true;

            int[] testSuite = getnormalPrimes(10);

            for (int i = 3; i < testSuite.Length; i++)
            {
                BigInteger2 tester = new BigInteger2(1, 0);
                bool[] tmp = BigInteger2.ConvertToBinary(testSuite[i], 9);
                tester.bitlength = tmp;
                tester = BigInteger2.NegateZeros(tester);
                if (BigInteger2.ONE() != tester.modPower(power, numb))
                {
                    passed = false;
                    break;
                }
            }
            return passed;
        }

        //Sqareroot of BigInteger2
        public static BigInteger2 squareRoot(BigInteger2 numb)
        {
            BigInteger2 a = (BigInteger2)numb.Clone();
            BigInteger2 result = (BigInteger2)BigInteger2.ONE().Clone();
            BigInteger2 converge = (BigInteger2)BigInteger2.TWO().Clone();

            while (true)
            {
                result = ((result + ((a / result)[0])) / BigInteger2.TWO())[0];
                if (converge == result) break;
                converge = (BigInteger2)result.Clone();
            }
            return result;
        }


        public BigInteger2 power(int n)
        {
            BigInteger2 res = this;

            for (int i = 1; i < n; i++)
            {
                res = res * this;
                GC.Collect();
            }
            return res;
        }

        public static Int64 ConvertToInt64(BitArray ary)
        {
            const int two = 2;
            Int64 result = 0;
            for (int i = ary.Length - 1, j = 0; i >= 0; i--, j++)
            {
                if (ary[j])
                    result += (Int64)Math.Pow(two, i);
            }
            return result;
        }

        public static string BitArrayToString(BitArray res)
        {
            StringBuilder sb = new StringBuilder();
            foreach (bool b in res)
                sb.Append(b ? "1" : "0");

            return sb.ToString();
        }
        public static BitArray BigInteger2ToBitArray(BigInteger2 Num)
        {
            BigInteger2 Number = BigInteger2.NegateZeros((BigInteger2)Num.Clone());
            bool[] tmp = Number.bitlength;

            BitArray ary = new BitArray(Number.bitlength.Length - 2, false);

            for (int i = 2, j = 0; i < tmp.Length; i++, j++)
            {
                ary[j] = tmp[i];
            }
            Console.WriteLine("BitArryToString: " + BitArrayToString(ary));
            return ary;
        }

        public static BitArray BigInteger2ToBitArrayNoShrinkSize(BigInteger2 Number)
        {
            bool[] tmp = Number.bitlength;

            BitArray ary = new BitArray(Number.bitlength.Length - 2, false);

            for (int i = 2, j = 0; i < tmp.Length; i++, j++)
            {
                ary[j] = tmp[i];
            }
            Console.WriteLine("BitArryToString: " + BitArrayToString(ary));
            return ary;
        }

        public static bool[] ConvertToBinary(int Num, int bitsize)
        {
            bool[] res = new bool[bitsize + 2];

            //populate with Num bits
            for (int i = res.Length - 1, j = 1; i >= 1; i--, j++)
            {
                int twopower = (int)Math.Pow(2, i - 1);
                int whole = Num / twopower;
                int rem = Num % twopower;
                res[j] = whole == 1 ? true : false;
                Num = rem;
            }
            return res;
        }
        public static string ConvertToString(bool[] array)
        {
            string s = "";
            for (int i = 0; i < array.Length; i++)
            {
                s += (array[i] ? "1" : "0");
            }
            return s;
        }
        public static string ConvertToString(BitArray array)
        {
            string s = "";
            for (int i = 0; i < array.Count; i++)
            {
                s += (array[i] ? "1" : "0");
            }
            return s;
        }
        public static BigInteger2 NegateZeros(BigInteger2 num)
        {
            GC.Collect();
            int nzero = 0, i = 0;
            for (i = 2; i < num.bitlength.Length; i++)
            {
                if (num.bitlength[i]) break;
                nzero++;
            }
            if (i >= num.bitlength.Length)
            {
                return (BigInteger2)BigInteger2.Zero().Clone();
            }
            else if (i > 2)
            {
                BigInteger2 res = new BigInteger2(num.bitlength.Length - nzero - 2, 0);
                for (int j = 2; j < res.bitlength.Length; j++, i++)
                {
                    res.bitlength[j] = num.bitlength[i];
                }
                return res;
            }
            else return num;
        }

        //Used to Store BigInteger2 as byte array with padding of Zeros at the beginning
        public static byte[] getKeyBytes(BigInteger2 Bgint)
        {
            //Create and fill in bitarray with BigInteger2 values ignoring sign & carry bit.
            int rembits = (Bgint.bitlength.Length - 1) % 8;
            int numOfbytes = (Bgint.bitlength.Length - 1) / 8;
            int startRem = 8 - rembits;
            BitArray bitarray;
            byte[] mybytes;

            if (rembits > 0)
            {
                //MessageBox.Show("I am in Remainder");
                bitarray = new BitArray(numOfbytes * 8 + 8, false);
                mybytes = new byte[numOfbytes + 1];
                int i, j;
                for (i = 1, j = startRem; i < Bgint.bitlength.Length; i++, j++)
                    bitarray[j] = Bgint.bitlength[i];
            }
            else
            {
                //MessageBox.Show("I am not in Remainder");
                bitarray = new BitArray(numOfbytes * 8, false);
                mybytes = new byte[numOfbytes];
                for (int i = 1, j = 0; i < Bgint.bitlength.Length; i++, j++)
                    bitarray[j] = Bgint.bitlength[i];
            }

            //Create byte array with bitarray
            bitarray.CopyTo(mybytes, 0);
            GC.Collect();
            return mybytes;
        }
        //Used to Store BigInteger2 as byte array with padding of Zeros on the last byte
        public static byte[] getBytes(BigInteger2 Bgint)
        {
            //Create and fill in bitarray with BigInteger2 values ignoring sign & carry bit.
            int rembits = (Bgint.bitlength.Length - 2) % 8;
            int numOfbytes = (Bgint.bitlength.Length - 2) / 8;
            int startRem = 8 - rembits;
            BitArray bitarray;
            byte[] mybytes;

            if (rembits > 0)
            {
                //MessageBox.Show("I am in Remainder");
                bitarray = new BitArray(numOfbytes * 8 + 8, false);
                mybytes = new byte[numOfbytes + 1];
                int i, j;
                for (i = 2, j = 0; i < (numOfbytes * 8 + 2); i++, j++)
                    bitarray[j] = Bgint.bitlength[i];
                //remaining bits: last byte:
                for (int n = i, index = startRem; n < Bgint.bitlength.Length; n++, index++)
                {
                    bitarray[index] = Bgint.bitlength[n];
                }
            }
            else
            {
                //MessageBox.Show("I am not in Remainder");
                bitarray = new BitArray(numOfbytes * 8, false);
                mybytes = new byte[numOfbytes];
                for (int i = 2, j = 0; i < Bgint.bitlength.Length; i++, j++)
                    bitarray[j] = Bgint.bitlength[i];
            }

            //Create byte array with bitarray
            bitarray.CopyTo(mybytes, 0);
            GC.Collect();
            return mybytes;
        }
        public static byte[] GetBytesTwosCompliment(BigInteger2 Bgint)
        {
            //Create and fill in bitarray with BigInteger2 values not ignoring carry bit.
            int rembits = (Bgint.bitlength.Length - 2) % 8;
            int numOfbytes = (Bgint.bitlength.Length - 2) / 8;
            int startRem = 8 - rembits;
            BitArray bitarray;
            byte[] mybytes;

            if (rembits > 0)
            {
                //MessageBox.Show("I am in Remainder");
                bitarray = new BitArray(numOfbytes * 8 + 16, false);
                mybytes = new byte[numOfbytes + 2];
                int i, j;
                for (i = Bgint.bitlength.Length - 1, j = bitarray.Length - 1; i >=2 ; i--, j--)
                    bitarray[j] = Bgint.bitlength[i];
                //remaining bits: last byte:
                for (int n = i, index = startRem; n >=2; n--, index--)
                {
                    bitarray[index] = Bgint.bitlength[n];
                }
            }
            else
            {
                //MessageBox.Show("I am not in Remainder");
                bitarray = new BitArray(numOfbytes * 8 + 8, false);
                mybytes = new byte[numOfbytes + 1];
                int i, j;
                for (i = Bgint.bitlength.Length - 1, j = bitarray.Length - 1; i >= 2; i--, j--)
                    bitarray[j] = Bgint.bitlength[i];
            }
            //Create byte array with bitarray
            bitarray = BigInteger2.ReverseBitArray(bitarray);
            bitarray.CopyTo(mybytes, 0);
            GC.Collect();
            return mybytes;
        }
        public static BigInteger2 ToBigInteger2(byte[] array)
        {
            BitArray bitarray = new BitArray(array);
            int length = bitarray.Length;
            BigInteger2 bgint = new BigInteger2(length, 0);

            //fill the bgint with consecutive bits from index 2 bit ignoring sign, and carry bits
            for (int i = 0; i < bitarray.Length; i++)
                bgint.bitlength[i + 2] = bitarray[i];
            GC.Collect();
            return bgint;
        }
        public static bool[] ConvertToBitArray(int[] temp)
        {
            bool[] bits = new bool[temp.Length + 2];
            for (int i = 0; i < temp.Length; i++)
            {
                bool boolVal = (temp[i] == 0 ? false : true);
                bits[i + 2] = boolVal;
            }
            GC.Collect();
            return bits;
        }
        public static BitArray ReverseBigIntegerBytesToBitArray(byte[] contents) {

            BitArray bitAry = new BitArray(contents);
            byte[] tmpBytes = new byte[contents.Length];
            int i, n;
            for (i = 0, n = contents.Length - 1; n >= 0; n--, i++)
            {
                tmpBytes[i] = contents[n];
            }
            return new BitArray(tmpBytes);
        }

        public static BitArray ReverseBigIntegerBitsToBitArray(byte[] contents)
        {

            BitArray bitAry = new BitArray(contents);
            BitArray tmp = new BitArray(bitAry.Length, false);

            int i, n;
            for (i = 0, n = contents.Length * 8 - 1; n >= 0; n--, i++)
            {
                tmp[i] = bitAry[n];
            }
            return new BitArray(tmp);
        }
        public static BitArray ReverseBitArray(BitArray bits)
        {
            BitArray tmp = new BitArray(bits.Length, false);
            int i, n;
            for (i = 0, n = bits.Length - 1; n >= 0; i++, n--)
            {
                tmp[i] = bits[n];
            }
            return tmp;
        }

        public static byte[] ReverseBigIntegerBytes(byte[] contents){

            byte[] tmpBytes = new byte[contents.Length];
            int i, n;
            for (i = 0, n = contents.Length - 1; n >= 0;n--, i++ ){
                tmpBytes[i] = contents[n];
            }
            BitArray bitAryTmp = new BitArray(tmpBytes);
            bitAryTmp = BigInteger2.ReverseBitArray(bitAryTmp);
            var bInt2 = new BigInteger2(tmpBytes.Length * 8,0);
            BigInteger2.CopyFromBitArrayFromIndex(bitAryTmp, bInt2, 0);
            return BigInteger2.getBytes(bInt2);
        }
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
                Console.WriteLine(ni);
                if (num < nem || num == nem)
                    foreach (int i in mySet)
                    {
                        bool[] val = ConvertToBinary(i, 11);
                        bool[] t = new bool[val.Length + 2];
                        for (int x = 0; x < val.Length; x++)
                        {
                            t[x + 2] = (val[x] ? true : false);
                        }
                        temp.bitlength = t;
                        if ((num / temp)[1] == zero)
                        {
                            success = false;
                            break;
                        }
                    }
                if (success && num.MillerRabinIsPrime(num)) break;
                if (num == nem)
                {
                    success = true;
                    num = num + BigInteger2.T50();
                    nem = nem + BigInteger2.T100();
                }
                else
                {
                    success = true;
                    num = num + BigInteger2.TWO();
                }
            }
            return num;
        }
        /*
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
                Console.WriteLine(ni);
                if (num < nem || num == nem)
                    foreach (int i in mySet)
                    {
                        int[] val = ConvertToBinary(i, 10);
                        bool[] t = new bool[val.Length + 2];
                        for (int x = 0; x < val.Length; x++)
                        {
                            t[x + 2] = (val[x] == 1 ? true : false);
                        }
                        temp.bitlength = t;
                        if ((num / temp)[1] == zero)
                        {
                            success = false;
                            break;
                        }
                    }
                if (success && num.IsPrimeNumber(num)) break;
                if (num == nem)
                {
                    success = true;
                    num = num + BigInteger2.T50();
                    nem = nem + BigInteger2.T100();
                }
                else
                {
                    success = true;
                    num = num + BigInteger2.TWO();
                }
            }
            return num;
        }*/

        public static int[] getnormalPrimes(int upto)
        {
            ArrayList smPrime = new ArrayList();
            for (int j = 3; j <= upto; j++)
            {
                if (isSmallPrime(j, j - 1))
                    smPrime.Add(j);
            }
            int[] smPAry = new int[smPrime.Count];
            for (int i = 0; i < smPAry.Length; i++)
            {
                smPAry[i] = (int)smPrime[i];
            }
            return smPAry;
        }
        public static int[] getNonePrimes(int upto)
        {
            List<int> noPrimes = new List<int>();
            bool gotNonPrime = true;

            for (int i = 2; i <= upto + 2; i++)
            {
                foreach (int prime in mySet)
                {
                    if (prime == i)
                    {
                        gotNonPrime = false;
                        break;
                    }
                }
                if (gotNonPrime) noPrimes.Add(i);
                gotNonPrime = true;
            }

            return noPrimes.ToArray();
        }
        public static bool isSmallPrime(Int64 num, Int64 divisor)
        {
            Int64 res = num % divisor;
            if (divisor == 1) return true;
            else if (res == 0) return false;
            else return isSmallPrime(num, divisor - 1);
        }
        public static BigInteger2 InvertBits(BigInteger2 numb)
        {
            for(var n=2; n< numb.bitlength.Length; n++)
            {
                numb.bitlength[n] = !numb.bitlength[n];
            }
            return numb;
        }

        public static bool AllBitsSetTwosComplimentMeansZero(BigInteger2 numb)
        {
            var allSet = true;

            for(var n=2;n < numb.bitlength.Length; n++)
            {
                if (!numb.bitlength[n])
                {
                    allSet = false;
                    break;
                }
            }
            return allSet;
        }
        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            foreach (bool b in this.bitlength)
            {
                str.Append(b ? "1" : "0");
            }
            return str.ToString();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #region ICloneable Members

        public object Clone()
        {
            // TODO:  Add BigInteger2.Clone implementation
            BigInteger2 temp = new BigInteger2(this.bitlength.Length - 2, 0);
            for (int i = 0; i < this.bitlength.Length; i++)
                temp.bitlength[i] = this.bitlength[i];
            return temp;
        }

        #endregion

    }
}
