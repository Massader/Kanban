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
    /// Interaction logic for JoinBoardWindow.xaml
    /// </summary>
    public partial class JoinBoardWindow : Window
    {
        ViewModel.JoinBoardViewModel viewModel;
        public JoinBoardWindow(Model.UserModel user, View.BoardMenu boardMenu, Model.BackendController Controller)
        {
            InitializeComponent();
            this.DataContext = new ViewModel.JoinBoardViewModel(user, boardMenu, Controller);
            this.viewModel = (ViewModel.JoinBoardViewModel)DataContext;
        }

        /// <summary>
        /// join the board by click it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = ((ListViewItem)sender).Content as Model.BoardModel;
            if (item != null)
            {
                Action ConfirmJoinBoard = () =>
                {
                    viewModel.JoinBoard(viewModel.userModel.Email, item.AdminEmail, item.Name);
                }; 
                ConfirmWindow confirmWindow = new ConfirmWindow(viewModel.Controller, ConfirmJoinBoard);
                confirmWindow.Show();
            }
        }
    }
}
