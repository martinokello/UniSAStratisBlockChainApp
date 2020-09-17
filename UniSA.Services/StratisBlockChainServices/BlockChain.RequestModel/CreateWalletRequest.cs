using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniSA.Services.StratisBlockChainServices.BlockChain.RequestModel
{
    public class CreateWalletRequest
    {
        public string mnemonic { get; set; }
        public string passphrase { get; set; }
        public string password { get; set; }
        public string name { get; set; }
    }
}
