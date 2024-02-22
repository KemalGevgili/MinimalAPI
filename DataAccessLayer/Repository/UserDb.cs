using System.Data.SQLite;
using DataAccessLayer.Entities;

namespace DataAccessLayer.Repository
{
    public class UserDb
    {
        private static readonly string connectionString = @"Data Source=UserDatabase.db;Version=3;";

        public UserDb()
        {
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            if (!File.Exists(@"UserDatabase.db"))
            {
                SQLiteConnection.CreateFile(@"UserDatabase.db");

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string createUserTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Users (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Surname TEXT NOT NULL
                    );";

                    using (var command = new SQLiteCommand(connection))
                    {
                        command.CommandText = createUserTableQuery;
                        command.ExecuteNonQuery();
                    }
                }



            }
            
        }

        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string getAllUsersQuery = "SELECT Id, Name, Surname FROM Users";

                using (var command = new SQLiteCommand(getAllUsersQuery, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new User
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Surname = reader.GetString(2)
                            });
                        }
                    }
                }
            }

            return users;
        }
        public User GetUserById(int id)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string getUserByIdQuery = "SELECT Id, Name, Surname FROM Users WHERE Id = @Id";

                using (var command = new SQLiteCommand(getUserByIdQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Surname = reader.GetString(2)
                            };
                        }
                    }
                }
            }

            return null;
        }

        public void AddUser(User user)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string addUserQuery = "INSERT INTO Users ( Name, Surname) VALUES ( @Name, @Surname);";

                using (var command = new SQLiteCommand(addUserQuery, connection))
                {
                    command.Parameters.AddWithValue("@Name", user.Name);
                    command.Parameters.AddWithValue("@Surname", user.Surname);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateUser(User user)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string updateUserQuery = "UPDATE Users SET Name = @Name, Surname = @Surname WHERE Id = @Id";

                using (var command = new SQLiteCommand(updateUserQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", user.Id);
                    command.Parameters.AddWithValue("@Name", user.Name);
                    command.Parameters.AddWithValue("@Surname", user.Surname);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteUser(int id)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string deleteUserQuery = "DELETE FROM Users WHERE Id = @Id";

                using (var command = new SQLiteCommand(deleteUserQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
