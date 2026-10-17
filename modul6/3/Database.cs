using System;
using System.Data;
using Microsoft.Data.Sqlite;

namespace WpfApp3
{
    internal class Database
    {
        string connectionString = "Data Source=books.db";

        // Создание таблицы
        public Database()      
        {
            using (SqliteConnection connection =
                   new SqliteConnection(connectionString))
            {
                connection.Open();

                string sql = "CREATE TABLE IF NOT EXISTS Books (" +
                    "Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                    "Title TEXT NOT NULL, " +
                    "Author TEXT NOT NULL, " +
                    "IsRented INTEGER DEFAULT 0)";

                SqliteCommand command =
                    new SqliteCommand(sql, connection);

                command.ExecuteNonQuery();
            }
        }

        // Получение книг и поиск
        public DataTable GetBooks(string search)
        {
            DataTable table = new DataTable();

            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Title", typeof(string));
            table.Columns.Add("Author", typeof(string));
            table.Columns.Add("IsRented", typeof(bool));
            table.Columns.Add("Status", typeof(string));

            using (SqliteConnection connection =
                   new SqliteConnection(connectionString))
            {
                connection.Open();

                string sql = "SELECT * FROM Books " +
                    "WHERE Title LIKE $search " +
                    "OR Author LIKE $search ORDER BY Title";

                SqliteCommand command =
                    new SqliteCommand(sql, connection);

                command.Parameters.AddWithValue(  // формаривание запроса с помощью параметра
                    "$search", "%" + search + "%");

                SqliteDataReader reader = command.ExecuteReader();
                // чтение найдденных записей
                while (reader.Read())
                {
                    bool isRented = // получение статуса
                        Convert.ToInt32(reader["IsRented"]) == 1;

                    table.Rows.Add(
                        Convert.ToInt32(reader["Id"]),
                        reader["Title"].ToString(),
                        reader["Author"].ToString(),
                        isRented,
                        isRented ? "В аренде" : "Доступна"); // если isRented равно true, вернуть текст «В аренде»
                }

                reader.Close();
            }

            return table;
        }

        // Добавление книги
        public void AddBook(string title, string author)
        {
            using (SqliteConnection connection =
                   new SqliteConnection(connectionString))
            {
                connection.Open();

                string sql = "INSERT INTO Books " +
                    "(Title, Author) VALUES ($title, $author)";

                SqliteCommand command =
                    new SqliteCommand(sql, connection);

                command.Parameters.AddWithValue("$title", title);
                command.Parameters.AddWithValue("$author", author);

                command.ExecuteNonQuery();
            }
        }

        // Изменение статуса аренды
        public void SetRentalStatus(int id, bool isRented)
        {
            using (SqliteConnection connection =
                   new SqliteConnection(connectionString))
            {
                connection.Open();

                string sql = "UPDATE Books SET IsRented = $status " +
                    "WHERE Id = $id";

                SqliteCommand command =
                    new SqliteCommand(sql, connection);
                // преобразование статуса
                command.Parameters.AddWithValue( 
                    "$status", isRented ? 1 : 0);

                command.Parameters.AddWithValue("$id", id);

                command.ExecuteNonQuery();
            }
        }

        // Удаление книги
        public void DeleteBook(int id)
        {
            using (SqliteConnection connection =
                   new SqliteConnection(connectionString))
            {
                connection.Open();

                string sql = "DELETE FROM Books WHERE Id = $id";

                SqliteCommand command =
                    new SqliteCommand(sql, connection);

                command.Parameters.AddWithValue("$id", id);

                command.ExecuteNonQuery();
            }
        }
    }
}