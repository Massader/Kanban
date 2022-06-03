using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    class InProgressTasksViewModel : NotifiableObject
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

        public InProgressTasksViewModel(Model.BackendController Controller, Model.UserModel user)
        {
            this.userModel = user;
            this.Controller = Controller;
            InProgressTasks();
        }

        /// <summary>
        /// shows all the tasks that are in progress
        /// </summary>
        public void InProgressTasks()
        {
            try
            {
                Tasks = Controller.InProgressTasks(userModel.Email);
            }
            catch { }
        }
    }
}
