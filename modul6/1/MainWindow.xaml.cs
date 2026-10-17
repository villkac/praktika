using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using _1;
namespace EmployeeApp
{
    public partial class MainWindow : Window // класс наследуется от стандартного окна WPF
    {
        private Database database = new Database(); // создание таблицы, тип объекта, класс из Database.cs
        public MainWindow()
        {
            InitializeComponent();
            LoadEmployees();
        }
        private void LoadEmployees() // получение сотрудников из бд
        {
            dgEmployees.ItemsSource = database.GetEmployees().DefaultView;
        }    
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (txtFullName.Text.Trim() == "" ||
                txtPosition.Text.Trim() == "")
            {
                MessageBox.Show("Введите ФИО и должность.");
                return;
            }
            database.AddEmployee(txtFullName.Text, txtPosition.Text);

            LoadEmployees();
            ClearFields();
        }     
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (dgEmployees.SelectedItem == null) // проыерка, выбрана ли запись
                return; 

            DataRowView row = (DataRowView)dgEmployees.SelectedItem;

            database.UpdateEmployee(
                Convert.ToInt32(row["Id"]),
                txtFullName.Text,
                txtPosition.Text);

            LoadEmployees();
            ClearFields();
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dgEmployees.SelectedItem == null) return;

            DataRowView row = (DataRowView)dgEmployees.SelectedItem; // полчуение выбранной строки

            database.DeleteEmployee(Convert.ToInt32(row["Id"]));

            LoadEmployees();
            ClearFields();
        }
        private void dgEmployees_SelectionChanged(
            object sender, SelectionChangedEventArgs e)
        {
            if (dgEmployees.SelectedItem == null) return;

            DataRowView row = (DataRowView)dgEmployees.SelectedItem;

            txtFullName.Text = row["FullName"].ToString() ?? "";
            txtPosition.Text = row["Position"].ToString() ?? "";
        }
        private void ClearFields_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            txtFullName.Clear();
            txtPosition.Clear();

            dgEmployees.SelectedItem = null; // снимает выделение с выбранной строки таблицы
        }
    }
}