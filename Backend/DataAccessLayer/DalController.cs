using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using log4net;
using System.Reflection;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    ///<summary>Class <c>DalController</c> is an abstract class in charge of performing actions on the database.
    ///<c>_connectionString</c> is a string containing the path to the database and sql version.
    ///<c>_tableName</c> holds the name of the sub-classes' corresponding tables.</summary>
    public abstract class DalController //public for testing
    {
        protected readonly string _connectionString;
        protected readonly string _tableName;
        protected static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// The constructor of the class
        /// </summary>
        /// <param name="tableName">The name of the table the controller works on</param>
        public DalController(string tableName)
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            this._connectionString = $"Data Source={path}; Version=3;";
            this._tableName = tableName;
        }
        /// <summary>
        /// Updates an entry in the database.
        /// </summary>
        /// <param name="id">the id by which to find the row.</param>
        /// <param name="attributeName">the name of the row</param>
        /// <param name="attributeValue">the value to which the entry must change to</param>
        /// <returns>returns a boolean value that answers with success or failure.</returns>
        public bool Update(long id, string attributeName, string attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {_tableName} set [{attributeName}]=@{attributeName} where ID={id}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch
                {
                    log.Debug(String.Format("{0} failed to update.", attributeName));
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }
        /// <summary>
        /// Updates an entry in the database.
        /// </summary>
        /// <param name="id">the id by which to find the row.</param>
        /// <param name="attributeName">the name of the row</param>
        /// <param name="attributeValue">the value to which the entry must change to</param>
        /// <returns>returns a boolean value that answers with success or failure.</returns>
        public bool Update(long id, string attributeName, long attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {_tableName} set [{attributeName}]=@{attributeName} where ID={id}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch
                {
                    log.Debug(String.Format("{0} failed to update.", attributeName));
                }
                finally
                {
                    command.Dispose();
                    connection.Close();

                }

            }
            return res > 0;
        }
        /// <summary>
        /// Performs a select* query on the database.
        /// </summary>
        /// <returns>returns all values in a table in the form of one DTO per row.</returns>
        public List<DTO> Select()
        {
            List<DTO> results = new List<DTO>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {_tableName};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(ConvertReaderToObject(dataReader));
                    }
                }
                catch
                {
                    log.Debug("Select action failed.");
                }
                finally
                {
                    if (dataReader != null)
                    {
                        dataReader.Close();
                    }

                    command.Dispose();
                    connection.Close();
                }

            }
            return results;
        }
        /// <summary>
        /// Converts a database entry into a DTO.
        /// </summary>
        /// <param name="reader">a return type from the database SQL query.</param>
        /// <returns>A corresponding DTO type.</returns>
        protected abstract DTO ConvertReaderToObject(SQLiteDataReader reader);
        /// <summary>
        /// Deletes an entry in the database.
        /// </summary>
        /// <param name="DTOObj">the DTO containing the information to delete in the database.</param>
        /// <returns>returns a boolean value that answers with success or failure.</returns>
        public bool Delete(DTO DTOObj)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {_tableName} where ID={DTOObj._id}"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch
                {
                    log.Debug("DTO failed to delete from database.");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }
        /// <summary>
        /// Deletes all data in the database.
        /// </summary>
        /// <returns>returns a boolean value that answers with success or failure.</returns>
        public bool DeleteData()
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {_tableName};"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch
                {
                    log.Debug($"Failed to delete data from {_tableName}.");
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