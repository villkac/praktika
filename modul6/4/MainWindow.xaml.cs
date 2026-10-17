using System;
using System.Data;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
namespace WpfApp4
{
    public partial class MainWindow : Window
    {
        private Database database = new Database();
        public MainWindow()
        {
            InitializeComponent();
            cmbType.SelectedIndex = 0; // выбор первого элемента в списке
            dpDate.SelectedDate = DateTime.Today;

            LoadRecords();
        }
        // Загрузка записей
        private void LoadRecords()
        {
            dgRecords.ItemsSource = database.GetRecords().DefaultView;
            UpdateTotals(); // вызов метода пересчёта 
        }
        // Получение выбранного типа операции
        private string GetSelectedType()
        {
            ComboBoxItem item = cmbType.SelectedItem as ComboBoxItem;  
            return item.Content.ToString();
        }
        // Добавление записи
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            string type = GetSelectedType(); 
            string category = txtCategory.Text;
            string amountText = txtAmount.Text.Replace(',', '.');

            double amount; // переменная для записи суммы после преобразования текста в число

            if (type == "" || category == "")
            {
                MessageBox.Show(
                    "Выберите тип операции и введите категорию.");
                return;
            }

            if (!double.TryParse( amountText, // строка, которую нужно преобразовать
                    NumberStyles.Any, // разрешает различные формы записи
                    CultureInfo.InvariantCulture,// использует единые правила разбора числа
                    out amount) || amount <= 0) // переменная для записи результатов преобразования
            {
                MessageBox.Show(
                    "Введите корректную сумму больше нуля.");
                return;
            }

            DateTime date = dpDate.SelectedDate ?? DateTime.Today; 

            database.AddRecord(
                type,
                category,
                amount,
                date,
                txtDescription.Text);
            LoadRecords();
            ClearFields();
        }

        // Изменение записи
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (dgRecords.SelectedItem == null)
            {
                MessageBox.Show("Выберите запись для изменения.");
                return;
            }

            string type = GetSelectedType();
            string category = txtCategory.Text;
            string amountText = txtAmount.Text.Replace(',', '.');

            double amount;

            if (type == "" || category == "")
            {
                MessageBox.Show(
                    "Выберите тип операции и введите категорию.");
                return;
            }

            if (!double.TryParse(
                    amountText,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out amount) || amount <= 0)
            {
                MessageBox.Show(
                    "Введите корректную сумму больше нуля.");
                return;
            }

            DataRowView row = (DataRowView)dgRecords.SelectedItem;

            int id = Convert.ToInt32(row["Id"]);

            DateTime date = dpDate.SelectedDate ?? DateTime.Today; // получение даты

            database.UpdateRecord(
                id,
                type,
                category,
                amount,
                date,
                txtDescription.Text);

            LoadRecords();
            ClearFields();
        }

        // Удаление записи
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dgRecords.SelectedItem == null)
            {
                MessageBox.Show("Выберите запись для удаления.");
                return;
            }

            DataRowView row = (DataRowView)dgRecords.SelectedItem; // получение строки

            int id = Convert.ToInt32(row["Id"]);

            database.DeleteRecord(id);

            LoadRecords();
            ClearFields();
        }
        // Заполнение полей при выборе записи
        private void dgRecords_SelectionChanged(
            object sender, SelectionChangedEventArgs e)
        {
            if (dgRecords.SelectedItem == null)
                return;

            DataRowView row = (DataRowView)dgRecords.SelectedItem;

            if (row["Type"].ToString() == "Доход")
                cmbType.SelectedIndex = 0;
            else
                cmbType.SelectedIndex = 1;

            txtCategory.Text = row["Category"].ToString();

            txtAmount.Text = Convert.ToDouble(row["Amount"]) // заполнение суммы
                .ToString(CultureInfo.CurrentCulture);

            DateTime date; 

            if (DateTime.TryParse(row["Date"].ToString(), out date))
                dpDate.SelectedDate = date; // установка даты

            txtDescription.Text = row["Description"].ToString();
        }
        // Обновление финансовых итогов
        private void UpdateTotals()
        {
            double income = database.GetTotalByType("Доход");
            double expense = database.GetTotalByType("Расход");

            double balance = income - expense; // рассчет баланса

            txtIncomeTotal.Text =
                income.ToString("0.00", CultureInfo.CurrentCulture); // вывод доходов

            txtExpenseTotal.Text =
                expense.ToString("0.00", CultureInfo.CurrentCulture); // вывод расходов

            txtBalance.Text =
                balance.ToString("0.00", CultureInfo.CurrentCulture); // вывод баланса
        }
        // Очистка полей по кнопке
        private void ClearFields_Click(
            object sender, RoutedEventArgs e)
        {
            ClearFields();
        }
        // Очистка полей
        private void ClearFields()
        {
            cmbType.SelectedIndex = 0;
            txtCategory.Clear();
            txtAmount.Clear();
            txtDescription.Clear();

            dpDate.SelectedDate = DateTime.Today;

            dgRecords.SelectedItem = null;
        }
    }
}