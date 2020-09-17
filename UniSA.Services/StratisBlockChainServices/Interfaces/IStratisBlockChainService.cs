using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UniSA.Domain;

namespace UniSA.Services.StratisBlockChainServices.Interfaces
{
    public interface IStratisBlockChainService
    {
        HttpClient HttpClient { get; set; }
        BlockChainResponse StratisMineBlockFromChain(string stratisMineUrl, StratisBlockData blockData);
        BlockChainResponse StratisMineManyBlocksFromChain(string stratisMineUrl, StratisBlockData[] blockData);
        StratisBlockData GetBlockWithHashFromChain(string stratisUrl, string stratisHashOfBlock);
    }
}
