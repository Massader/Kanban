using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    ///<summary>Class <c>TaskDTO</c> represents the data of a task in the program's database.
    ///the <c>{X}ColumnName</c> are the names of the columns in the corresponding database table.
    ///<c>ID</c> is the task ID and the key by which the data is found in the corresponding table.
    ///<c>Title</c> is the task title.
    ///<c>Description</c> is the task description.
    ///<c>DueDate</c> is the end date for the task.
    ///<c>CreationTime</c> is the date and time of creation of the task.
    ///<c>Position</c> is the column in which the task is stored.
    ///<c>Assignee</c> contains the email of the user assigned to the task.
    ///<c>Board</c> contains the id of the board holding the task.</summary>
    public class TaskDTO : DTO //public for testing
    {
        public const string TaskIDColumnName = "ID";
        public const string TitleColumnName = "Title";
        public const string DescriptionColumnName = "Description";
        public const string DueDateColumnName = "DueDate";
        public const string CreationTimeColumnName = "CreationTime";
        public const string PositionColumnName = "Position";
        public const string AssigneeColumnName = "Assignee";
        public const string BoardIDColumnName = "Board";

        public long ID { get => _id; set { _controller.Update(_id, TaskIDColumnName, value); _id = value; } }
        private string _title;
        public string Title { get => _title; set { _controller.Update(ID, TitleColumnName, value); _title = value; } }
        private string _description;
        public string Description { get => _description; set { _controller.Update(ID, DescriptionColumnName, value); _description = value; } }
        private string _dueDate;
        public string DueDate { get => _dueDate; set { _controller.Update(ID, DueDateColumnName, value); _dueDate = value; } }
        private string _creationTime;
        public string CreationTime { get => _creationTime; set { _controller.Update(ID, CreationTimeColumnName, value); _creationTime = value; } }
        private long _position;
        public long Position { get => _position; set { _controller.Update(ID, PositionColumnName, value); _position = value; } }
        private string _assignee;
        public string Assignee { get => _assignee; set { _controller.Update(ID, AssigneeColumnName, value); _assignee = value; } }
        private long _board;
        public long Board { get => _board; set { _controller.Update(ID, BoardIDColumnName, value); _board = value; } }
        /// <summary>
        /// The constructor of the class
        /// </summary>
        /// <param name="ID">the board ID</param>
        /// <param name="title">the task name</param>
        /// <param name="description">the task's description</param>
        /// <param name="dueDate">the task's due date</param>
        /// <param name="creationTime">the task's time and date of creation</param>
        /// <param name="position">the ordinal of the column this task is in</param>
        /// <param name="assignee">the email of the assigned user</param>
        /// <param name="boardID">the ID of the board this task is in</param>
        /// <param name="isNew">boolean value that controls whether or not to insert this dto as a new entry in the database</param>
        public TaskDTO(long ID, string title, string description, string dueDate, string creationTime, long position, string assignee, long boardID, bool isNew) : base(new TaskDalController())
        {
            _id = ID;
            _title = title;
            _description = description;
            _dueDate = dueDate;
            _creationTime = creationTime;
            _position = position;
            _assignee = assignee;
            _board = boardID;
            if (isNew) ((TaskDalController)_controller).Insert(this);
        }
    }
}