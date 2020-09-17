using System;
using System.Collections.Generic;
using System.Text;

namespace UniSA.Domain
{
    public class BlockChainResponse
    {
        public string[] hash { get; set; }
    }


    public class BlockChainDataStrDesc{

        public string description { get; set; }
        public int blockCount { get; set; }
    }

    public class BlockChainData
    {
        public StratisBlockData description { get; set; }
        public int blockCount { get; set; }
    }

    public class SignBlockRequest
    {
        public string walletName { get; set; }
        public string externalAddress { get; set; }
        public string message { get; set; }
        public string password { get; set; }
    }
    public class VerifyBlockRequest
    {
        public string signature { get; set; }
        public string externalAddress { get; set; }
        public string message { get; set; }
    }
}
