using System;

namespace NumberSorting
{
    public delegate void SortHandler(int[] numbers);

    public class Sorter
    {
        public void BubbleSort(int[] numbers)
        {
            for (int i = 0; i < numbers.Length - 1; i++)
            {
                for (int j = 0; j < numbers.Length - i - 1; j++)
                {
                    if (numbers[j] > numbers[j + 1])
                    {
                        int temp = numbers[j];
                        numbers[j] = numbers[j + 1];
                        numbers[j + 1] = temp;
                    }
                }
            }
        }

        public void RadixSort(int[] numbers)
        {
            int max = numbers[0];

            for (int i = 1; i < numbers.Length; i++)
            {
                if (numbers[i] > max)
                {
                    max = numbers[i];
                }
            }

            for (int place = 1; max / place > 0; place *= 10)
            {
                int[] output = new int[numbers.Length];
                int[] count = new int[10];

                for (int i = 0; i < numbers.Length; i++)
                {
                    int digit = (numbers[i] / place) % 10;
                    count[digit]++;
                }

                for (int i = 1; i < 10; i++)
                {
                    count[i] += count[i - 1];
                }

                for (int i = numbers.Length - 1; i >= 0; i--)
                {
                    int digit = (numbers[i] / place) % 10;
                    output[count[digit] - 1] = numbers[i];
                    count[digit]--;
                }

                for (int i = 0; i < numbers.Length; i++)
                {
                    numbers[i] = output[i];
                }
            }
        }
    }

    class Program
    {
        static void Main()
        {
            int[] numbers = { 81, 3, 26, 41, 129, 14, 24 };

            Sorter sorter = new Sorter();

            Console.WriteLine("Исходный массив:");

            for (int i = 0; i < numbers.Length; i++)
            {
                Console.Write(numbers[i] + " ");
            }

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("Выберите способ сортировки:");
            Console.WriteLine("1 - Пузырьковая сортировка");
            Console.WriteLine("2 - Поразрядная сортировка");
            Console.Write("Ваш выбор: ");

            int choice = int.Parse(Console.ReadLine());

            SortHandler handler;

            if (choice == 1)
            {
                handler = sorter.BubbleSort;
            }
            else if (choice == 2)
            {
                handler = sorter.RadixSort;
            }
            else
            {
                Console.WriteLine("Неверный выбор.");
                return;
            }

            handler(numbers);

            Console.WriteLine();
            Console.WriteLine("Отсортированный массив:");

            for (int i = 0; i < numbers.Length; i++)
            {
                Console.Write(numbers[i] + " ");
            }
        }
    }
}