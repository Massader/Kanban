using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    class LimitColumnViewModel : NotifiableObject
    {
        public Model.BackendController Controller;
        public Model.ColumnModel SelectedColumn;
        public Model.UserModel userModel;
        public Model.BoardModel boardModel;
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
        private int _newColumnLimit;
        public int NewColumnLimit
        {
            get => _newColumnLimit;
            set
            {
                this._newColumnLimit = value;
                RaisePropertyChanged("NewColumnLimit");
            }
        }
        public LimitColumnViewModel(Model.ColumnModel SelectedColumn, Model.UserModel userModel, Model.BoardModel boardModel, Model.BackendController Controller)
        {
            this.SelectedColumn = SelectedColumn;
            this.userModel = userModel;
            this.boardModel = boardModel;
            this.Controller = Controller;
        }

        /// <summary>
        /// give the column new limit
        /// </summary>
        /// <returns></returns>
        public bool LimitColumn()
        {
            Message = "";
            try
            {
                Controller.LimitColumn(userModel.Email, boardModel.AdminEmail, boardModel.Name, SelectedColumn.Position, NewColumnLimit);
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
