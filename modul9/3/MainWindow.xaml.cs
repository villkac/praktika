using System;
using System.Data;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.Sqlite;
namespace TaskManager
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                Database.Initialize();
                dpDueDate.SelectedDate = DateTime.Today.AddDays(1); // срок выполнения по умолчанию на завтра
                LoadTasks();
                CheckDeadlines(); // проверка сроков выполнения и отправка уведомления 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось загрузить приложение.\n" + ex.Message, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void LoadTasks()
        {
            dgvTasks.ItemsSource = Database.GetTasks().DefaultView;
        }
        private bool TryGetFormData( // проверка полей
            out string name,
            out string description,
            out string priority,
            out string dueDate,
            out string status,
            out int progress)
        {
            name = txtName.Text;
            description = txtDescription.Text;
            // ?. обращается к свойству, только если объект не равен null
            priority = (cmbPriority.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "";
            dueDate = "";
            status = (cmbStatus.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "";
            progress = 0;

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Введите название задачи.");
                return false;
            }
            if (cmbPriority.SelectedItem == null)
            {
                MessageBox.Show("Выберите приоритет задачи.");
                return false;
            }
            if (dpDueDate.SelectedDate == null)
            {
                MessageBox.Show("Выберите срок выполнения.");
                return false;
            }
            if (!int.TryParse(txtProgress.Text, out progress) || progress < 0 || progress > 100)
            {
                MessageBox.Show(
                    "Процент выполнения должен быть числом от 0 до 100.");
                return false;
            }
            dueDate = dpDueDate.SelectedDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            // автоматически устанавливаем статус по прогрессу
            if (status == "Завершена")
                progress = 100;
            else if (progress == 100)
                status = "Завершена";
            else if (progress > 0) ;
            else if (progress > 0)
                status = "В работе";
            else
                status = "Не начата";
            return true;
        }
        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            if (!TryGetFormData(
                out string name,
                out string description,
                out string priority,
                out string dueDate,
                out string status,
                out int progress))
                return;

            try
            {
                Database.AddTask(name, description, priority, dueDate, status, progress);
                LoadTasks();
                ClearFields();
                MessageBox.Show("Задача успешно добавлена.");
                CheckDeadlines();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка добавления задачи:\n" + ex.Message);
            }
        }
        private void EditTask_Click(object sender, RoutedEventArgs e)
        {
            if (dgvTasks.SelectedItem is not DataRowView row)
            {
                MessageBox.Show("Выберите задачу для изменения.");
                return;
            }
            if (!TryGetFormData(
                out string name,
                out string description,
                out string priority,
                out string dueDate,
                out string status,
                out int progress))
                return;
            try
            {
                int id = Convert.ToInt32(row["Id"]);

                Database.UpdateTask(id, name, description, priority, dueDate, status, progress);
                LoadTasks();
                ClearFields();
                MessageBox.Show("Задача успешно изменена.");
                CheckDeadlines();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка изменения задачи:\n" + ex.Message);
            }
        }
        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (dgvTasks.SelectedItem is not DataRowView row)
            {
                MessageBox.Show("Выберите задачу для удаления.");
                return;
            }
            var result = MessageBox.Show("Вы действительно хотите удалить выбранную задачу?", "Подтверждение удаления",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;
            try
            {
                int id = Convert.ToInt32(row["Id"]);
                Database.DeleteTask(id);

                LoadTasks();
                ClearFields();

                MessageBox.Show("Задача удалена.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка удаления задачи:\n" + ex.Message);
            }
        }
        private void dgvTasks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgvTasks.SelectedItem is not DataRowView row)
                return;
            // перенос значения в поля формы
            txtName.Text = row["Name"]?.ToString() ?? "";
            txtDescription.Text = row["Description"]?.ToString() ?? "";

            SelectComboItem(cmbPriority, row["Priority"]?.ToString());
            SelectComboItem(cmbStatus, row["Status"]?.ToString());

            if (DateTime.TryParse(row["DueDate"]?.ToString(), out DateTime date))
                dpDueDate.SelectedDate = date;

            txtProgress.Text = row["Progress"]?.ToString() ?? "0";
        }
        // выбор элементов в ComboBox
        private void SelectComboItem(ComboBox comboBox, string value)
        {
            foreach (ComboBoxItem item in comboBox.Items)
            {
                if (item.Content?.ToString() == value)
                {
                    comboBox.SelectedItem = item;
                    return;
                }
            }
        }
        private void ClearFields()
        {
            txtName.Clear();
            txtDescription.Clear();
            cmbPriority.SelectedIndex = 1;
            cmbStatus.SelectedIndex = 0;
            dpDueDate.SelectedDate = DateTime.Today.AddDays(1);
            txtProgress.Text = "0";
            dgvTasks.SelectedItem = null;
        }
        private void ClearFields_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
        }
        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadTasks();
            CheckDeadlines();
        }
        private void CheckDeadlines()
        {
            DataTable tasks = Database.GetDeadlineTasks(); // получение незавершенных задач
            StringBuilder messages = new StringBuilder();

            foreach (DataRow row in tasks.Rows)
            {
                int id = Convert.ToInt32(row["Id"]);
                string name = row["Name"].ToString() ?? "";
                string dueDateText = row["DueDate"].ToString() ?? "";
                // если дата не распознана, continue пропускает текущую задачу
                if (!DateTime.TryParse(dueDateText, out DateTime dueDate))
                    continue;

                string message;

                if (dueDate.Date < DateTime.Today)
                {
                    message = $"Просрочена задача «{name}». Срок: {dueDate:dd.MM.yyyy}.";
                }
                else if (dueDate.Date == DateTime.Today)
                {
                    message = $"Сегодня истекает срок задачи «{name}».";
                }
                else
                {
                    message = $"Скоро истекает срок задачи «{name}». Дата: {dueDate:dd.MM.yyyy}.";
                }
                // Сохраняем уведомление, только если такого сообщения
                // по этой задаче ещё не было сегодня.
                if (Database.AddNotificationIfNew(id, message))
                    messages.AppendLine("• " + message);
            }
            if (messages.Length > 0)
            {
                MessageBox.Show(
                    messages.ToString(),
                    "Уведомления о сроках",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }
    }
}