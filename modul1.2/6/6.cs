using System;

class Program
{
    static void Main()
    {
        double[] array = new double[10];
        int[] indexes = new int[10];

        Random random = new Random();

        for (int i = 0; i < 10; i++)
        {
            array[i] = random.NextDouble() * 20 - 10;
            indexes[i] = i;
        }

        for (int i = 0; i < 9; i++)
        {
            for (int j = i + 1; j < 10; j++)
            {
                if (array[indexes[i]] > array[indexes[j]])
                {
                    int temp = indexes[i];
                    indexes[i] = indexes[j];
                    indexes[j] = temp;
                }
            }
        }

        Console.WriteLine("Исходный массив:");

        for (int i = 0; i < 10; i++)
        {
            Console.Write(array[i].ToString("F2") + " ");
        }

        Console.WriteLine();
        Console.WriteLine("Индексы в порядке возрастания значений:");

        for (int i = 0; i < 10; i++)
        {
            Console.Write(indexes[i] + " ");
        }
    }
}