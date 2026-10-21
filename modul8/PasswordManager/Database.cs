using System;
using System.Data;
using Microsoft.Data.Sqlite;

namespace PasswordManager
{
    public static class Database
    {
        private static string dbPath = @"D:\Практика\modul8\PasswordManager\passwords.db";
        private static string connectionString = $"Data Source={dbPath}";  // подключение к базе       
        public static SqliteConnection GetConnection()
        {
            return new SqliteConnection(connectionString); // создание соединения
        }
        // поиск пользователя по логину
        public static DataTable GetUser(string login)
        {
            DataTable table = new DataTable();
            using (SqliteConnection connection = Database.GetConnection())
            {
                connection.Open();
                string sql = "SELECT * FROM Users WHERE Login = $login";
                SqliteCommand command = new SqliteCommand(sql, connection);                
                command.Parameters.AddWithValue("$login", login);
                // чтение данных
                SqliteDataReader reader = command.ExecuteReader();
                table.Load(reader);   // переносим всё из reader в таблицу
                reader.Close();
            }
            return table;
        }
        // Регистрация пользователя
        public static void AddUser(string login, string hash, string authSalt, string keySalt)
        {
            using (SqliteConnection connection = Database.GetConnection())
            {
                connection.Open();
                string sql = "INSERT INTO Users (Login, PasswordHash, AuthSalt, KeySalt) " +
                    "VALUES ($login, $hash, $authSalt, $keySalt)";
                SqliteCommand command = new SqliteCommand(sql, connection);
                command.Parameters.AddWithValue("$login", login);
                command.Parameters.AddWithValue("$hash", hash);
                command.Parameters.AddWithValue("$authSalt", authSalt);
                command.Parameters.AddWithValue("$keySalt", keySalt);
                command.ExecuteNonQuery();
            }
        }
        // Список категорий для выпадающего списка
        public static DataTable GetCategories()
        {
            DataTable table = new DataTable();
            using (SqliteConnection connection = Database.GetConnection())
            {
                connection.Open();
                string sql = "SELECT Id, Name FROM Categories ORDER BY Name";
                SqliteCommand command = new SqliteCommand(sql, connection);
                SqliteDataReader reader = command.ExecuteReader();
                table.Load(reader);
                reader.Close();
            }
            return table;
        }
        // Получение записей пользователя и поиск
        public static DataTable GetEntries(int userId, string search)
        {
            DataTable table = new DataTable();
            using (SqliteConnection connection = Database.GetConnection())
            {
                connection.Open();
                string sql = "SELECT e.Id, e.ServiceName, e.Login, e.Notes, " +
                    "e.EncryptedPassword, e.IV, e.CategoryId, " +
                    "c.Name AS Category, '••••••••' AS Pass " +
                    "FROM PasswordEntries e " +
                    "LEFT JOIN Categories c ON c.Id = e.CategoryId " +
                    "WHERE e.UserId = $userId " +
                    "AND (e.ServiceName LIKE $search OR e.Login LIKE $search) " +
                    "ORDER BY e.ServiceName";
                SqliteCommand command = new SqliteCommand(sql, connection);
                command.Parameters.AddWithValue("$userId", userId);
                command.Parameters.AddWithValue("$search", "%" + search + "%"); // получение шаблона поиска
                SqliteDataReader reader = command.ExecuteReader();
                table.Load(reader);
                reader.Close();
            }
            return table;
        }
        // Добавление записи 
        public static void AddEntry(int userId, object categoryId, string service,
            string login, string encryptedPassword, string iv, string notes)
        {
            using (SqliteConnection connection = Database.GetConnection())
            {
                connection.Open();
                string sql = "INSERT INTO PasswordEntries " +
                    "(UserId, CategoryId, ServiceName, Login, EncryptedPassword, IV, Notes) " +
                    "VALUES ($userId, $categoryId, $service, $login, $password, $iv, $notes)";
                SqliteCommand command = new SqliteCommand(sql, connection);
                command.Parameters.AddWithValue("$userId", userId);
                command.Parameters.AddWithValue("$categoryId", categoryId);
                command.Parameters.AddWithValue("$service", service);
                command.Parameters.AddWithValue("$login", login);
                command.Parameters.AddWithValue("$password", encryptedPassword);
                command.Parameters.AddWithValue("$iv", iv);
                command.Parameters.AddWithValue("$notes", notes);
                command.ExecuteNonQuery();
            }
        }
        // Изменение записи
        public static void UpdateEntry(int id, object categoryId, string service,
            string login, string encryptedPassword, string iv, string notes)
        {
            using (SqliteConnection connection = Database.GetConnection())
            {
                connection.Open();
                string sql = "UPDATE PasswordEntries SET CategoryId = $categoryId, " +
                    "ServiceName = $service, Login = $login, " +
                    "EncryptedPassword = $password, IV = $iv, Notes = $notes " +
                    "WHERE Id = $id";
                SqliteCommand command = new SqliteCommand(sql, connection);
                command.Parameters.AddWithValue("$id", id);
                command.Parameters.AddWithValue("$categoryId", categoryId);
                command.Parameters.AddWithValue("$service", service);
                command.Parameters.AddWithValue("$login", login);
                command.Parameters.AddWithValue("$password", encryptedPassword);
                command.Parameters.AddWithValue("$iv", iv);
                command.Parameters.AddWithValue("$notes", notes);
                command.ExecuteNonQuery();
            }
        }
        // Удаление записи
        public static void DeleteEntry(int id)
        {
            using (SqliteConnection connection = Database.GetConnection())
            {
                connection.Open();
                string sql = "DELETE FROM PasswordEntries WHERE Id = $id";
                SqliteCommand command = new SqliteCommand(sql, connection);
                command.Parameters.AddWithValue("$id", id);
                command.ExecuteNonQuery();
            }
        }
    }
}