using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using RSA316Infinito.BigRsaCrypto;
using UniSA.Domain;
using Newtonsoft.Json;
using UniSA.Services.StratisBlockChainServices.Interfaces;
using System.Configuration;
using UniSA.Services.RepositoryServices;
using UniSA.Services.StratisBlockChainServices.StratisApi;

namespace UniSA.Services.StratisBlockChainServices.Providers
{
    public class MoocMicroCredentialProvider : IMicroCredentialProvidercs
    {
        private EncryptDecrypt Rsa316Engine;
        public StratisEndPointAdhocService StratisEndPointService { get; set; }
        public RepositoryEndPointServices RepositoryEndPointService { get; set; }
        public MoocMicroCredentialProvider(StratisEndPointAdhocService stratisEndPointService, EncryptDecrypt rsa316Engine)
        {
            Rsa316Engine = rsa316Engine;
            StratisEndPointService = stratisEndPointService;
        }
        public StratisApiFullfilRequestComponent StratisApiFullfilRequestComponent { get; set; }
        public BlockChainResponse StratisMineBlockThenReturnResponse(StratisBlockData block)
        {
            try
            {
                var stratisMineRelativeUrl = ConfigurationManager.AppSettings["StratisBlockChainMineRelativeUrl"];
                var hashOfMineData = StratisEndPointService.StratisMineBlockFromChain(stratisMineRelativeUrl, block);
                return hashOfMineData;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void AddUserToMicroCredentialCourse(string emailAddress, MicroCredential microCredential)
        {
            RepositoryEndPointService.EnroleUserToMicroCredentialCourseUsingEmailId(emailAddress, microCredential.MicroCredentialId);
        }
        public bool VerifyUserCert(string emailAddress, MicroCredential microCredential)
        {
            Candidate candidate = RepositoryEndPointService.GetCandidateByEmail(emailAddress);
            CandidateMicroCredentialCourse candidateWithCourse = RepositoryEndPointService.GetCandidateAndCourseForVerification(candidate.CandidateId, microCredential.MicroCredentialId);

            if (candidateWithCourse == null) return false;

            var hashOfMicroCredential = candidateWithCourse.HashOfMine;
            var moocProvider = RepositoryEndPointService.GetMoocProviderById(microCredential.MoocProviderId);
            if (moocProvider == null) throw new Exception("No Mooc Course Provider for this course!");
            //return this.StratisApiFullfilRequestComponent.VerifyBlockData(new VerifyBlockRequest { externalAddress = ConfigurationManager.AppSettings["ExternalAddress"], message = this.MoocProvider.GetUserDataToCertify(candidate.EmailAddress, this.MoocProvider.Mooc.EmailAddress, microCredential), signature = hashOfMicroCredential }).Result;
            var moocEmailAddress = moocProvider.EmailAddress;
            var content = this.GetUserDataToCertify(emailAddress, moocEmailAddress, microCredential);
            var signedContent = this.CreateSignature(content);
            return candidateWithCourse.HashOfMine.Equals(signedContent);
        }
        public string GetUserDataToCertify(string emailAddress, string moocEmailAddress, MicroCredential MicroCredential)
        {
            var block = new StratisBlockData(DateTime.Now, new List<Transactions> {
                new Transactions(moocEmailAddress, emailAddress, MicroCredential.CertificateFee) {
                     MicroCredentials= new List<MicroCredential> {
                    new MicroCredential{
                        MicroCredentialId = MicroCredential.MicroCredentialId,
                        /*MicroCredentialDescription = MicroCredential.MicroCredentialDescription,*/
                        MicroCredentialName = MicroCredential.MicroCredentialName,
                        Fee = MicroCredential.Fee,
                        CertificateFee=MicroCredential.CertificateFee
                        }
                    }
                }
            });
            return $"Fr:{block.Transactions.First().From},To:{block.Transactions.First().To},Fee:{block.Transactions.First().MicroCredentials.First().Fee},CFee:{block.Transactions.First().MicroCredentials.First().CertificateFee},CrId:{block.Transactions.First().MicroCredentials.First().MicroCredentialId}";
        }
        public StratisBlockData CertifyUser(string emailAddress, string moocEmailAddress, MicroCredential MicroCredential)
        {
            var block = new StratisBlockData(DateTime.Now, new List<Transactions> {
                new Transactions(moocEmailAddress, emailAddress, MicroCredential.CertificateFee) {
                     MicroCredentials= new List<MicroCredential> {
                    new MicroCredential{
                        MicroCredentialId = MicroCredential.MicroCredentialId,
                        /*MicroCredentialDescription = MicroCredential.MicroCredentialDescription,*/
                        MicroCredentialName = MicroCredential.MicroCredentialName
                        }
                    }
                }
            });

            //Get RSAKeysFromDB

            var moocProvider = RepositoryEndPointService.GetMoocProviderFromDB();
            RestoreMoocKeys(moocProvider);
            //Note We Sign with Mooc's Public Key. and Expose Private Key to the other parites to verify Mooc's Signature!! 
            block.KeyModulus = moocProvider.MoocModulus;//Rsa316Engine.getModValue();
            block.MoocPublicKey = moocProvider.MoocPublicKey;//Rsa316Engine.getPublicKey();

            block.MoocSignature = CreateSignature(block);
            return block;
        }

        public List<MicroCredential> GetAllMicroCredentials()
        {
            return RepositoryEndPointService.GetAllMicroCredentials();
        }

        public string CreateSignature(string stratisBlockDataToSign)
        {
            var moocProvider = RepositoryEndPointService.GetMoocProviderFromDB();
            RestoreMoocKeys(moocProvider);

            //Note: We are signing with Private Key
            var signBytes = Rsa316Engine.Encrypt(stratisBlockDataToSign);

            return Convert.ToBase64String(signBytes);
        }

        private void RestoreMoocKeys(MoocProvider moocProvider)
        {
            if (moocProvider == null) throw new KeyNotFoundException("Mooc Provider Does not Exist");
            if (moocProvider.MoocModulus == null || moocProvider.MoocPublicKey == null || moocProvider.MoocPrivateKey == null)
            {
                Rsa316Engine.generateNewKey();
                moocProvider.MoocPrivateKey = Rsa316Engine.getPrivateKey();
                moocProvider.MoocPublicKey = Rsa316Engine.getPublicKey();
                moocProvider.MoocModulus = Rsa316Engine.getModValue();

                //Encrypt with Private Key: therefore swap RsaEngineKeys:
                byte[] temp = Rsa316Engine.getPrivateKey();
                Rsa316Engine.setPrivateKey(Rsa316Engine.getPublicKey());
                Rsa316Engine.setPublicKey(temp);

                RepositoryEndPointService.UpdateMoocProviderKeys(moocProvider);
            }
            else
            {
                //Encrypt with Private Key: therefore swap RsaEngineKeys:
                byte[] temp = moocProvider.MoocPrivateKey;
                Rsa316Engine.setPrivateKey(moocProvider.MoocPublicKey);
                Rsa316Engine.setPublicKey(temp);
                Rsa316Engine.setModValue(moocProvider.MoocModulus);
            }
        }

        public string CreateSignature(StratisBlockData blockToVerify)
        {
            var signaturesRaw = blockToVerify.Transactions.Select(q =>
            {
                var individualDetails = q.Amount.ToString() + q.From + q.To;
                var microCredentials = q.MicroCredentials.Select(p => { return p.MicroCredentialId.ToString() + p.MicroCredentialCode + p.MicroCredentialDescription + p.MicroCredentialName; }).ToList();
                var strBuilder = new StringBuilder();
                microCredentials.ForEach(p => strBuilder.Append(p));
                var subResult = individualDetails + ":" + strBuilder.ToString();
                return subResult;
            });

            var absoluteContentSignature = new StringBuilder();
            signaturesRaw.ToList().ForEach(s => absoluteContentSignature.Append(s));
            var sigBytes = Rsa316Engine.Encrypt(absoluteContentSignature.ToString());

            return Convert.ToBase64String(sigBytes);
        }

    }
}
