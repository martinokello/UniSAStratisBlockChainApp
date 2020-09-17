using System;
using System.Collections.Generic;
using System.Text;
using UniSA.Domain;

namespace UniSA.Services.StratisBlockChainServices.Interfaces
{
    public interface IMicroCredentialProvidercs
    {
        List<MicroCredential> GetAllMicroCredentials();
        bool VerifyUserCert(string emailAddress, MicroCredential microCredential);
        void AddUserToMicroCredentialCourse(string emailAddress, MicroCredential MicroCredential);
        string GetUserDataToCertify(string emailAddress, string moocEmailAddress, MicroCredential MicroCredential);
        StratisBlockData CertifyUser(string emailAddress, string mookEmailAddress, MicroCredential MicroCredential);

        BlockChainResponse StratisMineBlockThenReturnResponse(StratisBlockData block);

        string CreateSignature(StratisBlockData blockToVerify);
        string CreateSignature(string stratisBlockDataToSign);
    }
}
