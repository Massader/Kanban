using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    class NewColumnViewModel : NotifiableObject
    {
        public Model.BackendController Controller;
        public Model.UserModel user;
        public Model.BoardModel board;
        public View.BoardView boardView;
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
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                this._name = value;
                RaisePropertyChanged("Name");
            }
        }
        private int _position;
        public int Position
        {
            get => _position;
            set
            {
                this._position = value;
                RaisePropertyChanged("Position");
            }
        }

        public NewColumnViewModel(Model.BackendController controller, Model.BoardModel board, Model.UserModel user, View.BoardView boardView)
        {
            Controller = controller;
            this.user = user;
            this.board = board;
            this.boardView = boardView;
        }

        /// <summary>
        /// adds new column
        /// </summary>
        /// <param name="name">new column's name</param>
        /// <param name="position">new column's position</param>
        /// <returns></returns>
        public bool AddColumn(string name, int position)
        {
            Message = "";
            try
            {
                Controller.AddColumn(user.Email, board.AdminEmail, board.Name, Position, Name);
                Message = "Column Added!";
                return true;
            }
            catch (Exception e)
            {
                Message = e.Message;
                return false;
            }
        }
    }
}
