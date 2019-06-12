using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using DataObjects;

namespace LogicLayer
{
    public class EmployeeManager
    {

        public Employee AuthenticateUser(string username, string password)
        {
            Employee employee = null;

            password = hashSHA256(password);

            try
            {

                if (1 == EmployeeAccessor.VerifyUsernameAndPassword(username, password))
                {
                    employee = EmployeeAccessor.RetrieveEmployeeByEmail(username);
                }
                else
                {
                    throw new ApplicationException("Employee not found");
                }

            }
            catch (Exception ex)
            {

                throw new ApplicationException("Employee not validated. ", ex);
            }

            return employee;
        }

        public List<Employee> RetrieveAllEmployees(string email)
        {
            List<Employee> employees = null;

            try
            {
                employees = EmployeeAccessor.SelectAllEmployees(email);
            }
            catch (Exception)
            {

                throw;
            }

            return employees;
        }

        public bool AddEmployee(Employee employee)
        {
            bool result = false;

            try
            {
                result = (1 == EmployeeAccessor.InsertEmployee(employee));
            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }

        public bool DeactivateEmployee(int employeeID)
        {
            bool result = false;

            try
            {
                result = (1 == EmployeeAccessor.DeactivateEmployee(employeeID));
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }

        public bool DeactivateEmployeeByEmail(string email)
        {
            bool result = false;

            try
            {
                result = (1 == EmployeeAccessor.DeactivateEmployeeByEmail(email));
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }

        private string hashSHA256(string source)
        {
            string result = "";
            
            byte[] data;
            
            using (SHA256 sha256hash = SHA256.Create())
            {
                data = sha256hash.ComputeHash(Encoding.UTF8.GetBytes(source));
            }
            var s = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                s.Append(data[i].ToString("x2"));
            }
            result = s.ToString();

            return result;
        }
    }
}
