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
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace TechnoService.Models
{
    public class DBManager
    {
        private const string ConnectionString = "Data Source=TechnoService.db;Version=3;";

        // Авторизация пользователя
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


        // Регистрация пользователя
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


        // Поиск пользователя
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

        // Вывод всех заявок
        public static List<Request> GetAllRequests()
        {
            List<Request> requests = new List<Request>();

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string query = "SELECT RequestId, TechType, Model, ProblemDescription, RepairPart, Status, StartDate, EndDate, ClientId, MasterId FROM Requests";

                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            requests.Add(new Request
                            {
                                RequestId = reader.GetInt32(0),
                                TechType = reader.IsDBNull(1) ? null : reader.GetString(1),
                                Model = reader.IsDBNull(2) ? null : reader.GetString(2),
                                ProblemDescription = reader.IsDBNull(3) ? null : reader.GetString(3),
                                RepairPart = reader.IsDBNull(4) ? null : reader.GetString(4),
                                Status = reader.IsDBNull(5) ? null : reader.GetString(5),
                                StartDate = reader.GetDateTime(6),
                                EndDate = reader.IsDBNull(7) ? (DateTime?)null : reader.GetDateTime(7),
                                ClientId = reader.GetInt32(8),
                                MasterId = reader.IsDBNull(9) ? (int?)null : reader.GetInt32(9)
                            });
                        }
                    }
                }
            }

            return requests;
        }


        // Поиск заявки
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



        // Вывод заявок 
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


        // Добавление заявки
        public static void AddRequest(string techType, string model, string description, int clientId)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string query = @"
        INSERT INTO Requests (TechType, Model, ProblemDescription, StartDate, Status, ClientId, RepairPart, EndDate, MasterId)
        VALUES (@TechType, @Model, @Description, @StartDate, @Status, @ClientId, NULL, NULL, NULL)";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TechType", techType);
                    command.Parameters.AddWithValue("@Model", model);
                    command.Parameters.AddWithValue("@Description", description);
                    command.Parameters.AddWithValue("@StartDate", DateTime.Now);
                    command.Parameters.AddWithValue("@Status", "Новая заявка");
                    command.Parameters.AddWithValue("@ClientId", clientId);

                    command.ExecuteNonQuery();
                }
            }
        }

        // Вывод всех мастеров
        public static List<User> GetMasters()
        {
            List<User> masters = new List<User>();

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string query = "SELECT UserId, FIO FROM Users WHERE Type = 'Мастер'";

                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            masters.Add(new User
                            {
                                UserId = reader.GetInt32(0),
                                FIO = reader.GetString(1)
                            });
                        }
                    }
                }
            }
            return masters;
        }
        public static void UpdateRequestMaster(int requestId, int masterId)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string query = "UPDATE Requests SET MasterId = @MasterId WHERE RequestId = @RequestId";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MasterId", masterId);
                    command.Parameters.AddWithValue("@RequestId", requestId);

                    command.ExecuteNonQuery();
                }
            }
        }

        // Мастер
        public static List<Request> LoadRequestsForCurrentMaster(int masterId)
        {
            var requests = new List<Request>();

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = @"
                SELECT r.RequestId, r.TechType, r.RepairPart, r.Status, c.Message, c.CommentId
                FROM Requests r
                LEFT JOIN Comments c ON r.RequestId = c.RequestId
                WHERE r.MasterId = @MasterId";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MasterId", masterId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var request = new Request
                            {
                                RequestId = reader.GetInt32(0),
                                TechType = reader.GetString(1),
                                RepairPart = reader.IsDBNull(2) ? null : reader.GetString(2),
                                Status = reader.GetString(3),
                                Comment = reader.IsDBNull(4) ? null : new Comment
                                {
                                    Message = reader.GetString(4),
                                    CommentId = reader.GetInt32(5)
                                }
                            };
                            requests.Add(request);
                        }
                    }
                }
            }
            return requests;
        }

        // Обновление комментария
        public static void UpdateComment(int commentId, string newComment)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "UPDATE Comments SET Message = @Message WHERE CommentId = @CommentId";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Message", newComment);
                    command.Parameters.AddWithValue("@CommentId", commentId);
                    command.ExecuteNonQuery();
                }
            }
        }

        

        // Обновление поломанной детали
        public static void UpdateRepairPart(int requestId, string newRepairPart)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "UPDATE Requests SET RepairPart = @RepairPart WHERE RequestId = @RequestId";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RepairPart", newRepairPart);
                    command.Parameters.AddWithValue("@RequestId", requestId);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Обновление статуса
        public static void UpdateStatus(int requestId, string newStatus)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "UPDATE Requests SET Status = @Status WHERE RequestId = @RequestId";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Status", newStatus);
                    command.Parameters.AddWithValue("@RequestId", requestId);
                    command.ExecuteNonQuery();
                }
            }
        }
        public static void AddComment(Comment comment)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string query = @"
            INSERT INTO Comments (Message, RequestId, MasterId)
            VALUES (@Message, @RequestId, @MasterId)";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Message", comment.Message);
                    command.Parameters.AddWithValue("@RequestId", comment.RequestId);
                    command.Parameters.AddWithValue("@MasterId", comment.MasterId);

                    command.ExecuteNonQuery();
                }
            }
        }

        // Статистика
        public static int GetNumberOfDoneRequestsByType(string type)
        {
            int numberOfDoneRequests = 0;

            try
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();

                    string query = @"
                SELECT COUNT(*)
                FROM Requests
                WHERE TechType = @type";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@type", type);

                        object result = command.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            numberOfDoneRequests = Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Здесь можно добавить логирование ошибки
                Console.WriteLine($"Error getting number of done requests by type: {ex.Message}");
            }

            return numberOfDoneRequests;
        }
        public static int GetMidTimeOfDoneRequestsByType(string type)
        {
            int midTime = 0;

            try
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();

                    string query = @"
                SELECT AVG(julianday(EndDate) - julianday(StartDate)) 
                FROM Requests
                WHERE Status = 'Готов к выдаче'
                AND TechType = @type";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@type", type);

                        object result = command.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            midTime = Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Здесь можно добавить логирование ошибки
                Console.WriteLine($"Error getting mid time of done requests by type: {ex.Message}");
            }
            return midTime;
        }
        public static int GetNumberOfDoneRequests()
        {
            int numberOfDoneRequests = 0;

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string query = @"
            SELECT COUNT(*) 
            FROM Requests 
            WHERE Status = 'Готов к выдаче';";

                using (var command = new SQLiteCommand(query, connection))
                {
                    numberOfDoneRequests = Convert.ToInt32(command.ExecuteScalar());
                }
            }

            return numberOfDoneRequests;
        }
        // Метод для получения среднего времени выполнения заявки
        public static int GetAverageTime()
        {
            int averageDays = 0;

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string query = @"
            SELECT AVG(julianday(EndDate) - julianday(StartDate)) 
            FROM Requests 
            WHERE Status = 'Готов к выдаче';";

                using (var command = new SQLiteCommand(query, connection))
                {
                    var result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        averageDays = Convert.ToInt32(Convert.ToDouble(result));
                    }
                }
            }

            return averageDays;
        }

        public static void UpdateEndDate(int requestId, DateTime endDate)
        {
            string query = $"UPDATE Requests SET EndDate = '{endDate:yyyy-MM-dd}' WHERE RequestId = {requestId}";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);
                command.ExecuteNonQuery();
            }
        }

        // Уведомления
        public static void AddNotification(int clientId, string message)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string query = @"
            INSERT INTO Notifications (ClientId, Message)
            VALUES (@ClientId, @Message)";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ClientId", clientId);
                    command.Parameters.AddWithValue("@Message", message);

                    command.ExecuteNonQuery();
                }
            }
        }

        public static List<Notification> GetNotificationsByClientId(int clientId)
        {
            List<Notification> notifications = new List<Notification>();


            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string query = @"
            SELECT *
            FROM Notifications
            WHERE ClientId = @ClientId";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ClientId", clientId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Notification notification = new Notification
                            {
                                NotificationId = reader.GetInt32(0),
                                ClientId = reader.GetInt32(1),
                                Message = reader.GetString(2),
                                Date = reader.GetDateTime(3)
                            };

                            notifications.Add(notification);
                        }
                    }
                }
            }

            return notifications;
        }

    }
}


