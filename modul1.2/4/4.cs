using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите количество элементов K: ");
        int k = int.Parse(Console.ReadLine()!);

        Console.Write("Введите A: ");
        int a = int.Parse(Console.ReadLine()!);

        Console.Write("Введите B: ");
        int b = int.Parse(Console.ReadLine()!);

        int[] array = new int[k];
        Random random = new Random();

        for (int i = 0; i < k; i++)
        {
            array[i] = random.Next(a, b);
        }

        Console.WriteLine("Массив:");

        for (int i = 0; i < k; i++)
        {
            Console.Write(array[i] + " ");
        }

        int minIndex = 0;
        int maxIndex = 0;

        for (int i = 1; i < k; i++)
        {
            if (array[i] < array[minIndex])
            {
                minIndex = i;
            }

            if (array[i] > array[maxIndex])
            {
                maxIndex = i;
            }
        }

        Console.WriteLine();
        Console.WriteLine("Индекс минимального элемента: " + (minIndex + 1));
        Console.WriteLine("Индекс максимального элемента: " + (maxIndex + 1));

        int start = Math.Min(minIndex, maxIndex);
        int end = Math.Max(minIndex, maxIndex);

        Console.WriteLine("Элементы между найденными:");

        for (int i = start; i <= end; i++)
        {
            Console.Write(array[i] + " ");
        }
    }
}