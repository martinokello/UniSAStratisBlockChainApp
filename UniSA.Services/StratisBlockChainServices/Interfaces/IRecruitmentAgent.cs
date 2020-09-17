using System;
using System.Collections.Generic;
using System.Text;
using UniSA.Domain;

namespace UniSA.Services.StratisBlockChainServices.Interfaces
{
    public interface IRecruitmentAgent
    {
        Candidate[] GetCandidatesForJobId(int jobId, int[] microCredentialIds);
    }
}
