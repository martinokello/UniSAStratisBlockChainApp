using System;

namespace RSA316Infinito.BigRsaCrypto
{
	/// <summary>
	/// 
	/// </summary>
	
	[Serializable]
	public class Storage
	{
		private string password;
		private byte[] publickey;
		private byte[] privatekey;
		private byte[] mod;
		public Storage(string pass)
		{
			// 
			// TODO: Add constructor logic here
			//
			password=pass;

		}

		public string Password
		{
			get
			{
				return password;
			}
			set
			{
				password=value;
			}
		}

		public byte[] Privatekey
		{
			get
			{
				return privatekey;
			}
			set
			{
				privatekey=value;
			}
		}

		public byte[] Publickey
		{
			get
			{
				return publickey;
			}
			set
			{
				publickey=value;
			}
		}	

		public byte[] Mod
		{
			get
			{
				return mod;
			}
			set
			{
				mod=value;
			}
		}	
	}
}
