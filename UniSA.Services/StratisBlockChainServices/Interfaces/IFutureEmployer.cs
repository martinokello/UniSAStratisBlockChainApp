using System;
using System.Collections.Generic;
using System.Text;
using UniSA.Domain;


namespace UniSA.Services.StratisBlockChainServices.Interfaces
{
    public interface IFutureEmployee
    {
        //StratisBlockData GetUserBlockCerts(string emailAddress, string serviceProviderEmail);
        bool VerifyUserCert(string emailAddress, MicroCredential microCredential);
        bool VerifyMoocSignature(StratisBlockData blockToVerify);
    }
}
