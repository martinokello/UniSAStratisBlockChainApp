
using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RSA316Infinito.BigRsaCrypto
{
    /// <summary>
    /// Summary description for EncryptDecrypt.
    /// Used for 897 bit RSA Cryptography!!
    /// </summary>
    public class EncryptDecrypt
    {
        /** Creates a new instance of EncryptDecrypt */
        public BigInteger2 Mod = null;
        public BigInteger2 e = null;
        public BigInteger2 d = null;
        public BigInteger2 p = null, q = null;
        private int n;
        private const int num1 = 107;
        private const int num2 = 209;
        private int[] _oddsContainer = { 3, 5, 7 };
        public bool keyset;
        BigInteger2[] mess;



        public EncryptDecrypt()
        {
            keyset = false;
        }

        public BigInteger2 SetMod
        {
            get { return Mod; }
            set { Mod = value; }
        }

        public BigInteger2 SetE
        {
            get { return e; }
            set { e = value; }
        }

        public BigInteger2 SetD
        {
            get { return d; }
            set { d = value; }
        }
        public int SetN
        {
            set { n = value; }
            get { return n; }
        }
        public byte[] Encrypt(String str)
        {
            BigInteger2[] message = this.splitMessage(Encoding.UTF8.GetBytes(str), this.n - 1, "encrypt");
            mess = new BigInteger2[message.Length];
            mess = message;
            //MessageBox.Show("Message Length: " + message.Length);
            //return data;
            ArrayList resultSet = new ArrayList();

            GC.Collect();
            //Encrypt each Message and populate resultSet
            for (int i = 0; i < message.Length; i++)
            {

                BigInteger2 unitResult = BigIntegerExtensions.modPower(message[i], e, Mod);
                unitResult = BigInteger2.NegateZeros(unitResult);

                if (i == 0 && unitResult.bitlength.Length - 2 < n)
                {
                    //MessageBox.Show("Padding with 'a'");
                    int ind = n + 4 - unitResult.bitlength.Length;
                    BigInteger2 tmp = new BigInteger2(n, 0);

                    for (int xi = ind, xj = 2; xj < unitResult.bitlength.Length; xi++, xj++)
                    {
                        tmp.bitlength[xi] = unitResult.bitlength[xj];
                    }
                    unitResult = tmp;
                    //PAD Left with byte 'a'
                    byte[] pad = ASCIIEncoding.ASCII.GetBytes("a");
                    BitArray padAry = new BitArray(pad);

                    //PAD left of unitResult with padAry
                    BigInteger2 padded = new BigInteger2(unitResult.bitlength.Length - 2 + padAry.Length, 0);
                    int pi, pj;
                    for (pi = 2, pj = 0; pj < padAry.Length; pi++, pj++)
                    {
                        padded.bitlength[pi] = padAry[pj];
                    }

                    //Continue filling from unitResult
                    for (int indi = pi, indj = 2; indi < padded.bitlength.Length; indi++, indj++)
                    {
                        padded.bitlength[indi] = unitResult.bitlength[indj];
                    }
                    unitResult = padded;
                }
                if (i > 0 && unitResult.bitlength.Length - 2 < n)
                {
                    //MessageBox.Show("Padding with zeros");
                    //pad result's leftside with zeros
                    int ind = n + 4 - unitResult.bitlength.Length;
                    BigInteger2 tmp = new BigInteger2(n, 0);
                    for (int xi = ind, xj = 2; xj < unitResult.bitlength.Length; xi++, xj++)
                    {
                        tmp.bitlength[xi] = unitResult.bitlength[xj];
                    }

                    unitResult = null;
                    unitResult = tmp;
                }

                GC.Collect();
                resultSet.Add(unitResult);

            }

            //Create Full BigInteger2 containing bits from resultSet BigIntegers

            int bitSize = 0;
            foreach (BigInteger2 b in resultSet)
            {
                bitSize += (b.bitlength.Length - 2);
            }

            BigInteger2 wholeMessage = new BigInteger2(bitSize, 0);

            //Fill wholeMessage
            int curIndex = 2;
            foreach (BigInteger2 b in resultSet)
            {
                for (int i = 2; i < b.bitlength.Length; i++, curIndex++)
                {
                    wholeMessage.bitlength[curIndex] = b.bitlength[i];
                }
            }

            byte[] result = BigInteger2.getKeyBytes(wholeMessage);
            GC.Collect();
            // MessageBox.Show("Encryption Completed!!!");
            return result;
        }
        public byte[] Decrypt(String str)
        {
            ArrayList resultSet = new ArrayList();
            BigInteger2[] message = splitMessage(Convert.FromBase64String(str), n, "decrypt");

            GC.Collect();
            for (int i = 0; i < message.Length; i++)
            {
                //message[i] = BigInteger2.NegateZeros(message[i]);
                BigInteger2 unitResult = BigIntegerExtensions.modPower(message[i], d, Mod);
                unitResult = BigInteger2.NegateZeros(unitResult);
                BigInteger2 result = new BigInteger2(unitResult.bitlength.Length - 3, 0);
                for (int x = 3, xj = 2; x < unitResult.bitlength.Length; x++, xj++)
                {
                    result.bitlength[xj] = unitResult.bitlength[x];
                }
                unitResult = result;
                resultSet.Add(unitResult);
                GC.Collect();
            }

            //Create Full BigInteger2 containing bits from resultSet BigIntegers

            int bitSize = 0;
            foreach (BigInteger2 b in resultSet)
            {
                bitSize += (b.bitlength.Length - 2);
            }
            //MessageBox.Show("Whole decrypted Text length: " + bitSize);
            BigInteger2 wholeMessage = new BigInteger2(bitSize, 0);

            //Fill wholeMessage
            int curIndex = 2;
            foreach (BigInteger2 b in resultSet)
            {
                for (int i = 2; i < b.bitlength.Length; i++, curIndex++)
                {
                    wholeMessage.bitlength[curIndex] = b.bitlength[i];
                }
            }
            byte[] res = DeInject(wholeMessage);
            GC.Collect();
            // MessageBox.Show("Decryption Completed!!!");
            return res;
        }

        public byte[] getPublicKey()
        {
            e = BigInteger2.NegateZeros(e);
            return BigInteger2.getKeyBytes(e);
        }

        public void setPublicKey(byte[] key)
        {
            BitArray bitArray = new BitArray(key);
            e = new BigInteger2(bitArray.Length, 0);
            BigInteger2.CopyFromBitArrayFromIndex(bitArray, e, 0);
            e = BigInteger2.NegateZeros(e);

        }
        public void setPrivateKey(byte[] key)
        {
            BitArray bitArray = new BitArray(key);
            d = new BigInteger2(bitArray.Length, 0);
            BigInteger2.CopyFromBitArrayFromIndex(bitArray, d, 0);
            d = BigInteger2.NegateZeros(d);
        }

        public byte[] getPrivateKey()
        {
            d = BigInteger2.NegateZeros(d);
            return BigInteger2.getKeyBytes(d);
        }

        public bool IsKeySet()
        {
            return keyset;
        }

        public byte[] getModValue()
        {
            return BigInteger2.getKeyBytes(Mod);
        }

        public void setModValue(byte[] value)
        {
            BitArray bitArray = new BitArray(value);
            Mod = new BigInteger2(bitArray.Length, 0);
            BigInteger2.CopyFromBitArrayFromIndex(bitArray, Mod, 0);
            Mod = BigInteger2.NegateZeros(Mod);
            n = Mod.bitlength.Length - 2;
            //MessageBox.Show(""+(Mod.bitlength.Length - 2));
        }


        public void generateNewKey()
        {
            var testMessage = "[Administrator] wrote: ==>I am fed up of taking tests where you sore highly yet fail to progress to the next rounds of interviews. Given to abnormalities stated as team fit or cultural fit." + System.Environment.NewLine + " How can you say such crap, when you've never seen me sitted in your teams!!. Please visit and support the secure cryptology aspects of the ethical world, without the likes of Secret Service tampering or peeking through your mail: https://martinlayooinc.co.uk is therefore open to do business with you." + System.Environment.NewLine +
            "So I have taken up my projects under my own company umbrella, and winning: https://www.martinlayooinc.co.uk/Home/Product?prodId=checkMutableUber" + System.Environment.NewLine +
            " If in doubt of the buy and immediate download procedures or unsure whether you are spending money on value, then check out free samples and engage in the buying process for 0.01 - a penny.How can you discredit that.https://www.martinlayooinc.co.uk/Home/Product?prodId=checkAES" + System.Environment.NewLine +
            " Feel free to test drive my apps.Yes, to survive I am running my own business as I am fed up of spurious claims from employers amidst interviewing processes viewable link below this";

            var testMessage2 = "If in doubt of the buy and immediate download procedures or unsure whether you are spending money on value, then check out free samples and engage in the buying process for 0.01 - a penny.How can you discredit that.https://www.martinlayooinc.co.uk/Home/Product?prodId=checkAES";
            MessageBox.Show("RSA Public & Private Key Pair For Signing Is About to Get Generated. Please Click Ok, and Wait...\nYou will be notified when Keys are generated to Continue Process.");
            DateTime startTime = DateTime.Now;
            var tasks = new List<Task>();
            Task tp = new TaskFactory().StartNew(() =>
            {

                p = BigIntegerExtensions.getProbablePrime(num2);//should be a prime number
                p = BigInteger2.NegateZeros(p);
                GC.Collect();
            });
            tasks.Add(tp);
            //MessageBox.Show("P Got:");

            Task tq = new TaskFactory().StartNew(() =>
            {

                q = BigIntegerExtensions.getProbablePrime(num1);//should be a prime number
                q = BigInteger2.NegateZeros(q);
                //MessageBox.Show("p and q determined!!");
                GC.Collect();
            });
            tasks.Add(tq);
            Task.WaitAll(tasks.ToArray());

            Task tm = new TaskFactory().StartNew(() =>
            {
                while (true)
                {
                    try
                    {
                        while (p.gcd(p, q) != BigInteger2.ONE())
                        {
                            q = BigIntegerExtensions.getProbablePrime(num1);//should be a prime number
                            q = BigInteger2.NegateZeros(q);
                            //MessageBox.Show("p and q determined!!");
                            GC.Collect();
                        }
                        Mod = p * q;

                        //MessageBox.Show("p Is:");
                        //MessageBox.Show(p.ToString());

                        //MessageBox.Show("q Is:");
                        //MessageBox.Show(q.ToString());
                        Mod = BigInteger2.NegateZeros(Mod);
                        //MessageBox.Show("Mod Is:");
                        //MessageBox.Show(Mod.ToString());
                        n = Mod.bitlength.Length - 2;
                        GC.Collect();
                        BigInteger2 px = p - BigInteger2.ONE();
                        BigInteger2 qx = q - BigInteger2.ONE();
                        BigInteger2 f = px * qx;
                        f = BigInteger2.NegateZeros(f);

                        GC.Collect();

                        while (true)
                        {
                            e = BigIntegerExtensions.getProbablePrime(n - 1);
                            e = BigInteger2.NegateZeros(e);
                            d = e.inverseMod(e, f);
                            if (d == new BigInteger2(1, 0)) continue;
                            d = BigInteger2.NegateZeros(d);
                            break;
                        }

                        //MessageBox.Show("Found decryption Key");

                        //MessageBox.Show("Encryption Key Got:");
                        // MessageBox.Show(e.ToString());

                        //MessageBox.Show("Decryption Key Got:");
                        //MessageBox.Show(d.ToString());

                        GC.Collect();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Exception:" + ex.Message);
                    }
                    try
                    {
                        var resultBytes = Encrypt(testMessage);
                        var encryptedString = Convert.ToBase64String(resultBytes);
                        var decryptBytes = Decrypt(encryptedString);
                        var decryptedString = ASCIIEncoding.UTF8.GetString(decryptBytes);
                        var isKeys = false;

                        if (decryptedString.Equals(testMessage))
                        {
                            isKeys = true;
                        }
                        else { isKeys = false; }
                        if (!isKeys) continue;

                        resultBytes = Encrypt(testMessage2);
                        encryptedString = Convert.ToBase64String(resultBytes);
                        decryptBytes = Decrypt(encryptedString);
                        decryptedString = ASCIIEncoding.UTF8.GetString(decryptBytes);

                        if (decryptedString.Equals(testMessage2))
                        {
                            DateTime endTime = DateTime.Now;
                            TimeSpan tspan = endTime - startTime;
                            MessageBox.Show(String.Format("Keys Set!! Key generation Time elapsed=>{0} Hours: {1} Minutes: {2} Seconds", tspan.Hours, tspan.Minutes, tspan.Seconds));

                            break;
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message + System.Environment.NewLine + e.StackTrace);
                    }
                }
            });
            Task.WaitAny(tm);
        }
        public BigInteger2[] splitMessage(byte[] wholeMesg, int mesgCellBits, string direction)
        {
            BigInteger2[] result = null;

            if (direction.Equals("decrypt"))
            {
                BitArray fullMesgArry = new BitArray(wholeMesg);
                int wholeBits = wholeMesg.Length * 8;

                int cells = 0;

                //remove front few zeros of padding
                BitArray firstByte = new BitArray(8, false);
                for (int m = 0; m < firstByte.Length; m++)
                    firstByte[m] = fullMesgArry[m];
                int x = 0;
                for (x = 0; x < firstByte.Length; x++)
                {
                    if (firstByte[x])
                    {
                        break;
                    }
                }
                //value of first x zero values to be deleted
                BitArray temp = new BitArray(wholeBits - x, false);

                //Fill the temp array
                int ix, jx;
                for (ix = x, jx = 0; ix < fullMesgArry.Length; ix++, jx++)
                {
                    temp[jx] = fullMesgArry[ix];
                }
                fullMesgArry = null;
                fullMesgArry = temp;

                //First Test to see whether 1st byte=='a'
                BitArray pad = new BitArray(8, false);

                //fill pad with first byte of wholeMesg
                for (int pi = 0; pi < 8; pi++)
                {
                    pad[pi] = fullMesgArry[pi];
                }

                BigInteger2 padInteger = new BigInteger2(8, 0);
                for (int pi = 2, pj = 0; pi < padInteger.bitlength.Length; pi++, pj++)
                {
                    padInteger.bitlength[pi] = pad[pj];
                }


                byte[] padbytes = BigInteger2.getBytes(padInteger);
                if (ASCIIEncoding.ASCII.GetBytes("a")[0] == padbytes[0])
                {
                    //MessageBox.Show("I am in removing 'a' byte from padding");
                    //Strip the first byte of fullMsgAry
                    BitArray fullAry = new BitArray(fullMesgArry.Length - 8, false);
                    for (int pi = 8, pj = 0; pi < fullMesgArry.Length; pi++, pj++)
                    {
                        fullAry[pj] = fullMesgArry[pi];
                    }
                    fullMesgArry = fullAry;
                }

                wholeBits = fullMesgArry.Length;
                cells = wholeBits / mesgCellBits;

                int remBits = wholeBits % mesgCellBits;

                //number of BigInteger2 arry
                int numbOfFullCells = cells;
                int fullBits = cells * mesgCellBits;
                if (remBits > 0)
                {
                    cells += 1;
                    //MessageBox.Show("Added 1 cell for remaining bits");
                }

                result = new BigInteger2[cells];

                //Fill cells:
                int i = 0;
                int curIndex = 2;
                int curCell = 0;

                if (numbOfFullCells > 0)
                {
                    //MessageBox.Show("I am in Full Cells"); 
                    BigInteger2 cellBgInt = new BigInteger2(mesgCellBits, 0);
                    for (i = 0; i < fullBits; i++, curIndex++)
                    {
                        cellBgInt.bitlength[curIndex] = fullMesgArry.Get(i);
                        if (curIndex == mesgCellBits + 1)
                        {
                            result[curCell] = cellBgInt;
                            curCell++;
                            curIndex = 1;
                            cellBgInt = new BigInteger2(mesgCellBits, 0);
                        }
                    }
                }

                //Fill last Cell!!!
                if (remBits > 0)
                {
                    //MessageBox.Show("I am in remaining Cells"); 
                    BigInteger2 cellBgInt = new BigInteger2(remBits, 0);
                    for (int n = i, j = 2; n < fullMesgArry.Length; n++, j++)
                    {
                        cellBgInt.bitlength[j] = fullMesgArry.Get(n);
                    }
                    result[result.Length - 1] = cellBgInt;
                }

                GC.Collect();

            }
            else//encrypt
            {
                var list = GetBoolListFromBytes(wholeMesg);

                BitArray fullMesgArry = new BitArray(list.Count);
                for (var index = 0; index < fullMesgArry.Length; index++)
                    fullMesgArry[index] = list[index];

                int wholeBits = fullMesgArry.Count;

                int cells = 0;
                cells = wholeBits / (mesgCellBits - 1);

                int remBits = wholeBits % (mesgCellBits - 1);

                //number of BigInteger2 arry
                int numbOfFullCells = cells;
                int fullBits = cells * (mesgCellBits - 1);
                if (remBits > 0) cells += 1;

                result = new BigInteger2[cells];

                //Fill cells:
                int i = 0;
                int curIndex = 3;
                int curCell = 0;

                if (numbOfFullCells > 0)
                {
                    //MessageBox.Show("I am in Full Cells");
                    BigInteger2 cellBgInt = new BigInteger2(mesgCellBits, 0);
                    try
                    {
                        for (i = 0; i < fullBits; i++, curIndex++)
                        {
                            cellBgInt.bitlength[curIndex] = fullMesgArry.Get(i);
                            if (curIndex == mesgCellBits + 1)
                            {
                                cellBgInt.bitlength[2] = true;
                                result[curCell] = cellBgInt;
                                curCell++;
                                curIndex = 2;
                                cellBgInt = new BigInteger2(mesgCellBits, 0);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Happens in 1st: " + ex.Message);
                    }
                }

                //Fill last Cell!!!
                if (remBits > 0)
                {
                    //MessageBox.Show("I am in remaining Cells");
                    BigInteger2 cellBgInt = new BigInteger2(remBits + 1, 0);
                    cellBgInt.bitlength[2] = true;
                    try
                    {
                        for (int n = i, j = 3; n < fullMesgArry.Length; n++, j++)
                        {
                            cellBgInt.bitlength[j] = fullMesgArry.Get(n);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Exception happens in 2nd: " + ex.Message);
                    }
                    result[result.Length - 1] = cellBgInt;
                }
                GC.Collect();
            }

            return result;
        }

        public List<bool> GetBoolListFromBytes(byte[] bytes)
        {
            var list = new List<bool>();
            var bitArray1 = new BitArray(bytes);
            int index1 = 0;
            while (index1 < bitArray1.Length)
            {
                if (_oddsContainer.Contains(index1 % 8))
                {
                    list.Add(true);
                }
                list.Add(bitArray1[index1]);
                index1++;
            }

            return list;
        }
        public BitArray PerformDeInjection(BitArray bitArray1)
        {
            int index1 = 0, modulsIndex = 0;
            var list = new List<bool>();

            while (index1 < bitArray1.Length)
            {
                if (_oddsContainer.Contains(modulsIndex % 8))
                {
                    index1++;
                }
                if (index1 >= bitArray1.Length) break;
                list.Add(bitArray1[index1]);
                index1++;
                modulsIndex++;
            }
            BitArray bitArray2 = new BitArray(list.Count);
            for (int index3 = 0; index3 < bitArray2.Length; index3++)
                bitArray2[index3] = list[index3];

            return bitArray2;
        }
        public byte[] DeInject(BigInteger2 decryptedMessageBits)
        {
            BitArray bitArray1 = BigInteger2.BigIntegerToBitArrayNoShrinkSize(decryptedMessageBits);

            var bitArray2 = PerformDeInjection(bitArray1);
            byte[] numArray = new byte[(bitArray2.Length - 1) / 8 + 1];
            bitArray2.CopyTo(numArray, 0);
            return numArray;
        }


    }
}
