using Microsoft.Data.Sqlite;
using System;
using System.Data;
using System.IO;

namespace TaskManager
{
    public class Database
    {
        private static string connectionString = @"Data Source=D:\Практика\modul9\3\Tasks.db";
        public static void Initialize()
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            string sql = @"
                PRAGMA foreign_keys = ON;

                CREATE TABLE IF NOT EXISTS Tasks (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Description TEXT,
                    Priority TEXT NOT NULL DEFAULT 'Средний'
                        CHECK (Priority IN ('Низкий', 'Средний', 'Высокий')),
                    CreatedDate TEXT NOT NULL
                        DEFAULT (date('now', 'localtime')),
                    DueDate TEXT NOT NULL,
                    Status TEXT NOT NULL DEFAULT 'Не начата'
                        CHECK (Status IN ('Не начата', 'В работе', 'Завершена')),
                    Progress INTEGER NOT NULL DEFAULT 0
                        CHECK (Progress BETWEEN 0 AND 100)
                );

                CREATE TABLE IF NOT EXISTS Notifications (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    TaskId INTEGER NOT NULL,
                    Message TEXT NOT NULL,
                    CreatedDate TEXT NOT NULL
                        DEFAULT (datetime('now', 'localtime')),
                    IsRead INTEGER NOT NULL DEFAULT 0,
                    FOREIGN KEY (TaskId) REFERENCES Tasks(Id)
                        ON DELETE CASCADE
                );";

            using var command = new SqliteCommand(sql, connection);
            command.ExecuteNonQuery();
        }

        public static DataTable GetTasks()
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            string sql = @"
                SELECT Id, Name, Description, Priority,
                       CreatedDate, DueDate, Status, Progress
                FROM Tasks
                ORDER BY DueDate, Priority DESC, Id";

            using var command = new SqliteCommand(sql, connection);
            using var reader = command.ExecuteReader();

            DataTable table = new DataTable();
            table.Load(reader);

            return table;
        }

        public static void AddTask(
            string name, string description, string priority,
            string dueDate, string status, int progress)
        {
            Execute(@"
                INSERT INTO Tasks
                    (Name, Description, Priority, DueDate, Status, Progress)
                VALUES
                    (@name, @description, @priority, @dueDate, @status, @progress)",
                ("@name", name),
                ("@description", description),
                ("@priority", priority),
                ("@dueDate", dueDate),
                ("@status", status),
                ("@progress", progress));
        }

        public static void UpdateTask(
            int id, string name, string description, string priority,
            string dueDate, string status, int progress)
        {
            Execute(@"
                UPDATE Tasks
                SET Name = @name,
                    Description = @description,
                    Priority = @priority,
                    DueDate = @dueDate,
                    Status = @status,
                    Progress = @progress
                WHERE Id = @id",
                ("@name", name),
                ("@description", description),
                ("@priority", priority),
                ("@dueDate", dueDate),
                ("@status", status),
                ("@progress", progress),
                ("@id", id));
        }

        public static void DeleteTask(int id)
        {
            Execute("DELETE FROM Tasks WHERE Id = @id",
                ("@id", id));
        }

        private static void Execute(string sql,
            params (string Name, object Value)[] parameters)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            // Включаем поддержку внешних ключей в этом соединении.
            using (var pragma = new SqliteCommand(
                "PRAGMA foreign_keys = ON;", connection))
            {
                pragma.ExecuteNonQuery();
            }

            using var command = new SqliteCommand(sql, connection);

            foreach (var parameter in parameters)
            {
                command.Parameters.AddWithValue(
                    parameter.Name, parameter.Value ?? DBNull.Value);
            }

            command.ExecuteNonQuery();
        }

        public static DataTable GetDeadlineTasks()
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            string sql = @"
                SELECT Id, Name, DueDate
                FROM Tasks
                WHERE Status <> 'Завершена'
                  AND DueDate <= date('now', 'localtime', '+2 days')
                ORDER BY DueDate";

            using var command = new SqliteCommand(sql, connection);
            using var reader = command.ExecuteReader();

            DataTable table = new DataTable();
            table.Load(reader);

            return table;
        }

        public static bool AddNotificationIfNew(int taskId, string message)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            string sql = @"
                INSERT INTO Notifications (TaskId, Message)
                SELECT @taskId, @message
                WHERE NOT EXISTS (
                    SELECT 1
                    FROM Notifications
                    WHERE TaskId = @taskId
                      AND Message = @message
                      AND date(CreatedDate) = date('now', 'localtime')
                )";

            using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@taskId", taskId);
            command.Parameters.AddWithValue("@message", message);

            return command.ExecuteNonQuery() > 0;
        }
    }
}
