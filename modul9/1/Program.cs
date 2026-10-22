using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Group { get; set; }
    public double AverageGrade { get; set; }
}
class Program
{
    static List<Student> students = new List<Student>();

    static string filePath = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory,
        "students.txt");
    static void Main()
    {
        int choice;
        do
        {
            Console.WriteLine("УПРАВЛЕНИЕ СТУДЕНТАМИ");
            Console.WriteLine("1. Добавить студента");
            Console.WriteLine("2. Показать всех студентов");
            Console.WriteLine("3. Редактировать студента");
            Console.WriteLine("4. Удалить студента");
            Console.WriteLine("5. Найти студента");
            Console.WriteLine("6. Сортировать студентов");
            Console.WriteLine("7. Сохранить список в файл");
            Console.WriteLine("8. Загрузить список из файла");
            Console.WriteLine("0. Выход");
            choice = ReadInt("Выберите действие: ");

            switch (choice)
            {
                case 1:
                    AddStudent();
                    break;

                case 2:
                    ShowStudents(students);
                    break;

                case 3:
                    EditStudent();
                    break;

                case 4:
                    DeleteStudent();
                    break;

                case 5:
                    SearchStudents();
                    break;

                case 6:
                    SortStudents();
                    break;

                case 7:
                    SaveToFile();
                    break;

                case 8:
                    LoadFromFile();
                    break;

                case 0:
                    Console.WriteLine("Программа завершена.");
                    break;

                default:
                    Console.WriteLine("Такого пункта меню нет.");
                    break;
            }
        } while (choice != 0);
    }
    // Добавление студента
    static void AddStudent()
    {
        Student student = new Student();

        int maxId = 0;
        foreach (Student s in students)
        {
            if (s.Id > maxId)
            {
                maxId = s.Id;
            }
        }
        student.Id = maxId + 1;

        student.Name = ReadText("Введите ФИО студента: ");
        student.Age = ReadInt("Введите возраст: ");
        while (student.Age <= 0)
        {
            Console.WriteLine("Возраст должен быть положительным.");
            student.Age = ReadInt("Введите возраст: ");
        }

        student.Group = ReadText("Введите группу: ");
        student.AverageGrade = ReadGrade("Введите средний балл: ");

        students.Add(student);

        Console.WriteLine("Студент добавлен.");
    }
    // Вывод списка студентов
    static void ShowStudents(List<Student> list)
    {
        if (list.Count == 0)
        {
            Console.WriteLine("Список студентов пуст.");
            return;
        }

        Console.WriteLine(" Список студентов ");
        foreach (Student student in list)
        {
            Console.WriteLine(
                $"ID: {student.Id} | " +
                $"ФИО: {student.Name} | " +
                $"Возраст: {student.Age} | " +
                $"Группа: {student.Group} | " +
                $"Средний балл: {student.AverageGrade}");
        }
    }
    // Редактирование студента
    static void EditStudent()
    {
        ShowStudents(students);
        if (students.Count == 0)
            return;
        int id = ReadInt("Введите ID студента для редактирования: ");
        // поиск элемента для которого выполняется условие
        Student student = students.FirstOrDefault(s => s.Id == id); 

        if (student == null)
        {
            Console.WriteLine("Студент не найден.");
            return;
        }
        student.Name = ReadText("Введите новое ФИО: ");

        student.Age = ReadInt("Введите новый возраст: ");
        while (student.Age <= 0)
        {
            Console.WriteLine("Возраст должен быть положительным.");
            student.Age = ReadInt("Введите новый возраст: ");
        }
        student.Group = ReadText("Введите новую группу: ");
        student.AverageGrade = ReadGrade("Введите новый средний балл: ");
        Console.WriteLine("Данные студента изменены.");
    }
    // Удаление студента
    static void DeleteStudent()
    {
        ShowStudents(students);

        if (students.Count == 0)
            return;

        int id = ReadInt("Введите ID студента для удаления: ");

        Student student = students.FirstOrDefault(s => s.Id == id);

        if (student == null)
        {
            Console.WriteLine("Студент не найден.");
            return;
        }
        students.Remove(student);
        Console.WriteLine("Студент удалён.");
    }
    // Поиск студента по ФИО или группе
    static void SearchStudents()
    {
        Console.Write("Введите ФИО или группу для поиска: ");
        string search = Console.ReadLine() ?? "";

        List<Student> result = students
            .Where(s =>
             // поиск подстроки в ФИО
                s.Name.IndexOf(search, StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                s.Group.IndexOf(search, StringComparison.CurrentCultureIgnoreCase) >= 0) // посик подстроки в группе
            .ToList();

        if (result.Count == 0)
            Console.WriteLine("Студенты не найдены.");
        else
            ShowStudents(result);
    }
    // Сортировка студентов
    static void SortStudents()
    {
        Console.WriteLine(" Сортировка: ");
        Console.WriteLine("1. По ФИО");
        Console.WriteLine("2. По возрасту");
        Console.WriteLine("3. По группе");
        Console.WriteLine("4. По среднему баллу");
        int choice = ReadInt("Выберите критерий: ");

        List<Student> sorted;
        switch (choice)
        {
            case 1:
                sorted = students
                    .OrderBy(s => s.Name, StringComparer.CurrentCultureIgnoreCase) // для сортировки берется имя
                    .ToList();
                break;
            case 2:
                sorted = students.OrderBy(s => s.Age).ToList();
                break;
            case 3:
                sorted = students
                    .OrderBy(s => s.Group, StringComparer.CurrentCultureIgnoreCase)
                    .ToList();
                break;
            case 4:
                sorted = students.OrderBy(s => s.AverageGrade).ToList();
                break;
            default:
                Console.WriteLine("Неверный критерий сортировки.");
                return;
        }
        ShowStudents(sorted);
    }
    // Сохранение списка в файл
    static void SaveToFile()
    {
        try
        {
            string[] lines = students.Select(s => // преобразование в строки
                $"{s.Id}|{s.Name}|{s.Age}|{s.Group}|" +
                $"{s.AverageGrade.ToString(CultureInfo.InvariantCulture)}")
                .ToArray();

            File.WriteAllLines(filePath, lines); // запись в файл

            Console.WriteLine("Список сохранён в файл.");
            Console.WriteLine($"Путь: {filePath}");
        }
        catch (IOException)
        {
            Console.WriteLine("Не удалось сохранить данные в файл.");
        }
    }
    // Загрузка списка из файла
    static void LoadFromFile()
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("Файл не найден. Сначала сохраните список.");
            return;
        }

        try
        {
            string[] lines = File.ReadAllLines(filePath);
            List<Student> loadedStudents = new List<Student>(); // временный список
            // обработка строк
            foreach (string line in lines)
            {
                string[] parts = line.Split('|');
                if (parts.Length != 5)
                    continue;
                // проверка числовых значений
                if (!int.TryParse(parts[0], out int id))
                    continue;
                if (!int.TryParse(parts[2], out int age))
                    continue;
                // обработка среднего балла
                if (!double.TryParse(
                    parts[4],
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out double grade))
                    continue;
                // создание объекта загруженного студента
                loadedStudents.Add(new Student
                {
                    Id = id,
                    Name = parts[1],
                    Age = age,
                    Group = parts[3],
                    AverageGrade = grade
                });
            }
            students.Clear();
            students.AddRange(loadedStudents);
            Console.WriteLine($"Загружено студентов: {students.Count}.");
        }
        catch (IOException)
        {
            Console.WriteLine("Не удалось прочитать файл.");
        }
    }
    // Ввод целого числа
    static int ReadInt(string message)
    {
        int value;

        while (true)
        {
            Console.Write(message);

            if (int.TryParse(Console.ReadLine(), out value))
                return value;

            Console.WriteLine("Введите целое число.");
        }
    }

    // Ввод текста
    static string ReadText(string message)
    {
        while (true)
        {
            Console.Write(message);
            string value = (Console.ReadLine() ?? "");

            if (value.Length > 0 && !value.Contains("|"))
                return value;

            Console.WriteLine("Введите непустое значение без символа |.");
        }
    }

    // Ввод среднего балла
    static double ReadGrade(string message)
    {
        double grade;

        while (true)
        {
            Console.Write(message);

            if (double.TryParse(Console.ReadLine(), out grade) &&
                grade >= 1 && grade <= 10)
            {
                return grade;
            }

            Console.WriteLine("Средний балл должен быть от 1 до 10.");
        }
    }
}