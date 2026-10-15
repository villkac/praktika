using System;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Media.Imaging;

namespace _4
{
    public partial class MainWindow : Window
    {
        private BitmapImage? originalImage; // хранение исходного изображения
        private double zoomFactor = 1.0; // коэффициент масштаба

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Файлы изображений (*.png;*.jpg)|*.png;*.jpg"
            };

            if (dialog.ShowDialog() != true)
            {
                return;
            }

            try
            {
                BitmapImage image = new BitmapImage();

                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; // загружает изображение полностью
                image.UriSource = new Uri(dialog.FileName);
                image.EndInit();
                image.Freeze(); // делает обьект неизменяемым 

                originalImage = image;
                zoomFactor = 1.0; // исходный масштаб 

                DisplayImage();

                Title = "Просмотр изображений — " + System.IO.Path.GetFileName(dialog.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Не удалось открыть изображение: " + ex.Message);
            }
        }

        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            if (originalImage == null)
            {
                MessageBox.Show("Сначала откройте изображение.");
                return;
            }

            zoomFactor = Math.Min(zoomFactor * 1.25, 5.0); // увеличивает текущий масштаб
            DisplayImage(); // отображение с новым масштабом
        }
        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            if (originalImage == null)
            {
                MessageBox.Show("Сначала откройте изображение.");
                return;
            }

            zoomFactor = Math.Max(zoomFactor / 1.25, 0.1);
            DisplayImage();
        }

        private void OriginalSize_Click(object sender, RoutedEventArgs e)
        {
            if (originalImage == null)
            {
                MessageBox.Show("Сначала откройте изображение.");
                return;
            }

            zoomFactor = 1.0;
            DisplayImage(); // повторный вывод изображения 
        }

        private void DisplayImage()
        {
            if (originalImage == null)
            {
                return;
            }

            imageView.Source = originalImage; // установка исходного изображения 
            // задает ширину и высоту с учетом масштаба
            imageView.Width = Math.Max(1, originalImage.PixelWidth * zoomFactor);
            imageView.Height = Math.Max(1, originalImage.PixelHeight * zoomFactor);
        }
    }
}
