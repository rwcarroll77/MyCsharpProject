using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class Customer
    {
        public Customer(int customerID, string firstName, string lastName, string phone)
        {
            CustomerID = customerID;
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
        }

        public Customer(string firstName, string lastName, string phone, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            Email = email;
        }

        public Customer(int customerID, string firstName, string lastName, string phone, string email)
        {
            CustomerID = customerID;
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            Email = email;
        }

        public int CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
