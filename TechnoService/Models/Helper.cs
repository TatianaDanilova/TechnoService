using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;


namespace TechnoService.Models
{
    public class Helper
    {
        private const string DatabaseName = "TechnoService.db";
        private const int CurrentDatabaseVersion = 3; // Установите текущую версию базы данных

        public Helper()
        {
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            bool databaseExists = File.Exists(DatabaseName);
            bool versionChanged = CheckDatabaseVersionChanged();

            if (databaseExists && versionChanged)
            {
                File.Delete(DatabaseName);
            }

            if (!databaseExists || versionChanged)
            {
                CreateDatabase();
                CreateTables();
                FillInitialData();
                UpdateDatabaseVersion();
            }
        }

        private bool CheckDatabaseVersionChanged()
        {
            // Проверяем, существует ли файл версии
            if (!File.Exists("version.txt"))
            {
                return true;
            }

            int savedVersion;
            if (int.TryParse(File.ReadAllText("version.txt"), out savedVersion))
            {
                return savedVersion != CurrentDatabaseVersion;
            }

            return true;
        }

        private void UpdateDatabaseVersion()
        {
            File.WriteAllText("version.txt", CurrentDatabaseVersion.ToString());
        }

        private void CreateDatabase()
        {
            SQLiteConnection.CreateFile(DatabaseName);
        }

        private void CreateTables()
        {
            using (var connection = new SQLiteConnection($"Data Source={DatabaseName};Version=3;"))
            {
                connection.Open();

                string createUsersTable = @"
                    CREATE TABLE Users (
                        UserId INTEGER PRIMARY KEY AUTOINCREMENT,
                        FIO NCHAR(50) NOT NULL,
                        Phone NCHAR(50) NOT NULL,
                        Login NCHAR(50) NOT NULL,
                        Password NCHAR(50) NOT NULL,
                        Type NCHAR(50) NOT NULL);";

                string createRequestsTable = @"
                    CREATE TABLE Requests (
                        RequestId INTEGER PRIMARY KEY AUTOINCREMENT,
                        StartDate DATE NOT NULL,
                        TechType NCHAR(50) NOT NULL,
                        Model NCHAR(50) NOT NULL,
                        ProblemDescription NCHAR(50) NOT NULL,
                        Status NCHAR(50) NOT NULL,
                        EndDate DATE,
                        RepairPart NCHAR(50),
                        MasterId INTEGER,
                        ClientId INTEGER NOT NULL,
                        StatusChangeDate TEXT DEFAULT CURRENT_TIMESTAMP,
                        FOREIGN KEY (MasterId) REFERENCES Users(UserId),
                        FOREIGN KEY (ClientId) REFERENCES Users(UserId)
                    );";

                string createCommentsTable = @"
                    CREATE TABLE Comments (
                        CommentId INTEGER PRIMARY KEY AUTOINCREMENT,
                        Message NCHAR(50) NOT NULL,
                        MasterId INTEGER NOT NULL,
                        RequestId INTEGER NOT NULL,
                        FOREIGN KEY (RequestId) REFERENCES Requests(RequestId),
                        FOREIGN KEY (MasterId) REFERENCES Users(UserId)
                    );"
                ;

                using (var command = new SQLiteCommand(createUsersTable, connection))
                {
                    command.ExecuteNonQuery();
                }

                using (var command = new SQLiteCommand(createRequestsTable, connection))
                {
                    command.ExecuteNonQuery();
                }

                using (var command = new SQLiteCommand(createCommentsTable, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private void FillInitialData()
        {
            using (var connection = new SQLiteConnection($"Data Source={DatabaseName};Version=3;"))
            {
                connection.Open();

                string insertUsers = @"
                    INSERT INTO Users (FIO, Phone, Login, Password, Type) VALUES
                    ('Трубин Никита Юрьевич', '89210563128', 'kasoo', 'root', 'Менеджер'),
                    ('Мурашов Андрей Юрьевич', '89535078985', 'murashov123', 'qwerty', 'Мастер'),
                    ('Степанов Андрей Викторович', '89210673849', 'test1', 'test1', 'Мастер'),
                    ('Перина Анастасия Денисовна', '89990563748', 'perinaAD', '250519', 'Оператор'),
                    ('Мажитова Ксения Сергеевна', '89994563847', 'krutiha1234567', '1234567890', 'Оператор'),
                    ('Семенова Ясмина Марковна', '89994563847', 'login1', 'pass1', 'Мастер'),
                    ('Баранова Эмилия Марковна', '89994563841', 'login2', 'pass2', 'Заказчик'),
                    ('Егорова Алиса Платоновна', '89994563842', 'login3', 'pass3', 'Заказчик'),
                    ('Титов Максим Иванович', '89994563843', 'login4', 'pass4', 'Заказчик'),
                    ('Иванов Марк Максимович', '89994563844', 'login5', 'pass5', 'Мастер');";

                string insertRequests = @"
                    INSERT INTO Requests (StartDate, TechType, Model, ProblemDescription, Status, EndDate, RepairPart, MasterId, ClientId) VALUES
                    ('2023-06-06', 'Фен', 'Ладомир ТА112 белый', 'Перестал работать', 'В процессе ремонта', NULL, NULL, 2, 7),
                    ('2023-05-05', 'Тостер', 'Redmond RT-437 черный', 'Перестал работать', 'В процессе ремонта', NULL, NULL, 3, 7),
                    ('2022-07-07', 'Холодильник', 'Indesit DS 316 W белый', 'Не морозит одна из камер холодильника', 'Готов к выдаче', '2023-01-01', NULL, 2, 8),
                    ('2023-08-02', 'Стиральная машина', 'DEXP WM-F610NTMA/WW белый', 'Перестали работать многие режимы стирки', 'Новая заявка', NULL, NULL, NULL, 8),
                    ('2023-08-02', 'Мультиварка', 'Redmond RMC-M95 черный', 'Перестала включаться', 'Новая заявка', NULL, NULL, NULL, 9),
                    ('2023-08-02', 'Фен', 'Ладомир ТА113 чёрный', 'Перестал работать', 'Готов к выдаче', '2023-08-03', NULL, 2, 7),
                    ('2023-07-09', 'Холодильник', 'Indesit DS 314 W серый', 'Гудит, но не замораживает', 'Готов к выдаче', '2023-08-03', 'Мотор обдува морозильной камеры холодильника', 2, 8);";

                string insertComments = @"
                    INSERT INTO Comments (Message, MasterId, RequestId) VALUES
                    ('Интересная поломка', 2, 1),
                    ('Очень странно, будем разбираться!', 3, 2),
                    ('Скорее всего потребуется мотор обдува!', 2, 7),
                    ('Интересная проблема', 2, 1),
                    ('Очень странно, будем разбираться!', 3, 6);"
                ;

                using (var command = new SQLiteCommand(insertUsers, connection))
                {
                    command.ExecuteNonQuery();
                }

                using (var command = new SQLiteCommand(insertRequests, connection))
                {
                    command.ExecuteNonQuery();
                }

                using (var command = new SQLiteCommand(insertComments, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
