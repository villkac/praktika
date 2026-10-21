using System;
using System.Data;
using System.IO;
using System.Windows;
namespace PasswordManager
{
    public partial class LoginWindow : Window
    {
        private bool isRegister = false;   // false = вход, true = регистрация
        public LoginWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!File.Exists("passwords.db"))
            {
                MessageBox.Show("Не найден файл passwords.db рядом с программой.");
                Application.Current.Shutdown();
            }
        }
        // Переключение между входом и регистрацией
        private void BtnSwitch_Click(object sender, RoutedEventArgs e)
        {
            SwitchMode();
        }
        private void SwitchMode()
        {
            isRegister = !isRegister;
            if (isRegister)
            {
                txtTitle.Text = "Регистрация";
                txtSubtitle.Text = "Придумайте логин и пароль";
                btnMain.Content = "Создать аккаунт";
                btnSwitch.Content = "Уже есть аккаунт? Войти";
                confirmPanel.Visibility = Visibility.Visible;
            }
            else
            {
                txtTitle.Text = "Вход";
                txtSubtitle.Text = "Войдите, чтобы открыть свои пароли";
                btnMain.Content = "Войти";
                btnSwitch.Content = "Нет аккаунта? Регистрация";
                confirmPanel.Visibility = Visibility.Collapsed;
            }
        }      
        private void BtnMain_Click(object sender, RoutedEventArgs e)
        {
            if (isRegister)
                Register();
            else
                Login();
        }
        // Регистрация
        private void Register()
        {
            string login = txtLogin.Text;
            string password = pwdLogin.Password;

            if (login.Length < 3 || password.Length < 6)
            {
                MessageBox.Show("Логин — минимум 3 символа, пароль — минимум 6.");
                return;
            }
            if (password != pwdConfirm.Password)
            {
                MessageBox.Show("Пароли не совпадают.");
                return;
            }
            if (Database.GetUser(login).Rows.Count > 0)
            {
                MessageBox.Show("Такой логин уже занят.");
                return;
            }  
            // создание соли
            string authSalt = Crypto.NewSalt(); // для проверки пароля при входе
            string keySalt = Crypto.NewSalt(); // для получения ключа шифрования
            string hash = Crypto.Hash(password, authSalt);

             Database.AddUser(login, hash, authSalt, keySalt);          
            Login(); // вход сразу после регистрации
        }
        // Вход
        private void Login()
        {
            string login = txtLogin.Text;
            string password = pwdLogin.Password;

            DataTable user = Database.GetUser(login);
            if (user.Rows.Count == 0)
            {
                MessageBox.Show("Неверный логин или пароль.");
                return;
            }
            DataRow row = user.Rows[0];

            // проверка пароля с солью загруженной из бд
            string hash = Crypto.Hash(password, row["AuthSalt"].ToString());
            if (hash != row["PasswordHash"].ToString()) 
            {
                MessageBox.Show("Неверный логин или пароль.");
                return;
            }
            int userId = Convert.ToInt32(row["Id"]);
            byte[] key = Crypto.MakeKey(password, row["KeySalt"].ToString());

            MainWindow main = new MainWindow(userId, login, key);
            main.Show();
            Close(); // закрытие окна входа
        }
    }
}