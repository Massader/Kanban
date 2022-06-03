using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    class MoveColumnViewModel : NotifiableObject
    {
        public Model.BackendController Controller;
        public Model.UserModel user;
        public Model.ColumnModel column;
        public Model.BoardModel board;
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

        private int _shiftSize;
        public int ShiftSize
        {
            get => _shiftSize;
            set
            {
                this._shiftSize = value;
                RaisePropertyChanged("ShiftSize");
            }
        }

        public MoveColumnViewModel(Model.BackendController controller, Model.UserModel user, Model.BoardModel board, Model.ColumnModel column)
        {
            Controller = controller;
            this.user = user;
            this.column = column;
            this.board = board;
        }

        /// <summary>
        /// move column to new position
        /// </summary>
        public void MoveColumn()
        {
            Controller.MoveColumn(user.Email, board.AdminEmail, board.Name, column.Position, ShiftSize - column.Position);
        }
    }
}
