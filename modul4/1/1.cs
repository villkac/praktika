using System;
namespace Figures
{
    public interface IFigure
    {
        double CalculateArea(); // методы, которые должны быть в каждом реализующем классе
        double CalculatePerimeter();
    }

    public class Circle : IFigure
    {
        public double Radius { get; set; }

        public Circle(double radius)
        {
            Radius = radius;
        }

        public double CalculateArea()
        {
            return Math.PI * Radius * Radius;
        }

        public double CalculatePerimeter()
        {
            return 2 * Math.PI * Radius;
        }
    }

    public class Rectangle : IFigure
    {
        public double Width { get; set; }
        public double Height { get; set; }

        public Rectangle(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public double CalculateArea()
        {
            return Width * Height;
        }

        public double CalculatePerimeter()
        {
            return 2 * (Width + Height);
        }
    }

    public class Triangle : IFigure
    {
        public double Side1 { get; set; }
        public double Side2 { get; set; }
        public double Side3 { get; set; }
        public double Height { get; set; }

        public Triangle(double side1, double side2, double side3, double height)
        {
            Side1 = side1;
            Side2 = side2;
            Side3 = side3;
            Height = height;
        }

        public double CalculateArea()
        {
            return 0.5 * Side1 * Height;
        }

        public double CalculatePerimeter()
        {
            return Side1 + Side2 + Side3;
        }
    }

    class Program
    {
        static void Main()
        {
            Circle circle = new Circle(5);
            Rectangle rectangle = new Rectangle(4, 6);
            Triangle triangle = new Triangle(3, 4, 5, 4);

            Console.WriteLine("Круг:");
            Console.WriteLine("Площадь: " + circle.CalculateArea().ToString("F2"));
            Console.WriteLine("Периметр: " + circle.CalculatePerimeter().ToString("F2"));

            Console.WriteLine();

            Console.WriteLine("Прямоугольник:");
            Console.WriteLine("Площадь: " + rectangle.CalculateArea().ToString("F2"));
            Console.WriteLine("Периметр: " + rectangle.CalculatePerimeter().ToString("F2"));

            Console.WriteLine();

            Console.WriteLine("Треугольник:");
            Console.WriteLine("Площадь: " + triangle.CalculateArea().ToString("F2"));
            Console.WriteLine("Периметр: " + triangle.CalculatePerimeter().ToString("F2"));
        }
    }
}