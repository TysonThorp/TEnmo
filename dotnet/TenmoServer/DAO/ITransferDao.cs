using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferDao
    {

        AccountBalance Transfer(int userId, int account_to, decimal amount);

        Transaction GetAllTransactions(int userId);

        Transaction GetTransaction(int transferId);
        
        
 }
}
