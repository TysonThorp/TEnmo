using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class TransferSqlDao: ITransferDao
    {
        private readonly string connectionString;

        public TransferSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }


         //***** REMEMBER CODERS - THE CLIENT IS GOING TO CALL THE METHODS TO MAKE IT HAPPEN, IE: DON'T MAKE THE SERVER DO THINGS THE CLIENT SHOULD DO
         //       WHEN INITIATING A TRANSFER THE CLIENT WILL CALL THE METHODS TO UPDATE THE BALANCES AND ALSO TO INSERT A TRANSACTION RECORD *****
         
        public string SendTEBucks(int userId_from, int userId_to, decimal amount)
        {
            AccountBalance accountBalance = new AccountBalance();

            int accountIdFrom = GetAccountId(userId_from);
            int accountIdTo = GetAccountId(userId_to);
            string success = "Transfer Successful";
            string failure = "Insufficient funds - no money transferred.";

            if (accountBalance.Balance < amount)
            {
                return failure;
            }
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();


                    SqlCommand cmd = new SqlCommand("UPDATE accounts SET balance = balance - @amount WHERE userId_from = @userId_from: UPDATE accounts SET balance + @amount WHERE userId_to = @userId_to", conn);
                    cmd.Parameters.AddWithValue("@userId_from", userId_from);

                    SqlCommand cmdd = new SqlCommand("INSERT INTO transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount) VALUES (2, 2, @accountIdFrom, @accountIdTo, @amount)", conn); 

                    cmd.ExecuteNonQuery();
                    cmdd.ExecuteNonQuery();
                    return success;
                }
            }
            catch (SqlException)
            {

                throw new Exception("Error adding transfer");
            }
        
        }
        public string RequestTEBucks(int userId_from, int userId_to, decimal amount)
        {
            AccountBalance accountBalance = new AccountBalance();

            int accountIdFrom = GetAccountId(userId_from);
            int accountIdTo = GetAccountId(userId_to);
            string success = "Transfer Successful";
            
                        
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();


                    SqlCommand cmd = new SqlCommand("INSERT INTO transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount) VALUES (1, 1, @accountIdFrom, @accountIdTo, @amount ", conn);
                    cmd.Parameters.AddWithValue("@amount", amount);                
                                       
                    cmd.ExecuteNonQuery();
                    
                    return success;
                }
            }
            catch (SqlException)
            {

                throw new Exception("Error adding transfer");
            }

        }
    
        public List<Transactions> ViewPastTransfers(int userId)
        {
            List<Transactions> transactions = new List<Transactions>();
            int accountId = GetAccountId(userId);
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM transfers WHERE account_to = @accountId OR account_from = @accountId", conn);

                    cmd.Parameters.AddWithValue("@accountId", accountId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Transactions transaction = GetTransactionFromReader(reader);
                        transactions.Add(transaction);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return transactions;
        }
        public List<Transactions> PendingTransactions(int user_id)
        {
            List<Transactions> transactions = new List<Transactions>();
            int accountId = GetAccountId(user_id);
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM transfers JOIN transfer_statuses ON transfer_statuses.transfer_status_id = transfers.transfer_status_id WHERE transfer_statuses.transfer_status_desc = 'Pending' AND transfers.account_from = @accountId", conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Transactions transaction = GetTransactionFromReader(reader);
                        transactions.Add(transaction);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return transactions;



        }

        public Transactions GetTransactionById(int transferId)
        {
            Transactions transaction = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM transfers WHERE transfer_id = @transferId", conn);
                    cmd.Parameters.AddWithValue("@transferId", transferId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        transaction = GetTransactionFromReader(reader);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return transaction;
        }

        private int GetAccountId(int user_id)
        {
            Transactions transaction = new Transactions();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT account_id FROM accounts WHERE user_id = @user_id", conn);
                    cmd.Parameters.AddWithValue("@user_id", user_id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        
                        transaction.Account_Id = Convert.ToInt32(reader["account_id"]);
                        return transaction.Account_Id;
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return transaction.Account_Id;


        }
        private Transactions GetTransactionFromReader(SqlDataReader reader)
        {
            Transactions transaction = new Transactions();
            {
                transaction.Transfer_Id = Convert.ToInt32(reader["transfer_id"]);
                transaction.Transfer_Type_Id = Convert.ToInt32(reader["transfer_type_id"]);
                transaction.Transfer_Status_Id = Convert.ToInt32(reader["transfer_status_id"]);
                transaction.Account_From = Convert.ToInt32(reader["account_from"]);
                transaction.Account_To = Convert.ToInt32(reader["account_to"]);
                transaction.Amount = Convert.ToDecimal(reader["amount"]);

            }

            return transaction;
        }
    }
}

    



 