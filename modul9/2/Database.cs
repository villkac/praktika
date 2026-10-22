using Microsoft.Data.Sqlite;
using System;
using System.Data;
namespace ProjectManager
{
    public class Database
    {
        private static string connectionString =
            @"Data Source=D:\Практика\modul9\2\Task.db";
        // Общие методы работы с базой
        public static DataTable GetData(string sql, params SqliteParameter[] parameters)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddRange(parameters);

            using var reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return table;
        }
        public static void Execute(string sql, params SqliteParameter[] parameters)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddRange(parameters);
            command.ExecuteNonQuery();
        }
        private static SqliteParameter Param(string name, object value) =>
            new SqliteParameter(name, value ?? DBNull.Value);
        public static bool Exists(string table, int id) =>
            GetData($"SELECT Id FROM {table} WHERE Id = @id",
                Param("@id", id)).Rows.Count > 0;
        // Проекты
        public static DataTable GetProjects() =>
            GetData("SELECT Id, Name, Description, StartDate, EndDate, Status FROM Projects ORDER BY Id");
        public static void AddProject(string name) =>
            Execute("INSERT INTO Projects (Name) VALUES (@name)",
                Param("@name", name));
        public static void UpdateProject(int id, string name) =>
            Execute("UPDATE Projects SET Name = @name WHERE Id = @id",
                Param("@name", name), Param("@id", id));
        public static void DeleteProject(int id) =>
            Execute("DELETE FROM Projects WHERE Id = @id", Param("@id", id));
        // Задачи
        public static DataTable GetTasks() =>
            GetData(@"SELECT t.Id, p.Name AS ProjectName, t.Name,
                      t.Description, t.StartDate, t.EndDate,
                      t.Status, t.Progress
                      FROM Tasks t JOIN Projects p ON p.Id = t.ProjectId
                      ORDER BY t.Id");
        public static void AddTask(int projectId, string name) =>
            Execute("INSERT INTO Tasks (ProjectId, Name) VALUES (@projectId, @name)",
                Param("@projectId", projectId), Param("@name", name));
        public static void UpdateTask(int id, string name, int progress, string status) =>
            Execute(@"UPDATE Tasks SET Name = @name, Progress = @progress,
                      Status = @status WHERE Id = @id",
                Param("@name", name), Param("@progress", progress),
                Param("@status", status), Param("@id", id));
        public static void DeleteTask(int id) =>
            Execute("DELETE FROM Tasks WHERE Id = @id", Param("@id", id));
        public static void AssignTask(int taskId, int employeeId) =>
            Execute("INSERT INTO TaskAssignments (TaskId, EmployeeId) VALUES (@taskId, @employeeId)",
                Param("@taskId", taskId), Param("@employeeId", employeeId));
        // Сотрудники
        public static DataTable GetEmployees() =>
            GetData("SELECT Id, FullName, Position, Email FROM Employees ORDER BY Id");
        public static void AddEmployee(string name, string position, string email) =>
            Execute(@"INSERT INTO Employees (FullName, Position, Email)
                      VALUES (@name, @position, @email)",
                Param("@name", name), Param("@position", position),
                Param("@email", string.IsNullOrWhiteSpace(email) ? null : email.Trim()));
        public static void UpdateEmployee(int id, string name, string position, string email) =>
            Execute(@"UPDATE Employees SET FullName = @name,
                      Position = @position, Email = @email WHERE Id = @id",
                Param("@name", name), Param("@position", position),
                Param("@email", string.IsNullOrWhiteSpace(email) ? null : email.Trim()),
                Param("@id", id));
        public static void DeleteEmployee(int id) =>
            Execute("DELETE FROM Employees WHERE Id = @id", Param("@id", id));
        // Отчёт
        public static DataTable GetReport() =>
            GetData(@"SELECT p.Id, p.Name AS ProjectName, p.Status,
                      COUNT(t.Id) AS TotalTasks,
                      SUM(CASE WHEN t.Status = 'Завершена' THEN 1 ELSE 0 END) AS CompletedTasks,
                      ROUND(COALESCE(AVG(t.Progress), 0), 1) AS AverageProgress
                      FROM Projects p LEFT JOIN Tasks t ON t.ProjectId = p.Id
                      GROUP BY p.Id, p.Name, p.Status ORDER BY p.Id");
    }
}
