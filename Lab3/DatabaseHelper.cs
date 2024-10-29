using System.Configuration;

public class DatabaseHelper
{
    public string GetConnectionString()
    {
        return ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
    }
}
