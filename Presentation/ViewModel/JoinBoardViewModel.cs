using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    class JoinBoardViewModel : NotifiableObject
    {
        public Model.BackendController Controller;
        public Model.UserModel userModel;
        public View.BoardMenu boardMenu;
        public Model.BoardModel SelectedBoard { get; set; }
        private IList<Model.BoardModel> _boards;
        public IList<Model.BoardModel> Boards
        {
            get => _boards;
            set
            {
                _boards = value;
                RaisePropertyChanged("Boards");
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

        public JoinBoardViewModel(Model.UserModel user, View.BoardMenu boardMenu, Model.BackendController Controller)
        {
            this.userModel = user;
            this.Controller = Controller;
            this.boardMenu = boardMenu;
            GetAllBoards();
        }

        /// <summary>
        /// get all the boards
        /// </summary>
        public void GetAllBoards()
        {
            try
            {
                Boards = Controller.GetAllBoards();
            }
            catch { }
        }

        /// <summary>
        /// join the board
        /// </summary>
        /// <param name="userEmail">user's email</param>
        /// <param name="creatorEmail">creator's email</param>
        /// <param name="boardName">board's name</param>

        public void JoinBoard(String userEmail, String creatorEmail, String boardName)
        {
            Controller.JoinBoard(userEmail, creatorEmail, boardName);
            IList<Model.BoardModel> updatedBoards = Controller.GetUserBoards(userModel);
            Dictionary<String, Model.BoardModel> userBoards = new Dictionary<String, Model.BoardModel>();
            boardMenu.BoardCB.Items.Clear();
            foreach(Model.BoardModel boardModel in updatedBoards)
            {
                userBoards.Add(boardModel.Name, boardModel);
                boardMenu.BoardCB.Items.Add(boardModel.Name);
            }
            boardMenu.viewModel.UserBoards = userBoards;
        }
    }
}
