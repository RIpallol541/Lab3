using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System;

public class AuthService
{
    private MySqlConnection connection;

    public AuthService(string connectionString)
    {
        connection = new MySqlConnection(connectionString);
    }

    public string AuthenticateUser(string username, string password)
    {
        string query = "SELECT role FROM users WHERE username = @username AND password = @password";
        MySqlCommand command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@username", username);
        command.Parameters.AddWithValue("@password", password); // Используйте хеширование, если пароли хранятся в зашифрованном виде
        try
        {
            connection.Open();
            object roleObj = command.ExecuteScalar();

            // Проверяем, нашелся ли пользователь с таким логином и паролем
            if (roleObj != null)
            {
                return roleObj.ToString(); // Возвращаем роль пользователя (например, "user" или "admin")
            }
            else
            {
                return null; // Пользователь не найден
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка подключения к базе данных: " + ex.Message);
            return null;
        }
        finally
        {
            connection.Close();
        }
    }

}

