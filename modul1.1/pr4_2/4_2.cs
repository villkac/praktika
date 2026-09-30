using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите первое число: ");
        double a = double.Parse(Console.ReadLine()!);

        Console.Write("Введите второе число: ");
        double b = double.Parse(Console.ReadLine()!);

        Console.Write("Введите третье число: ");
        double c = double.Parse(Console.ReadLine()!);

        double average = (a + b + c) / 3;

        Console.WriteLine("Среднее арифметическое: " + average);
    }
}