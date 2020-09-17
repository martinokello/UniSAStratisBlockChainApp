using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniSA.Domain
{
    public class StratisAccount
    {
        [Key]
        public int StratisAccountId { get; set; }
        public string AccountName { get; set; } = "account 0";
        public string EmailAddress { get; set; }
        public string AccountStratisAddress1 { get; set; }
        public string AccountStratisAddress2 { get; set; }
        public string AccountStratisAddress3 { get; set; }
        public string AccountWalletName { get; set; }
        public bool IsWalletLoaded { get; set; } = false;
    }
}
