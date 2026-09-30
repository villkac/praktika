using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите первую строку: ");
        string first = Console.ReadLine()!;

        Console.Write("Введите вторую строку: ");
        string second = Console.ReadLine()!;

        if (first.Contains(second))
        {
            Console.WriteLine("Вторая строка является подстрокой первой.");
        }
        else
        {
            Console.WriteLine("Вторая строка не является подстрокой первой.");
        }
    }
}