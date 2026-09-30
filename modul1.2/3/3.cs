using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите количество простых чисел K: ");
        int k = int.Parse(Console.ReadLine()!);

        int count = 0;
        int number = 2;

        while (count < k)
        {
            bool prime = true;

            for (int i = 2; i < number; i++)
            {
                if (number % i == 0)
                {
                    prime = false;
                    break;
                }
            }

            if (prime)
            {
                Console.Write(number + " ");
                count++;

                if (count % 10 == 0)
                {
                    Console.WriteLine();
                }
            }

            number++;
        }
    }
}