using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniSA.Services.StratisBlockChainServices.BlockChain.RequestModel
{
    public class AccountAddressesResponse
    {
        public AddressContent[] addresses { get; set; }
    }

    public class AddressContent
    {
        public string address { get; set; }
        public bool isUsed { get; set; }
        public bool isChange { get; set; }
        public decimal amountConfirmed { get; set; }
        public decimal amountUnconfirmed { get; set; }
    }
}
