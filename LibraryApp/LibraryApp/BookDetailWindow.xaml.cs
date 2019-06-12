using DataObjects;
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

namespace LibraryApp
{
    /// <summary>
    /// Interaction logic for BookDetailWindow.xaml
    /// </summary>
    public partial class BookDetailWindow : Window
    {
        private BookManager _bookManager = new BookManager();
        private Author currentAuthor = new Author();
        private List<Author> authors = new List<Author>();
        private AuthorManager _authorManager = new AuthorManager();
        private VMBook _oldBook;
        private VMBook _currentBook;

        public BookDetailWindow()
        {
            InitializeComponent();
            setupAuthorIds();
            txtStatus.Text = "Active";
            setupEditing();
            
        }

        public BookDetailWindow(VMBook book, Employee employee)
        {
            InitializeComponent();

            _oldBook = book;
            setupBook();
            setupViewing();
            if (employee != null)
            {
                setupEmployeeView();
            }
        }

        private void setupEmployeeView()
        {
            btnBack.Visibility = Visibility.Visible;
            btnEdit.Visibility = Visibility.Visible;
            btnSave.Visibility = Visibility.Hidden;
            btnAdd.Visibility = Visibility.Hidden;
            btnCancel.Visibility = Visibility.Hidden;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void setupBook()
        {
            txtBookID.Text = _oldBook.BookID;
            txtTitle.Text = _oldBook.Title;
            setupAuthorIds();
            cboAuthorID.SelectedItem = _oldBook.AuthorID;
            txtAuthorName.Text = _oldBook.AuthorName;
            txtPublisher.Text = _oldBook.Publisher;
            txtGenre.Text = _oldBook.Genre;
            txtDescription.Text = _oldBook.Description;
            txtStatus.Text = _oldBook.Status;
            

        }

        private void setupAuthorIds()
        {
            try
            {
                authors = _authorManager.RetrieveAllAuthors();
                cboAuthorID.ItemsSource = _authorManager.RetrieveAllAuthorIDs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load Authors \n" + ex.Message);
            }
        }



        private void setupEditing()
        {
            txtBookID.IsReadOnly = false;
            txtTitle.IsReadOnly = false;
            cboAuthorID.IsEnabled = true;
            txtAuthorName.IsReadOnly = true;
            txtPublisher.IsReadOnly = false;
            txtGenre.IsReadOnly = false;
            txtDescription.IsReadOnly = false;
            txtStatus.IsReadOnly = true;
            btnSave.Visibility = Visibility.Visible;
            btnEdit.Visibility = Visibility.Hidden;
            btnBack.Visibility = Visibility.Hidden;
            btnCancel.Visibility = Visibility.Visible;

        }
        private void setupViewing()
        {
            txtBookID.IsReadOnly = true;
            txtTitle.IsReadOnly = true;
            cboAuthorID.IsEnabled = false;
            txtAuthorName.IsReadOnly = true;
            txtPublisher.IsReadOnly = true;
            txtGenre.IsReadOnly = true;
            txtDescription.IsReadOnly = true;
            txtStatus.IsReadOnly = true;
            btnAdd.Visibility = Visibility.Hidden;
            btnBack.Visibility = Visibility.Visible;
            btnCancel.Visibility = Visibility.Hidden;
            btnEdit.Visibility = Visibility.Hidden;
            btnSave.Visibility = Visibility.Hidden;
        }


        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (checkFields())
            {
                addNewBook();
            }
        }

        private bool checkFields()
        {
            bool isGoodData = true;
            if (txtBookID.Text == "" || txtBookID.Text == null || txtBookID.Text.Length > 15)
            {
                isGoodData = false;
                MessageBox.Show("Invalid entry for ISBN, please try again.");
            }
            else if (txtTitle.Text == "" || txtTitle.Text == null || txtTitle.Text.Length > 75)
            {
                isGoodData = false;
                MessageBox.Show("Invalid entry for Title, please try again.");
            }
            else if (cboAuthorID.SelectedIndex == -1)
            {
                isGoodData = false;
                MessageBox.Show("Must select an author for the book, please try again.");
            }
            else if (txtPublisher.Text == "" || txtPublisher.Text == null || txtPublisher.Text.Length > 50)
            {
                isGoodData = false;
                MessageBox.Show("Invalid entry for Publisher, please try again.");
            }
            else if (txtGenre.Text == "" || txtGenre.Text == null || txtGenre.Text.Length > 20)
            {
                isGoodData = false;
                MessageBox.Show("Invalid entry for Genre, please try again.");
            }
            else if (txtDescription.Text.Length > 200)
            {
                isGoodData = false;
                MessageBox.Show("Invalid entry for Description, please try again.");
            }
            return isGoodData;

        }

        private void addNewBook()
        {
            setupCurrentBook();

            try
            {
                if (_bookManager.AddBook(_currentBook))
                {
                    MessageBox.Show("Book Successfully Added");
                    this.DialogResult = true;
                }
                
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add book: \n" + ex.Message);
            }
        }

        private void setupCurrentBook()
        {
            string bookID = txtBookID.Text;
            string title = txtTitle.Text;
            string authorID = cboAuthorID.SelectedItem.ToString();
            string authorName = txtAuthorName.Text;
            string publisher = txtPublisher.Text;
            string genre = txtGenre.Text;
            string description = txtDescription.Text;
            string status = txtStatus.Text;
            _currentBook = new VMBook(bookID, title, authorID, publisher, genre, description, status, authorName);
        }

        private void cboAuthorID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                currentAuthor = authors.FirstOrDefault(a => a.AuthorID ==
                cboAuthorID.SelectedItem.ToString());
                txtAuthorName.Text = currentAuthor.FirstName + " " + currentAuthor.LastName;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Loading Author Name \n" + ex.Message);

            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
              this.Close();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            setupEditing();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (checkFields())
            {
                updateBook();
            }
        }

        private void updateBook()
        {
            setupCurrentBook();
            try
            {
                if (_bookManager.UpdateBook(_currentBook, _oldBook))
                {
                    MessageBox.Show("Book Successfully Updated");
                    this.DialogResult = true;
                    this.Close();
                }
                
                else
                {
                    MessageBox.Show("Failed to Update Book");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to update book: \n" + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to stop? Changes will be discarded.",
                "Canceling Editing...", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                if (_oldBook != null)
                {
                    setupBook();
                    setupViewing();
                }
            }
        }
    }
}

//public string BookID { get; set; }
//public string Title { get; set; }
//public string AuthorID { get; set; }
//public string AuthorName { get; set; }
//public string Publisher { get; set; }
//public string Genre { get; set; }
//public string Description { get; set; }
//public string Status { get; set; }