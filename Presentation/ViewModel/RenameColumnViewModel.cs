using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    class RenameColumnViewModel : NotifiableObject
    {
        public Model.BackendController Controller;
        public Model.ColumnModel SelectedColumn;
        public Model.UserModel userModel;
        public Model.BoardModel boardModel;
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
        private string _newColumnName;
        public string NewColumnName
        {
            get => _newColumnName;
            set
            {
                this._newColumnName = value;
                RaisePropertyChanged("NewColumnName");
            }
        }

        public RenameColumnViewModel(Model.ColumnModel SelectedColumn, Model.UserModel userModel, Model.BoardModel boardModel, View.BoardView boardView, Model.BackendController Controller)
        {
            this.SelectedColumn = SelectedColumn;
            this.userModel = userModel;
            this.boardModel = boardModel;
            this.boardView = boardView;
            this.Controller = Controller;
        }

        /// <summary>
        /// rename column
        /// </summary>
        /// <returns></returns>
        public bool RenameColumn()
        {
            Message = "";
            try
            {
                Controller.RenameColumn(userModel.Email, boardModel.AdminEmail, boardModel.Name, SelectedColumn.Position, NewColumnName);
                boardView.ColumnCB.Items[boardView.ColumnCB.Items.IndexOf(SelectedColumn.Name)] = NewColumnName; 
                SelectedColumn.Name = NewColumnName;
                SelectedColumn = SelectedColumn;
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
