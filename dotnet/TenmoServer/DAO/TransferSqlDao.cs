using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.DAO
{
    public class TransferSqlDao : ITransferDao
    {
        private readonly string connectionString;

        public TransferSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
        public TransferSqlDao Transfer(int user_id, decimal amount)
        {
            //AccountBalance accountBalance = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT u.user_name, a.user_id FROM accounts a JOIN users u ON u.user_id = a.user_id : SELECT t.account_from, t.account_to, t.amount, a.account_id, a.user_id FROM transfers t JOIN accounts a ON a.account_id = t.transfer_id JOIN users u ON u.user_id = a.user_id", conn);
                    cmd.Parameters.AddWithValue("@a.user_id", user_id);
                    cmd.Parameters.AddWithValue("@t.amount", amount);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        accountBalance = GetBalanceFromReader(reader);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return accountBalance;
        }



    }
}
