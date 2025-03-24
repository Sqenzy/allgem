using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text;
using Allgem01.Components;

namespace Allgem01.Logic;

public class Repository
{
    // package name: System.Data.SQLite

    private string _connectionString = "Data Source=database.sqlite";
    public bool IsLoggedIn { get; set; }
    public bool IsEmailVerified { get; set; } = false;
    
    // abys mohl zobrazit ty data, tak si stahni DbBrowser for sqlite
    
    public Repository()
    {
        InitDb();
    }



    public void SignUp(string email, string password)
    {
        string hashedPassword = HashPassword(password);
        InsertUser(email, hashedPassword);
    }

    public bool Login(string email, string password)
    {
        string hashedPassword = HashPassword(password);

        string sql = "SELECT * FROM users WHERE email = @email AND password = @password"; // WHERE - neco jako if, po nem das podminku

        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();

            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@password", hashedPassword);

                using (SQLiteDataReader reader = command.ExecuteReader()) // class pomoci ktery muzes cist data z databaze
                {
                    //return reader.Read(); // .Read() vraci hodnotu, jestli vubec se nejaky radky vybraly
                    if (reader.Read())
                    {
                        IsLoggedIn = true;
                        return true;
                    }
                    else
                    {
                        IsLoggedIn = false;
                        return false;
                    }
                }
            }
        }
    }

    public void SetLoggedIn(bool value)
    {
        IsLoggedIn = value;
    }

    private void InitDb()
    {
        string sql = """
                     CREATE TABLE IF NOT EXISTS users(
                         id INTEGER PRIMARY KEY, 
                         email TEXT NOT NULL, 
                         password TEXT NOT NULL, 
                         role TEXT,
                         hasPremium INTEGER);
                     """;

        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            using (var command = new SQLiteCommand(sql, connection))
            {
                command.ExecuteNonQuery(); // ExecuteNonQuery - nevraci zadne data
            }
        }
    }

    private void InsertUser(string email, string hashedPassword)
    {
        string sql = """
                     INSERT INTO users(email, password) 
                     VALUES (@Email, @Password);
                     """;

        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();

            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", hashedPassword);
                
                command.ExecuteNonQuery();
            }
        }
    }

    private string HashPassword(string password)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(password); // konvertuje string na byty ve formatu UTF-8 ASCII
        byte[] hashedPasswordBytes = SHA256.HashData(bytes);
        //return Encoding.UTF8.GetString(hashedPasswordBytes);
        string hashedPassword = Encoding.UTF8.GetString(hashedPasswordBytes);
        return hashedPassword;
    }
    public void AddPremiumToUser(string email)
{
    string sql = """
                 UPDATE users
                 SET hasPremium = 1
                 WHERE email = @Email;
                 """;

    using (var connection = new SQLiteConnection(_connectionString))
    {
        connection.Open();

        using (var command = new SQLiteCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@Email", email);
            command.ExecuteNonQuery();
        }
    }
}
}