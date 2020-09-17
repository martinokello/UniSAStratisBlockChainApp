using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UniSAEmloyeeEmployerCertificationAndEngagement;

using UniSA.Services.StratisBlockChainServices.Providers;
using UniSA.Services.StratisBlockChainServices;
using RSA316Infinito.BigRsaCrypto;
using UniSA.Domain;
using System.Collections.Generic;

namespace UniSA.Tests
{
    [TestClass]
    public class UnisaServicesTests
    {
        private MoocMicroCredentialProvider _moocMicroCredentialProvider;
        private EmployeeOperations _employeeOperations;
        private EmployerOperations _employerOperations;

        [TestInitialize]
        public void Setup()
        {
            AutofacConfig.RegisterTypes();
            var stratisEndPoint = new StratisEndPointAdhocService();
            var rsaEngine = new EncryptDecrypt();
            _moocMicroCredentialProvider = new MoocMicroCredentialProvider(stratisEndPoint, rsaEngine);
            _employeeOperations = new EmployeeOperations(stratisEndPoint, rsaEngine, _moocMicroCredentialProvider);
            _employerOperations = new EmployerOperations(stratisEndPoint,rsaEngine, _moocMicroCredentialProvider);
        }
        [TestMethod]
        public void TestStratisBlockChainCertifyUser_moocMicroCredentialProvider()
        {
            var microCredential = new MicroCredential { JobId = 1, MicroCredentialName = "MVP Certification", MicroCredentialId = 1 };
            var JobVacancy = new Job { JobTitle = "ASP.net C# Developer", JobId = 1, NumberOfPositions = 1, QualificationsRequired = false };
            _employerOperations.Employer = new Domain.Employer { ContactEmailAddress = "itinfrastructure@ibm.com" };

            var employee = new Candidate { EmailAddress = "martin.okello@gmail.com", AppliedJobsId = 1, MicroCredentialEnrolledId = 1, CandidateId = 1, UserName = "martin.okello@gmail.com" };

            var stratisBlock = _moocMicroCredentialProvider.CertifyUser("martin.okello@gmail.com", "admin@mooc.com", microCredential);
        }
    }
}
