using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UniSA.Domain;
using UniSA.Services.StratisBlockChainServices.BlockChain.RequestModel;

namespace UniSA.Services.StratisBlockChainServices.Interfaces
{
    public interface IStratisApiConfigure
    {
        HttpClient GetSetHttpClient { get; set; }

        Task<bool> BeginStaking(string walletName, string password);
        Task<bool> StopStaking();
        Task<bool> LoadWallet(string walletName, string password);
        Task<StratAddressSpendableAmount> GetSpendableAmountAtWalletAddress(string walletAddress);
        Task<TransactionBuildResponse> BuildTransaction(TransactionBuildData txData);
        Task<bool> SendTransaction(string transactionHexDecimalId);
        Task<string> SignBlockData(SignBlockRequest signBlockRequest);
        Task<bool> VerifyBlockData(VerifyBlockRequest verifyBlockRequest);
        Task<string> CreateMnemonic();
        Task<bool> CreateWallet(string walletName, string password, string accountName);
        Task<AccountAddressesResponse> GetAccountAddresses(string walletName, string accountName);
    }
}
