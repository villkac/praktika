using System;

namespace MobileNotifications
{
    public delegate void NotificationHandler(string message);

    public class Notification
    {
        public event NotificationHandler MessageReceived; // событие получено сообщение
        public event NotificationHandler CallReceived;
        public event NotificationHandler EmailReceived;

        public void SendMessage(string message)
        {
            MessageReceived?.Invoke(message); // вызов подписанного метода
        }

        public void MakeCall(string caller)
        {
            CallReceived?.Invoke(caller);
        }

        public void SendEmail(string email)
        {
            EmailReceived?.Invoke(email);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Notification notification = new Notification();

            notification.MessageReceived += ShowMessage;
            notification.CallReceived += ShowCall;
            notification.EmailReceived += ShowEmail;

            notification.SendMessage("Привет! Как дела?");
            notification.MakeCall("Александр");
            notification.SendEmail("У вас новое письмо");

        }

        static void ShowMessage(string message)
        {
            Console.WriteLine("Новое сообщение: " + message);
        }

        static void ShowCall(string caller)
        {
            Console.WriteLine("Входящий звонок от: " + caller);
        }

        static void ShowEmail(string email)
        {
            Console.WriteLine("Электронная почта: " + email);
        }
    }
}