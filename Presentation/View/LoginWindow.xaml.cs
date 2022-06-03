using Presentation.View;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private ViewModel.LoginViewModel viewModel;
        public LoginWindow()
        {
            InitializeComponent();
            this.DataContext = new ViewModel.LoginViewModel();
            this.viewModel = (ViewModel.LoginViewModel) DataContext;
        }

        /// <summary>
        /// login button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Login(object sender, RoutedEventArgs e)
        {
            Model.UserModel u = viewModel.Login();
            if (u != null)
            {
                BoardMenu boardMenu = new BoardMenu(u, viewModel.Controller);
                boardMenu.Show();
                this.Close();
            }

            //viewModel.Login();
        }

        /// <summary>
        /// register button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Register(object sender, RoutedEventArgs e)
        {
            viewModel.Register();
        }
    }
}
