using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniSA.Services.StratisBlockChainServices.BlockChain.RequestModel
{
    public class StratAddressSpendableAmount
    {
        public string address{get;set; }
        public string amountConfirmed { get; set; }
        public string amountUnconfirmed { get; set; }
        public decimal spendableAmount { get; set; }

    }
}
