using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;
namespace _2
{
    public partial class MainWindow : Window
    {
        private string? currentFilePath = null; // хранение пути к файлу
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Open_Click(object? sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Текстовые файлы (*.txt)|*.txt"
            };

            if (dialog.ShowDialog() != true)
                return;
            try
            {
                string text = File.ReadAllText(dialog.FileName);
                editorText.Text = text;
                currentFilePath = dialog.FileName; //прочитаный текст в поле редактора
                UpdateTitle(); // обновление заголовка окна
            }
            catch (Exception ex) // перехват возникшей ошибки
            {
                MessageBox.Show(
                    "Не удалось открыть файл: " + ex.Message);
            }
        }
        private void Save_Click(object? sender, RoutedEventArgs e)
        {
            if (currentFilePath == null)
            {
                SaveAs_Click(sender, e);
                return;
            }
            SaveToFile(currentFilePath); // сохранение по известному пути
        }
        private void SaveAs_Click(object? sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "Текстовые файлы (*.txt)|*.txt",
                DefaultExt = "txt",
                AddExtension = true // автоматически добавляет расширение если пользователь не указал
            };

            if (dialog.ShowDialog() != true)
                return;
            SaveToFile(dialog.FileName);
        }
        private void SaveToFile(string path)
        {
            try
            {
                File.WriteAllText(path, editorText.Text); // запись текста из редактора в файл
                currentFilePath = path;
                UpdateTitle();
                MessageBox.Show("Файл успешно сохранён!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Не удалось сохранить файл: " + ex.Message);
            }
        }
        private void UpdateTitle()
        {
            if (currentFilePath == null)
            {
                Title = "Текстовый редактор";
            }
            else
            {
                Title = "Текстовый редактор — " +
                        Path.GetFileName(currentFilePath);
            }
        }
        private void Exit_Click(object? sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
