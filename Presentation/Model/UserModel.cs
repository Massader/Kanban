using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Model
{
    /// <summary>
    /// presentation's user
    /// </summary>
    public class UserModel : NotifiableModelObject
    {
        private string _email;

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                RaisePropertyChanged("Email");
            }
        }

        public UserModel(BackendController controller, string email) : base(controller)
        {
            this.Email = email;
        }
    }
}
