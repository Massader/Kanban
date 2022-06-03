using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Model
{
    /// <summary>
    /// presentation's task
    /// </summary>
    public class TaskModel : NotifiableModelObject
    {
        public Boolean PastDate { get; set; }
        public Boolean CloseToDueDate { get; set; }
        public Boolean AssigneeLoggedIn { get; set; }
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                RaisePropertyChanged("Description");
            }
        }
        private string _description;
        private DateTime _dueDate;
        public DateTime DueDate
        {
            get => _dueDate;
            set
            {
                _dueDate = value;
                RaisePropertyChanged("DueDate");
                RaisePropertyChanged("DueDateOnly");
                if (_dueDate < DateTime.Today) PastDate = true;
                else {
                    PastDate = false;
                    if (((_dueDate - DateTime.Today).TotalDays / (_dueDate - creationTime).TotalDays) < 0.25) CloseToDueDate = true;
                    else CloseToDueDate = false;
                }
            }
        }
        public DateTime creationTime;
        public int ID; /* :) */
        public int position;
        private string _emailAssignee;
        public string EmailAssignee
        {
            get => _emailAssignee;
            set
            {
                _emailAssignee = value;
                RaisePropertyChanged("EmailAssignee");
            }
        }
        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                RaisePropertyChanged("Title");
            }
        }
        public string DueDateOnly
        {
            get => _dueDate.Date.ToShortDateString();
        }
        public string CreationTimeOnly
        {
            get => creationTime.Date.ToShortDateString();
        }
        public TaskModel(string title, string description, DateTime dueDate, DateTime creationTime, string emailAssignee, int ID, int position, BackendController Controller, string loggedInUser) : base(Controller)
        {
            this._title = title;
            this._description = description;
            this.creationTime = creationTime;
            this.DueDate = dueDate;
            this._emailAssignee = emailAssignee;
            this.ID = ID;
            this.position = position;
            if (loggedInUser == emailAssignee && !PastDate && !CloseToDueDate) AssigneeLoggedIn = true;

        }
    }
}
