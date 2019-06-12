using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        //EmployeeID
        //FirstName
        //LastName
        //PhoneNumber
        //Email
        //PasswordHash
        //Active
        //RoleID

        public Employee(int employeeID, string firstName, string lastName, string phone, string role)
        {
            EmployeeID = employeeID;
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            Role = role;
        }

        public Employee(int employeeID, string firstName, string lastName, string phone, string email, string role)
        {
            EmployeeID = employeeID;
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            Email = email;
            Role = role;
        }

        public Employee(string firstName, string lastName, string phone, string email, string role)
        {
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            Email = email;
            Role = role;
        }
    }
}
