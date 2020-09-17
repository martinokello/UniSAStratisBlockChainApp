using System;
using System.Collections.Generic;
using System.Text;
using UniSA.Domain;

namespace UniSA.Services.StratisBlockChainServices.Interfaces
{
    public interface IGovernment
    {
        string Country { get; set; }
        bool GovernmentAknowledged{ get; set; }

        void AssignToDepartment(Government governmentDepartment);
    }
}
