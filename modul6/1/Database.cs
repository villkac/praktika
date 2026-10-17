using System.Data;
using Microsoft.Data.Sqlite;

namespace _1
{
    internal class Database
    {
        string connectionString = "Data Source=employees.db";

        // Создание таблицы
        public Database()
        {
            using (SqliteConnection connection =
                   new SqliteConnection(connectionString)) // подлкючение к базе
            {
                connection.Open();

                string sql = "CREATE TABLE IF NOT EXISTS Employees (" +
                    "Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                    "FullName TEXT, Position TEXT)";

                SqliteCommand command = new SqliteCommand(sql, connection);
                command.ExecuteNonQuery();
            }
        }

        // Получение сотрудников
        public DataTable GetEmployees()
        {
            DataTable table = new DataTable(); //  создание пустой таблицы

            using (SqliteConnection connection =
                   new SqliteConnection(connectionString)) // создание подключения 
            {
                connection.Open();

                SqliteCommand command = new SqliteCommand( // запрос на получения сотрудников
                    "SELECT * FROM Employees", connection);

                table.Load(command.ExecuteReader()); // загрузка в табицу полученных данных
            }

            return table;
        }
        // Добавление сотрудника
        public void AddEmployee(string name, string position)
        {
            using (SqliteConnection connection =
                   new SqliteConnection(connectionString))
            {
                connection.Open();

                string sql = "INSERT INTO Employees (FullName, Position) " +
                    "VALUES ($name, $position)";

                SqliteCommand command = new SqliteCommand(sql, connection);
                // связка параметров запроса с данными сотрудника
                command.Parameters.AddWithValue("$name", name);
                command.Parameters.AddWithValue("$position", position);

                command.ExecuteNonQuery(); // добавление в базу
            }
        }
        // Изменение сотрудника
        public void UpdateEmployee(int id, string name, string position)
        {
            using (SqliteConnection connection =
                   new SqliteConnection(connectionString))
            {
                connection.Open();

                string sql = "UPDATE Employees SET FullName = $name, " +
                    "Position = $position WHERE Id = $id";

                SqliteCommand command = new SqliteCommand(sql, connection);
                // передача измененных данных
                command.Parameters.AddWithValue("$name", name);
                command.Parameters.AddWithValue("$position", position);
                command.Parameters.AddWithValue("$id", id);

                command.ExecuteNonQuery(); // выполнение запроса
            }
        }

        // Удаление сотрудника
        public void DeleteEmployee(int id)
        {
            using (SqliteConnection connection =
                   new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = new SqliteCommand(
                    "DELETE FROM Employees WHERE Id = $id", connection);

                command.Parameters.AddWithValue("$id", id); // передача ID сотрудника в запрос
                command.ExecuteNonQuery();
            }
        }
    }
}