using System;
namespace RSA316Infinito.BigRsaCrypto
{
	/// <summary>
	/// Summary description for Factors.
	/// </summary>
	public class Factors: ICloneable
	{
		public System.Numerics.BigInteger Mod;
		public System.Numerics.BigInteger numb;
		public System.Numerics.BigInteger fMod;
		public System.Numerics.BigInteger fnumb;
		public Factors()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		#region ICloneable Members

		public object Clone()
		{
			// TODO:  Add Factors.Clone implementation
			return null;
		}

        #endregion
    }	public class Factors2: ICloneable
	{
		public BigInteger2 Mod;
		public BigInteger2 numb;
		public BigInteger2 fMod;
		public BigInteger2 fnumb;
		public Factors2()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		#region ICloneable Members

		public object Clone()
		{
			// TODO:  Add Factors.Clone implementation
			return null;
		}

        #endregion
    }
}
