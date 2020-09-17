using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using UniSA.Domain;
using UniSA.Services.StratisBlockChainServices.BlockChain.RequestModel;
using UniSA.Services.StratisBlockChainServices.Interfaces;

namespace UniSA.Services.StratisBlockChainServices.StratisApi
{
    public class StratisApiFullfilRequestComponent: IStratisApiConfigure
    {
        public StratisApiFullfilRequestComponent(HttpClient httpClient)
        {
            GetSetHttpClient = httpClient;
        }
        public HttpClient GetSetHttpClient { get; set; }

        public async Task<bool> LoadWallet(string walletName, string password)
        {
            var wallet = new Wallet { name = walletName, password = password };
            ////Post /api/Wallet/load
            HttpResponseMessage result = await GetSetHttpClient.PostAsJsonAsync<Wallet>(ConfigurationManager.AppSettings["LoadWalletUrl"], wallet);
            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }
        public async Task<bool> BeginStaking(string walletName, string password)
        {
            var wallet = new Wallet { name = walletName, password = password };
            ////Post: api/Staking/startstaking            
            HttpResponseMessage result = await GetSetHttpClient.PostAsJsonAsync<Wallet>(ConfigurationManager.AppSettings["BeginStakingUrl"],wallet);
            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }
        public async Task<bool> StopStaking()
        {
            ////Post: api/Staking/startstaking            
            HttpResponseMessage result = await GetSetHttpClient.PostAsJsonAsync<bool>(ConfigurationManager.AppSettings["StopStakingUrl"],true);
            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task<StratAddressSpendableAmount> GetSpendableAmountAtWalletAddress(string walletAddress)
        {
            ///Get /api/Wallet/received-by-address
            HttpResponseMessage result = await GetSetHttpClient.GetAsync(ConfigurationManager.AppSettings["SpendableAmountWalletAddressUrl"]+$"?address={walletAddress}");
            return JsonConvert.DeserializeObject<StratAddressSpendableAmount>(result.Content.ReadAsStringAsync().Result);
        }

        public async Task<TransactionBuildResponse> BuildTransaction(TransactionBuildData txData)
        {
            HttpResponseMessage result = await GetSetHttpClient.PostAsJsonAsync<TransactionBuildData>(ConfigurationManager.AppSettings["BuildTransactionUrl"], txData);
            return JsonConvert.DeserializeObject<TransactionBuildResponse>(result.Content.ReadAsStringAsync().Result);
        }
        public async Task<bool> SendTransaction(string transactionHexDecimalId)
        {
            HttpResponseMessage result = await GetSetHttpClient.PostAsJsonAsync<string>(ConfigurationManager.AppSettings["SendTransactionUrl"], transactionHexDecimalId);
            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task<string> SignBlockData(SignBlockRequest signBlockRequest)
        {
            HttpResponseMessage result = await GetSetHttpClient.PostAsJsonAsync<SignBlockRequest>(ConfigurationManager.AppSettings["SignDataUrl"], signBlockRequest);
            return result.Content.ReadAsStringAsync().Result;
        }
        public async Task<bool> VerifyBlockData(VerifyBlockRequest verifyBlockRequest)
        {
            HttpResponseMessage result = await GetSetHttpClient.PostAsJsonAsync<VerifyBlockRequest>(ConfigurationManager.AppSettings["SignDataUrl"], verifyBlockRequest);
            return bool.Parse(result.Content.ReadAsStringAsync().Result);
        }

        public async Task<string> CreateMnemonic()
        {
            var baseUrl = ConfigurationManager.AppSettings["StratisBlockChainBaseUrl"];
            var mnemonicUrl = baseUrl+ConfigurationManager.AppSettings["WalletCreationMneumonicUrl"] + "?language=english&wordCount=15";
            HttpResponseMessage result = await GetSetHttpClient.GetAsync(mnemonicUrl);
            return await result.Content.ReadAsStringAsync();
        }
        public async Task<bool> CreateWallet(string walletName, string password, string accountName)
        {
            //api/Wallet/create
            var baseUrl = ConfigurationManager.AppSettings["StratisBlockChainBaseUrl"];
            string mnemonicResult = await CreateMnemonic();
            mnemonicResult = mnemonicResult.Trim('"');
            var crateWalletRequest = new CreateWalletRequest { name = walletName, password = password, mnemonic = mnemonicResult, passphrase = password };
            var createWalletUrl = baseUrl + ConfigurationManager.AppSettings["CreateWalletUrl"];
            var result = await GetSetHttpClient.PostAsJsonAsync<CreateWalletRequest>(createWalletUrl, crateWalletRequest);
            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task<AccountAddressesResponse> GetAccountAddresses(string walletName, string accountName)
        {
            //api/Wallet/addresses 
            var baseUrl = ConfigurationManager.AppSettings["StratisBlockChainBaseUrl"];
            var getAddressUrl = baseUrl + ConfigurationManager.AppSettings["GetAccountAddressesUrl"] + $"?walletName={walletName}&accountName={accountName}";
            HttpResponseMessage result = await GetSetHttpClient.GetAsync(getAddressUrl);
            var results = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<AccountAddressesResponse>(results);

        }
    }
}
