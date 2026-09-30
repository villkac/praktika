using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите число: ");
        int number = int.Parse(Console.ReadLine()!);

        int[] array = new int[number];
        Random random = new Random();

        int sum = 0;
        int count = 0;

        while (sum < number)
        {
            int value = random.Next(1, 10);

            if (sum + value <= number)
            {
                array[count] = value;
                sum += value;
                count++;
            }
            else
            {
                break;
            }
        }

        Console.WriteLine("Массив:");

        for (int i = 0; i < count; i++)
        {
            Console.Write(array[i] + " ");
        }

        Console.WriteLine();
        Console.WriteLine("Сумма элементов: " + sum);
    }
}