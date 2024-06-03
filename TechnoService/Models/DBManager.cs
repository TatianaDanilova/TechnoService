using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Data.SQLite;
using System.Collections.ObjectModel;
using System.Data;

namespace TechnoService.Models
{
    public class DBManager
    {
        private const string ConnectionString = "Data Source=TechnoService.db;Version=3;";
        public static int AuthenticateUser(string login, string password)
        {
            try
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT userID 
                        FROM Users 
                        WHERE Login = @login AND Password = @password;";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@login", login);
                        command.Parameters.AddWithValue("@password", password);

                        var result = command.ExecuteScalar();

                        if (result != null)
                        {
                            return Convert.ToInt32(result);
                        }
                        else
                        {
                            return -1; // Пользователь не найден
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Здесь можно добавить логирование ошибки
                Console.WriteLine($"Error authenticating user: {ex.Message}");
                return -1; // Ошибка при аутентификации
            }
        }
        public static void RegisterUser(string name, string phone, string login, string password)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string query = @"
        INSERT INTO Users (FIO, Phone, Login, Password, Type)
        VALUES (@Name, @Phone, @Login, @Password, 'Заказчик')";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Phone", phone);
                    command.Parameters.AddWithValue("@Login", login);
                    command.Parameters.AddWithValue("@Password", password);

                    command.ExecuteNonQuery();
                }
            }
        }

        public static User GetUserById(int userId)
        {
            User user = null;

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string query = "SELECT UserId, FIO, Phone, Login, Password, Type FROM Users WHERE UserID = @userId";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new User
                            {
                                UserId = reader.GetInt32(0),
                                FIO = reader.GetString(1),
                                Phone = reader.GetString(2),
                                Login = reader.GetString(3),
                                Password = reader.GetString(4),
                                Type = reader.GetString(5)
                            };
                        }
                    }
                }
            }
            return user;
        }
        public static Request GetRequestById(int requestId)
        {
            Request request = null;

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string query = @"
        SELECT RequestId, TechType, Model, ProblemDescription, MasterId, RepairPart, Status, StartDate, EndDate, ClientId
        FROM Requests
        WHERE RequestId = @RequestId";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RequestId", requestId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            request = new Request
                            {
                                RequestId = reader.GetInt32(0),
                                TechType = reader.IsDBNull(1) ? null : reader.GetString(1),
                                Model = reader.IsDBNull(2) ? null : reader.GetString(2),
                                ProblemDescription = reader.IsDBNull(3) ? null : reader.GetString(3),
                                MasterId = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4),
                                RepairPart = reader.IsDBNull(5) ? null : reader.GetString(5),
                                Status = reader.IsDBNull(6) ? null : reader.GetString(6),
                                StartDate = reader.GetDateTime(7),
                                EndDate = reader.IsDBNull(8) ? (DateTime?)null : reader.GetDateTime(8),
                                ClientId = reader.GetInt32(9)
                            };
                        }
                    }
                }
            }

            return request;
        }

        public static DataTable GetRequests(string status, int clientId)
        {
            DataTable requestsTable = new DataTable();

            string query = @"
        SELECT RequestId, TechType, ProblemDescription 
        FROM Requests 
        WHERE Status = @status AND ClientId = @clientId;";

            try
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@status", status);
                        command.Parameters.AddWithValue("@clientId", clientId);

                        using (var adapter = new SQLiteDataAdapter(command))
                        {
                            adapter.Fill(requestsTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving requests: {ex.Message}");
            }

            return requestsTable;
        }
        public static User GetClientByRequestId(int requestId)
        {
            User client = null;

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string query = @"
            SELECT u.UserId, u.FIO, u.Phone, u.Login, u.Password, u.Type
            FROM Users u
            INNER JOIN Requests r ON u.UserId = r.ClientId
            WHERE r.RequestId = @RequestId";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RequestId", requestId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            client = new User
                            {
                                UserId = reader.GetInt32(0),
                                FIO = reader.GetString(1),
                                Phone = reader.GetString(2),
                                Login = reader.GetString(3),
                                Password = reader.GetString(4),
                                Type = reader.GetString(5)
                            };
                        }
                    }
                }
            }

            return client;
        }

        public static User GetMasterByRequestId(int requestId)
        {
            User master = null;

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string query = @"
            SELECT u.UserId, u.FIO, u.Phone, u.Login, u.Password, u.Type
            FROM Users u
            INNER JOIN Requests r ON u.UserId = r.MasterId
            WHERE r.RequestId = @RequestId";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RequestId", requestId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            master = new User
                            {
                                UserId = reader.GetInt32(0),
                                FIO = reader.GetString(1),
                                Phone = reader.GetString(2),
                                Login = reader.GetString(3),
                                Password = reader.GetString(4),
                                Type = reader.GetString(5)
                            };
                        }
                    }
                }
            }

            return master;
        }

        public static Comment GetCommentsByRequestId(int requestId)
{
    Comment comment = null;

    using (var connection = new SQLiteConnection(ConnectionString))
    {
        connection.Open();

        string query = @"
            SELECT c.CommentId, c.Message, c.MasterId
            FROM Comments c
            INNER JOIN Requests r ON c.RequestId = r.RequestId
            WHERE r.RequestId = @RequestId
            LIMIT 1"; // Добавляем условие LIMIT 1

        using (var command = new SQLiteCommand(query, connection))
        {
            command.Parameters.AddWithValue("@RequestId", requestId);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    // Создаем экземпляр класса Comment, если комментарий найден
                    comment = new Comment
                    {
                        CommentId = reader.GetInt32(0),
                        Message = reader.GetString(1),
                        MasterId = reader.GetInt32(2),
                    };
                }
            }
        }
    }
    return comment; // Возвращаем одиночный комментарий или null, если комментарий не найден
}

    }
}
