using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace _1
{
    public partial class MainWindow : Window
    {
        private string tool = "Линия";
        private Point start;
        private Shape? current;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SelectTool_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button) // проверка что событие вызвала кнопка и сохранаение в переменную button
                tool = button.Content.ToString() ?? "Линия";
        }

        private void Down(object sender, MouseButtonEventArgs e)
        {
            start = e.GetPosition(canvas);

            if (tool == "Линия")
                current = new Line
                {
                    X1 = start.X,
                    Y1 = start.Y,
                    X2 = start.X,
                    Y2 = start.Y
                };
            else if (tool == "Круг")
                current = new Ellipse();
            else
                current = new Rectangle();

            current.Stroke = Brushes.Black; // контур фигуры
            current.StrokeThickness = 2; // толщина 
            current.Fill = Brushes.Transparent; // внутренняя область прозрачная

            if (current is not Line)
            {
                current.Width = 0;
                current.Height = 0;
                Canvas.SetLeft(current, start.X); // положение фигуры на холсте
                Canvas.SetTop(current, start.Y);
            }

            canvas.Children.Add(current);
            canvas.CaptureMouse();
        }

        private void Move(object sender, MouseEventArgs e)
        {
            if (!canvas.IsMouseCaptured || current == null) // проверка нужно ли изменять фигуру
                return;

            Point p = e.GetPosition(canvas);

            if (current is Line line)
            {
                line.X2 = p.X;
                line.Y2 = p.Y;
                return;
            }

            double dx = p.X - start.X; // расчет на сколько переместился курсор
            double dy = p.Y - start.Y;
            // рассчет ширины, высоты и положения
            double w = Math.Abs(dx);
            double h = Math.Abs(dy);
            double x = Math.Min(start.X, p.X);
            double y = Math.Min(start.Y, p.Y);

            if (current is Rectangle)
            {
                double size = Math.Max(w, h);
                w = h = size;
                // рассчет полодения квадрата
                x = dx < 0 ? start.X - size : start.X;
                y = dy < 0 ? start.Y - size : start.Y;
            }
            // применение координат и размера к текущей фигуре
            Canvas.SetLeft(current, x);
            Canvas.SetTop(current, y);
            current.Width = w;
            current.Height = h;
        }

        private void Up(object sender, MouseButtonEventArgs e)
        {
            if (canvas.IsMouseCaptured)
                canvas.ReleaseMouseCapture(); // освобождение мыши

            current = null;
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
            current = null;

            if (canvas.IsMouseCaptured)
                canvas.ReleaseMouseCapture();
        }
    }
}
