using System;
namespace DelegateShapes
{
    public delegate double CalculateAreaDelegate();
    public class Shape
    {
        public virtual double CalculateArea()
        {
            return 0;
        }
    }
    public class Circle : Shape
    {
        public double Radius { get; set; }
        public Circle(double radius)
        {
            Radius = radius;
        }
        public override double CalculateArea()
        {
            return Math.PI * Radius * Radius;
        }
    }
    public class Rectangle : Shape
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public Rectangle(double width, double height)
        {
            Width = width;
            Height = height;
        }
        public override double CalculateArea()
        {
            return Width * Height;
        }
    }
    public class Triangle : Shape
    {
        public double Base { get; set; }
        public double Height { get; set; }
        public Triangle(double triangleBase, double height)
        {
            Base = triangleBase;
            Height = height;
        }
        public override double CalculateArea()
        {
            return 0.5 * Base * Height;
        }
    }
    class Program
    {
        static void Main()
        {
            Circle circle = new Circle(5);
            Rectangle rectangle = new Rectangle(4, 6);
            Triangle triangle = new Triangle(3, 8);
            Console.WriteLine("Площади фигур:");
            CalculateAreaDelegate circleDelegate = circle.CalculateArea;
            CalculateAreaDelegate rectangleDelegate = rectangle.CalculateArea;
            CalculateAreaDelegate triangleDelegate = triangle.CalculateArea;
            Console.WriteLine("Площадь круга: " + circleDelegate().ToString("F2"));
            Console.WriteLine("Площадь прямоугольника: " + rectangleDelegate());
            Console.WriteLine("Площадь треугольника: " + triangleDelegate());
        }
    }
}
