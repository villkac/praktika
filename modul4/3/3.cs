using System;

namespace University
{
    public interface IStudent
    {
        string Name { get; set; }
        double CalculateAverage();
        int GetCourse();
    }

    public class FirstYearStudent : IStudent
    {
        public string Name { get; set; }
        public int Course { get; set; }
        public double Grade1 { get; set; }
        public double Grade2 { get; set; }
        public double Grade3 { get; set; }

        public FirstYearStudent(string name, double grade1, double grade2, double grade3)
        {
            Name = name;
            Grade1 = grade1;
            Grade2 = grade2;
            Grade3 = grade3;
        }

        public double CalculateAverage()
        {
            return (Grade1 + Grade2 + Grade3) / 3;
        }

        public int GetCourse()
        {
            return Course;
        }
    }

    public class SecondYearStudent : IStudent
    {
        public string Name { get; set; }
        public int Course { get; set; }
        public double Grade1 { get; set; }
        public double Grade2 { get; set; }
        public double Grade3 { get; set; }

        public SecondYearStudent(string name, double grade1, double grade2, double grade3)
        {
            Name = name;
            Grade1 = grade1;
            Grade2 = grade2;
            Grade3 = grade3;
        }

        public double CalculateAverage()
        {
            return (Grade1 + Grade2 + Grade3) / 3;
        }

        public int GetCourse()
        {
            return Course;
        }
    }

    public class ThirdYearStudent : IStudent
    {
        public string Name { get; set; }
        public int Course { get; set; }
        public double Grade1 { get; set; }
        public double Grade2 { get; set; }
        public double Grade3 { get; set; }

        public ThirdYearStudent(string name, double grade1, double grade2, double grade3)
        {
            Name = name;
            Grade1 = grade1;
            Grade2 = grade2;
            Grade3 = grade3;
        }

        public double CalculateAverage()
        {
            return (Grade1 + Grade2 + Grade3) / 3;
        }

        public int GetCourse()
        {
            return Course;
        }
    }

    class Program
    {
        static void Main()
        {
            FirstYearStudent student1 = new FirstYearStudent("Анна", 9, 8, 8);
            student1.Course = 1;

            SecondYearStudent student2 = new SecondYearStudent("Александр", 6, 9, 8);
            student2.Course = 2;

            ThirdYearStudent student3 = new ThirdYearStudent("Диана", 8, 7, 9);
            student3.Course = 3;

            IStudent[] students = { student1, student2, student3 };

            for (int i = 0; i < students.Length; i++)
            {
                Console.WriteLine("Студент: " + students[i].Name);
                Console.WriteLine("Средний балл: " + students[i].CalculateAverage().ToString("F2"));
                Console.WriteLine("Курс: " + students[i].GetCourse());
                Console.WriteLine();
            }
        }
    }
}