using System;

namespace TaskManager
{
    public delegate void TaskHandler(string task);

    public class TaskManager
    {
        public void SendNotification(string task)
        {
            Console.WriteLine("Уведомление: задача \"" + task + "\" добавлена.");
        }

        public void WriteToLog(string task)
        {
            Console.WriteLine("Запись в журнал: задача \"" + task + "\" добавлена.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            TaskManager manager = new TaskManager();

            Console.Write("Введите задачу: ");
            string task = Console.ReadLine();

            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1 - Отправить уведомление");
            Console.WriteLine("2 - Записать в журнал");
            Console.Write("Ваш выбор: ");

            int choice = int.Parse(Console.ReadLine());

            TaskHandler handler;

            if (choice == 1)
            {
                handler = manager.SendNotification;
            }
            else
            {
                handler = manager.WriteToLog;
            }

            handler(task);            
        }
    }
}