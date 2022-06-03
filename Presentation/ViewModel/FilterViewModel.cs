using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    class FilterViewModel : NotifiableObject
    {
        public Model.BackendController Controller;
        public Model.UserModel userModel;
        private IList<Model.TaskModel> _tasks;
        public IList<Model.TaskModel> Tasks
        {
            get => _tasks;
            set
            {
                _tasks = value;
                RaisePropertyChanged("Tasks");
            }
        }
        
        public FilterViewModel(Model.BackendController Controller, Model.UserModel user, IList<Model.TaskModel> tasks)
        {
            this.userModel = user;
            this.Controller = Controller;
            this.Tasks = tasks;
        }
        
    }
}
