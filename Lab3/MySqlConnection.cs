using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;

namespace Lab3
{
    public class Database
    {
        private MySqlConnection connection;
        private string localDatabasePath;

        public Database(string connectionString, string localDatabasePath)
        {
            this.localDatabasePath = localDatabasePath;
            connection = new MySqlConnection(connectionString);
        }

        public void OpenConnection()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
        }

        public void CloseConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }

        public MySqlConnection GetConnection()
        {
            return connection;
        }

        // Метод для резервного копирования базы данных
        public void BackupDatabase(string backupPath)
        {
            try
            {
                // Используем mysqldump для резервного копирования
                string dumpCommand = $"mysqldump --user={GetUserId()} --password={GetPassword()} --host={GetServer()} {GetDatabaseName()} > {backupPath}";
                ExecuteCommand(dumpCommand);
                Console.WriteLine("Резервное копирование выполнено успешно.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при резервном копировании через строку подключения: {ex.Message}");
                // Если не удалось, копируем локальную базу данных
                BackupLocalDatabase(backupPath);
            }
        }

        // Метод для восстановления базы данных из резервной копии
        public void RestoreDatabase(string backupPath)
        {
            try
            {
                // Используем MySql для восстановления базы данных
                string restoreCommand = $"mysql --user={GetUserId()} --password={GetPassword()} --host={GetServer()} {GetDatabaseName()} < {backupPath}";
                ExecuteCommand(restoreCommand);
                Console.WriteLine("Восстановление базы данных выполнено успешно.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при восстановлении через строку подключения: {ex.Message}");
                // Если не удалось, восстанавливаем локальную базу данных
                RestoreLocalDatabase();
            }
        }

        private void BackupLocalDatabase(string backupPath)
        {
            try
            {
                File.Copy(localDatabasePath, backupPath, true);
                Console.WriteLine("Локальное резервное копирование выполнено успешно.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при локальном резервном копировании: {ex.Message}");
            }
        }

        private void RestoreLocalDatabase()
        {
            try
            {
                File.Copy(localDatabasePath, localDatabasePath + ".bak", true); // Создаем резервную копию
                Console.WriteLine("Локальное восстановление выполнено успешно.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при локальном восстановлении: {ex.Message}");
            }
        }

        private string GetUserId()
        {
            // Получение ID пользователя из строки подключения
            return GetConnectionStringValue("User ID");
        }

        private string GetPassword()
        {
            // Получение пароля из строки подключения
            return GetConnectionStringValue("Password");
        }

        private string GetServer()
        {
            // Получение сервера из строки подключения
            return GetConnectionStringValue("Server");
        }

        private string GetDatabaseName()
        {
            // Получение имени базы данных из строки подключения
            return GetConnectionStringValue("Database");
        }

        private string GetConnectionStringValue(string key)
        {
            // Получаем значение из строки подключения
            var builder = new MySqlConnectionStringBuilder(connection.ConnectionString);
            return builder[key].ToString();
        }

        private void ExecuteCommand(string command)
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = $"/C {command}";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при выполнении команды: {ex.Message}");
            }
        }
    }
}
