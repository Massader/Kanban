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
    /// Interaction logic for Task.xaml
    /// </summary>
    public partial class Task : Window
    {
        private ViewModel.TaskViewModel viewModel;
        public Task(Model.BackendController controller, Model.TaskModel task, Model.BoardModel board, Model.UserModel user, IList<string> members, IList<Model.ColumnModel> columns)
        {
            InitializeComponent();
            this.DataContext = new ViewModel.TaskViewModel(controller, task, board, user, members, columns);
            this.viewModel = (ViewModel.TaskViewModel)DataContext;
        }

        /// <summary>
        /// save the new title button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveTitle_Click(object sender, RoutedEventArgs e)
        {
            viewModel.UpdateTaskTitle();
        }

        /// <summary>
        /// save the new description button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveDesc_Click(object sender, RoutedEventArgs e)
        {
            viewModel.UpdateTaskDescription();
        }

        /// <summary>
        /// save the new duedate button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveDate_Click(object sender, RoutedEventArgs e)
        {
            viewModel.UpdateTaskDueDate();
        }

        /// <summary>
        /// return back button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Return_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// save the new assignee button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveAsignee_Click(object sender, RoutedEventArgs e)
        {
            viewModel.UpdateTaskAssignee();
        }

        /// <summary>
        /// advance the task button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AdvanceTask_Click(object sender, RoutedEventArgs e)
        {
            Action ConfirmAdvanceTask = () =>
            {
                viewModel.AdvanceTask();
            };
            ConfirmWindow confirmWindow = new ConfirmWindow(viewModel.Controller, ConfirmAdvanceTask);
            confirmWindow.Show();
        }

    }
}
