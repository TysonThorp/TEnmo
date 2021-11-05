using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.DAO
{
    public class TransferSqlDao 
    {
        private readonly string connectionString;

        public TransferSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
        public string Transfer(int user_id, decimal amount)
        {
            //AccountBalance accountBalance = null;
            string success = "Transfer Successful";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("UPDATE transfers SET balance = balance - amount FROM transfers t JOIN accounts a ON a.account_id = transfers.account_from :" +
                        " UPDATE transfers SET balance = balance + amount FROM transfers t JOIN accounts a ON a.account_id = transfers.account_from : " +
                        " INSERT INTO transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount) VALUES (2, 2, account_from, account_to, amount)", conn);
                   

                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    return success;

                }
            }
            catch (SqlException)
            {
                throw new Exception("Error adding transfer");
            }

       
        }



    }
}
