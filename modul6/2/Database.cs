using System;
using System.Data;
using Microsoft.Data.Sqlite;
namespace TaskManagerApp
{
    internal class Database
    {
        string connectionString = "Data Source=tasks.db"; // указание с какой бд работать
        // Создание таблицы
        public Database()
        {
            using (SqliteConnection connection =
                   new SqliteConnection(connectionString))
            {
                connection.Open();

                string sql = "CREATE TABLE IF NOT EXISTS Tasks (" +
                    "Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                    "TaskName TEXT, Description TEXT, " +
                    "IsCompleted INTEGER DEFAULT 0)";

                SqliteCommand command = new SqliteCommand(sql, connection); // создание команды для выполнения в бд
                command.ExecuteNonQuery(); // выполнение созданной команды               
            }
        }
        // Получение задач
        public DataTable GetTasks()
        {
            DataTable table = new DataTable();

            // Создаём столбцы с нужными типами данных
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("TaskName", typeof(string));
            table.Columns.Add("Description", typeof(string));
            table.Columns.Add("IsCompleted", typeof(bool));
            // соединение с бд
            using (SqliteConnection connection =
                   new SqliteConnection(connectionString))
            {
                connection.Open();
                // запрос на получение данных
                SqliteCommand command = new SqliteCommand(
                    "SELECT * FROM Tasks ORDER BY Id", connection);

                SqliteDataReader reader = command.ExecuteReader();
                // Переносим данные из базы в таблицу программы
                while (reader.Read())
                {
                    table.Rows.Add(
                        Convert.ToInt32(reader["Id"]),
                        reader["TaskName"].ToString(),
                        reader["Description"].ToString(),
                        Convert.ToInt32(reader["IsCompleted"]) == 1);
                }
                reader.Close();
            }
            return table;
        }
        // Добавление задачи
        public void AddTask(string name, string description)
        {
            using (SqliteConnection connection =
                   new SqliteConnection(connectionString))
            {
                connection.Open();
                // запрос, который добавляет новую запись в таблицу
                string sql = "INSERT INTO Tasks " +
                    "(TaskName, Description) " +
                    "VALUES ($name, $description)";

                SqliteCommand command = new SqliteCommand(sql, connection);
                // передача параметров
                command.Parameters.AddWithValue("$name", name);
                command.Parameters.AddWithValue("$description", description);

                command.ExecuteNonQuery();
            }
        }
        // Изменение задачи
        public void UpdateTask(int id, string name, string description)
        {
            using (SqliteConnection connection =
                   new SqliteConnection(connectionString))
            {
                connection.Open();

                string sql = "UPDATE Tasks SET TaskName = $name, " +
                    "Description = $description WHERE Id = $id";

                SqliteCommand command = new SqliteCommand(sql, connection);

                command.Parameters.AddWithValue("$name", name);
                command.Parameters.AddWithValue("$description", description);
                command.Parameters.AddWithValue("$id", id);

                command.ExecuteNonQuery();
            }
        }
        // Изменение статуса выполнения
        public void UpdateTaskStatus(int id, bool isCompleted)
        {
            using (SqliteConnection connection =
                   new SqliteConnection(connectionString))
            {
                connection.Open();

                string sql = "UPDATE Tasks SET IsCompleted = $status " +
                    "WHERE Id = $id";

                SqliteCommand command = new SqliteCommand(sql, connection);
                // преобразование статуса
                command.Parameters.AddWithValue(
                    "$status", isCompleted ? 1 : 0);

                command.Parameters.AddWithValue("$id", id);

                command.ExecuteNonQuery();
            }
        }
        // Удаление задачи
        public void DeleteTask(int id)
        {
            using (SqliteConnection connection =
                   new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = new SqliteCommand(
                    "DELETE FROM Tasks WHERE Id = $id", connection);

                command.Parameters.AddWithValue("$id", id);
                command.ExecuteNonQuery();
            }
        }
    }
}