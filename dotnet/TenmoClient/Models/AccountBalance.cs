using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Models
{
    public class AccountBalance
    {
        public int Account_Id { get; set; }
        public int User_Id { get; set; }
        public decimal Balance { get; set; }

        public AccountBalance()
        { }
        public AccountBalance(int account_id, int user_id, int balance)
        {
            this.Account_Id = account_id;
            this.User_Id = user_id;
            this.Balance = balance;

        }
        

    }
}
