using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    public class BoardViewModel : NotifiableObject
    {
        public Model.BackendController Controller;
        public Model.UserModel userModel;
        public Model.BoardModel boardModel;
        public IList<Model.ColumnModel> columnModels;
        private IList<Model.TaskModel> _selectedColumn;
        private IList<String> _boardMembers;
        public IList<String> BoardMembers
        {
            get => _boardMembers;
        }
        public IList<Model.TaskModel> SelectedColumn
        {
            get => _selectedColumn;
            set
            {
                this._selectedColumn = value;
                RaisePropertyChanged("SelectedColumn");
            }
        }

        private string _boardName;
        public string BoardName
        {
            get => _boardName;
            set
            {
                this._boardName = value;
                RaisePropertyChanged("BoardName");
            }
        }

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
        private string _filter;
        public string Filter
        {
            get => _filter;
            set
            {
                _filter = value;
                RaisePropertyChanged("Filter");
            }
        }
        public BoardViewModel(Model.BackendController controller, Model.UserModel userModel, Model.BoardModel boardModel, IList<Model.ColumnModel> columnModels)
        {
            Controller = controller;
            this.userModel = userModel;
            this.boardModel = boardModel;
            this.columnModels = columnModels;
            _boardName = boardModel.Name;
            _boardMembers = Controller.GetBoardMembers(userModel.Email, boardModel.Name, boardModel.AdminEmail);
            
        }
        
        /// <summary>
        /// move column to new position
        /// </summary>
        /// <param name="user">user's email</param>
        /// <param name="creatorEmail">board's creator email</param>
        /// <param name="boardName">board's email</param>
        /// <param name="columnOrdinal">column's position</param>
        /// <param name="shiftSize">new column's position</param>
        public void MoveColumn(Model.UserModel user, string creatorEmail, string boardName, int columnOrdinal, int shiftSize)
        {
            Controller.MoveColumn(user.Email, creatorEmail, boardName, columnOrdinal, shiftSize);
        }

        /// <summary>
        /// remove column
        /// </summary>
        /// <param name="columnOrdinal"></param>
        public void RemoveColumn(int columnOrdinal)
        {
            Controller.RemoveColumn(userModel.Email, boardModel.AdminEmail, boardModel.Name, columnOrdinal);
            foreach (Model.ColumnModel c in columnModels)
            {
                if (c.Position == columnOrdinal)
                {
                    columnModels.Remove(c);
                }
            }
        }

        /// <summary>
        /// gets the board's members
        /// </summary>
        /// <param name="userEmail">user's email</param>
        /// <param name="boardName">board's name</param>
        /// <param name="creatorEmail">board's creator email</param>
        /// <returns></returns>
        public IList<String> GetBoardMembers(String userEmail, String boardName, String creatorEmail)
        {
            return Controller.GetBoardMembers(userEmail, boardName, creatorEmail);
        }

        /// <summary>
        /// gets the column by its name
        /// </summary>
        /// <param name="columnName">column's name</param>
        /// <returns></returns>
        public Model.ColumnModel GetColumnByName(string columnName)
        {
            foreach(Model.ColumnModel c in columnModels)
            {
                if  (c.Name == columnName) return c;
            }
            return null;
        }

    }
}
