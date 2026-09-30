using System;

class Program
{
    static void Main()
    {
        int[] array = new int[10];
        Random random = new Random();

        int sum = 0;

        for (int i = 0; i < array.Length; i++)
        {
            array[i] = random.Next(1, 101);
            sum += array[i];
        }

        Console.WriteLine("Массив:");

        for (int i = 0; i < array.Length; i++)
        {
            Console.Write(array[i] + " ");
        }

        Console.WriteLine();
        Console.WriteLine("Сумма всех элементов: " + sum);
    }
}