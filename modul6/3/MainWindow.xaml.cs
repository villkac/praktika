using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp3
{
    public partial class MainWindow : Window
    {
        private Database database = new Database();

        public MainWindow()
        {
            InitializeComponent();
            LoadBooks();
        }
        // Загрузка книг
        private void LoadBooks()
        {
            dgBooks.ItemsSource =
                database.GetBooks(txtSearch.Text).DefaultView; // передача данных из бд в таблицу
        }
        // Добавление книги
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            string title = txtTitle.Text;
            string author = txtAuthor.Text;

            if (title == "" || author == "")
            {
                MessageBox.Show("Введите название книги и автора.");
                return;
            }

            database.AddBook(title, author);

            LoadBooks();
            ClearFields();
        }

        // Поиск книг
        private void Search_TextChanged(
            object sender, TextChangedEventArgs e)
        {
            LoadBooks(); // передаёт текст поиска в GetBooks
        }

        // Аренда книги
        private void Rent_Click(object sender, RoutedEventArgs e)
        {
            if (dgBooks.SelectedItem == null)
            {
                MessageBox.Show("Выберите книгу.");
                return;
            }

            DataRowView row = (DataRowView)dgBooks.SelectedItem;

            if (Convert.ToBoolean(row["IsRented"]))
            {
                MessageBox.Show("Эта книга уже в аренде.");
                return;
            }

            int id = Convert.ToInt32(row["Id"]);

            database.SetRentalStatus(id, true); // изменение статуса

            LoadBooks();
            ClearFields();
        }
        // Возврат книги
        private void Return_Click(object sender, RoutedEventArgs e)
        {
            if (dgBooks.SelectedItem == null)
            {
                MessageBox.Show("Выберите книгу.");
                return;
            }

            DataRowView row = (DataRowView)dgBooks.SelectedItem;

            if (!Convert.ToBoolean(row["IsRented"]))
            {
                MessageBox.Show("Эта книга не находится в аренде.");
                return;
            }

            int id = Convert.ToInt32(row["Id"]);

            database.SetRentalStatus(id, false);

            LoadBooks();
            ClearFields();
        }
        // Удаление книги
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dgBooks.SelectedItem == null)
            {
                MessageBox.Show("Выберите книгу для удаления.");
                return;
            }

            DataRowView row = (DataRowView)dgBooks.SelectedItem; // получаем выбранную запись
            int id = Convert.ToInt32(row["Id"]);

            database.DeleteBook(id);

            LoadBooks();
            ClearFields();
        }
        // Заполнение полей при выборе книги
        private void dgBooks_SelectionChanged(
            object sender, SelectionChangedEventArgs e)
        {
            if (dgBooks.SelectedItem == null)
                return;

            DataRowView row = (DataRowView)dgBooks.SelectedItem;

            txtTitle.Text = row["Title"].ToString();
            txtAuthor.Text = row["Author"].ToString();
        }

        // Очистка полей
        private void ClearFields_Click(
            object sender, RoutedEventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            txtTitle.Clear();
            txtAuthor.Clear();
            dgBooks.SelectedItem = null;
        }
    }
}