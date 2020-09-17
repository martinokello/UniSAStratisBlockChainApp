using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using UniSA.Domain;
using UniSA.Services.StratisBlockChainServices.Interfaces;
using Newtonsoft.Json;
using System.Configuration;
using System.Collections.Generic;

namespace UniSA.Services.StratisBlockChainServices
{
    public class StratisEndPointAdhocService:IStratisBlockChainService
    {
        public HttpClient HttpClient { get; set; }

        public StratisEndPointAdhocService()
        {
            HttpClient = new HttpClient();

            HttpClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["StratisBlockChainBaseUrl"]);
        }

        protected async Task<BlockChainResponse> StratisMineBlock(string stratisMineUrl, StratisBlockData blockData)
        {
            var jsonMediaTypeFormatter = new JsonMediaTypeFormatter();
            BlockChainData request = new BlockChainData { blockCount = blockData.blockCount, description = blockData };

            var httpResponse = await HttpClient.PostAsJsonAsync<BlockChainData>(stratisMineUrl, request);

            return JsonConvert.DeserializeObject<BlockChainResponse>(httpResponse.Content.ReadAsStringAsync().Result);
        }
        protected async Task<BlockChainResponse> StratisMineManyBlocks(string stratisMineUrl, StratisBlockData[] blockData)
        {
            var jsonMediaTypeFormatter = new JsonMediaTypeFormatter();
            var request = new List<BlockChainData>();

            Array.ForEach(blockData, b => { request.Add(new BlockChainData { blockCount = 1, description = b }); });
            var requestData = request.ToArray();

            var httpResponse = await HttpClient.PostAsJsonAsync<BlockChainData[]>(stratisMineUrl, requestData);

            return JsonConvert.DeserializeObject<BlockChainResponse>(httpResponse.Content.ReadAsStringAsync().Result);
        }
        protected async Task<HttpResponseMessage> GetBlockWithHash(string stratisUrl, string stratisHashOfBlock)
        {
            var jsonMediaTypeFormatter = new JsonMediaTypeFormatter();

            var httpResponse = await HttpClient.GetAsync(stratisUrl + $"?Hash={ stratisHashOfBlock}&ShowTransactionDetails={true}");
            return httpResponse;
        }

        public BlockChainResponse StratisMineBlockFromChain(string stratisMineUrl, StratisBlockData blockData)
        {
            blockData.blockCount = 1;
            return StratisMineBlock(stratisMineUrl, blockData).Result;
        }
        public BlockChainResponse StratisMineManyBlocksFromChain(string stratisMineUrl, StratisBlockData[] blockData)
        {
            Array.ForEach(blockData, b => { b.blockCount = blockData.Length; });
            return StratisMineManyBlocks(stratisMineUrl, blockData).Result;
        }
        public StratisBlockData GetBlockWithHashFromChain(string stratisUrl, string stratisHashOfBlock)
        {

            var httpResult = GetBlockWithHash(stratisUrl, stratisHashOfBlock).Result;

            return JsonConvert.DeserializeObject<StratisBlockData>(httpResult.Content.ReadAsStringAsync().Result);
        }

    }
}
