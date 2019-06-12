using DataAccessLayer;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class CustomerManager
    {

        public Customer AuthenticateUser(string username, string password)
        {
            Customer customer = null;

            password = hashSHA256(password);
            
            try
            {

                if (1 == CustomerAccessor.VerifyUsernameAndPassword(username, password))
                {
                    customer = CustomerAccessor.RetrieveUserByEmail(username);
                }
                else
                {
                    throw new ApplicationException("User not found");
                }
                
            }
            catch (Exception ex) 
            {

                throw new ApplicationException("User not validated. ", ex);
            }

            return customer;
        }

        public Customer RetrieveCustomerByEmail(string email)
        {
            Customer customer = null;

            try
            {
                customer = CustomerAccessor.RetrieveUserByEmail(email);
            }
            catch (Exception)
            {

                throw;
            }

            return customer;
        }

        public List<Customer> RetrieveAllCustomers()
        {
            List<Customer> customers = null;

            try
            {
                customers = CustomerAccessor.SelectAllCustomers();
            }
            catch (Exception)
            {

                throw;
            }

            return customers;
        }

        public bool AddCustomer(Customer customer)
        {
            bool result = false;

            try
            {
                result = (1 == CustomerAccessor.InsertCustomer(customer));
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }

        public bool DeactivateCustomer(int customerID)
        {
            bool result = false;

            try
            {
                result = (1 == CustomerAccessor.DeactivateCustomer(customerID));
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
