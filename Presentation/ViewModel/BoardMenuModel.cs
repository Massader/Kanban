using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    public class BoardMenuModel : NotifiableObject
    {
        public Model.UserModel user;
        public Dictionary<String, Model.BoardModel> UserBoards;
        public Model.BackendController Controller;
        public String newBoardName = "";
        public string _hello;
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
        public string Hello
        {
            get => _hello;
            set
            {
                this._hello = value;
                RaisePropertyChanged("Hello");
            }
        }

        private string _deleteMessage;
        public string DeleteMessage
        {
            get => _deleteMessage;
            set
            {
                this._deleteMessage = value;
                RaisePropertyChanged("DeleteMessage");
            }
        }
        private string _newBoardName;
        public string NewBoardName
        {
            get => _newBoardName;
            set
            {
                this._newBoardName = value;
                RaisePropertyChanged("NewBoardName");
            }
        }
        public BoardMenuModel(Model.BackendController controller, Model.UserModel user, Dictionary<String, Model.BoardModel> userBoards)
        {
            Controller = controller;
            this.user = user;
            this.UserBoards = userBoards;
            _hello = "Welcome " + user.Email;
        }

        /// <summary>
        /// add new board
        /// </summary>
        /// <param name="Email">user's email</param>
        /// <param name="BoardName">board's name</param>
        /// <returns></returns>
        public bool AddBoard(string Email,string BoardName)
        {
            Message = "";
            try
            {
                Controller.AddBoard(Email, BoardName);
                Message = "Board Added!";
                return true;
            }
            catch (Exception e)
            {
                Message = e.Message;
                return false;
            }
        }

        /// <summary>
        /// remove board
        /// </summary>
        /// <param name="Email">user's email</param>
        /// <param name="BoardName">board's name</param>
        /// <returns></returns>
        public bool RemoveBoard(string Email, string BoardName)
        {
            DeleteMessage = "";
            try
            {
                Controller.RemoveBoard(Email, BoardName);
                DeleteMessage = "Board Removed!";
                return true;
            }
            catch (Exception e)
            {
                DeleteMessage = e.Message;
                return false;
            }
        }

        /// <summary>
        /// get user's boards names
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        public IList<string> GetBoardNames(string Email)
        {
            return Controller.GetBoardNames(Email).Value;
        }

        /// <summary>
        /// get user's boards
        /// </summary>
        /// <param name="user">user's email</param>
        /// <returns></returns>
        public IList<Model.BoardModel> GetUserBoards(Model.UserModel user)
        {
            return Controller.GetUserBoards(user);
        }

        /// <summary>
        /// get board
        /// </summary>
        /// <param name="userEmail">user's email</param>
        /// <param name="boardName">board's name</param>
        /// <param name="creatorEmail">board's creator email</param>
        /// <returns></returns>
        public IList<Model.ColumnModel> GetBoard(string userEmail, string boardName, string creatorEmail)
        {
            return Controller.GetBoard(userEmail, boardName, creatorEmail);
        }

        /// <summary>
        /// log out
        /// </summary>
        public void Logout()
        {
            Controller.Logout(user.Email);
        }

        /// <summary>
        /// gets the tasks that are in progress columns
        /// </summary>
        /// <returns></returns>
        public IList<Model.TaskModel> InProgressTasks()
        {
            return Controller.InProgressTasks(user.Email);
        }
    }
}

