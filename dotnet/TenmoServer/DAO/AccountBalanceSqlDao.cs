﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TenmoServer.Models;
using TenmoServer.Security;
using TenmoServer.Security.Models;

namespace TenmoServer.DAO
{
    public class AccountBalanceSqlDao : IAccountBalanceDao
    {
        private readonly string connectionString;
     
        public AccountBalanceSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
        public AccountBalance GetBalance(int user_id)
        {
            AccountBalance accountBalance = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT balance FROM accounts WHERE user_id = @user_id", conn);
                    cmd.Parameters.AddWithValue("@user_id", user_id);
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

        private AccountBalance GetBalanceFromReader(SqlDataReader reader)
        {
            AccountBalance b = new AccountBalance()
            {
               Balance = Convert.ToDecimal(reader["balance"]),
               
            };

            return b;
        }
    }
}

    