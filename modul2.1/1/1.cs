using System;

class Person
{
    private string name;
    private int age;
    private string address;
        
    public void SetName(string name)
    {
        this.name = name;
    }

    public string GetName()
    {
        return name;
    }

    public void SetAge(int age)
    {
        this.age = age;
    }

    public int GetAge()
    {
        return age;
    }

    public void SetAddress(string address)
    {
        this.address = address;
    }

    public string GetAddress()
    {
        return address;
    }
}

class Program
{
    static void Main()
    {
        Person person1 = new Person();
        person1.SetName("Вика");
        person1.SetAge(17);
        person1.SetAddress("Орша");

        Person person2 = new Person();
        person2.SetName("Анна");
        person2.SetAge(25);
        person2.SetAddress("Минск");

        Console.WriteLine("Первый человек:");
        Console.WriteLine("Имя: " + person1.GetName());
        Console.WriteLine("Возраст: " + person1.GetAge());
        Console.WriteLine("Адрес: " + person1.GetAddress());

        Console.WriteLine();

        Console.WriteLine("Второй человек:");
        Console.WriteLine("Имя: " + person2.GetName());
        Console.WriteLine("Возраст: " + person2.GetAge());
        Console.WriteLine("Адрес: " + person2.GetAddress());
    }
}