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
    /// Interaction logic for InProgressTasksWindow.xaml
    /// </summary>
    public partial class InProgressTasksWindow : Window
    {
        ViewModel.InProgressTasksViewModel viewModel;
        public InProgressTasksWindow(/*IList<Model.TaskModel> tasks,*/ Model.BackendController Controller, Model.UserModel user)
        {
            this.DataContext = new ViewModel.InProgressTasksViewModel(Controller, user);
            this.viewModel = (ViewModel.InProgressTasksViewModel)DataContext;
            InitializeComponent();
        }
    }
}
