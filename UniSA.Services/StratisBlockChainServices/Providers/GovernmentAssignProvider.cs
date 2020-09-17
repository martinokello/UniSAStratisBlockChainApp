using System;
using System.Collections.Generic;
using System.Text;
using UniSA.Domain;
using UniSA.Services.StratisBlockChainServices.Interfaces;

namespace UniSA.Services.StratisBlockChainServices.Providers
{
    public class GovernmentAssignProvider : IGovernment
    {
        public bool GovernmentAknowledged { get; set; }
        public string Country { get; set; }

        public void AssignToDepartment(Government governmentDepartment)
        {
            throw new NotImplementedException();
        }
    }
}
