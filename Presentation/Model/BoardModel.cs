using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Model
{
    /// <summary>
    /// presetation's board
    /// </summary>
    public class BoardModel : NotifiableModelObject
    {
        public String Name { get; set; }
        public String AdminEmail { get; set; }
        public int ID;
        public BoardModel(BackendController controller, IntroSE.Kanban.Backend.ServiceLayer.Board board) : base(controller)
        {
            this.Name = board.Name;
            this.AdminEmail = board.AdminEmail;
            this.ID = board.Id;
        }
    }
}
