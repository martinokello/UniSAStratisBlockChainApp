using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UniSA.Domain;
using RSA316Infinito.BigRsaCrypto;
using UniSA.Services.StratisBlockChainServices.Interfaces;
using System.Configuration;
using UniSA.Services.RepositoryServices;
using UniSA.Services.StratisBlockChainServices.StratisApi;
using Newtonsoft.Json;

namespace UniSA.Services.StratisBlockChainServices.Providers
{
    public class EmployerOperations : IEmployer
    {
        private EncryptDecrypt Rsa316Engine;
        private MoocMicroCredentialProvider MoocProvider;
        private StratisEndPointAdhocService _stratisEndPointService;

        public EmployerOperations(StratisEndPointAdhocService stratisEndPointService, EncryptDecrypt rsa316Engine, MoocMicroCredentialProvider moocProvider)
        {
            Rsa316Engine = rsa316Engine;
            _stratisEndPointService = stratisEndPointService;
            MoocProvider = moocProvider;
        
        }

        public StratisApiFullfilRequestComponent StratisApiFullfilRequestComponent { get; set; }

        public bool AddUserToInterviewList(string emailAddress, int vacancyId)
        {
            Console.Out.WriteLine($"User: {emailAddress} has beed added to Interview List for Vacancy: {vacancyId}");
            return true;
        }

        public Employer Employer { get; set; }
        public RepositoryEndPointServices RepositoryEndPointService { get; set; }

        public List<MicroCredential> GetAllAppropriateMicroCredentials()
        {
            return MoocProvider.GetAllMicroCredentials();
        }

        public void NotifyUserAboutMicroCredential(string emailAddress, MicroCredential MicroCredential)
        {
            Console.Out.WriteLine($"{emailAddress} this MicroCredential: {MicroCredential.MicroCredentialName} Suits the Job Requirements/");
        }
        public bool VerifyUserCert(string emailAddress, MicroCredential microCredential)
        {
            Candidate candidate = RepositoryEndPointService.GetCandidateByEmail(emailAddress);
            CandidateMicroCredentialCourse candidateWithCourse = RepositoryEndPointService.GetCandidateAndCourseForVerification(candidate.CandidateId, microCredential.MicroCredentialId);

            if (candidateWithCourse == null) return false;

            var hashOfMicroCredential = candidateWithCourse.HashOfMine;

            //return this.StratisApiFullfilRequestComponent.VerifyBlockData(new VerifyBlockRequest { externalAddress = ConfigurationManager.AppSettings["ExternalAddress"], message = this.MoocProvider.GetUserDataToCertify(candidate.EmailAddress, this.MoocProvider.Mooc.EmailAddress, microCredential), signature = hashOfMicroCredential }).Result;
            var moocEmailAddress = microCredential.MoocProvider.EmailAddress;

            var content = MoocProvider.GetUserDataToCertify(emailAddress, moocEmailAddress, microCredential);
            var signedContent = MoocProvider.CreateSignature(content);
            return candidateWithCourse.HashOfMine.Equals(signedContent);
        }
        public bool VerifyMoocSignature(StratisBlockData blockToVerify)
        {
            var signaturesRaw = blockToVerify.Transactions.Select(q => {
                var individualDetails = q.Amount.ToString() + q.From + q.To;
                var microCredential = q.MicroCredentials.Select(p => { return p.MicroCredentialId.ToString() + p.MicroCredentialCode + p.MicroCredentialDescription.ToString() + p.MicroCredentialName; }).ToList();
                var strBuilder = new StringBuilder();
                microCredential.ForEach(p => strBuilder.Append(p));
                var subResult = individualDetails + strBuilder.ToString();
                return subResult;
            });

            var absoluteContentSignature = new StringBuilder();
            signaturesRaw.ToList().ForEach(s => absoluteContentSignature.Append(s));
            Rsa316Engine.setModValue(blockToVerify.KeyModulus);
            Rsa316Engine.setPrivateKey(blockToVerify.MoocPublicKey);

            var decryptedSignature = Encoding.UTF8.GetString(Rsa316Engine.Decrypt(blockToVerify.MoocSignature));

            return decryptedSignature.Equals(absoluteContentSignature.ToString());
        }

        public StratisBlockData MoocSignBlock(StratisBlockData blockToSign)
        {
            var signaturesRaw = blockToSign.Transactions.Select(q => {
                var individualDetails = q.Amount.ToString() + q.From + q.To;
                var microCredential = q.MicroCredentials.Select(p => { return p.MicroCredentialId.ToString() + p.MicroCredentialId.ToString() + p.MicroCredentialName; }).ToList();
                var strBuilder = new StringBuilder();
                microCredential.ForEach(p => strBuilder.Append(p));
                var subResult = individualDetails + strBuilder.ToString();
                return subResult;
            });

            var absoluteContentSignature = new StringBuilder();
            signaturesRaw.ToList().ForEach(s => absoluteContentSignature.Append(s));

            blockToSign.MoocSignature = Convert.ToBase64String(Rsa316Engine.Encrypt(absoluteContentSignature.ToString()));
            return blockToSign;
        }
    }


}
