using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualBasic;
namespace ProjectManager
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadAll();
        }       
        private void LoadAll()
        {
            LoadProjects();
            LoadTasks();
            LoadEmployees();
            GenerateReport();
        }
        // проверка выбранной строки
        private DataRowView Selected(DataGrid grid, string message)
        {
            if (grid.SelectedItem is DataRowView row)
                return row;

            MessageBox.Show(message);
            return null;
        }
        private bool ConfirmDelete(string name) =>
            MessageBox.Show("Удалить выбранную " + name + "?",
                "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes;
        // загрузка данных
        private void LoadProjects() 
        {
            dgvProjects.ItemsSource = Database.GetProjects().DefaultView;
        }
        private void LoadTasks()
        {
            dgvTasks.ItemsSource = Database.GetTasks().DefaultView;
        }
        private void LoadEmployees()
        {
            dgvEmployees.ItemsSource = Database.GetEmployees().DefaultView;
        }
        private void GenerateReport()
        {
            dgvReports.ItemsSource = Database.GetReport().DefaultView;
        }
        // кнопки обновления
        private void LoadProjects_Click(object sender, RoutedEventArgs e) => LoadProjects();
        private void LoadTasks_Click(object sender, RoutedEventArgs e) => LoadTasks();
        private void LoadEmployees_Click(object sender, RoutedEventArgs e) => LoadEmployees();
        private void GenerateReport_Click(object sender, RoutedEventArgs e) => GenerateReport();        
        private void AddProject_Click(object sender, RoutedEventArgs e)
        {
            string name = Interaction.InputBox("Введите название проекта:", "Добавление проекта");
            if (string.IsNullOrWhiteSpace(name)) return;

            Database.AddProject(name);
            // обнолвение таблиц
            LoadProjects();
            LoadTasks();
            GenerateReport();
        }
        private void EditProject_Click(object sender, RoutedEventArgs e)
        {
            var row = Selected(dgvProjects, "Выберите проект.");
            if (row == null) return;

            string name = Interaction.InputBox("Введите новое название проекта:","Редактирование проекта", row["Name"].ToString());
            if (string.IsNullOrWhiteSpace(name)) return;
            // изменение в базе данных
            Database.UpdateProject(Convert.ToInt32(row["Id"]), name);
            LoadProjects();
            LoadTasks();
            GenerateReport();
        }
        private void DeleteProject_Click(object sender, RoutedEventArgs e)
        {
            var row = Selected(dgvProjects, "Выберите проект");
            if (row == null || !ConfirmDelete("проект")) return;

            Database.DeleteProject(Convert.ToInt32(row["Id"]));
            LoadAll();
        }
        // Задачи
        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            string text = Interaction.InputBox("Введите номер проекта:", "Добавление задачи");
            if (!int.TryParse(text, out int projectId))
            {
                MessageBox.Show("Введите корректный номер проекта.");
                return;
            }
            if (!Database.Exists("Projects", projectId))
            {
                MessageBox.Show("Проект не найден.");
                return;
            }
            string name = Interaction.InputBox("Введите название задачи:", "Добавление задачи");

            if (string.IsNullOrWhiteSpace(name)) return;

            Database.AddTask(projectId, name);
            LoadTasks();
            GenerateReport();
        }
        private void EditTask_Click(object sender, RoutedEventArgs e)
        {
            var row = Selected(dgvTasks, "Выберите задачу.");
            if (row == null) return;

            string name = Interaction.InputBox("Введите название задачи:","Редактирование задачи", row["Name"].ToString());
            if (string.IsNullOrWhiteSpace(name)) return;

            string text = Interaction.InputBox("Введите процент выполнения от 0 до 100:",
                "Изменение выполнения", row["Progress"].ToString());
            if (!int.TryParse(text, out int progress) ||
                progress < 0 || progress > 100)
            {
                MessageBox.Show("Процент должен быть числом от 0 до 100.");
                return;
            }
            // : отделяет значение, которое выбирается, если условие ложно
            string status = progress == 100 ? "Завершена" : progress > 0 ? "В работе" : "Не начата";
            Database.UpdateTask(Convert.ToInt32(row["Id"]), name, progress, status);

            LoadTasks();
            GenerateReport();
        }
        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            var row = Selected(dgvTasks, "Выберите задачу.");
            if (row == null || !ConfirmDelete("задачу")) return;

            Database.DeleteTask(Convert.ToInt32(row["Id"]));
            LoadTasks();
            GenerateReport();
        }
        private void AssignTask_Click(object sender, RoutedEventArgs e)
        {
            var row = Selected(dgvTasks, "Выберите задачу.");
            if (row == null) return;

            string text = Interaction.InputBox(
                "Введите номер сотрудника:",
                "Назначение сотрудника");

            if (!int.TryParse(text, out int employeeId))
            {
                MessageBox.Show("Введите корректный номер сотрудника.");
                return;
            }

            if (!Database.Exists("Employees", employeeId))
            {
                MessageBox.Show("Сотрудник не найден.");
                return;
            }

            try
            {
                Database.AssignTask(Convert.ToInt32(row["Id"]), employeeId);
                MessageBox.Show("Сотрудник назначен на задачу.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Не удалось назначить сотрудника. Возможно, он уже назначен.\n"
                    + ex.Message);
            }
        }
        // Сотрудники
        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            string name = Interaction.InputBox(
                "Введите ФИО:", "Добавление сотрудника");

            if (string.IsNullOrWhiteSpace(name)) return;

            string position = Interaction.InputBox(
                "Введите должность:", "Добавление сотрудника");

            string email = Interaction.InputBox(
                "Введите электронную почту (можно оставить пустой):",
                "Добавление сотрудника");

            try
            {
                Database.AddEmployee(name.Trim(), position.Trim(), email);
                LoadEmployees();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось добавить сотрудника.\n" + ex.Message);
            }
        }
        private void EditEmployee_Click(object sender, RoutedEventArgs e)
        {
            var row = Selected(dgvEmployees, "Выберите сотрудника.");
            if (row == null) return;

            string name = Interaction.InputBox(
                "Введите ФИО:", "Редактирование сотрудника",
                row["FullName"].ToString());

            if (string.IsNullOrWhiteSpace(name)) return;

            string position = Interaction.InputBox(
                "Введите должность:", "Редактирование сотрудника",
                row["Position"].ToString());

            string email = Interaction.InputBox(
                "Введите электронную почту:",
                "Редактирование сотрудника",
                row["Email"] == DBNull.Value ? "" : row["Email"].ToString());

            try
            {
                Database.UpdateEmployee(
                    Convert.ToInt32(row["Id"]),
                    name.Trim(), position.Trim(), email);

                LoadEmployees();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось изменить данные.\n" + ex.Message);
            }
        }

        private void DeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            var row = Selected(dgvEmployees, "Выберите сотрудника.");
            if (row == null || !ConfirmDelete("сотрудника")) return;

            Database.DeleteEmployee(Convert.ToInt32(row["Id"]));
            LoadEmployees();
        }
    }
}
