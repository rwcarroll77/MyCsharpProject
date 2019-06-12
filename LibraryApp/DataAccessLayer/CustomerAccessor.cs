using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;

namespace DataAccessLayer
{
    public class CustomerAccessor
    { 
        public static int VerifyUsernameAndPassword(string username, string password)
        {
            int result = 0;

            var conn = Connection.GetDbConnection();
            string cmdText = @"sp_authenticate_user";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 250);
            cmd.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, 100);

            cmd.Parameters["@Email"].Value = username;
            cmd.Parameters["@PasswordHash"].Value = password;

            try
            {
                conn.Open();

                result = (int)cmd.ExecuteScalar();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public static Customer RetrieveUserByEmail(string email)
        {
            Customer customer = null;

            var conn = Connection.GetDbConnection();
            var cmdText = @"sp_retrieve_user_by_email";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 250);
            cmd.Parameters["@Email"].Value = email;

            int customerID = 0;
            string firstName = null;
            string lastName = null;
            string phone = null;

            try
            {
                conn.Open();

                var r = cmd.ExecuteReader();

                if (r.HasRows)
                {
                    r.Read();
                    customerID = r.GetInt32(0);
                    firstName = r.GetString(1);
                    lastName = r.GetString(2);
                    phone = r.GetString(3);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }

            customer = new Customer(customerID, firstName, lastName, phone);

            return customer;
        }

        public static List<Customer> SelectAllCustomers()
        {
            List<Customer> customers = new List<Customer>();

            var cmdText = @"sp_select_all_users";
            var conn = Connection.GetDbConnection();
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                conn.Open();

                var r = cmd.ExecuteReader();
                if (r.HasRows)
                {
                    while (r.Read())
                    {
                        int CustomerID = r.GetInt32(0);
                        string FirstName = r.GetString(1);
                        string LastName = r.GetString(2);
                        string Phone = r.GetString(3);
                        string Email = r.GetString(4);
                        Customer customer = new Customer(CustomerID, FirstName, LastName, Phone, Email);
                        customers.Add(customer);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }

            return customers;
        } 

        public static int InsertCustomer(Customer customer)
        {
            int result = 0;

            var cmdText = @"sp_insert_customer";
            var conn = Connection.GetDbConnection();
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FirstName", customer.FirstName);
            cmd.Parameters.AddWithValue("@LastName", customer.LastName);
            cmd.Parameters.AddWithValue("@PhoneNumber", customer.Phone);
            cmd.Parameters.AddWithValue("@Email", customer.Email);

            try
            {
                conn.Open();
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public static int DeactivateCustomer(int customerID)
        {
            int result = 0;

            var cmdText = @"sp_deactivate_customer";
            var conn = Connection.GetDbConnection();
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustomerID", customerID);

            try
            {
                conn.Open();
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public static int UpdatePassword(string email, string oldPassword, string newPassword)
        {
            int result = 0;

            var conn = Connection.GetDbConnection();
            var cmdText = @"sp_update_user_password";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;


            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 250);
            cmd.Parameters.Add("@OldPasswordHash", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@NewPasswordHash", SqlDbType.NVarChar, 100);

            cmd.Parameters["@Email"].Value = email;
            cmd.Parameters["@OldPasswordHash"].Value = oldPassword;
            cmd.Parameters["@NewPasswordHash"].Value = newPassword;

            try
            {
                conn.Open();

                result = cmd.ExecuteNonQuery();

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

    }
}
