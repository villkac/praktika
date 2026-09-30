using System;

class Program
{
    static void Main()
    {
        for (int i = 1; i <= 100; i++)
        {
            if (i % 3 == 0 && i % 5 == 0)
            {
                Console.Write("FizzBuzz\t");
            }
            else if (i % 3 == 0)
            {
                Console.Write("Fizz\t");
            }
            else if (i % 5 == 0)
            {
                Console.Write("Buzz\t");
            }
            else
            {
                Console.Write(i + "\t");
            }

            if (i % 10 == 0)
            {
                Console.WriteLine();
            }
        }
    }
}