using System;
using System.Collections.Generic;
using System.Text;
using UniSA.Domain;

namespace UniSA.Services.StratisBlockChainServices.Interfaces
{
    public interface IEmployer
    {
        List<MicroCredential> GetAllAppropriateMicroCredentials();

        void NotifyUserAboutMicroCredential(string emailAddress, MicroCredential MicroCredential);

        bool VerifyUserCert(string emailAddress, MicroCredential MicroCredential);

        bool AddUserToInterviewList(string emailAddress, int vacancyId);
        bool VerifyMoocSignature(StratisBlockData blockToVerify);
    }
}
