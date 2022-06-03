    using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    ///<summary>Class <c>TaskDalController</c> is a class in charge of performing actions on the Tasks table in the database.
    ///<c>_connectionString</c> is a string containing the path to the database and sql version.
    ///<c>_tableName</c> holds the name of the Tasks table.
    ///<c>TaskTableName</c> is a constant holding the table name.</summary>
    class TaskDalController : DalController
    {
        private const String TaskTableName = "Tasks";
        /// <summary>
        /// The constructor of the class
        /// </summary>
        public TaskDalController() : base(TaskTableName) { }

        /// <summary>
        /// Inserts an entry into the database.
        /// </summary>
        /// <param name="task">the DTO containing the data to enter into the database.</param>
        /// <returns>returns a boolean value that answers with success or failure.</returns>
        public bool Insert(TaskDTO task)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {_tableName} ({TaskDTO.TaskIDColumnName}, {TaskDTO.TitleColumnName}, {TaskDTO.DescriptionColumnName}, " +
                        $"{TaskDTO.DueDateColumnName}, {TaskDTO.CreationTimeColumnName}, {TaskDTO.PositionColumnName}, {TaskDTO.AssigneeColumnName}, {TaskDTO.BoardIDColumnName})" +
                        $" VALUES (@idVal,@titleVal,@descriptionVal,@dueDateVal,@creationTimeVal,@positionVal," +
                        $"@assigneeVal,@boardIDVal);";

                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", task.ID);
                    SQLiteParameter titleParam = new SQLiteParameter(@"titleVal", task.Title);
                    SQLiteParameter descriptionParam = new SQLiteParameter(@"descriptionVal", task.Description);
                    SQLiteParameter dueDateParam = new SQLiteParameter(@"dueDateVal", task.DueDate);
                    SQLiteParameter creationTimeParam = new SQLiteParameter(@"creationTimeVal", task.CreationTime);
                    SQLiteParameter positionParam = new SQLiteParameter(@"positionVal", task.Position);
                    SQLiteParameter assigneeParam = new SQLiteParameter(@"assigneeVal", task.Assignee);
                    SQLiteParameter boardIDParam = new SQLiteParameter(@"boardIDVal", task.Board);

                    command.Parameters.Add(idParam);
                    command.Parameters.Add(titleParam);
                    command.Parameters.Add(descriptionParam);
                    command.Parameters.Add(dueDateParam);
                    command.Parameters.Add(creationTimeParam);
                    command.Parameters.Add(positionParam);
                    command.Parameters.Add(assigneeParam);
                    command.Parameters.Add(boardIDParam);
                    command.Prepare();

                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    log.Debug(String.Format("Task DTO failed in creation in database - Exception Message: {0}", e.Message));
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
            TaskDTO result = new TaskDTO((long)reader.GetValue(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), (long)reader.GetValue(5), reader.GetString(6), (long)reader.GetValue(7), false);
            return result;
        }

    }
}