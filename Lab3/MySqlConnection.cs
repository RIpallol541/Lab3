using MySql.Data.MySqlClient;

public class Database
{
    private MySqlConnection connection;

    public Database(string connectionString)
    {

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
}
