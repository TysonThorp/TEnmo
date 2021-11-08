using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Models
{
   public class Transactions
    {
        public int Transfer_Id { get; set; }

        public int Transfer_Type_Id { get; set; }

        public int Transfer_Status_Id { get; set; }

        public int Account_From { get; set; }

        public int Account_To { get; set; }

        public decimal Amount { get; set; }

        public int Account_Id { get; set; }

        public Transactions()
        { }

        public Transactions(int transfer_id, int transfer_type_id, int transfer_status_id, int account_from, int account_to, decimal amount, int account_id)
        {
            this.Transfer_Id = transfer_id;
            this.Transfer_Type_Id = transfer_type_id;
            this.Transfer_Status_Id = transfer_status_id;
            this.Account_From = account_from;
            this.Account_To = account_to;
            this.Amount = amount;
            this.Account_Id = account_id;

        }

    }

}

