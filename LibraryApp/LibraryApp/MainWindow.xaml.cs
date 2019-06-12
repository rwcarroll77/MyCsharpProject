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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DataObjects;
using LogicLayer;

namespace LibraryApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<VMBook> bookList;
        private List<Employee> employeeList;
        private List<Customer> customerList;
        private CustomerManager _customerManager = new CustomerManager();
        private EmployeeManager _employeeManager = new EmployeeManager();
        private BookManager _bookManager = new BookManager();

        private Customer _customer;
        private Employee _employee;
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void txtEmail_GotFocus(object sender, RoutedEventArgs e)
        {
            txtEmail.SelectAll();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            HideAllTabs();
            txtEmail.Focus();
        }

        private void pwdPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            pwdPassword.SelectAll();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text;
            string password = pwdPassword.Password;
            if ("Log in" == btnSubmit.Content.ToString())
            {
                try
                {
                    _customer = _customerManager.AuthenticateUser(email, password);
                    ShowCustomerTabs();
                    HideLogin();

                }
                catch (Exception ex)
                {

                    try
                    {
                        _employee = _employeeManager.AuthenticateUser(email, password);
                        ShowEmployeeTabs();
                        if (_employee.Role == "Admin")
                        {
                            showAdminTabs();
                        }
                        HideLogin();
                    }
                    catch (Exception ex2)
                    {
                        MessageBox.Show("Email/Password Combination Invalid.");
                    }
                }
            }
            else
            {
                ShowLogin();
                HideAllTabs();
            }
        }

        private void showAdminTabs()
        {
            tabEmployeeList.Visibility = Visibility.Visible;
            tabCustomerList.Visibility = Visibility.Visible;
        }

        private void btnBorrow_Click(object sender, RoutedEventArgs e)
        {
            var book = (VMBook)bookGrid.SelectedItem;
            if (book != null)
            {
                _bookManager.BorrowBook(_customer.CustomerID, DateTime.Now, book.BookID, "Borrowed", book.Status);
                ShowAvailableBooks();
            }
        }

        private void tabAvailableBooks_GotFocus(object sender, RoutedEventArgs e)
        {
            ShowAvailableBooks();
            btnReturn.Visibility = Visibility.Hidden;
            btnBorrow.Visibility = Visibility.Visible;
        }

        private void tabBorrowedBooks_GotFocus(object sender, RoutedEventArgs e)
        {
            ShowBorrowedBooks();
            btnReturn.Visibility = Visibility.Visible;
            btnBorrow.Visibility = Visibility.Hidden;
        }
        

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            var book = (VMBook)bookGrid.SelectedItem;
            if (book != null)
            {
                _bookManager.ReturnBook(_customer.CustomerID, DateTime.Now, book.BookID, "Active", book.Status);
                ShowBorrowedBooks();
            }
        }
        private void HideLogin()
        {

            btnSubmit.Content = "Log out";
            txtEmail.Visibility = Visibility.Hidden;
            pwdPassword.Visibility = Visibility.Hidden;
            btnSubmit.IsDefault = false;
        }
        private void ShowLogin()
        {
            _customer = null;
            _employee = null;
            btnSubmit.Content = "Log in";
            txtEmail.Visibility = Visibility.Visible;
            pwdPassword.Visibility = Visibility.Visible;
            txtEmail.Text = "Email Address";
            pwdPassword.Password = "password";
            txtEmail.Focus();
            txtEmail.SelectAll();
            btnSubmit.IsDefault = true;
        }
        private void ShowCustomerTabs()
        {
            tabAvailableBooks.Visibility = Visibility.Visible;
            tabBorrowedBooks.Visibility = Visibility.Visible;
            bookGrid.Visibility = Visibility.Visible;
            btnBorrow.Visibility = Visibility.Visible;
            ShowAvailableBooks();

        }
        private void ShowEmployeeTabs()
        {
            tabEmployeeBookList.Visibility = Visibility.Visible;
            showEmployeeBookTab();
        }

        private void showEmployeeBookTab()
        {
            customerGrid.Visibility = Visibility.Hidden;
            employeeGrid.Visibility = Visibility.Hidden;
            btnDetails.Visibility = Visibility.Visible;
            btnAdd.Visibility = Visibility.Visible;
            bookGrid.Visibility = Visibility.Visible;
            btnDelete.Visibility = Visibility.Visible;
            btnAddCustomer.Visibility = Visibility.Hidden;
            btnAddEmployee.Visibility = Visibility.Hidden;
            btnDeactivateCustomer.Visibility = Visibility.Hidden;
            btnDeactivateEmployee.Visibility = Visibility.Hidden;
            ShowAvailableBooks();
        }

        private void ShowAvailableBooks()
        {
            try
            {
                bookList = _bookManager.RetrieveAllBooks();
                bookList = bookList.FindAll(b => b.Status == "Active");
                bookGrid.ItemsSource = bookList;
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to load Book List");
            }
        }
        private void HideAllTabs()
        {
            foreach (var item in tabControl.Items)
            {
                ((TabItem)item).Visibility = Visibility.Collapsed;
            }
            bookGrid.Visibility = Visibility.Hidden;
            customerGrid.Visibility = Visibility.Hidden;
            employeeGrid.Visibility = Visibility.Hidden;
            btnBorrow.Visibility = Visibility.Hidden;
            btnReturn.Visibility = Visibility.Hidden;
            btnDetails.Visibility = Visibility.Hidden;
            btnAdd.Visibility = Visibility.Hidden;
            btnDelete.Visibility = Visibility.Hidden;
            btnAddCustomer.Visibility = Visibility.Hidden;
            btnAddEmployee.Visibility = Visibility.Hidden;
            btnDeactivateCustomer.Visibility = Visibility.Hidden;
            btnDeactivateEmployee.Visibility = Visibility.Hidden;

        }
        private void ShowBorrowedBooks()
        {
            try
            {
                bookList = _bookManager.RetrieveBorrowedBooksByCustomer(_customer.CustomerID);
                bookGrid.ItemsSource = bookList;
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to load Book List");
            }
        }

        private void btnDetails_Click(object sender, RoutedEventArgs e)
        {
            var book = (VMBook)bookGrid.SelectedItem;
            if (book != null)
            {
                var _bookDetailWindow = new BookDetailWindow(book, _employee);
                _bookDetailWindow.ShowDialog();
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var bookDetailWindow = new BookDetailWindow();
            var result = bookDetailWindow.ShowDialog();
            if (result == true)
            {
                ShowEmployeeTabs();
            }
        }

        private void bookGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var book = (VMBook)bookGrid.SelectedItem;
            var _bookDetailWindow = new BookDetailWindow(book, _employee);
            _bookDetailWindow.ShowDialog();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to delete this book?",
                "Deleting the book...", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                var bookToDelete = (VMBook)bookGrid.SelectedItem;
                try
                {
                    if (_bookManager.DeleteBook(bookToDelete.BookID, bookToDelete.Title))
                    {
                        MessageBox.Show("Book Deleted Successfully");
                        ShowEmployeeTabs();
                    }
                    else
                    {
                        MessageBox.Show("Failed to Delete Book");
                    }
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Deleting Book.\n" + ex.Message);
                }
            }
        }

        private void tabEmployeeBookList_GotFocus(object sender, RoutedEventArgs e)
        {
            showEmployeeBookTab();
        }

        private void tabEmployeeList_GotFocus(object sender, RoutedEventArgs e)
        {
            showEmployeeListTab();
        }

        private void showEmployeeListTab()
        {
            customerGrid.Visibility = Visibility.Hidden;
            employeeGrid.Visibility = Visibility.Visible;
            btnDetails.Visibility = Visibility.Hidden;
            btnAdd.Visibility = Visibility.Hidden;
            bookGrid.Visibility = Visibility.Hidden;
            btnDelete.Visibility = Visibility.Hidden;
            btnAddCustomer.Visibility = Visibility.Hidden;
            btnAddEmployee.Visibility = Visibility.Visible;
            btnDeactivateCustomer.Visibility = Visibility.Hidden;
            btnDeactivateEmployee.Visibility = Visibility.Visible;
            try
            {
                employeeList = _employeeManager.RetrieveAllEmployees(_employee.EmployeeID.ToString());
                employeeGrid.ItemsSource = employeeList;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading Employee List\n" + ex.Message);
            }
        }

        private void showCustomerListTab()
        {
            customerGrid.Visibility = Visibility.Visible;
            employeeGrid.Visibility = Visibility.Hidden;
            btnDetails.Visibility = Visibility.Hidden;
            btnAdd.Visibility = Visibility.Hidden;
            bookGrid.Visibility = Visibility.Hidden;
            btnDelete.Visibility = Visibility.Hidden;
            btnAddCustomer.Visibility = Visibility.Visible;
            btnAddEmployee.Visibility = Visibility.Hidden;
            btnDeactivateCustomer.Visibility = Visibility.Visible;
            btnDeactivateEmployee.Visibility = Visibility.Hidden;
            try
            {
                customerList = _customerManager.RetrieveAllCustomers();
                customerGrid.ItemsSource = customerList;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading Customer List\n" + ex.Message);
            }
        }

        private void tabCustomerList_GotFocus(object sender, RoutedEventArgs e)
        {
            showCustomerListTab();
        }

        private void btnDeactivateCustomer_Click(object sender, RoutedEventArgs e)
        {
            var customer = (Customer)customerGrid.SelectedItem;
            if (customer != null)
            {

                var result = MessageBox.Show("Are you sure you want to delete this customer?",
                    "Deleting the customer...", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        if (_customerManager.DeactivateCustomer(customer.CustomerID))
                        {
                            MessageBox.Show("Customer Successfully Deleted");
                            showCustomerListTab();
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete Customer");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error deleting Customer\n" + ex.Message);
                    }
                }
            }
        }

        private void btnDeactivateEmployee_Click(object sender, RoutedEventArgs e)
        {
            var employee = (Employee)customerGrid.SelectedItem;
            if (employee != null)
            {

                var result = MessageBox.Show("Are you sure you want to delete this employee?",
                    "Deleting the employee...", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        if (_employeeManager.DeactivateEmployee(employee.EmployeeID))
                        {
                            MessageBox.Show("Employee Successfully Deleted");
                            showEmployeeListTab();
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete Employee");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error deleting Employee\n" + ex.Message);
                    }
                }
            }
        }

        private void btnAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            var isEmployee = true;
            var userCreate = new UserCreateWindow(isEmployee);
            var result = userCreate.ShowDialog();
            if (result == true)
            {
                showEmployeeListTab();
            }
        }

        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            var isEmployee = false;
            var userCreate = new UserCreateWindow(isEmployee);
            var result = userCreate.ShowDialog();
            if (result == true)
            {
                showCustomerListTab();
            }
        }
    }
}
