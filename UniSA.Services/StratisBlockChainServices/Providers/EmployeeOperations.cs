using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UniSA.Domain;
using RSA316Infinito.BigRsaCrypto;

using UniSA.Services.StratisBlockChainServices.Interfaces;
using System.Configuration;
using Newtonsoft.Json;
using UniSA.Services.StratisBlockChainServices.StratisApi;
using UniSA.Services.RepositoryServices;

namespace UniSA.Services.StratisBlockChainServices.Providers
{
    public class EmployeeOperations : IFutureEmployee
    {
        private EncryptDecrypt Rsa316Engine;
        private StratisEndPointAdhocService _stratisEndPointService;
        private MoocMicroCredentialProvider MoocProvider;
        public EmployeeOperations(StratisEndPointAdhocService stratisEndPointService, EncryptDecrypt rsa316Engine, MoocMicroCredentialProvider moocProvider)
        {
            Rsa316Engine = rsa316Engine;
            _stratisEndPointService = stratisEndPointService;
            MoocProvider = moocProvider;
        }

        public StratisApiFullfilRequestComponent StratisApiFullfilRequestComponent { get; set; }
        public RepositoryEndPointServices RepositoryEndPointService { get; set; }
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
                var microCredential = q.MicroCredentials.Select(p => { return p.MicroCredentialId.ToString() + p.MicroCredentialId.ToString() + p.MicroCredentialName; }).ToList();
                var strBuilder = new StringBuilder();
                microCredential.ForEach(p => strBuilder.Append(p));
                var subResult = individualDetails +":"+ strBuilder.ToString();
                return subResult;
            });

            var absoluteContentSignature = new StringBuilder();
            signaturesRaw.ToList().ForEach(s => absoluteContentSignature.Append(s));

            var decryptedSignature = Encoding.UTF8.GetString(Rsa316Engine.Decrypt(blockToVerify.MoocSignature));

            return decryptedSignature.Equals(absoluteContentSignature.ToString());
        }
    }
}
