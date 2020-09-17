using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UniSA.Domain;
using RSA316Infinito.BigRsaCrypto;
using UniSA.Services.StratisBlockChainServices.Interfaces;

namespace UniSA.Services.StratisBlockChainServices.Providers
{
    public class RecruitmentAgentOperations : IRecruitmentAgent
    {
        public Candidate[] GetCandidatesForJobId(int jobId, int[] microCredentialIds)
        {
            throw new NotImplementedException();
        }
    }
}
