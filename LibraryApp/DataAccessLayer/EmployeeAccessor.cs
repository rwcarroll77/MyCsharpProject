using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;

namespace DataAccessLayer
{
    public class EmployeeAccessor
    {  
        public static int VerifyUsernameAndPassword(string username, string password)
        {
            int result = 0;

            var conn = Connection.GetDbConnection();
            string cmdText = @"sp_authenticate_employee";
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

        public static Employee RetrieveEmployeeByEmail(string email)
        {
            Employee employee = null;

            var conn = Connection.GetDbConnection();
            var cmdText = @"sp_retrieve_employee_by_email";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 250);
            cmd.Parameters["@Email"].Value = email;
            int employeeID = 0;
            string firstName = null;
            string lastName = null;
            string phone = null;
            string role = null;

            try
            {
                conn.Open();

                var r = cmd.ExecuteReader();

                if (r.HasRows)
                {
                    r.Read();
                    employeeID = r.GetInt32(0);
                    firstName = r.GetString(1);
                    lastName = r.GetString(2);
                    phone = r.GetString(3);
                    role = r.GetString(4);
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

            employee = new Employee(employeeID, firstName, lastName, phone, role);
            return employee;
        }

        public static int UpdatePassword(string email, string oldPassword, string newPassword)
        {
            int result = 0;

            var conn = Connection.GetDbConnection();
            var cmdText = @"sp_update_employee_password";
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

        public static List<Employee> SelectAllEmployees(string email)
        {
            List<Employee> employees = new List<Employee>();

            var cmdText = @"sp_select_all_employees";
            var conn = Connection.GetDbConnection();
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Email", email);

            try
            {
                conn.Open();
                var r = cmd.ExecuteReader();
                if (r.HasRows)
                {
                    while (r.Read())
                    {
                        int EmployeeID = r.GetInt32(0);
                        string FirstName = r.GetString(1);
                        string LastName = r.GetString(2);
                        string Phone = r.GetString(3);
                        string Email = r.GetString(4);
                        string Role = r.GetString(5);
                        Employee employee = new Employee(EmployeeID, FirstName, LastName, Phone, Email, Role);
                        employees.Add(employee);
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

            return employees;
        }

        public static int InsertEmployee(Employee employee)
        {
            int result = 0;

            var cmdText = @"sp_insert_employee";
            var conn = Connection.GetDbConnection();
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
            cmd.Parameters.AddWithValue("@LastName", employee.LastName);
            cmd.Parameters.AddWithValue("@PhoneNumber", employee.Phone);
            cmd.Parameters.AddWithValue("@Email", employee.Email);
            cmd.Parameters.AddWithValue("@RoleID", employee.Role);

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

        public static int DeactivateEmployee(int employeeID)
        {
            int result = 0;

            var cmdText = @"sp_deactivate_employee";
            var conn = Connection.GetDbConnection();
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmployeeID", employeeID);

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

        public static int DeactivateEmployeeByEmail(string email)
        {
            int result = 0;

            var cmdText = "sp_deactivate_employee_by_email";
            var conn = Connection.GetDbConnection();
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Email", email);

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
