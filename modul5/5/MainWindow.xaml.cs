using System;
using System.Data;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace _5
{
    public partial class MainWindow : Window
    {
        private bool resultShown = false; // показывает, получен ли результат

        public MainWindow()
        {
            InitializeComponent();
            btnComma.Content = ","; // устанавливаем запятую
            txtDisplay.Text = "0"; // начальное значение на экране
        }

        private void CalculatorButton_Click(object? sender, RoutedEventArgs e)
        {
            if (sender is not Button button) return; // проверяем, что нажата кнопка

            string value = button.Content?.ToString() ?? ""; // получаем надпись кнопки
            string display = txtDisplay.Text; // получаем текст с экрана

            if (value == "C")
            {
                txtDisplay.Text = "0"; // очищаем экран
                resultShown = false;
                return;
            }

            if (value == "=")
            {
                try
                {
                    txtDisplay.Text = Calculate(display); // вычисляем выражение
                    resultShown = true; // результат получен
                }
                catch
                {
                    MessageBox.Show("Ошибка вычисления.");
                }

                return;
            }

            if (value == ",")
            {
                // Если начинаем новое число, ставим перед запятой ноль
                if (resultShown || display == "0")
                {
                    txtDisplay.Text = "0,";
                    resultShown = false;
                    return;
                }

                // Находим последний знак операции
                int index = display.LastIndexOfAny(
                    new char[] { '+', '-', '*', '/' });

                // Получаем число после последней операции
                string number = display.Substring(index + 1);

                if (number == "")
                    txtDisplay.Text += "0,"; // начинаем дробное число
                else if (!number.Contains(","))
                    txtDisplay.Text += ","; // добавляем запятую, если её ещё нет

                return;
            }

            if ("+-*/".Contains(value))
            {
                if (resultShown)
                {
                    txtDisplay.Text = display + value; // продолжаем расчёт с результатом
                    resultShown = false;
                    return;
                }

                // Если последний символ уже операция, заменяем его
                if (display.Length > 0 &&
                    "+-*/".Contains(display[display.Length - 1]))
                {
                    txtDisplay.Text =
                        display.Substring(0, display.Length - 1) + value;
                    return;
                }

                // Если выражение уже содержит операцию, сначала считаем его
                if (display.IndexOfAny(
                    new char[] { '+', '-', '*', '/' }) >= 0)
                {
                    try
                    {
                        display = Calculate(display);
                    }
                    catch
                    {
                        MessageBox.Show("Ошибка вычисления.");
                        return;
                    }
                }

                txtDisplay.Text = display + value; // добавляем новый знак
                return;
            }

            // Если получен результат или на экране ноль, начинаем ввод заново
            if (resultShown || display == "0")
                txtDisplay.Text = value;
            else
                txtDisplay.Text += value; // добавляем цифру к выражению

            resultShown = false;
        }

        private string Calculate(string expression)
        {
            // Заменяем запятую точкой для вычисления
            object result = new DataTable().Compute(
                expression.Replace(',', '.'), null);

            // Преобразуем ответ в строку и возвращаем запятую
            return (Convert.ToString(
                result, CultureInfo.InvariantCulture) ?? "0")
                .Replace('.', ',');
        }
    }
}