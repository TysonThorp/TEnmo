using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferDao
    {

        public string SendTEBucks(int accountId, int userId);

        public string RequestTEBucks(int userId_from, int userId_to, decimal amount);

        public List<PastTransfer> ViewPastTransfers(int userId);

        Transactions GetTransactionById(int transferId);

        public List<PendingTransfer> PendingTransactions(int accountId);



    }
}
