using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Model
{
    /// <summary>
    /// Connects the Presentation layer with the Bussines layer
    /// </summary>
    public class BackendController
    {
        private Service Service { get; set; }
        public BackendController(Service service)
        {
            this.Service = service;
        }

        public BackendController()
        {
            this.Service = new Service();
            Service.LoadData();
        }
        /// <summary>
        /// logs the user in
        /// </summary>
        /// <param name="email">user's email</param>
        /// <param name="password">user's password</param>
        /// <returns></returns>
        public UserModel Login(string email, string password)
        {
            Response<User> user = Service.Login(email, password);
            if (user.ErrorOccured)
            {
                throw new Exception(user.ErrorMessage);
            }
            return new UserModel(this, email);
        }
        /// <summary>
        /// logs the user out
        /// </summary>
        /// <param name="email">user's email</param>
        public void Logout(string email)
        {
            if (Service.Logout(email).ErrorOccured)
                throw new Exception("logout failed");
        }
        /// <summary>
        /// register new user
        /// </summary>
        /// <param name="email">new user's email</param>
        /// <param name="password">new user's password</param>
        internal void Register(string email, string password)
        {
            Response res = Service.Register(email, password);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }
        /// <summary>
        /// add new board
        /// </summary>
        /// <param name="email">user's email</param>
        /// <param name="boardname">new board's name</param>
        public void AddBoard(string email, string boardname)
        {
            Response res = Service.AddBoard(email, boardname);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }
        /// <summary>
        /// remove board
        /// </summary>
        /// <param name="email">user's email</param>
        /// <param name="boardname">name of the board to remove</param>
        public void RemoveBoard(string email, string boardname)
        {
            Response res = Service.RemoveBoard(email, email, boardname);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }
        /// <summary>
        /// add new task
        /// </summary>
        /// <param name="userEmail">user's email</param>
        /// <param name="creatorEmail">task's creator email</param>
        /// <param name="boardName">name of the board of the new task</param>
        /// <param name="title">new task's title</param>
        /// <param name="description">new task's description</param>
        /// <param name="dueDate">new task's due date</param>
        /// <returns></returns>
        public Model.TaskModel AddTask(string userEmail, string creatorEmail, string boardName, string title, string description, DateTime dueDate)
        {
            Response<IntroSE.Kanban.Backend.ServiceLayer.Task> res = Service.AddTask(userEmail, creatorEmail, boardName, title, description, dueDate);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
            else
            {
                return new Model.TaskModel(res.Value.Title, res.Value.Description, res.Value.DueDate, res.Value.CreationTime, res.Value.emailAssignee, res.Value.ID, res.Value.Position, this, userEmail);
            }
        }
        /// <summary>
        /// add new column
        /// </summary>
        /// <param name="email">user's email</param>
        /// <param name="creatorEmail">column's creator email</param>
        /// <param name="boardName">the name of the board the new column is in</param>
        /// <param name="position">column's position</param>
        /// <param name="name">column's name</param>
        public void AddColumn(string email, string creatorEmail, string boardName, int position, string name)
        {
            Response res = Service.AddColumn(email, creatorEmail, boardName, position, name);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }
        /// <summary>
        /// get all the user's boards' names
        /// </summary>
        /// <param name="email">user's email</param>
        /// <returns></returns>
        public Response<IList<string>> GetBoardNames(string email)
        {
            return Service.GetBoardNames(email);
        }

        /// <summary>
        /// get the user's boards
        /// </summary>
        /// <param name="user">user's email</param>
        /// <returns></returns>
        public IList<BoardModel> GetUserBoards(UserModel user)
        {
            IList<Board> boards = Service.GetUserBoards(user.Email).Value;
            IList<BoardModel> boardModels = new List<BoardModel>();
            foreach(IntroSE.Kanban.Backend.ServiceLayer.Board board in boards)
            {
                boardModels.Add(new BoardModel(this, board));
            }
            return boardModels;
        }

        /// <summary>
        /// change column's position
        /// </summary>
        /// <param name="userEmail">user's email</param>
        /// <param name="creatorEmail">column's creator email</param>
        /// <param name="boardName">board's name</param>
        /// <param name="columnOrdinal">column's position</param>
        /// <param name="shiftSize">new column's position</param>
        public void MoveColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int shiftSize)
        { 
            Response res = Service.MoveColumn(userEmail, creatorEmail, boardName, columnOrdinal, shiftSize);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);
        }

        /// <summary>
        /// get board by name
        /// </summary>
        /// <param name="userEmail">user's email</param>
        /// <param name="boardName">board's name</param>
        /// <param name="creatorEmail">board's creator email</param>
        /// <returns></returns>
        public IList<ColumnModel> GetBoard(string userEmail, string boardName, string creatorEmail)
        {
            IList<Column> columns = Service.GetBoard(userEmail, boardName, creatorEmail).Value;
            List<ColumnModel> columnModels = new List<ColumnModel>();
            foreach (Column c in columns)
            {
                ColumnModel columnModel = new ColumnModel(c.Name, c.Position, new List<TaskModel>(), this);
                columnModels.Add(columnModel);
                foreach (IntroSE.Kanban.Backend.ServiceLayer.Task t in c.TaskList)
                {
                    TaskModel taskModel = new TaskModel(t.Title, t.Description, t.DueDate, t.CreationTime, t.emailAssignee, t.ID, c.Position, this, userEmail);
                    columnModel.TaskList.Add(taskModel);
                }
            }
            return columnModels;
        }

        /// <summary>
        /// get the board's members' list
        /// </summary>
        /// <param name="userEmail">user's email</param>
        /// <param name="boardName">board's name</param>
        /// <param name="creatorEmail">board's creator's email</param>
        /// <returns></returns>
        public IList<String> GetBoardMembers(String userEmail, String boardName, String creatorEmail)
        {
            return Service.GetBoardMembers(userEmail, boardName, creatorEmail).Value;
        }

        /// <summary>
        /// change the task's title
        /// </summary>
        /// <param name="userEmail">user's email</param>
        /// <param name="creatorEmail">task's creator email</param>
        /// <param name="boardName">board's name</param>
        /// <param name="columnOrdinal">column's position</param>
        /// <param name="taskId">task's ID</param>
        /// <param name="title">new title</param>
        public void UpdateTaskTitle(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string title)
        {
            Response res = Service.UpdateTaskTitle(userEmail, creatorEmail, boardName, columnOrdinal, taskId, title);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);
        }

        /// <summary>
        /// change the task's description
        /// </summary>
        /// <param name="userEmail">user's email</param>
        /// <param name="creatorEmail">task's creator email</param>
        /// <param name="boardName">board's name</param>
        /// <param name="columnOrdinal">column's position</param>
        /// <param name="taskId">task's ID</param>
        /// <param name="description">new description</param>
        public void UpdateTaskDescription(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string description)
        {
            Response res = Service.UpdateTaskDescription(userEmail, creatorEmail, boardName, columnOrdinal, taskId, description);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);
        }

        /// <summary>
        /// change the task's due date
        /// </summary>
        /// <param name="userEmail">user's email</param>
        /// <param name="creatorEmail">task's creator email</param>
        /// <param name="boardName">board's name</param>
        /// <param name="columnOrdinal">column's position</param>
        /// <param name="taskId">task's ID</param>
        /// <param name="dueDate">new due date</param>
        public void UpdateTaskDueDate(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {
            Response res = Service.UpdateTaskDueDate(userEmail, creatorEmail, boardName, columnOrdinal, taskId, dueDate);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);
        }

        /// <summary>
        /// change the task's assignee
        /// </summary>
        /// <param name="userEmail">user's email</param>
        /// <param name="creatorEmail">task's creator email</param>
        /// <param name="boardName">board's name</param>
        /// <param name="columnOrdinal">column's position</param>
        /// <param name="taskId">task's ID</param>
        /// <param name="assigneeEmail">new assignee's email</param>
        public void UpdateTaskAssignee(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string assigneeEmail)
        {
            Response res = Service.AssignTask(userEmail, creatorEmail, boardName, columnOrdinal, taskId, assigneeEmail);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);
        }


        /// <summary>
        /// get all the boards exists
        /// </summary>
        /// <returns></returns>
        public IList<BoardModel> GetAllBoards()
        {
            Response<IList<IntroSE.Kanban.Backend.ServiceLayer.Board>> res = Service.GetAllBoards();
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
            else
            {
                List<BoardModel> boardModels = new List<BoardModel>();
                foreach (IntroSE.Kanban.Backend.ServiceLayer.Board board in res.Value)
                {
                    boardModels.Add(new BoardModel(this, board));
                }
                return boardModels;
            }
        }

        /// <summary>
        /// join board from diffrent user
        /// </summary>
        /// <param name="userEmail">user's email</param>
        /// <param name="creatorEmail">board's creator email</param>
        /// <param name="boardName">board's name</param>
        public void JoinBoard(string userEmail, string creatorEmail, string boardName)
        {
            Response res = Service.JoinBoard(userEmail, creatorEmail, boardName);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);
        }

        /// <summary>
        /// move the task to the next column
        /// </summary>
        /// <param name="userEmail">user's email</param>
        /// <param name="creatorEmail">board's creator email</param>
        /// <param name="boardName">board's name</param>
        /// <param name="columnOrdinal">task position</param>
        /// <param name="taskId">task's id</param>
        public void AdvanceTask(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId)
        {
            Response res = Service.AdvanceTask(userEmail, creatorEmail, boardName, columnOrdinal, taskId);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);
        }

        /// <summary>
        /// remove column
        /// </summary>
        /// <param name="userEmail">user's email</param>
        /// <param name="creatorEmail">board's creator email</param>
        /// <param name="boardName">board's name</param>
        /// <param name="columnOrdinal">column's position</param>
        public void RemoveColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            Response res = Service.RemoveColumn(userEmail, creatorEmail, boardName, columnOrdinal);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);
        }
        
        /// <summary>
        /// give the column task's limit
        /// </summary>
        /// <param name="userEmail">user's email</param>
        /// <param name="creatorEmail">board's creator email</param>
        /// <param name="boardName">board's name</param>
        /// <param name="columnOrdinal">column's position</param>
        /// <param name="limit">new limit</param>
        public void LimitColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int limit)
        {
            Response res = Service.LimitColumn(userEmail, creatorEmail, boardName, columnOrdinal, limit);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);
        }

        /// <summary>
        /// get all the tasks in the boards that are not in backlog or done columns
        /// </summary>
        /// <param name="userEmail">user's email</param>
        /// <returns></returns>
        public IList<Model.TaskModel> InProgressTasks(string userEmail)
        {
            Response<IList<IntroSE.Kanban.Backend.ServiceLayer.Task>> res = Service.InProgressTasks(userEmail);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);
            else
            {
                List<TaskModel> inProgressTasks = new List<TaskModel>();
                foreach(IntroSE.Kanban.Backend.ServiceLayer.Task t in res.Value)
                {
                    inProgressTasks.Add(new TaskModel(t.Title, t.Description, t.DueDate, t.CreationTime, t.emailAssignee, t.ID, t.Position, this, userEmail));
                }
                return inProgressTasks;
            }
        }

        /// <summary>
        /// rename column
        /// </summary>
        /// <param name="userEmail">user's email</param>
        /// <param name="creatorEmail">board's creator email</param>
        /// <param name="boardName">board's name</param>
        /// <param name="columnOrdinal">column position</param>
        /// <param name="newColumnName">new column's name</param>
        public void RenameColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, string newColumnName)
        {
            Response res = Service.RenameColumn(userEmail, creatorEmail, boardName, columnOrdinal, newColumnName);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

    }
}
