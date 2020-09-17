using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniSA.Services.StratisBlockChainServices.BlockChain.RequestModel
{
    public class CreateMnemonicRequest
    {
        public string language { get; set; }
        public int wordcount { get; set; }
    }
}
