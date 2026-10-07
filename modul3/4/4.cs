using System;
using System.Collections.Generic;

namespace DataFilter
{
    public delegate bool FilterHandler(string data);

    public class DataFilter
    {
        public bool FilterByLength(string data)
        {
            return data.Length > 5;
        }
    }

    class Program
    {
        static void Main()
        {
            List<string> data = new List<string>();

            data.Add("Программа");
            data.Add("Телефон");
            data.Add("Книга");
            data.Add("Компьютер");
            data.Add("Задача");

            DataFilter filter = new DataFilter();

            Console.WriteLine("Выберите фильтр:");
            Console.WriteLine("1 - По ключевому слову");
            Console.WriteLine("2 - По длине строки");
            Console.Write("Ваш выбор: ");

            int choice = int.Parse(Console.ReadLine());

            FilterHandler handler;

            if (choice == 1)
            {
                Console.Write("Введите ключевое слово: ");
                string keyword = Console.ReadLine();

                handler = delegate (string dataItem)
                {
                    return dataItem.Contains(keyword);
                };
            }
            else if (choice == 2)
            {
                handler = filter.FilterByLength;
            }
            else
            {
                Console.WriteLine("Неверный выбор.");
                return;
            }

            Console.WriteLine("Результат:");

            for (int i = 0; i < data.Count; i++)
            {
                if (handler(data[i]))
                {
                    Console.WriteLine(data[i]);
                }
            }
        }
    }
}