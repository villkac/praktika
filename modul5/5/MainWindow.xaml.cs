using System;
using System.Data;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace _5
{
    public partial class MainWindow : Window
    {
        private bool resultShown = false; // показан ли результат

        public MainWindow()
        {
            InitializeComponent();
            btnComma.Content = ","; // устанавливаем запятую
            txtDisplay.Text = "0"; // начальное значение на экране
        }

        private void CalculatorButton_Click(object? sender, RoutedEventArgs e)
        {
            if (sender is not Button button) return; // проверяем, что нажата кнопка

            string value = button.Content?.ToString() ?? ""; // получаем текст кнопки
            string display = txtDisplay.Text; // получаем выражение с экрана

            // Очистка калькулятора
            if (value == "C")
            {
                txtDisplay.Text = "0";
                resultShown = false;
                return;
            }

            // Вычисление выражения при нажатии на =
            if (value == "=")
            {
                // Если выражение не закончено, ничего не вычисляем
                if (display.Length == 0 ||
                    "+-*/,".Contains(display[display.Length - 1]))
                    return;

                try
                {
                    // Заменяем запятую точкой, чтобы DataTable.Compute() правильно вычислял дробные числа
                    object result = new DataTable().Compute(
                        display.Replace(',', '.'), null);

                    // Преобразуем результат в строку и возвращаем запятую
                    txtDisplay.Text = (Convert.ToString(
                        result, CultureInfo.InvariantCulture) ?? "0")
                        .Replace('.', ',');

                    resultShown = true; // отмечаем, что результат получен
                }
                catch
                {
                    MessageBox.Show("Ошибка вычисления или деление на ноль!");
                }

                return;
            }

            // Проверяем, нажата ли цифра
            if (value.Length == 1 && char.IsDigit(value[0]))
            {
                // Начинаем новое число, если уже получили результат или на экране 0
                if (resultShown || display == "0")
                    txtDisplay.Text = value;
                else
                    txtDisplay.Text += value; // добавляем цифру к выражению

                resultShown = false;
                return;
            }

            // Обработка запятой
            if (value == ",")
            {
                // После результата начинаем ввод нового дробного числа
                if (resultShown)
                {
                    txtDisplay.Text = "0,";
                    resultShown = false;
                    return;
                }

                // Ищем последний математический знак
                int index = display.LastIndexOfAny(
                    new char[] { '+', '-', '*', '/' });

                // Получаем число после последнего знака
                string number = display.Substring(index + 1);

                if (number.Contains(",")) return; // запятая уже есть

                if (number == "")
                    txtDisplay.Text += "0,"; // если число ещё не введено
                else
                    txtDisplay.Text += ","; // добавляем запятую

                return;
            }

            // Обработка математических операций
            if ("+-*/".Contains(value))
            {
                // Если результат уже получен, продолжаем вычисление с ним
                if (resultShown)
                {
                    txtDisplay.Text = display + value;
                    resultShown = false;
                }
                // Если предыдущий символ уже операция, заменяем его
                else if ("+-*/".Contains(display[display.Length - 1]))
                {
                    txtDisplay.Text =
                        display.Substring(0, display.Length - 1) + value;
                }
                else
                {
                    txtDisplay.Text += value; // добавляем знак к выражению
                }
            }
        }
    }
}