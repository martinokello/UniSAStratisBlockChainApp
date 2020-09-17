using System;
using System.Collections.Generic;
using System.Text;
using UniSA.Domain;

namespace UniSA.Services.StratisBlockChainServices.Interfaces
{
    public interface IAccreditationBody
    {
        List<MicroCredential> GetAllAccreditedMicroCredentials();
        List<MicroCredential> GetAllEndorsedMicroCredentials();
        bool AccreditateMicroCredential(int microCredentialId);
        bool EndorseMicroCredential(int microCredentialId);
    }
}
