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
    public bool hasPremium { get; set; }
    public string rank { get; set; } = string.Empty;
    public int rankCount { get; set; }
    public int CasinoMoney { get; set; } = 100;
    public string devGameName { get; set; } = "";


    public Repository()
    {
        InitDb();
    }

    



    public void SignUp(string email, string password)
    {
        string hashedPassword = HashPassword(password);
        string rnn = "Bronze";
        InsertUser(email, hashedPassword, rnn);
    }

    public bool Login(string email, string password)
    {
        string hashedPassword = HashPassword(password);

        string sql = "SELECT hasPremium, role FROM users WHERE email = @email AND password = @password"; // WHERE - neco jako if, po nem das podminku

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
                        hasPremium = !reader.IsDBNull(0) && reader.GetInt32(0) == 1;
                        rank = reader.IsDBNull(1) ? null : reader.GetString(1);
                        return true;
                    }
                    else
                    {
                        IsLoggedIn = false;
                        hasPremium = false;
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
    public void SetHasPremium(bool value)
    {
        hasPremium = value;
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

    private void InsertUser(string email, string hashedPassword, string rnn)
    {
        string sql = """
                     INSERT INTO users(email, password , role) 
                     VALUES (@Email, @Password , @rnn);
                     """;

        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();

            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", hashedPassword);
                command.Parameters.AddWithValue("@rnn", rnn);
                command.Parameters.AddWithValue("@hasPremium", 0); // 0 = false, 1 = true

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
                hasPremium = true;
            }
        }
    }

    public void RemovePremiumFromUser(string email)
    {
        string sql = """
                 UPDATE users
                 SET hasPremium = null
                 WHERE email = @Email;
                 """;

        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();

            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Email", email);
                command.ExecuteNonQuery();
                hasPremium = true;
            }
        }
    }

    public void AddRankToUser(string email, string rank)
    {
        string sql = """
                 UPDATE users
                 SET role = @rank
                 WHERE email = @Email;
                 """;

        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();

            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@rank", rank);
                command.ExecuteNonQuery();
                hasPremium = true;
            }
        }
    }

    public void CheckRank(string email)
    {
        string sql = "SELECT role FROM users WHERE email = @Email";

        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();

            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Email", email);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        rank = reader.IsDBNull(0) ? null : reader.GetString(0);
                    }
                    else
                    {
                        rank = null;
                    }
                }
            }
        }
    }



    public void SetCasinoMoney(int amount)
    {
        CasinoMoney = amount;
    }

public bool ResetPassword(string email, string newPassword)
{
    if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(newPassword))
        return false;

    string hashedPassword = HashPassword(newPassword);

    string sql = """
             UPDATE users
             SET password = @Password
             WHERE email = @Email;
             """;

    using (var connection = new SQLiteConnection(_connectionString))
    {
        connection.Open();

        using (var command = new SQLiteCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@Password", hashedPassword);
            int rowsAffected = command.ExecuteNonQuery();
            
            return rowsAffected > 0; // Returns true only if a user was actually updated
        }
    }
}


    public void addGameName(string email, string gamename)
    {
        string sql = """
                 UPDATE users
                 SET gamename = @gamename
                 WHERE email = @Email;
                 """;

        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();

            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@gamename", gamename);
                command.ExecuteNonQuery();
                hasPremium = true;
            }
        }
    }
    public void addGameDescription(string email, string gamedescription)
    {
        string sql = """
                 UPDATE users
                 SET gamedescription = @gamedescription
                 WHERE email = @Email;
                 """;

        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();

            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@gamedescription", gamedescription);
                command.ExecuteNonQuery();
                hasPremium = true;
            }
        }
    }
    

    public void CheckGameName(string email)
    {
        string sql = "SELECT gamename FROM users WHERE email = @Email";

        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();

            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Email", email);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        devGameName = reader.IsDBNull(0) ? null : reader.GetString(0);
                    }
                    else
                    {
                        devGameName = null;
                    }
                }
            }
        }
    }


// public void AddColumnToUsersTable(string defaultValue = null)
    // {
    //     string sql = "ALTER TABLE users ADD COLUMN gamedescription TEXT";

    //     if (!string.IsNullOrEmpty(defaultValue))
    //     {
    //         // Properly escape the default value for SQL
    //         sql += $" DEFAULT '{defaultValue.Replace("'", "''")}'";
    //     }

    //     try
    //     {
    //         using (var connection = new SQLiteConnection(_connectionString))
    //         {
    //             connection.Open();
    //             using (var command = new SQLiteCommand(sql, connection))
    //             {
    //                 command.ExecuteNonQuery();
    //             }
    //         }
    //     }
    //     catch (SQLiteException ex)
    //     {
    //         // Handle cases where column might already exist
    //         if (ex.Message.Contains("duplicate column name"))
    //         {
    //             // Column already exists, you might want to log this or handle it differently
    //             Console.WriteLine("Column 'gamename' already exists in users table.");
    //         }
    //         else
    //         {
    //             throw; // Re-throw other SQLite exceptions
    //         }
    //     }
    // }

}