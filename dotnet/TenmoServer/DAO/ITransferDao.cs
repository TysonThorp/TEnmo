using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferDao
    {

        string Transfer(int userId_from, int userId_to, decimal amount);

        List<Transaction> GetAllTransactions(int userId);

        Transaction GetTransactionById(int transferId);
        
        
 }
}
