using System;

namespace Store
{
    public interface IProduct
    {
        double CalculateCost();
        int GetStock();
    }

    public class Food : IProduct
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }

        public Food(string name, double price, int quantity)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
        }

        public double CalculateCost()
        {
            return Price;
        }

        public int GetStock()
        {
            return Quantity;
        }
    }

    public class Drink : IProduct
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }

        public Drink(string name, double price, int quantity)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
        }

        public double CalculateCost()
        {
            return Price;
        }

        public int GetStock()
        {
            return Quantity;
        }
    }

    class Program
    {
        static void Main()
        {
            while (true)
            {
                Console.WriteLine("Выберите тип товара:");
                Console.WriteLine("1 - Еда");
                Console.WriteLine("2 - Напиток");
                Console.Write("Ваш выбор: ");

                int choice = int.Parse(Console.ReadLine());

                Console.Write("Введите название товара: ");
                string name = Console.ReadLine();

                Console.Write("Введите цену: ");
                double price = double.Parse(Console.ReadLine());

                Console.Write("Введите количество: ");
                int quantity = int.Parse(Console.ReadLine());

                if (choice == 1)
                {
                    Food food = new Food(name, price, quantity);

                    Console.WriteLine();
                    Console.WriteLine("Товар добавлен.");
                    Console.WriteLine("Товар: " + food.Name);
                    Console.WriteLine("Стоимость: " + food.CalculateCost());
                    Console.WriteLine("Остаток: " + food.GetStock());
                }
                else if (choice == 2)
                {
                    Drink drink = new Drink(name, price, quantity);

                    Console.WriteLine();
                    Console.WriteLine("Товар добавлен.");
                    Console.WriteLine("Товар: " + drink.Name);
                    Console.WriteLine("Стоимость: " + drink.CalculateCost());
                    Console.WriteLine("Остаток: " + drink.GetStock());
                }
                else
                {
                    Console.WriteLine("Неверный выбор.");
                }
            }
        }
    }
}