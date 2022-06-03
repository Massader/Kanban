using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Model
{
    /// <summary>
    /// presentation's column
    /// </summary>
    public class ColumnModel : NotifiableModelObject
    {
        public String Name;
        public int Position;
        public ObservableCollection<TaskModel> TaskList { get; set; }

        public ColumnModel(String name, int position, IList<TaskModel> taskList, BackendController Controller) : base(Controller)
        {
            this.Name = name;
            this.Position = position;
            this.TaskList = new ObservableCollection<TaskModel>(taskList);
        }

    }
}
