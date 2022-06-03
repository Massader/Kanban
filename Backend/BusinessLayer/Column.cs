using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    /// <summary>
    /// this class represents the columns in the board. each column has a name, contains tasks and has a limit of tasks that it can contain
    /// </summary>
    public class Column //public for testing
    {
        private Dictionary<int, Task> _tasks;
        public Dictionary<int, Task> Tasks { get => _tasks; }
        private int _limit;
        public int Limit { get => _limit; set { dto.Limit = value; _limit = value; } }
        private string _columnName;
        public string ColumnName { get => _columnName; set { dto.Name = value; _columnName = value; } }
        private int _position;
        public int Position { get => _position; set { dto.Position = value; foreach (Task t in Tasks.Values) { t.Position = value; } _position = value; } }
        private int _boardID;
        public int BoardID { get => _boardID; set { dto.BoardID = value; _boardID = value; } }
        private int _columnID;
        public int ColumnID { get => _columnID; set { dto.ID = value; _columnID = value; } }
        private ColumnDTO dto;

        /// <summary>
        /// column's constrctor. builds new empty column.
        /// </summary>
        /// <param name="columnName">name of the column</param>
        public Column(String columnName, int columnID, Board board, int position)
        {
            dto = new ColumnDTO(columnID, columnName, -1, position, board.BoardID, true);
            this._tasks = new Dictionary<int, Task>();
            this._limit = -1;
            this._columnName = columnName;
            this._columnID = columnID;
            this._position = position;
            this._boardID = board.BoardID;

        }
        /// <summary>
        /// column's constrctor. builds new column with tasks and specific limit.
        /// </summary>
        /// <param name="tasks">tasks in the column</param>
        /// <param name="limit">number of tasks the column can contain</param>
        /// <param name="columnName">column's name</param>
        public Column(ColumnDTO dto)
        {
            this.dto = dto;
            this._columnID = (int)dto.ID;
            this._columnName = dto.Name;
            this._position = (int)dto.Position;
            this._limit = (int)dto.Limit;
            this._boardID = (int)dto.BoardID;
            this._tasks = new Dictionary<int, Task>(); 
            TaskDalController tdc = new TaskDalController();
            List<DTO> tasks = tdc.Select();
            foreach (TaskDTO task in tasks)
            {
                if (task.Board == BoardID && task.Position == Position && !Tasks.ContainsKey((int)task.ID)) AddTask(task);
            }
        }

        /// <summary>
        /// add task to the column
        /// </summary>
        /// <param name="title">task's title</param>
        /// <param name="dueDate">tasks's due date</param>
        /// <param name="description">task's description</param>
        /// <param name="taskID">task's ID</param>
        /// <param name="creator">task's creator</param>
        /// <param name="board">the id of the board the task is in</param>
        /// <returns></returns>
        public Task AddTask(string title, DateTime dueDate, string description, int taskID, User creator, int board)
        {
            if (Tasks.Count >= Limit && Limit != -1)
                throw new Exception("Can't add task to full column.");
            else if (_tasks.ContainsKey(taskID))
                throw new Exception("Task already created.");
            else
            {
                Task newTask = new Task(title, dueDate, description, taskID, creator, board);
                Tasks.Add(taskID, newTask);
                return newTask;
            }
        }

        /// <summary>
        /// add task to the column from DTO
        /// </summary>
        /// <param name="task">task's DTO</param>
        /// <param name="assignee">user who created the task</param>
        /// <param name="board">the board the task in</param>
        /// <returns></returns>
        public Task AddTask(TaskDTO task)
        {
            Task newTask = new Task(task);
            Tasks.Add((int)task.ID, newTask);
            return newTask;
        }
        /// <summary>
        /// add task by ID
        /// </summary>
        /// <param name="taskID">task's ID</param>
        /// <param name="task">the task</param>
        public void AddTask(int taskID, Task task)
        {
            Tasks.Add(taskID, task);
        }
        /// <summary>
        /// removes the task
        /// </summary>
        /// <param name="taskID">task's ID</param>
        /// <returns></returns>
        public Task RemoveTask(int taskID)
        {
            Task t = Tasks[taskID];
            Tasks.Remove(taskID);
            return t;
        }
        /// <summary>
        /// updateds the task title
        /// </summary>
        /// <param name="TaskID">task's ID</param>
        /// <param name="title">task's title</param>
        /// <param name="activeUser">user who update the task</param>
        public void UpdateTaskTitle(int TaskID, string title, User activeUser)
        {
            if (Tasks.ContainsKey(TaskID))
                Tasks[TaskID].UpdateTaskTitle(title, activeUser);
            else throw new Exception("Input Column number does not contain the task inputted.");
        }
        /// <summary>
        /// updateds the task description
        /// </summary>
        /// <param name="taskID">task's ID</param>
        /// <param name="description">task's description</param>
        /// <param name="activeUser">user who update the task</param>
        public void UpdateTaskDescription(int taskID, string description, User activeUser)
        {
            if (Tasks.ContainsKey(taskID))
                Tasks[taskID].UpdateTaskDescription(description, activeUser);
            else throw new Exception("Input Column number does not contain the task inputted.");
        }
        /// <summary>
        /// updateds the task description
        /// </summary>
        /// <param name="TaskID">task's ID</param>
        /// <param name="dueDate">task's dueDate</param>
        /// <param name="activeUser">user who update the task</param>
        public void UpdateTaskDueDate(int TaskID, DateTime dueDate, User activeUser)
        {
            if (Tasks.ContainsKey(TaskID))
                Tasks[TaskID].UpdateTaskDueDate(dueDate, activeUser);
            else throw new Exception("Input Column number does not contain the task inputted.");
        }
        /// <summary>
        /// update the assignee to the task
        /// </summary>
        /// <param name="taskID">task's ID</param>
        /// <param name="assignee">new assignee</param>
        public void AssignTask(int taskID, User assignee)
        {
            Tasks[taskID].UpdateTaskAssignee(assignee);
        }
        /// <summary>
        /// return the column's size
        /// </summary>
        /// <returns>column's size</returns>
        public int GetColumnSize()
        {
            return Tasks.Count;
        }
        /// <summary>
        /// changes the column's position
        /// </summary>
        /// <param name="columnOrdinal">the position to change to</param>
        public void MoveColumn(int columnOrdinal)
        {
            if (Tasks.Count != 0) throw new Exception("Only empty columns can be moved");
            else
            {
                Position = columnOrdinal;
            }
        }
        /// <summary>
        /// removes the column from the db
        /// </summary>
        public void RemoveColumn()
        {
            dto.DeleteDTO();
        }
    }
}