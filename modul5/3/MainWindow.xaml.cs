using System.Collections.ObjectModel;
using System.Windows;

namespace _3
{
    public partial class MainWindow : Window
    {
        // коллекция, которая хранит обьекты типа TaskItem
        private ObservableCollection<TaskItem> tasks = new ObservableCollection<TaskItem>(); 
        public MainWindow()
        {
            InitializeComponent();

            lstTasks.ItemsSource = tasks;
        }   
        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            string taskName = txtTask.Text;

            if (taskName == "")
            {
                MessageBox.Show("Введите название задачи!");
                txtTask.Focus(); // устанавливает курсор в поле ввода
                return;
            }

            tasks.Add(new TaskItem
            {
                Name = taskName,
                IsCompleted = false // "не выполнена"
            });

            txtTask.Clear();
            txtTask.Focus();
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (lstTasks.SelectedItem is TaskItem selectedTask) // проверка является ли элемент обьектом
            {
                tasks.Remove(selectedTask);
            }
            else
            {
                MessageBox.Show("Сначала выберите задачу для удаления!");
            }
        }
    }

    public class TaskItem
    {
        public string Name { get; set; } = "";

        public bool IsCompleted { get; set; }
    }
}
