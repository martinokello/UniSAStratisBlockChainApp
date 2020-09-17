using RSA316Infinito.BigRsaCrypto;
using System;
using System.Collections.Generic;
using System.Text;
using UniSA.Domain;
using UniSA.Services.StratisBlockChainServices.Interfaces;

namespace UniSA.Services.StratisBlockChainServices.Providers
{
    public class AccreditationBodyOperations : IAccreditationBody
    {
        private EncryptDecrypt Rsa316Engine;
        private StratisEndPointAdhocService _stratisEndPointService;

        public AccreditationBodyOperations(StratisEndPointAdhocService stratisEndPointService, EncryptDecrypt rsa316Engine)
        {
            Rsa316Engine = rsa316Engine;
            _stratisEndPointService = stratisEndPointService;
        }
            public bool AccreditateMicroCredential(int microCredentialId)
        {
            throw new NotImplementedException();
        }

        public bool EndorseMicroCredential(int microCredentialId)
        {
            throw new NotImplementedException();
        }

        public List<MicroCredential> GetAllAccreditedMicroCredentials()
        {
            throw new NotImplementedException();
        }

        public List<MicroCredential> GetAllEndorsedMicroCredentials()
        {
            throw new NotImplementedException();
        }
    }
}
