using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class TransferSqlDao
    {
        private readonly string connectionString;

        public TransferSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
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
                        "UPDATE accounts SET balance = balance + @amount WHERE user_id = @userId_to : " +
                        "INSERT INTO transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount) VALUES (2, 2, @userId_from, @userId_to, @amount)", conn);

                    cmd.Parameters.AddWithValue("@amount", amount);
                    cmd.Parameters.AddWithValue("@userId_from", userId_from);
                    cmd.Parameters.AddWithValue("@userId_to", userId_to);

                    SqlDataReader reader = cmd.ExecuteReader();

                    return success;

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

    



 