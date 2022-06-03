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
    /// Interaction logic for MoveColumn.xaml
    /// </summary>
    public partial class MoveColumn : Window
    {
        private ViewModel.MoveColumnViewModel viewModel;
        public MoveColumn(Model.BackendController controller, Model.UserModel user, Model.BoardModel board, Model.ColumnModel column)
        {
            InitializeComponent();
            this.DataContext = new ViewModel.MoveColumnViewModel(controller, user, board, column);
            this.viewModel = (ViewModel.MoveColumnViewModel)DataContext;
        }

        /// <summary>
        /// move column button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Action ConfirmMoveColumn = () =>
            {
                viewModel.MoveColumn();
            };
            ConfirmWindow confirmWindow = new ConfirmWindow(viewModel.Controller, ConfirmMoveColumn);
            confirmWindow.Show();
        }
    }
}
