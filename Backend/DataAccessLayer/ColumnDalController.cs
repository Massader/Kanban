using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class ColumnDalController : DalController
    {
        private const string ColumnTableName = "Columns";
        /// <summary>
        /// The constructor of the class
        /// </summary>
        public ColumnDalController() : base(ColumnTableName) { }

        /// <summary>
        /// Inserts an entry into the database.
        /// </summary>
        /// <param name="column">the DTO containing the data to enter into the database.</param>
        /// <returns>returns a boolean value that answers with success or failure.</returns>
        public bool Insert(ColumnDTO column)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {ColumnTableName} ({ColumnDTO.ColumnIDColumnName}, {ColumnDTO.ColumnNameColumnName}, " +
                        $"{ColumnDTO.LimitColumnName}, {ColumnDTO.PositionColumnName}, {ColumnDTO.BoardIDColumnName}) " +
                        $"VALUES (@idVal,@nameVal,@limitVal,@positionVal,@boardIDVal);";

                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", column.ID);
                    SQLiteParameter nameParam = new SQLiteParameter(@"nameVal", column.Name);
                    SQLiteParameter limitParam = new SQLiteParameter(@"limitVal", column.Limit);
                    SQLiteParameter positionParam = new SQLiteParameter(@"positionVal", column.Position);
                    SQLiteParameter boardIDParam = new SQLiteParameter(@"boardIDVal", column.BoardID);

                    command.Parameters.Add(idParam);
                    command.Parameters.Add(nameParam);
                    command.Parameters.Add(limitParam);
                    command.Parameters.Add(positionParam);
                    command.Parameters.Add(boardIDParam);
                    command.Prepare();

                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    log.Debug(String.Format("Column DTO failed in creation in database - Exception Message: {0}", e.Message));
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
            ColumnDTO result = new ColumnDTO((long)reader.GetValue(0), reader.GetString(1), (long)reader.GetValue(2), (long)reader.GetValue(3), (long)reader.GetValue(4), false);
            return result;
        }
    }
}