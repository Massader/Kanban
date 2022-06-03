using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Model
{
    /// <summary>
    /// notify changes to all classes
    /// </summary>
    public class NotifiableModelObject : NotifiableObject
    {
        public BackendController Controller { get; private set; }

        protected NotifiableModelObject(BackendController controller)
        {
            this.Controller = controller;
        }
    }
}
