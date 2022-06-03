using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System;
using System.Data.SQLite;
using System.Text;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    ///<summary>Class <c>BoardDalController</c> is a class in charge of performing actions on the Boards table in the database.
    ///<c>_connectionString</c> is a string containing the path to the database and sql version.
    ///<c>_tableName</c> holds the name of the Boards table.
    ///<c>BoardTableName</c> is a constant holding the table name.</summary>
    class BoardDalController : DalController
    {
        private const String BoardTableName = "Boards";
        /// <summary>
        /// The constructor of the class
        /// </summary>
        public BoardDalController() : base(BoardTableName) { }

        /// <summary>
        /// Inserts an entry into the database.
        /// </summary>
        /// <param name="board">the DTO containing the data to enter into the database.</param>
        /// <returns>returns a boolean value that answers with success or failure.</returns>
        public bool Insert(BoardDTO board)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {_tableName} ({BoardDTO.BoardIDColumnName} ,{BoardDTO.BoardNameColumnName}, {BoardDTO.CreatorEmailColumnName}) " +

                        $"VALUES (@idVal,@nameVal,@creatorVal);";

                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", board.ID);
                    SQLiteParameter titleParam = new SQLiteParameter(@"nameVal", board.Name);
                    SQLiteParameter creatorParam = new SQLiteParameter(@"creatorVal", board.CreatorID);

                    command.Parameters.Add(idParam);
                    command.Parameters.Add(titleParam);
                    command.Parameters.Add(creatorParam);
                    command.Prepare();

                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    log.Debug(String.Format("Board DTO failed in creation in database - Exception Message: {0}", e.Message));
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
            BoardDTO result = new BoardDTO((long)reader.GetValue(0), reader.GetString(1), (long)reader.GetValue(2), false);
            return result;
        }
    }
}