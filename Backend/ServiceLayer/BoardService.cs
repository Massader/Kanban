using IntroSE.Kanban.Backend.BusinessLayer;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    class BoardService
    {
        private BoardController BC;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// The constructor of the BoardService
        /// </summary>
        /// <param name="BC">the board controller</param>
        public BoardService(BoardController BC)
        {
            this.BC = BC;
        }
        /// <summary>
        /// loading the board data from the persistance
        /// </summary>
        ///<returns cref="Response">The response of the action</returns>
        public Response LoadData()
        {
            try
            {
                BC.LoadData();
                log.Info("Board data loaded.");
                return new Response();
            }
            catch (Exception e)
            {
                log.Fatal(String.Format("Board data failed to load - Exception Message: {0}", e.Message));
                return new Response(e.Message);
            }
        }
        /// <summary>
        /// deleting the board data from the persistance
        /// </summary>
        ///<returns cref="Response">The response of the action</returns>
        public Response DeleteData()
        {
            try
            {
                BC.DeleteData();
                log.Info("Board data deleted.");
                return new Response();
            }
            catch (Exception e)
            {
                log.Debug(String.Format("Board data failed to delete - Exception Message: {0}", e.Message));
                return new Response(e.Message);
            }
        }
        /// <summary>
        /// Limit the number of tasks in a specific column
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response LimitColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int limit)
        {
            try
            {
                BC.LimitColumn(userEmail, creatorEmail, boardName, columnOrdinal, limit);
                log.Info(String.Format("{0} limited the column in {1}", userEmail, boardName));
                return new Response();
            }
            catch (Exception e)
            {
                log.Debug(String.Format("{0} failed to limit the column in {1} - Exception Message: {2}", userEmail, boardName, e.Message));
                return new Response(e.Message);
            }

        }
        /// <summary>
        /// Get the limit of a specific column
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>The limit of the column.</returns>
        public Response<int> GetColumnLimit(string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            try
            {
                int columnLimit = BC.GetColumnLimit(userEmail, creatorEmail, boardName, columnOrdinal);
                log.Info(String.Format("{0} got the limit of column in {1}", userEmail, boardName));
                return Response<int>.FromValue(columnLimit);
            }
            catch (Exception e)
            {
                log.Debug(String.Format("{0} failed to get the limit of column in {1} - Exception Message: {2}", userEmail, boardName, e.Message));
                return Response<int>.FromError(e.Message);
            }
        }
        /// <summary>
        /// Get the name of a specific column
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>The name of the column.</returns>
        public Response<string> GetColumnName(string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            try
            {
                string columnName = BC.GetColumnName(userEmail, creatorEmail, boardName, columnOrdinal);
                log.Info(String.Format("{0} got the name of column in {1}", userEmail, boardName));
                return Response<string>.FromValue(columnName);
            }
            catch (Exception e)
            {
                log.Debug(String.Format("{0} failed to get the name of column in {1} - Exception Message: {2}", userEmail, boardName, e.Message));
                return Response<string>.FromError(e.Message);
            }
        }
        /// <summary>
        /// Add a new task.
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns>A response object with a value set to the Task, instead the response should contain a error message in case of an error</returns>
        public Response<Task> AddTask(string userEmail, string creatorEmail, string boardName, string title, string description, DateTime dueDate)
        {
            try
            {
                BusinessLayer.Task t = BC.AddTask(userEmail, creatorEmail, boardName, title, description, dueDate);
                Task task = new Task(t.ID, t.CreationTime, title, description, dueDate, t.TaskAssignee, t.Position);
                log.Info(String.Format("{0} added the task {1} to {2}", userEmail, title, boardName));
                return Response<Task>.FromValue(task);
            }
            catch (Exception e)
            {
                log.Debug(String.Format("{0} failed to add the task {1} to {2} - Exception Message: {3}", userEmail, title, boardName, e.Message));
                return Response<Task>.FromError(e.Message);
            }

        }
        /// <summary>
        /// Update the due date of a task
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">The new due date of the column</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response UpdateTaskDueDate(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {
            try
            {
                BC.UpdateTaskDueDate(userEmail, creatorEmail, boardName, columnOrdinal, taskId, dueDate);
                log.Info(String.Format("{0} updated a task duedate to:{1} ", userEmail, dueDate));
                return new Response();
            }
            catch (Exception e)
            {
                log.Debug(String.Format("{0} failed to update a task due date to:{1} - Exception Message: {2}", userEmail, dueDate, e.Message));
                return new Response(e.Message);
            }
        }
        /// <summary>
        /// Update task title
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="title">New title for the task</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response UpdateTaskTitle(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string title)
        {
            try
            {
                BC.UpdateTaskTitle(userEmail, creatorEmail, boardName, columnOrdinal, taskId, title);
                log.Info(String.Format("{0} updated a task title to:{1} ", userEmail, title));
                return new Response();
            }
            catch (Exception e)
            {
                log.Debug(String.Format("{0} failed to update a task title to:{1}  - Exception Message: {2}", userEmail, title, e.Message));
                return new Response(e.Message);
            }
        }
        /// <summary>
        /// Update the description of a task
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="description">New description for the task</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response UpdateTaskDescription(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string description)
        {
            try
            {
                BC.UpdateTaskDescription(userEmail, creatorEmail, boardName, columnOrdinal, taskId, description);
                log.Info(String.Format("{0} updated a task description to:{1} ", userEmail, description));
                return new Response();
            }
            catch (Exception e)
            {
                log.Debug(String.Format("{0} failed to update a task description to:{1} - Exception Message: {2} ", userEmail, description, e.Message));
                return new Response(e.Message);
            }
        }
        /// <summary>
        /// Advance a task to the next column
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response AdvanceTask(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId)
        {
            try
            {
                BC.AdvanceTask(userEmail, creatorEmail, boardName, columnOrdinal, taskId);
                log.Info(String.Format("{0} advanced a task", userEmail));
                return new Response();
            }
            catch (Exception e)
            {
                log.Debug(String.Format("{0} failed to advance a task - Exception Message: {1}", userEmail, e.Message));
                return new Response(e.Message);
            }
        }
        /// <summary>
        /// Returns a column given it's name
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response object with a value set to the Column, The response should contain a error message in case of an error</returns>
        public Response<IList<Task>> GetColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            try
            {
                IList<Task> cTasks = new List<Task>();
                foreach (BusinessLayer.Task t in BC.GetColumn(userEmail, creatorEmail, boardName, columnOrdinal).Tasks.Values)
                {
                    cTasks.Add(new Task(t.ID, t.CreationTime, t.Title, t.Description, t.DueDate, t.TaskAssignee, t.Position));
                }
                log.Info(String.Format("{0} got a column from {1}", userEmail, boardName));
                return Response<IList<Task>>.FromValue(cTasks);
            }
            catch (Exception e)
            {
                log.Debug(String.Format("{0} failed to get a column from {1} - Exception Message: {2}", userEmail, boardName, e.Message));
                return Response<IList<Task>>.FromError(e.Message);
            }
        }
        /// <summary>
        /// Adds a board to the specific user.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="name">The name of the new board</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response AddBoard(string email, string name)
        {
            try
            {
                BC.CreateBoard(email, name);
                log.Info(String.Format("{0} added new board", email));
                return new Response();
            }
            catch (Exception e)
            {
                log.Debug(String.Format("{0} failed to add new board - Exception Message: {1}", email, e.Message));
                return new Response(e.Message);
            }
        }
        /// <summary>
        /// Removes a board.
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response RemoveBoard(string userEmail, string creatorEmail, string boardName)
        {
            try
            {
                BC.RemoveBoard(userEmail, creatorEmail, boardName);
                log.Info(String.Format("{0} removed a board", userEmail));
                return new Response();
            }
            catch (Exception e)
            {
                log.Debug(String.Format("{0} failed to remove a board - Exception Message: {1}", userEmail, e.Message));
                return new Response(e.Message);
            }
        }
        /// <summary>
        /// Returns all the in-progress tasks of the logged-in user is assigned to.
        /// </summary>
        /// <param name="email">Email of the logged in user</param>
        /// <returns>A response object with a value set to the list of tasks, The response should contain a error message in case of an error</returns>
        public Response<IList<Task>> InProgressTasks(string email)
        {
            try
            {
                IList<Task> sTasks = new List<Task>();
                foreach (BusinessLayer.Task t in BC.ShowInProgress(email))
                {
                    sTasks.Add(new Task(t.ID, t.CreationTime, t.Title, t.Description, t.DueDate, t.TaskAssignee, t.Position));
                }
                log.Info(String.Format("{0} got his inProgress tasks", email));
                return Response<IList<Task>>.FromValue(sTasks);
            }
            catch (Exception e)
            {
                log.Debug(String.Format("{0} failed to get his inProgress tasks - Exception Message: {1}", email, e.Message));
                return Response<IList<Task>>.FromError(e.Message);
            }
        }
        /// <summary>
        /// Assigns a task to a user
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>        
        /// <param name="assigneeEmail">Email of the user to assign to task to</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response AssignTask(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string assigneeEmail)
        {
            try
            {
                BC.AssignTask(userEmail, creatorEmail, boardName, columnOrdinal, taskId, assigneeEmail);
                log.Info(String.Format("{0} assigned to a task", assigneeEmail));
                return new Response();
            }
            catch (Exception e)
            {
                log.Debug(String.Format("{0} failed to assign to a task - Exception Message: {1}", assigneeEmail, e.Message));
                return new Response(e.Message);
            }
        }

        /// <summary>
        /// Adds a board created by another user to the logged-in user. 
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the new board</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response JoinBoard(string userEmail, string creatorEmail, string boardName)
        {
            try
            {
                BC.JoinBoard(userEmail, creatorEmail, boardName);
                log.Info(String.Format("{0} joined the board {1}", userEmail, boardName));
                return new Response();
            }
            catch (Exception e)
            {
                log.Debug(String.Format("{0} failed to join the board {1} - Exception Message: {2}", userEmail, boardName, e.Message));
                return new Response(e.Message);
            }
        }

        /// <summary>
        /// Adds a new column
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The location of the new column. Location for old columns with index>=columnOrdinal is increased by 1 (moved right). The first column is identified by 0, the location increases by 1 for each column.</param>
        /// <param name="columnName">The name for the new columns</param>        
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response AddColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, string columnName)
        {
            try
            {
                BC.AddColumn(userEmail, creatorEmail, boardName, columnOrdinal, columnName);
                log.Info(String.Format("Added column {0} to board {1} in ordinal {2}.", columnName, boardName, columnOrdinal));
                return new Response();
            }
            catch (Exception e)
            {
                log.Debug(String.Format("Failed to add column {0} to board {1} - Exception Message: {2}", columnName, boardName, e.Message));
                return new Response(e.Message);
            }
        }



        /// <summary>
        /// Removes a specific column
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column location. The first column location is identified by 0, the location increases by 1 for each column</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response RemoveColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            try
            {
                BC.RemoveColumn(userEmail, creatorEmail, boardName, columnOrdinal);
                log.Info(String.Format("Removed column from board {0} in ordinal {1}.", boardName, columnOrdinal));
                return new Response();
            }
            catch (Exception e)
            {
                log.Debug(String.Format("Failed to remove column from board {0} - Exception Message: {1}", boardName, e.Message));
                return new Response(e.Message);
            }
        }


        /// <summary>
        /// Renames a specific column
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column location. The first column location is identified by 0, the location increases by 1 for each column</param>
        /// <param name="newColumnName">The new column name</param>        
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response RenameColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, string newColumnName)
        {
            try
            {
                BC.RenameColumn(userEmail, creatorEmail, boardName, columnOrdinal, newColumnName);
                log.Info(String.Format("Renamed column in board {0} at ordinal {1} to {2}.", boardName, columnOrdinal, newColumnName));
                return new Response();
            }
            catch (Exception e)
            {
                log.Debug(String.Format("Failed to rename column in board {0} - Exception Message: {1}", boardName, e.Message));
                return new Response(e.Message);
            }
        }

        /// <summary>
        /// Moves a column shiftSize times to the right. If shiftSize is negative, the column moves to the left
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column location. The first column location is identified by 0, the location increases by 1 for each column</param>
        /// <param name="shiftSize">The number of times to move the column, relativly to its current location. Negative values are allowed</param>  
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response MoveColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int shiftSize)
        {
            try
            {
                BC.MoveColumn(userEmail, creatorEmail, boardName, columnOrdinal, shiftSize);
                log.Info(String.Format("Moved column in board {0} at ordinal {1} to {2}.", boardName, columnOrdinal, columnOrdinal + shiftSize));

                return new Response();
            }
            catch (Exception e)
            {
                log.Debug(String.Format("Failed to move column in board {0} from ordinal {1} to {2} - Exception Message: {3}", boardName, columnOrdinal, columnOrdinal + shiftSize, e.Message));
                return new Response(e.Message);
            }
        }

        public Response<IList<Column>> GetBoard(string userEmail, string boardName, string creatorEmail)
        {
            try
            {
                BusinessLayer.Board board = BC.GetBoard(userEmail, boardName, creatorEmail);
                List<Column> columns = new List<Column>();
                foreach(BusinessLayer.Column c in board.Columns.Values)
                {
                    Column column = new Column(c.ColumnName, c.Limit, c.Position, new List<Task>());
                    columns.Add(column);
                    foreach(BusinessLayer.Task t in c.Tasks.Values)
                    {
                        Task task = new Task(t.ID, t.CreationTime, t.Title, t.Description, t.DueDate, t.TaskAssignee, t.Position);
                        column.TaskList.Add(task);
                    }
                }
                return Response<IList<Column>>.FromValue(columns);
            }
            catch(Exception e)
            {
                log.Debug(String.Format("Failed to get board {0} for {1} - Exception Message: {2}", boardName, userEmail, e.Message));
                return Response<IList<Column>>.FromError(e.Message);
            }
        }

        public Response<IList<String>> GetBoardMembers(String userEmail, String boardName, String creatorEmail)
        {
            try
            {
                IList<BusinessLayer.User> members = BC.GetBoardMembers(userEmail, boardName, creatorEmail);
                IList<String> memberEmails = new List<String>();
                foreach (BusinessLayer.User u in members)
                {
                    memberEmails.Add(u.Email);
                }
                return Response<IList<String>>.FromValue(memberEmails);
            }
            catch (Exception e)
            {
                log.Debug(String.Format("Failed to get board {0} members - Exception Message: {1}", boardName, e.Message));
                return Response<IList<String>>.FromError(e.Message);
            }
        }

        public Response<IList<Board>> GetAllBoards()
        {
            try
            {
                IList<BusinessLayer.Board> boardsList = BC.Boards.Values.ToList<BusinessLayer.Board>();
                IList<Board> boards = new List<Board>();
                foreach (BusinessLayer.Board b in boardsList)
                {
                    boards.Add(new Board(b.BoardID, b.Name, b.Admin.Email));
                }
                log.Info("Got all boards.");
                return Response<IList<Board>>.FromValue(boards);
            }
            catch(Exception e)
            {
                log.Debug(String.Format("Failed to get all boards - Exception Message: {0}", e.Message));
                return Response<IList<Board>>.FromError(e.Message);
            }
        }
    }
}
