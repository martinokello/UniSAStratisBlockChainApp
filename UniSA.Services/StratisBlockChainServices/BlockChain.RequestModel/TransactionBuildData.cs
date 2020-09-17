using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniSA.Services.StratisBlockChainServices.BlockChain.RequestModel
{
    public class TransactionBuildData
    {
        public string feeAmount { get; set; }
        public string password { get; set; }
        public string walletName { get; set; }
        public string accountName { get; set; }
        public TransactionOut outpoints { get; set; }
        public Recipient[] recipients { get; set; }
        public string opReturnData { get; set; }
        public string opReturnAmount { get; set; }
        public string feeType { get; set; }
        public string changeAddress { get; set; }
        public bool allowUnconfirmed { get; set; }
        public bool shuffleOutputs { get; set; }

    }

    public class Recipient
    {
        public string amount { get; set; }
        public string destinationAddress { get; set; }
    }

    public class TransactionOut
    {
        public string transactionId { get; set; }
        public int index { get; set; }
    }

    public class TransactionBuildResponse
    {
        public string fee { get; set; }
        public string hex { get; set; }
        public string transactionId { get; set; }
    }
}
