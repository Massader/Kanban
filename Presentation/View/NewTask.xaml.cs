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
    /// Interaction logic for NewTask.xaml
    /// </summary>
    public partial class NewTask : Window
    {
        private ViewModel.NewTaskViewModel viewModel;

        public NewTask(Model.BackendController controller, Model.BoardModel board, Model.UserModel user, View.BoardView boardView, Model.ColumnModel backlogColumn)
        {
            InitializeComponent();
            this.DataContext = new ViewModel.NewTaskViewModel(controller, board, user, boardView, backlogColumn);
            this.viewModel = (ViewModel.NewTaskViewModel) DataContext;
        }

        /// <summary>
        /// add the new task button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            Model.TaskModel t = viewModel.AddTask(viewModel.Title, viewModel.Description, viewModel.DueDate);
            if (t != null)
            {
                viewModel.backlogColumn.TaskList.Add(t);
                viewModel.boardView.viewModel.SelectedColumn = viewModel.boardView.viewModel.SelectedColumn;
                this.Close();
            }
        }
    }
}
