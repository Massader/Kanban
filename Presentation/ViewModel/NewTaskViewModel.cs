using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    class NewTaskViewModel : NotifiableObject
    {
        public Model.BackendController Controller;
        public Model.UserModel user;
        public Model.BoardModel board;
        public View.BoardView boardView;
        public Model.ColumnModel backlogColumn;
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
        public NewTaskViewModel(Model.BackendController controller, Model.BoardModel board, Model.UserModel user, View.BoardView boardView, Model.ColumnModel backlogColumn)
        {
            Controller = controller;
            this.user = user;
            this.board = board;
            this._dueDate = DateTime.Now;
            this.boardView = boardView;
            this.backlogColumn = backlogColumn;
        }

        /// <summary>
        /// adds new task
        /// </summary>
        /// <param name="title">new task's title</param>
        /// <param name="description">new task's description</param>
        /// <param name="dueDate">new task's due date</param>
        /// <returns></returns>
        public Model.TaskModel AddTask(string title, string description, DateTime dueDate)
        {
            Message = "";
            try
            {
                Model.TaskModel t = Controller.AddTask(user.Email, board.AdminEmail, board.Name, title, description, dueDate);
                Message = "Task Added!";
                return t;
            }
            catch (Exception e)
            {
                Message = e.Message;
                return null;
            }
        }
    }
}
