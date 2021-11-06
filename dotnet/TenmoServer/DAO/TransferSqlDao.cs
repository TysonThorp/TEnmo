using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class TransferSqlDao : ITransferDao
    {
        private readonly string connectionString;

        public TransferSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public TransferSqlDao()
        {
        }



        /*
         * (done) NEED A NEW METHOD FOR THE INSERT BELOW (tracking transaction)
         * (done) --- note this will be fed stuff from the client side --- NEED A METHOD TO DERIVE ACCOUNT FROM AND ACCOUNT TO ID FROM THE USER ID (for the insert)
         *  (not done) OPTIONAL BUT HELPFUL IF WANT TO DO SOME OPTIONAL STUFF - SPLIT UP THE ADD AND SUBTRACT
         ***** REMEMBER CODERS - THE CLIENT IS GOING TO CALL THE METHODS TO MAKE IT HAPPEN, IE: DON'T MAKE THE SERVER DO THINGS THE CLIENT SHOULD DO
                WHEN INITIATING A TRANSFER THE CLIENT WILL CALL THE METHODS TO UPDATE THE BALANCES AND ALSO TO INSERT A TRANSACTION RECORD *****
         */
        public string Transfer(int userId_from, int userId_to, decimal amount)
        {
            //AccountBalance accountBalance = null;
            string success = "Transfer Successful";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();



                    SqlCommand cmd = new SqlCommand("UPDATE accounts SET balance = balance - @amount WHERE user_id = @userId_from : " +
                        "UPDATE accounts SET balance = balance + @amount WHERE user_id = @userId_to)", conn);
                    cmd.Parameters.AddWithValue("@amount", amount);
                    cmd.Parameters.AddWithValue("@userId_from", userId_from);
                    cmd.Parameters.AddWithValue("@userId_to", userId_to);


                    cmd.ExecuteNonQuery();

                    return success;

                }
            }
            catch (SqlException)
            {
                throw new Exception("Error adding transfer");
            }
        }
        public void AddTransfer(int account_from_id, int account_to_id, decimal amount)
        {


            try
            {
                using SqlConnection conn = new SqlConnection(connectionString);
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO transfers (transfer_tye_id, transfer_status_id, account_from, account_to, amount) VALUES (2, 2, @account_from_id, @account_to_id, @amount)", conn);
                    cmd.Parameters.AddWithValue("@account_from_id", account_from_id);
                    cmd.Parameters.AddWithValue("@account_to_id", account_to_id);
                    cmd.Parameters.AddWithValue("@amount", amount);
                    cmd.ExecuteNonQuery();

                }
            }
            catch (SqlException)
            {
                throw new Exception("Error adding transfer");
            }
        }



        public List<Transaction> GetAllTransactions(int userId)
        {
            List<Transaction> transactions = new List<Transaction>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM transfers", conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Transaction transaction = GetTransactionFromReader(reader);
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

        public Transaction GetTransactionById(int transferId)
        {
            Transaction transaction = null;

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
        private Transaction GetTransactionFromReader(SqlDataReader reader)
        {
            Transaction transaction = new Transaction();
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

    



 