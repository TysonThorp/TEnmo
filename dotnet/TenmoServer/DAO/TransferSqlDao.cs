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
         
        public string SendTEBucks(int accountId, int userId)
        {
            Account accountBalance = new Account();
            
            
            string success = "Transfer Successful";
            
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();


                    SqlCommand cmd = new SqlCommand("UPDATE accounts SET balance = balance - @amount WHERE userId = @userId: UPDATE accounts SET balance + @amount WHERE userId = @userId", conn);
                    cmd.Parameters.AddWithValue("@userId", userId);

                    SqlCommand cmdd = new SqlCommand("INSERT INTO transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount) VALUES (2, 2, @accountId, @accountId, @amount)", conn); 

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
            Account accountBalance = new Account();

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

        public List<PastTransfer> ViewPastTransfers(int accountId)
        {
            List<PastTransfer> viewPast = new List<PastTransfer>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT t.transfer_id, fu.username AS fromName, tu.username AS toName, t.amount " +
                                                    "FROM transfers t " +
                                                    "JOIN accounts fa ON fa.account_id = t.account_from " +
                                                    "JOIN users fu ON fu.user_id = fa.user_id " +
                                                    "JOIN accounts ta ON ta.account_id = t.account_to " +
                                                    "JOIN users tu ON tu.user_id = ta.user_id " +
                                                    "WHERE t.account_from = @accountId OR t.account_to = @accountId ", conn);

                    cmd.Parameters.AddWithValue("@accountId", accountId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        PastTransfer past = GetPastTransactionsFromReader(reader);
                        viewPast.Add(past);
                    }

                }
            }
            catch (SqlException)
            {
                throw;
            }

            return viewPast;
        }
        public List<PendingTransfer> PendingTransactions(int accountId)
        {
            List<PendingTransfer> pendingTransfers = new List<PendingTransfer>();
            
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT transfers.transfer_id, users.username AS FromName, transfers.amount " +
                                                    "FROM transfers " +
                                                    "JOIN transfer_statuses tr ON tr.transfer_status_id = transfers.transfer_status_id " +
                                                    "JOIN accounts ac ON ac.account_id = transfers.account_from " +
                                                    "JOIN users ON users.user_id = ac.user_id " +
                                                    "WHERE tr.transfer_status_id = 1 ", conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        PendingTransfer transaction = GetPendingTransactionsFromReader(reader);
                        pendingTransfers.Add(transaction);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return pendingTransfers;



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

        private int GetAccountId(int userId)
        {
            int accountId;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT account_id FROM accounts WHERE user_id = @user_id", conn);
                    cmd.Parameters.AddWithValue("@user_id", userId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        
                        accountId = Convert.ToInt32(reader["account_id"]);
                        return accountId;
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return 0;


        }
        private PastTransfer GetPastTransactionsFromReader(SqlDataReader reader)
        {
            PastTransfer pastTransfer = new PastTransfer();
            {
                pastTransfer.ToName = Convert.ToString(reader["toName"]);
                pastTransfer.FromName = Convert.ToString(reader["fromName"]);
                pastTransfer.TransferId = Convert.ToInt32(reader["transfer_id"]);
                pastTransfer.Amount = Convert.ToDecimal(reader["amount"]);
            }
            return pastTransfer;
        }
        private Transactions GetTransactionFromReader(SqlDataReader reader)
        {
            Transactions transaction = new Transactions();
            {
                transaction.Transfer_Id = Convert.ToInt32(reader["transfer_id"]);
                transaction.TransferTypeId = Convert.ToInt32(reader["transfer_type_id"]);
                transaction.Transfer_Status_Id = Convert.ToInt32(reader["transfer_status_id"]);
                transaction.Account_From = Convert.ToInt32(reader["account_from"]);
                transaction.Account_To = Convert.ToInt32(reader["account_to"]);
                transaction.Amount = Convert.ToDecimal(reader["amount"]);
                

            }

            return transaction;
        }

        private PendingTransfer GetPendingTransactionsFromReader(SqlDataReader reader)
        {
            PendingTransfer pendingTransfer = new PendingTransfer();
            {
                pendingTransfer.TransferId = Convert.ToInt32(reader["transfer_id"]);
                pendingTransfer.Amount = Convert.ToDecimal(reader["amount"]);
                pendingTransfer.FromName = Convert.ToString(reader["fromName"]);

            }
            return pendingTransfer;
        }
    }
}

    



 