using System;

namespace Drawing
{
    public interface IDrawing
    {
        void DrawLine();
        void DrawCircle();
        void DrawRectangle();
    }

    public class Canvas : IDrawing
    {
        public void DrawLine()
        {
            Console.WriteLine("Нарисована линия.");
        }

        public void DrawCircle()
        {
            Console.WriteLine("Нарисован круг.");
        }

        public void DrawRectangle()
        {
            Console.WriteLine("Нарисован прямоугольник.");
        }
    }

    class Program
    {
        static void Main()
        {
            Canvas canvas = new Canvas();

            while (true)
            {
                Console.WriteLine("Выберите фигуру для рисования:");
                Console.WriteLine("1 - Линия");
                Console.WriteLine("2 - Круг");
                Console.WriteLine("3 - Прямоугольник");
                Console.Write("Ваш выбор: ");

                int choice = int.Parse(Console.ReadLine());

                if (choice == 1)
                {
                    canvas.DrawLine();
                }
                else if (choice == 2)
                {
                    canvas.DrawCircle();
                }
                else if (choice == 3)
                {
                    canvas.DrawRectangle();
                }
                else
                {
                    Console.WriteLine("Неверный выбор.");
                }
            }
        }
    }
}