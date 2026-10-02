using System;

class BankAccount
{
    private int accountNumber;
    private string owner;
    private double balance;

    public BankAccount(int accountNumber, string owner, double balance)
    {
        this.accountNumber = accountNumber;
        this.owner = owner;
        this.balance = balance;
    }

    public int GetAccountNumber()
    {
        return accountNumber;
    }

    public void SetAccountNumber(int accountNumber)
    {
        this.accountNumber = accountNumber;
    }

    public string GetOwner()
    {
        return owner;
    }

    public void SetOwner(string owner)
    {
        this.owner = owner;
    }

    public double GetBalance()
    {
        return balance;
    }

    public void SetBalance(double balance)
    {
        this.balance = balance;
    }

    public void Deposit(double amount)
    {
        balance += amount;
        Console.WriteLine("Счет пополнен.");
    }

    public void Withdraw(double amount)
    {
        if (amount <= balance)
        {
            balance -= amount;
            Console.WriteLine("Средства сняты.");
        }
        else
        {
            Console.WriteLine("Недостаточно средств.");
        }
    }

    public void ShowInfo()
    {
        Console.WriteLine("Номер счета: " + accountNumber);
        Console.WriteLine("Владелец: " + owner);
        Console.WriteLine("Баланс: " + balance);
    }
}

class Program
{
    static void Main()
    {
        BankAccount account = new BankAccount(12345, "Шпак Виктория", 1000);

        int choice;

        do
        {          
            Console.WriteLine("БАНКОВСКИЙ СЧЕТ");
            Console.WriteLine("1. Показать информацию о счете");
            Console.WriteLine("2. Пополнить счет");
            Console.WriteLine("3. Снять средства");
            Console.WriteLine("4. Изменить владельца");
            Console.WriteLine("5. Изменить баланс");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите операцию: ");

            choice = int.Parse(Console.ReadLine()!);

            Console.WriteLine();

            switch (choice)
            {
                case 1:
                    account.ShowInfo();
                    break;

                case 2:
                    Console.Write("Введите сумму пополнения: ");
                    double deposit = double.Parse(Console.ReadLine()!);

                    account.Deposit(deposit);
                    Console.WriteLine("Текущий баланс: " + account.GetBalance());
                    break;

                case 3:
                    Console.Write("Введите сумму снятия: ");
                    double withdraw = double.Parse(Console.ReadLine()!);

                    account.Withdraw(withdraw);
                    Console.WriteLine("Текущий баланс: " + account.GetBalance());
                    break;

                case 4:
                    Console.Write("Введите нового владельца: ");
                    string owner = Console.ReadLine()!;

                    account.SetOwner(owner);
                    Console.WriteLine("Владелец изменен.");
                    break;

                case 5:
                    Console.Write("Введите новый баланс: ");
                    double balance = double.Parse(Console.ReadLine()!);

                    account.SetBalance(balance);
                    Console.WriteLine("Баланс изменен.");
                    break;

                case 0:
                    Console.WriteLine("Программа завершена.");
                    break;

                default:
                    Console.WriteLine("Такой операции нет.");
                    break;
            }

        } while (choice != 0);
    }
}