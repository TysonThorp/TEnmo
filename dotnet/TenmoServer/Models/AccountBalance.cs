using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Models
{
    public class AccountBalance
    {
       public decimal Balance { get; set; }
       public int Account_Id { get; }
    }
}
