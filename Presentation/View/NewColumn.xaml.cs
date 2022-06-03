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
    /// Interaction logic for NewColumn.xaml
    /// </summary>
    public partial class NewColumn : Window
    {
        private ViewModel.NewColumnViewModel viewModel;
        public NewColumn(Model.BackendController controller, Model.BoardModel board, Model.UserModel user, View.BoardView boardView)
        {
            this.DataContext = new ViewModel.NewColumnViewModel(controller, board, user, boardView);
            this.viewModel = (ViewModel.NewColumnViewModel)DataContext;
            InitializeComponent();
        }
        
        /// <summary>
        /// add the new column button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddColumn_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.AddColumn(viewModel.Name, viewModel.Position))
            {
                Model.ColumnModel newColumn = new Model.ColumnModel(viewModel.Name, viewModel.Position, new List<Model.TaskModel>(), viewModel.Controller);
                viewModel.boardView.viewModel.columnModels = viewModel.Controller.GetBoard(viewModel.user.Email, viewModel.board.Name, viewModel.board.AdminEmail);
                viewModel.boardView.ColumnCB.Items.Add(newColumn.Name);
                this.Close();
            }
        }
    }
}
