using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nynox_manage_prisma_master_console
{
    public class Account
    {
        public string group_account_code { get; set; }
        public string group_account_name { get; set; }
        public string detail_account_code { get; set; }
        public string detail_account_name { get; set; }
        public string detail { get; set; }
        public string calculate_in_report { get; set; }
        public string listing_order { get; set; }  
    }
}
