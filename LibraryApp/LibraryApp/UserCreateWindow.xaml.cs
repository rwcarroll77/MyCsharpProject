using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LogicLayer;
using DataObjects;

namespace LibraryApp
{
    /// <summary>
    /// Interaction logic for UserCreateWindow.xaml
    /// </summary>
    public partial class UserCreateWindow : Window
    {
        private RolesManager _rolesManager = new RolesManager();
        private bool employeeQuestionMark;
        private CustomerManager customerManager = new CustomerManager();
        private EmployeeManager employeeManager = new EmployeeManager();
        
        public UserCreateWindow(bool isEmployee)
        {
            employeeQuestionMark = isEmployee;
            InitializeComponent();
            if (isEmployee)
            {
                setupEmployee();
            }
            else
            {
                setupCustomer();
            }

        }

        private void setupCustomer()
        {
            lblRole.Visibility = Visibility.Hidden;
            cboRole.Visibility = Visibility.Hidden;
        }

        private void setupEmployee()
        {
            setupRoles();
        }

        private void setupRoles()
        {
            try
            {
                cboRole.ItemsSource = _rolesManager.RetrieveAllRoles();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading roles\n" + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (employeeQuestionMark)
            {
                string FirstName = txtFirstName.Text;
                string LastName = txtLastName.Text;
                string Phone = txtPhone.Text;
                string Email = txtEmail.Text;
                string Role = cboRole.SelectedItem.ToString();
                Employee employee = new Employee(FirstName, LastName, Phone, Email, Role);
                try
                {
                    if (employeeManager.AddEmployee(employee))
                    {
                        MessageBox.Show("Employee Successfully Added!");
                        this.DialogResult = true;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Failed to add Employee");
                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Saving Employee\n" + ex.Message);
                }
            }
            else
            {
                string FirstName = txtFirstName.Text;
                string LastName = txtLastName.Text;
                string Phone = txtPhone.Text;
                string Email = txtEmail.Text;
                Customer customer = new Customer(FirstName, LastName, Phone, Email);
                try
                {
                    if (customerManager.AddCustomer(customer))
                    {
                        MessageBox.Show("Customer successfully added!");
                        this.DialogResult = true;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Failed to add customer");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Adding Customer.\n" + ex.Message);
                }
            }
        }
    }
}
