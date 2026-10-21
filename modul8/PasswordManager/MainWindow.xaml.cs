using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
namespace PasswordManager
{
    public partial class MainWindow : Window
    {
        private int userId;     // кто вошёл
        private byte[] key;     // ключ для шифрования и расшифровки паролей
        public MainWindow(int userId, string login, byte[] key)
        {
            this.userId = userId;
            this.key = key; // Ключ, переданный из окна входа, сохраняется в поле класса

            InitializeComponent();

            txtUser.Text = "Мои пароли — " + login; 
            cmbCategory.ItemsSource = Database.GetCategories().DefaultView; // загрузка категорий
            LoadEntries(); // загрузка записей
        }       
        private void LoadEntries()
        {
            grid.ItemsSource = Database.GetEntries(userId, txtSearch.Text).DefaultView;
        }
        // Поиск
        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadEntries();
        }
        // Выход: возвращаемся на окно входа
        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            Close();
        }
        // Заполнение полей при выборе записи
        private void Grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (grid.SelectedItem == null)
                return;

            DataRowView row = (DataRowView)grid.SelectedItem; // получение выбранной записи
            txtService.Text = row["ServiceName"].ToString();
            txtEntryLogin.Text = row["Login"].ToString();
            txtNotes.Text = row["Notes"].ToString();

            if (row["CategoryId"] == DBNull.Value)
                cmbCategory.SelectedIndex = -1;
            else
                cmbCategory.SelectedValue = row["CategoryId"];

            // расшифровка пароля
            pwdEntry.Password = Crypto.Decrypt(
                row["EncryptedPassword"].ToString(), key, row["IV"].ToString());
        }
        // Добавление записи
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (txtService.Text == "" || txtEntryLogin.Text == "" || pwdEntry.Password == "")
            {
                MessageBox.Show("Заполните сервис, логин и пароль.");
                return;
            }

            string iv = Crypto.NewSalt(); // получение нового значения для пароля                                
            string encrypted = Crypto.Encrypt(pwdEntry.Password, key, iv);   // шифрование пароля
            object categoryId = cmbCategory.SelectedValue ?? DBNull.Value;  // категория может быть пустой
            // сохранение в бд
            Database.AddEntry(userId, categoryId, txtService.Text,txtEntryLogin.Text, encrypted, iv, txtNotes.Text);

            LoadEntries();
            ClearFields();
        }

        // Изменение записи
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (grid.SelectedItem == null)
            {
                MessageBox.Show("Выберите запись в таблице.");
                return;
            }
            if (txtService.Text == "" || txtEntryLogin.Text == "" || pwdEntry.Password == "")
            {
                MessageBox.Show("Заполните сервис, логин и пароль.");
                return;
            }
            // получаение выбранной записи
            DataRowView row = (DataRowView)grid.SelectedItem;
            int id = Convert.ToInt32(row["Id"]);

            string iv = Crypto.NewSalt();
            string encrypted = Crypto.Encrypt(pwdEntry.Password, key, iv);
            object categoryId = cmbCategory.SelectedValue ?? DBNull.Value;
            // передача новых данных
            Database.UpdateEntry(id, categoryId, txtService.Text,
                txtEntryLogin.Text, encrypted, iv, txtNotes.Text);

            LoadEntries();
            ClearFields();
        }
        // Удаление записи
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (grid.SelectedItem == null)
            {
                MessageBox.Show("Выберите запись для удаления.");
                return;
            }

            MessageBoxResult answer = MessageBox.Show("Удалить выбранную запись?",
                "Подтверждение", MessageBoxButton.YesNo);
            if (answer != MessageBoxResult.Yes)
                return;

            DataRowView row = (DataRowView)grid.SelectedItem;
            Database.DeleteEntry(Convert.ToInt32(row["Id"]));

            LoadEntries();
            ClearFields();
        }
        // Очистка полей
        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
        }
        private void ClearFields()
        {
            txtService.Clear();
            txtEntryLogin.Clear();
            pwdEntry.Clear();
            txtNotes.Clear();
            cmbCategory.SelectedIndex = -1;
            grid.SelectedItem = null;
        }
        // Показать пароль 
        private void BtnShow_Click(object sender, RoutedEventArgs e)
        {
            if (grid.SelectedItem == null)
            {
                MessageBox.Show("Выберите запись в таблице.");
                return;
            }

            DataRowView row = (DataRowView)grid.SelectedItem; // получение выбранной записи 
            string password = Crypto.Decrypt( // расшифровка
                row["EncryptedPassword"].ToString(), key, row["IV"].ToString());

            MessageBox.Show("Сервис: " + row["ServiceName"] + "\nЛогин: " + row["Login"] +
                "\nПароль: " + password);
        }
        // Скопировать пароль в буфер обмена
        private void BtnCopy_Click(object sender, RoutedEventArgs e)
        {
            if (grid.SelectedItem == null)
            {
                MessageBox.Show("Выберите запись в таблице.");
                return;
            }

            DataRowView row = (DataRowView)grid.SelectedItem;
            string password = Crypto.Decrypt(
                row["EncryptedPassword"].ToString(), key, row["IV"].ToString());

            Clipboard.SetText(password); // копирование
            MessageBox.Show("Пароль скопирован.");
        }
    }
}