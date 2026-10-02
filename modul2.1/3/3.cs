using System;

class Author
{
    public string Name;
    public int BirthYear;

    public Author(string name, int birthYear)
    {
        Name = name;
        BirthYear = birthYear;
    }
}

class Book
{
    public string Title;
    public int Year;
    public Author Author;

    public Book(string title, int year, Author author)
    {
        Title = title;
        Year = year;
        Author = author;
    }

    public void ShowInfo()
    {
        Console.WriteLine("Название: " + Title);
        Console.WriteLine("Год выпуска: " + Year);
        Console.WriteLine("Автор: " + Author.Name);
        Console.WriteLine("Год рождения автора: " + Author.BirthYear);
    }
}

class Program
{
    static void Main()
    {
        Author author1 = new Author("Лев Толстой", 1828);
        Author author2 = new Author("Фёдор Достоевский", 1821);

        Book book1 = new Book("Война и мир", 1869, author1);
        Book book2 = new Book("Преступление и наказание", 1866, author2);

        Console.WriteLine("Первая книга:");
        book1.ShowInfo();

        Console.WriteLine();

        Console.WriteLine("Вторая книга:");
        book2.ShowInfo();
    }
}