using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    ///<summary>Class <c>BoardDalController</c> is a class in charge of performing actions on the BoardMembers table in the database.
    ///<c>_connectionString</c> is a string containing the path to the database and sql version.
    ///<c>_tableName</c> holds the name of the BoardMembers table.
    ///<c>BoardMemberTableName</c> is a constant holding the table name.</summary>
    class BoardMemberDalController : DalController
    {
        private const string BoardMemberTableName = "BoardMembers";
        /// <summary>
        /// The constructor of the class
        /// </summary>
        public BoardMemberDalController() : base(BoardMemberTableName) { }

        /// <summary>
        /// Inserts an entry into the database.
        /// </summary>
        /// <param name="boardMember">the DTO containing the data to enter into the database.</param>
        /// <returns>returns a boolean value that answers with success or failure.</returns>
        public bool Insert(BoardMemberDTO boardMember)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {BoardMemberTableName} ({BoardMemberDTO.IDColumnName}, {BoardMemberDTO.UserIDColumnName}) " +
                        $"VALUES (@idVal,@userIDVal);";

                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", boardMember.ID);
                    SQLiteParameter userIDParam = new SQLiteParameter(@"userIDVal", boardMember.UserID);

                    command.Parameters.Add(idParam);
                    command.Parameters.Add(userIDParam);
                    command.Prepare();

                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    log.Debug(String.Format("Board Member DTO failed in creation in database - Exception Message: {0}", e.Message));
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
            BoardMemberDTO result = new BoardMemberDTO((long)reader.GetValue(0), (long)reader.GetValue(1), false);
            return result;
        }
        /// <summary>
        /// Deletes a database entry from the database.
        /// </summary>
        /// <param name="reader">a return type from the database SQL query.</param>
        /// <returns>a boolean value stating if the deletion worked.</returns>
        public bool Delete(BoardMemberDTO DTOObj)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {_tableName} where ID={DTOObj._id} and where UserID = {DTOObj.UserID}"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }


    }
}