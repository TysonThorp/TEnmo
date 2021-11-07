using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferDao
    {

        public string SendTEBucks(int userId_from, int userId_to, decimal amount);

        public string RequestTEBucks(int userId_from, int userId_to, decimal amount);

        public List<Transaction> ViewPastTransfers(int userId);

        Transaction GetTransactionById(int transferId);

        public List<Transaction> PendingTransactions(int user_id);


        //List<Transaction> GetAllTransactions(int userId);




    }
}
