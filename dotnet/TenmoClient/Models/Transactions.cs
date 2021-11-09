﻿using System;
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


        public Transactions()
        { }

        
    }
    public class PastTransfer
    {
        public string FromName { get; set; }

        public string ToName { get; set; }
        public decimal Amount { get; set; }
        public int TransferId { get; set; }

        public PastTransfer()
        { }
        
    }
    public class PendingTransfer
    {
        public string FromName { get; set; }
        public decimal Amount { get; set; }
        public int TransferId { get; set; }

        public PendingTransfer()
        {
        }
    }
}

