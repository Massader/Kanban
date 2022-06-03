using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    /// <summary>
    /// this class represents the tasks. each task has title, description and duedate.
    /// each task has his own ID and creation time. the tasks are inside the columns.
    /// </summary>
    public class Task //public for testing
    {
        private string _title;
        public string Title { get => _title; set { _title = value; } }
        private DateTime _dueDate;
        public DateTime DueDate { get => _dueDate; set { _dueDate = value; } }
        private DateTime _creationTime;
        public DateTime CreationTime { get => _creationTime; set { _creationTime = value; } }
        private string _description;
        public string Description { get => _description; set { _description = value; } }
        private int _id;
        public int ID { get => _id; private set { _id = value; } }
        private string _taskAssignee;
        public string TaskAssignee { get => _taskAssignee; private set { _taskAssignee = value; } }
        private int _position;
        public int Position { get => _position; set { dto.Position = value; _position = value; } }
        private int board;
        public const int MAX_DESC_LEN = 300;
        public const int MAX_TITLE_LEN = 50;
        private TaskDTO dto;

        /// <summary>
        /// Task constructor.
        /// </summary>
        /// <param name="title">Task's title</param>
        /// <param name="dueDate">Task's last date of finishing</param>
        /// <param name="description">Task's description</param>
        /// <param name="taskID">Task's unique ID</param>
        public Task(string title, DateTime dueDate, string description, int taskID, User creator, int board)
        {
            this.ID = taskID;
            if (title == null || title.Length > MAX_TITLE_LEN || title.Length <= 0)
                throw new Exception("The title's length should be between 1 to " + MAX_TITLE_LEN);
            else
                this.Title = title;
            if (description == null)
                this.Description = "";
            else if (description.Length > MAX_DESC_LEN)
                throw new Exception("The description's length should be under " + MAX_DESC_LEN + "characters");
            else
                this.Description = description;
            if (dueDate > DateTime.Now)
                this.DueDate = dueDate;
            else
                throw new Exception("The due date has passed");
            CreationTime = DateTime.Now;
            this._position = 0;
            this.TaskAssignee = creator.Email;
            this.board = board;
            this.dto = new TaskDTO(taskID, title, description, dueDate.ToString(), CreationTime.ToString(), 0, creator.Email, board, true);
        }

        public Task(TaskDTO dto)
        {
            this.ID = (int)dto.ID;
            this.Title = dto.Title;
            this.Description = dto.Description;
            this.DueDate = DateTime.Parse(dto.DueDate);
            this.CreationTime = DateTime.Parse(dto.CreationTime);
            this._position = (int)dto.Position;
            this.TaskAssignee = dto.Assignee;
            this.board = (int)dto.Board;
            this.dto = dto;
        }
        /// <summary>
        /// Changes the task's title.
        /// </summary>
        /// <param name="newTitle">The task's new title</param>
        public void UpdateTaskTitle(string newTitle, User activeUser)
        {
            if (activeUser.Email != TaskAssignee)
                throw new Exception("only the assignee can change task's title");
            else
            {
                if (newTitle == null || newTitle.Length > MAX_TITLE_LEN || newTitle.Length <= 0)
                    throw new Exception("The title's length should be between 1 to " + MAX_TITLE_LEN);
                else
                {
                    dto.Title = newTitle;
                    this.Title = newTitle;
                }
            }
        }
        /// <summary>
        /// Changes the task's due date.
        /// </summary>
        /// <param name="newDate">The task's new due date</param>
        public void UpdateTaskDueDate(DateTime newDate, User activeUser)
        {
            if (activeUser.Email != TaskAssignee)
                throw new Exception("only the assignee can change task's due date");
            if (newDate == null)
                throw new ArgumentException("Passed DateTime is null.");
            else
            {
                if (newDate > DateTime.Now)
                {
                    dto.DueDate = newDate.ToString();
                    this.DueDate = newDate;
                }
                else
                    throw new Exception("The due date has passed");
            }
        }
        /// <summary>
        /// Changes the task's description.
        /// </summary>
        /// <param name="newDesc">The task's new description.</param>
        public void UpdateTaskDescription(string newDesc, User activeUser)
        {
            if (activeUser.Email != TaskAssignee)
                throw new Exception("only the assignee can change task's description");
            else
            {
                if (newDesc == null || newDesc.Length > MAX_DESC_LEN || newDesc.Length < 0)
                    throw new Exception("The description's length should be between 1 to " + MAX_DESC_LEN);
                else
                {
                    dto.Description = newDesc;
                    this.Description = newDesc;
                }
            }
        }
        /// <summary>
        /// removes task from the database
        /// </summary>
        public void RemoveTask()
        {
            dto.DeleteDTO();
        }
        /// <summary>
        /// updates the task's assignee
        /// </summary>
        /// <param name="taskAssignee">the new task's assignee</param>
        public void UpdateTaskAssignee(User taskAssignee)
        {
            dto.Assignee = taskAssignee.Email;
            this.TaskAssignee = taskAssignee.Email;
        }
        /// <summary>
        /// advance the postion of the task
        /// <param name="activeUser">The logged in User.</param>
        /// </summary>
        public void AdvanceTask(User activeUser)
        {
            if (activeUser.Email != TaskAssignee)
                throw new ArgumentException("Only assignee can advance tasks.");
            else
            {
                Position++;
            }
        }
    }
}