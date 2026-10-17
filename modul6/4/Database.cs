using System;
using System.Data;
using Microsoft.Data.Sqlite;

namespace WpfApp4
{
    internal class Database
    {
        string connectionString = "Data Source=finance.db";
        // Создание таблицы
        public Database()
        {
            using (SqliteConnection connection =
                   new SqliteConnection(connectionString)) // создание соединения
            {
                connection.Open();
                string sql = "CREATE TABLE IF NOT EXISTS FinanceRecords (" +
                    "Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                    "Type TEXT NOT NULL, " +
                    "Category TEXT NOT NULL, " +
                    "Amount REAL NOT NULL, " +
                    "Date TEXT NOT NULL, " +
                    "Description TEXT)";
                SqliteCommand command = new SqliteCommand(sql, connection); // выполнение запроса
                command.ExecuteNonQuery();
            }
        }
        // Получение всех записей
        public DataTable GetRecords()
        {
            DataTable table = new DataTable();

            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Type", typeof(string));
            table.Columns.Add("Category", typeof(string));
            table.Columns.Add("Amount", typeof(double));
            table.Columns.Add("Date", typeof(string));
            table.Columns.Add("Description", typeof(string));

            using (SqliteConnection connection =
                   new SqliteConnection(connectionString))
            {
                connection.Open();
                // запрос на получение всех столбцов из таблицы
                string sql = "SELECT * FROM FinanceRecords " + 
                    "ORDER BY Date DESC, Id DESC";

                SqliteCommand command =
                    new SqliteCommand(sql, connection);

                SqliteDataReader reader = command.ExecuteReader(); // создание объекта для чтения

                while (reader.Read())
                {
                    table.Rows.Add(
                        Convert.ToInt32(reader["Id"]),
                        reader["Type"].ToString(),
                        reader["Category"].ToString(),
                        Convert.ToDouble(reader["Amount"]),
                        reader["Date"].ToString(),
                        reader["Description"].ToString());
                }
                reader.Close();
            }
            return table;
        }
        // Добавление записи
        public void AddRecord(
            string type,
            string category,
            double amount,
            DateTime date,
            string description)
        {
            using (SqliteConnection connection =
                   new SqliteConnection(connectionString))
            {
                connection.Open();

                string sql = "INSERT INTO FinanceRecords " +
                    "(Type, Category, Amount, Date, Description) " +
                    "VALUES ($type, $category, $amount, $date, $description)";

                SqliteCommand command =
                    new SqliteCommand(sql, connection);

                command.Parameters.AddWithValue("$type", type);
                command.Parameters.AddWithValue("$category", category);
                command.Parameters.AddWithValue("$amount", amount);

                command.Parameters.AddWithValue(
                    "$date", date.ToString("yyyy-MM-dd"));

                command.Parameters.AddWithValue("$description", description);

                command.ExecuteNonQuery();
            }
        }

        // Изменение записи
        public void UpdateRecord(
            int id,
            string type,
            string category,
            double amount,
            DateTime date,
            string description)
        {
            using (SqliteConnection connection =
                   new SqliteConnection(connectionString))
            {
                connection.Open();

                string sql = "UPDATE FinanceRecords SET " +
                    "Type = $type, Category = $category, " +
                    "Amount = $amount, Date = $date, " +
                    "Description = $description WHERE Id = $id";

                SqliteCommand command =
                    new SqliteCommand(sql, connection);

                command.Parameters.AddWithValue("$type", type);
                command.Parameters.AddWithValue("$category", category);
                command.Parameters.AddWithValue("$amount", amount);

                command.Parameters.AddWithValue(
                    "$date", date.ToString("yyyy-MM-dd"));

                command.Parameters.AddWithValue("$description", description);
                command.Parameters.AddWithValue("$id", id);

                command.ExecuteNonQuery();
            }
        }

        // Удаление записи
        public void DeleteRecord(int id)
        {
            using (SqliteConnection connection =
                   new SqliteConnection(connectionString))
            {
                connection.Open();

                string sql =
                    "DELETE FROM FinanceRecords WHERE Id = $id";

                SqliteCommand command =
                    new SqliteCommand(sql, connection);

                command.Parameters.AddWithValue("$id", id);

                command.ExecuteNonQuery();
            }
        }

        // Подсчёт суммы доходов или расходов
        public double GetTotalByType(string type)
        {
            using (SqliteConnection connection =
                   new SqliteConnection(connectionString))
            {
                connection.Open();

                string sql =
                    "SELECT COALESCE(SUM(Amount), 0) " +
                    "FROM FinanceRecords WHERE Type = $type";

                SqliteCommand command =
                    new SqliteCommand(sql, connection);

                command.Parameters.AddWithValue("$type", type); // передаём тип операции, которую нужно суммировать.

                object result = command.ExecuteScalar(); // получение результата

                return Convert.ToDouble(result);
            }
        }
    }
}