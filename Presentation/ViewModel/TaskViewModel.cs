using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    class TaskViewModel : NotifiableObject
    {
        public Model.BackendController Controller;
        public Model.UserModel user;
        public Model.BoardModel board;
        public Model.TaskModel task;
        public IList<string> MemberList { get; set; }
        public IList<Model.ColumnModel> columns;
        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                this._message = value;
                RaisePropertyChanged("Message");
            }
        }
        private string _existingTitle;
        public string ExistingTitle
        {
            get => _existingTitle;
            set
            {
                this._existingTitle = value;
                RaisePropertyChanged("ExistingTitle");
            }
        }
        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                this._title = value;
                RaisePropertyChanged("Title");
            }
        }
        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                this._description = value;
                RaisePropertyChanged("Description");
            }
        }
        private DateTime _dueDate;
        public DateTime DueDate
        {
            get => _dueDate;
            set
            {
                this._dueDate = value;
                RaisePropertyChanged("DueDate");
            }
        }

        private string _assignee;
        public string Assignee
        {
            get => _assignee;
            set
            {
                this._assignee = value;
                RaisePropertyChanged("Assignee");
            }
        }

        public TaskViewModel(Model.BackendController controller, Model.TaskModel task, Model.BoardModel board, Model.UserModel user, IList<string> members, IList<Model.ColumnModel> columns)
        {
            this.board = board;
            this.user = user;
            Controller = controller;
            this.task = task;
            _existingTitle = task.Title;
            _title = task.Title;
            _description = task.Description;
            _dueDate = task.DueDate;
            _assignee = task.EmailAssignee;
            this.MemberList = members;
            this.columns = columns;
        }

        /// <summary>
        /// change the task's title
        /// </summary>
        public void UpdateTaskTitle()
        {
            Message = "";
            try
            {
                Controller.UpdateTaskTitle(user.Email, board.AdminEmail, board.Name, task.position, task.ID, Title);
                Message = "New title saved!";
                ExistingTitle = Title;
                task.Title = Title;
                RaisePropertyChanged("Title");
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
        }

        /// <summary>
        /// changes the task's description
        /// </summary>
        public void UpdateTaskDescription()
        {
            Message = "";
            try
            {
                Controller.UpdateTaskDescription(user.Email, board.AdminEmail, board.Name, task.position, task.ID, Description);
                Message = "New description saved!";
                task.Description = Description;
                RaisePropertyChanged("Description");
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
        }

        /// <summary>
        /// changes the task's due date
        /// </summary>
        public void UpdateTaskDueDate()
        {
            Message = "";
            try
            {
                Controller.UpdateTaskDueDate(user.Email, board.AdminEmail, board.Name, task.position, task.ID, DueDate);
                Message = "New due date saved!";
                task.DueDate = DueDate;
                RaisePropertyChanged("DueDate");
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
        }

        /// <summary>
        /// changes the tasks assignee
        /// </summary>
        public void UpdateTaskAssignee()
        {
            Message = "";
            try
            {
                Controller.UpdateTaskAssignee(user.Email, board.AdminEmail, board.Name, task.position, task.ID, Assignee);
                Message = "New assignee saved!";
                task.EmailAssignee = Assignee;
                RaisePropertyChanged("Assignee");
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
        }

        /// <summary>
        /// advance the task to the next columns
        /// </summary>
        public void AdvanceTask()
        {
            Controller.AdvanceTask(user.Email, board.AdminEmail, board.Name, task.position, task.ID);
            Model.ColumnModel cInitial = null;
            Model.ColumnModel cFinal = null;
            foreach (Model.ColumnModel c in columns)
            {
                if (c.Position == task.position)
                {
                    cInitial = c;
                }
                else if (c.Position == task.position + 1)
                {
                    cFinal = c;
                }
            }
            if (cInitial != null && cFinal != null)
            {
                cInitial.TaskList.Remove(task);
                cFinal.TaskList.Add(task);
                task.position++;
                Message = "Task advanced!";
            }
        }

    }
}
