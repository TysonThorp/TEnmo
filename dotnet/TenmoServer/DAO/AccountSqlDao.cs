using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TenmoServer.Models;
using TenmoServer.Security;
using TenmoServer.Security.Models;

namespace TenmoServer.DAO
{
    public class AccountSqlDao : IAccountDao
    {
        private readonly string connectionString;
     
        public AccountSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
        public Account GetAccount(int userId)
        {
            Account account = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT account_id, user_id, balance FROM accounts WHERE user_id = @user_id", conn);
                    cmd.Parameters.AddWithValue("@user_id", userId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        account = GetAccountFromReader(reader);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return account;
        }

        private Account GetAccountFromReader(SqlDataReader reader)
        {
            Account account = new Account()
            {
                Balance = Convert.ToDecimal(reader["balance"]),
                AccountId = Convert.ToInt32(reader["account_id"]),
                UserId = Convert.ToInt32(reader["user_id"]),
            };

            return account;
        }
    }
}

    
