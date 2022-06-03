using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    class LoginViewModel : NotifiableObject
    {
        public Model.BackendController Controller { get; private set; }
        private string _email;
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                this._email = value;
                RaisePropertyChanged("Email");
            }
        }
        private string _password;
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                this._password = value;
                RaisePropertyChanged("Password");
            }
        }

        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                this._message = value;
                RaisePropertyChanged("Message");
            }
        }

        public LoginViewModel()
        {
            this.Controller = new Model.BackendController();
            this.Email = Email;
            this.Password = Password;
        }

        /// <summary>
        /// register new user
        /// </summary>
        public void Register()
        {
            Message = "";
            try
            {
                Controller.Register(Email, Password);
                Message = "Registerd successfully!";
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
        }

        /// <summary>
        /// login user
        /// </summary>
        /// <returns></returns>
        public Model.UserModel Login()
        {
            Message = "";
            try
            {
                return Controller.Login(Email, Password);
            }
            catch (Exception e)
            {
                Message = e.Message;
                return null;
            }
        }
    }
}
