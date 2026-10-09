using System;

namespace Library
{
    public interface IBook
    {
        string Title { get; set; }
        bool CheckAvailability();
        void TakeBook();
    }

    public class PaperBook : IBook
    {
        public string Title { get; set; }
        public bool IsAvailable { get; set; }

        public PaperBook(string title, bool isAvailable)
        {
            Title = title;
            IsAvailable = isAvailable;
        }

        public bool CheckAvailability()
        {
            return IsAvailable;
        }

        public void TakeBook()
        {
            if (IsAvailable)
            {
                IsAvailable = false;
                Console.WriteLine("Книга выдана.");
            }
            else
            {
                Console.WriteLine("Книга недоступна.");
            }
        }
    }

    public class EBook : IBook
    {
        public string Title { get; set; }
        public bool IsAvailable { get; set; }

        public EBook(string title, bool isAvailable)
        {
            Title = title;
            IsAvailable = isAvailable;
        }

        public bool CheckAvailability()
        {
            return IsAvailable;
        }

        public void TakeBook()
        {
            if (IsAvailable)
            {
                IsAvailable = false;
                Console.WriteLine("Книга выдана.");
            }
            else
            {
                Console.WriteLine("Книга недоступна.");
            }
        }
    }

    class Program
    {
        static void Main()
        {
            PaperBook book1 = new PaperBook("Война и мир", true);
            PaperBook book2 = new PaperBook("Преступление и наказание", false);
            EBook book3 = new EBook("Мастер и Маргарита", true);

            IBook[] books = { book1, book2, book3 };

            while (true)
            {
                Console.WriteLine("Список книг:");

                for (int i = 0; i < books.Length; i++)
                {
                    Console.WriteLine((i + 1) + " - " + books[i].Title);
                }

                Console.WriteLine();
                Console.Write("Выберите книгу: ");

                int choice = int.Parse(Console.ReadLine());

                if (choice >= 1 && choice <= books.Length)
                {
                    IBook selectedBook = books[choice - 1];

                    Console.WriteLine();
                    Console.WriteLine("Вы выбрали: " + selectedBook.Title);

                    if (selectedBook.CheckAvailability())
                    {
                        Console.WriteLine("Книга доступна.");
                        Console.Write("Выдать книгу? (1 - Да, 2 - Нет): ");

                        int answer = int.Parse(Console.ReadLine());

                        if (answer == 1)
                        {
                            selectedBook.TakeBook();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Книга недоступна.");
                    }
                }
                else
                {
                    Console.WriteLine("Неверный выбор.");
                }
            }
        }
    }
}