using System;

class Program
{
    static void Main()
    {
        int[] array = new int[10];

        Console.WriteLine("Введите 10 элементов массива:");

        for (int i = 0; i < 10; i++)
        {
            array[i] = int.Parse(Console.ReadLine()!);
        }

        int maxIndex = 0;

        for (int i = 1; i < 10; i++)
        {
            if (array[i] > array[maxIndex])
            {
                maxIndex = i;
            }
        }

        Console.Write("Введите целое число: ");
        int number = int.Parse(Console.ReadLine()!);

        array[maxIndex] = number;

        Console.WriteLine("Измененный массив:");

        for (int i = 0; i < 10; i++)
        {
            Console.Write(array[i] + " ");
        }
    }
}