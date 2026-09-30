using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите размер матрицы: ");
        int n = int.Parse(Console.ReadLine()!);

        int[,] matrix = new int[n, n];
        Random random = new Random();

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                matrix[i, j] = random.Next(-50, 51);
            }
        }

        Console.WriteLine("Исходная матрица:");

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                Console.Write(matrix[i, j] + "\t");
            }

            Console.WriteLine();
        }

        for (int i = 0; i < n - 1; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                int sumI = 0;
                int sumJ = 0;

                for (int k = 0; k < n; k++)
                {
                    sumI += matrix[i, k];
                    sumJ += matrix[j, k];
                }

                if (sumI > sumJ)
                {
                    for (int k = 0; k < n; k++)
                    {
                        int temp = matrix[i, k];
                        matrix[i, k] = matrix[j, k];
                        matrix[j, k] = temp;
                    }
                }
            }
        }

        Console.WriteLine("Матрица после упорядочивания:");

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                Console.Write(matrix[i, j] + "\t");
            }

            Console.WriteLine();
        }
    }
}
