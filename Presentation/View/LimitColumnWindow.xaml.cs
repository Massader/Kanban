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
    /// Interaction logic for LimitColumnWindow.xaml
    /// </summary>
    public partial class LimitColumnWindow : Window
    {
        private ViewModel.LimitColumnViewModel viewModel;
        public LimitColumnWindow(Model.ColumnModel SelectedColumn, Model.UserModel userModel, Model.BoardModel boardModel, Model.BackendController Controller)
        {
            InitializeComponent();
            this.DataContext = new ViewModel.LimitColumnViewModel(SelectedColumn, userModel, boardModel, Controller);
            this.viewModel = (ViewModel.LimitColumnViewModel)DataContext;
        }

        /// <summary>
        /// confirm the new column's limit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.LimitColumn())
                this.Close();
        }

    }
}
