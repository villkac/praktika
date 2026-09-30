using System;

class Program
{
    static void Main()
    {
        Random random = new Random();
        int secretNumber = random.Next(1, 101);
        int guess;

        Console.WriteLine("Угадайте загаданное число от 1 до 100");

        do
        {
            Console.Write("Введите число: ");
            guess = int.Parse(Console.ReadLine()!);

            if (guess < secretNumber)
            {
                Console.WriteLine("Загаданное число больше.");
            }
            else if (guess > secretNumber)
            {
                Console.WriteLine("Загаданное число меньше.");
            }
            else
            {
                Console.WriteLine("Поздравляю, вы угадали число.");
            }

        } while (guess != secretNumber);
    }
}