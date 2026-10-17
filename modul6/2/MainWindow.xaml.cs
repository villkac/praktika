using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
namespace TaskManagerApp
{
    public partial class MainWindow : Window
    {
        private Database database = new Database();
        public MainWindow()
        {
            InitializeComponent();
            LoadTasks();
        }
        private void LoadTasks()
        {
            dgTasks.ItemsSource = database.GetTasks().DefaultView; // получение таблицы
        }
        // Добавление задачи
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (txtTaskName.Text == "")
            {
                MessageBox.Show("Введите название задачи.");
                return;
            }
            database.AddTask(txtTaskName.Text, txtDescription.Text);

            LoadTasks();
            ClearFields();
        }
        // Изменение задачи
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (dgTasks.SelectedItem == null) // выбрана ли задача
                return;
            DataRowView row = (DataRowView)dgTasks.SelectedItem; // сохранение выбранной строки в переменную row

            database.UpdateTask(
                Convert.ToInt32(row["Id"]), // перобразование ID
                txtTaskName.Text,
                txtDescription.Text);

            LoadTasks();
            ClearFields();
        }

        // Удаление задачи
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dgTasks.SelectedItem == null) return;

            DataRowView row = (DataRowView)dgTasks.SelectedItem; 

            database.DeleteTask(Convert.ToInt32(row["Id"]));

            LoadTasks();
            ClearFields();
        }

        // Сохранение состояния галочки
        private void TaskStatus_Click(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            DataRowView row = (DataRowView)checkBox.DataContext; // получение строки к оторой относится галочка

            int id = Convert.ToInt32(row["Id"]);
            bool isCompleted = checkBox.IsChecked == true; // проверка состояния флажка

            database.UpdateTaskStatus(id, isCompleted); // сохранение
        }
        // При выборе задачи заполняем поля
        private void dgTasks_SelectionChanged(
            object sender, SelectionChangedEventArgs e)
        {
            if (dgTasks.SelectedItem == null) return;

            DataRowView row = (DataRowView)dgTasks.SelectedItem;
            // перенос выбранной задачи и описания в поле ввода
            txtTaskName.Text = row["TaskName"].ToString() ?? "";  
            txtDescription.Text = row["Description"].ToString() ?? "";
        }
        // Очистка полей
        private void ClearFields_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            txtTaskName.Clear();
            txtDescription.Clear();
            dgTasks.SelectedItem = null;
        }
    }
}