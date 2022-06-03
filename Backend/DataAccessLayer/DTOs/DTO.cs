using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    ///<summary>Class <c>DTO</c> represents a line of data in the program's database.
    ///<c>_controller</c> is the abstract class that performs the appropriate actions on the database.
    ///<c>_id</c> is the key by which the data is found in the corresponding table.</summary>
    public abstract class DTO //public for testing
    {
        protected DalController _controller;
        internal long _id;
        /// <summary>
        /// The constructor of the class
        /// </summary>
        /// <param name="controller">the controller with which the DTO communicates</param>
        protected DTO(DalController controller)
        {
            _controller = controller;
        }
        ///<summary>Deletes this DTO from the database.</summary>
        public void DeleteDTO()
        {
            _controller.Delete(this);
        }

    }
}