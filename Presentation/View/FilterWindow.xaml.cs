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
    /// Interaction logic for FilterWindow.xaml
    /// </summary>
    public partial class FilterWindow : Window
    {
        ViewModel.FilterViewModel viewModel;
        public FilterWindow(Model.BackendController Controller, Model.UserModel user, List<Model.TaskModel> tasks)
        {
            this.DataContext = new ViewModel.FilterViewModel(Controller, user, tasks);
            this.viewModel = (ViewModel.FilterViewModel)DataContext;
            InitializeComponent();
        }
    }
}
