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
    /// Interaction logic for ConfirmWindow.xaml
    /// </summary>
    public partial class ConfirmWindow : Window
    {
        private ViewModel.ConfirmViewModel viewModel;

        public ConfirmWindow(Model.BackendController controller, Action action)
        {
            InitializeComponent();
            this.DataContext = new ViewModel.ConfirmViewModel(controller, action);
            this.viewModel = (ViewModel.ConfirmViewModel)DataContext;
        }
        
        /// <summary>
        /// yes button (confirm)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            if(viewModel.Confirm())
                this.Close();
        }

        /// <summary>
        /// no button (return)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void No_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
