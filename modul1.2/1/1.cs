using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите размер массива N: ");
        int n = int.Parse(Console.ReadLine()!);

        double[] array = new double[n];

        Random random = new Random();

        Console.WriteLine("Исходный массив:");

        for (int i = 0; i < n; i++)
        {
            array[i] = random.Next(-10, 11);
            Console.Write(array[i] + " ");
        }

        double max = Math.Abs(array[0]);

        for (int i = 1; i < n; i++)
        {
            if (Math.Abs(array[i]) > max)
            {
                max = Math.Abs(array[i]);
            }
        }

        for (int i = 0; i < n; i++)
        {
            array[i] = array[i] / max;
        }

        Console.WriteLine();
        Console.WriteLine("Измененный массив:");

        for (int i = 0; i < n; i++)
        {
            Console.Write(Math.Round(array[i], 2) + " ");
        }
    }
}