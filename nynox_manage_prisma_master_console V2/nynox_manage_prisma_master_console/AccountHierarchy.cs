using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nynox_manage_prisma_master_console
{
    public class AccountHierarchy
    {
        public Account Parent { get; set; }
        public AccountCollection Children { get; }

        public AccountHierarchy()
        {
            Children = new AccountCollection();
        }
    }
}
