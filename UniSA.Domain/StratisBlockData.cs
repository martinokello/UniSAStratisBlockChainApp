using System;
using System.Collections.Generic;

namespace UniSA.Domain
{
    public class StratisBlockData
    {

        private readonly DateTime _timeStamp;
        public string MoocSignature { get; set; }
        public int blockCount { get; set; }
        public byte[] MoocPublicKey { get; set; }
        public byte[] KeyModulus { get; set; }
        public List<Transactions> Transactions { get; set; }
        public StratisBlockData(DateTime timeStamp, List<Transactions> transactions)
        {

            _timeStamp = timeStamp;
            //_nonce = 0;
            Transactions = transactions;
        }
    }
}
