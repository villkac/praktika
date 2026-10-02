using System;

interface IDrawable
{
    void Draw();
}

class Circle : IDrawable
{
    public void Draw()
    {
        Console.WriteLine("Рисуется круг.");
    }
}

class Rectangle : IDrawable
{
    public void Draw()
    {
        Console.WriteLine("Рисуется прямоугольник.");
    }
}

class Triangle : IDrawable
{
    public void Draw()
    {
        Console.WriteLine("Рисуется треугольник.");
    }
}

class Program
{
    static void Main()
    {
        IDrawable[] shapes =
        {
            new Circle(),
            new Rectangle(),
            new Triangle()
        };

        for (int i = 0; i < shapes.Length; i++)
        {
            shapes[i].Draw();
        }
    }
}