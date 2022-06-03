using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for BoardMenu.xaml
    /// </summary>
    public partial class BoardMenu : Window
    {
        
        public ViewModel.BoardMenuModel viewModel;
        public BoardMenu(Model.UserModel u,Model.BackendController controller)
        {
            InitializeComponent();
            this.DataContext = new ViewModel.BoardMenuModel(controller, u, new Dictionary<string, Model.BoardModel>());
            this.viewModel = (ViewModel.BoardMenuModel)DataContext;
        }
        
        /// <summary>
        /// preperations when the window is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IList<Model.BoardModel> userBoards = viewModel.GetUserBoards(viewModel.user);
            foreach(Model.BoardModel board in userBoards)
            {
                viewModel.UserBoards.Add(board.Name, board);
                BoardCB.Items.Add(board.Name);
            }
        }

        /// <summary>
        /// create new board button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.AddBoard(viewModel.user.Email, NewBoardName.Text))
            {
                IList<Model.BoardModel> userBoards = viewModel.GetUserBoards(viewModel.user);
                foreach(Model.BoardModel board in userBoards)
                {
                    if(board.Name == NewBoardName.Text)
                    {
                        viewModel.UserBoards.Add(NewBoardName.Text, board);
                        break;
                    }
                }
                BoardCB.Items.Add(NewBoardName.Text);
            }
        }

        /// <summary>
        /// display the board button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Display_Click(object sender, RoutedEventArgs e)
        {
            if (BoardCB.Text != "")
            {
                IList<Model.ColumnModel> columns = viewModel.GetBoard(viewModel.user.Email, BoardCB.Text, viewModel.UserBoards[BoardCB.Text].AdminEmail);
                BoardView boardView = new BoardView(viewModel.user, viewModel.UserBoards[BoardCB.Text], columns, viewModel.Controller);
                boardView.Show();
                this.Close();
            }
        }

        /// <summary>
        /// delete board button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (BoardCB.Text != "")
            {
                if (viewModel.RemoveBoard(viewModel.user.Email, BoardCB.Text))
                {
                    viewModel.UserBoards.Remove(BoardCB.Text);
                    BoardCB.Items.Remove(BoardCB.Text);
                }
            }
        }

        /// <summary>
        /// logout button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Logout();
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        /// <summary>
        /// join board button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void JoinBoard_Click(object sender, RoutedEventArgs e)
        {
            JoinBoardWindow joinBoardWindow = new JoinBoardWindow(viewModel.user, this, viewModel.Controller);
            joinBoardWindow.Show();
        }

        /// <summary>
        /// show in progress tasks button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InProgressTasks_Click(object sender, RoutedEventArgs e)
        {
            InProgressTasksWindow inProgressTasksWindow = new InProgressTasksWindow(/*viewModel.InProgressTasks(),*/ viewModel.Controller, viewModel.user);
            inProgressTasksWindow.Show();
        }
    }
}
