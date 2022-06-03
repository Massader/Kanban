using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    ///<summary>Class <c>UserDalController</c> is a class in charge of performing actions on the Users table in the database.
    ///<c>_connectionString</c> is a string containing the path to the database and sql version.
    ///<c>_tableName</c> holds the name of the Users table.
    ///<c>UserTableName</c> is a constant holding the table name.</summary>
    class UserDalController : DalController
    {
        private const string UserTableName = "Users";
        /// <summary>
        /// The constructor of the class
        /// </summary>
        public UserDalController() : base(UserTableName) { }
        /// <summary>
        /// Inserts an entry into the database.
        /// </summary>
        /// <param name="user">the DTO containing the data to enter into the database.</param>
        /// <returns>returns a boolean value that answers with success or failure.</returns>
        public bool Insert(UserDTO user)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {UserTableName}({UserDTO.IDColumnName}, {UserDTO.EmailColumnName}, {UserDTO.PasswordColumnName})" +
                        $" VALUES (@idVal, @emailVal, @passwordVal)";

                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", user.ID);
                    SQLiteParameter emailParam = new SQLiteParameter(@"emailVal", user.Email);
                    SQLiteParameter passwordParam = new SQLiteParameter(@"passwordVal", user.Password);

                    command.Parameters.Add(idParam);
                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(passwordParam);
                    command.Prepare();

                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    log.Debug(String.Format("User DTO failed in creation in database - Exception Message: {0}", e.Message));
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }
        /// <summary>
        /// Converts a database entry into a DTO.
        /// </summary>
        /// <param name="reader">a return type from the database SQL query.</param>
        /// <returns>A corresponding DTO type.</returns>
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            UserDTO result = new UserDTO((long)reader.GetValue(0), reader.GetString(1), reader.GetString(2), false);
            return result;
        }
    }
}