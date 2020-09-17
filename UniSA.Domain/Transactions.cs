using System;
using System.Collections.Generic;
using System.Text;

namespace UniSA.Domain
{
    public class Transactions
    {
        public string From { get; }
        public string To { get; }
        public decimal Amount { get; }

        public List<MicroCredential> MicroCredentials { get; set; }
        public Transactions(string from, string to, decimal amount)
        {
            From = from;
            To = to;
            Amount = amount;
        }
    }
}
