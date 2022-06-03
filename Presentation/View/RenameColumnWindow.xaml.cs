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
    /// Interaction logic for RenameColumnWindow.xaml
    /// </summary>
    public partial class RenameColumnWindow : Window
    {
        private ViewModel.RenameColumnViewModel viewModel;
        public RenameColumnWindow(Model.ColumnModel SelectedColumn, Model.UserModel userModel, Model.BoardModel boardModel, BoardView boardView, Model.BackendController Controller)
        {
            InitializeComponent();
            this.DataContext = new ViewModel.RenameColumnViewModel(SelectedColumn, userModel, boardModel, boardView, Controller);
            this.viewModel = (ViewModel.RenameColumnViewModel)DataContext;
        }

        /// <summary>
        /// rename the column button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.RenameColumn())
                this.Close();
        }
    }
}
