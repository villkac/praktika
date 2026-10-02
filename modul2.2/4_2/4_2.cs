using System;
using System.Collections.Generic;

class Book
{
    public string Title;
    public string Author;
    public int Year;

    public Book(string title, string author, int year)
    {
        Title = title;
        Author = author;
        Year = year;
    }

    public void ShowInfo()
    {
        Console.WriteLine("Название: " + Title);
        Console.WriteLine("Автор: " + Author);
        Console.WriteLine("Год издания: " + Year);
        Console.WriteLine();
    }
}

class HomeLibrary
{
    private List<Book> books = new List<Book>();

    public void AddBook(Book book)
    {
        books.Add(book);
        Console.WriteLine("Книга добавлена.");
    }

    public void RemoveBook(string title)
    {
        for (int i = 0; i < books.Count; i++)
        {
            if (books[i].Title == title)
            {
                books.RemoveAt(i);
                Console.WriteLine("Книга удалена.");
                return;
            }
        }

        Console.WriteLine("Книга не найдена.");
    }

    public void FindByAuthor(string author)
    {
        bool found = false;

        for (int i = 0; i < books.Count; i++)
        {
            if (books[i].Author == author)
            {
                books[i].ShowInfo();
                found = true;
            }
        }

        if (!found)
        {
            Console.WriteLine("Книги такого автора не найдены.");
        }
    }

    public void FindByYear(int year)
    {
        bool found = false;

        for (int i = 0; i < books.Count; i++)
        {
            if (books[i].Year == year)
            {
                books[i].ShowInfo();
                found = true;
            }
        }

        if (!found)
        {
            Console.WriteLine("Книги за этот год не найдены.");
        }
    }

    public void SortByTitle()
    {
        books.Sort((book1, book2) =>
            book1.Title.CompareTo(book2.Title));

        Console.WriteLine("Книги отсортированы по названию.");
    }

    public void SortByYear()
    {
        books.Sort((book1, book2) =>
            book1.Year.CompareTo(book2.Year));

        Console.WriteLine("Книги отсортированы по году.");
    }

    public void ShowAll()
    {
        if (books.Count == 0)
        {
            Console.WriteLine("Библиотека пуста.");
            return;
        }

        for (int i = 0; i < books.Count; i++)
        {
            Console.WriteLine("Книга №" + (i + 1));
            books[i].ShowInfo();
        }
    }
}

class Program
{
    static void Main()
    {
        HomeLibrary library = new HomeLibrary();

        int choice;

        do
        {
            Console.WriteLine("  ДОМАШНЯЯ БИБЛИОТЕКА  ");
            Console.WriteLine("1. Показать все книги");
            Console.WriteLine("2. Добавить книгу");
            Console.WriteLine("3. Удалить книгу");
            Console.WriteLine("4. Найти книги по автору");
            Console.WriteLine("5. Найти книги по году");
            Console.WriteLine("6. Сортировать по названию");
            Console.WriteLine("7. Сортировать по году");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите операцию: ");

            choice = int.Parse(Console.ReadLine()!);

            Console.WriteLine();

            switch (choice)
            {
                case 1:
                    library.ShowAll();
                    break;

                case 2:
                    Console.Write("Введите название книги: ");
                    string title = Console.ReadLine()!;

                    Console.Write("Введите автора: ");
                    string author = Console.ReadLine()!;

                    Console.Write("Введите год издания: ");
                    int year = int.Parse(Console.ReadLine()!);

                    library.AddBook(new Book(title, author, year));
                    break;

                case 3:
                    Console.Write("Введите название книги для удаления: ");
                    string titleToRemove = Console.ReadLine()!;

                    library.RemoveBook(titleToRemove);
                    break;

                case 4:
                    Console.Write("Введите автора: ");
                    string authorToFind = Console.ReadLine()!;

                    library.FindByAuthor(authorToFind);
                    break;

                case 5:
                    Console.Write("Введите год издания: ");
                    int yearToFind = int.Parse(Console.ReadLine()!);

                    library.FindByYear(yearToFind);
                    break;

                case 6:
                    library.SortByTitle();
                    break;

                case 7:
                    library.SortByYear();
                    break;

                case 0:
                    Console.WriteLine("Программа завершена.");
                    break;

                default:
                    Console.WriteLine("Такой операции нет.");
                    break;
            }

            Console.WriteLine();

        } while (choice != 0);
    }
}