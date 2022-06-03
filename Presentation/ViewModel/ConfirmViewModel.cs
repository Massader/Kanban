using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    class ConfirmViewModel : NotifiableObject
    {
        public Model.BackendController Controller;
        public Action action;
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
        public ConfirmViewModel(Model.BackendController controller, Action action)
        {
            Controller = controller;
            this.action = action;
        }

        /// <summary>
        /// confirm action
        /// </summary>
        /// <returns></returns>
        public bool Confirm()
        {
            Message = "";
            try
            {
                action();
                return true;
            }
            catch (Exception e)
            {
                Message = e.Message;
                return false;
            }
        }
    }
}
