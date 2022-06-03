using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    public class BoardDTO : DTO //public for testing
    {
        ///<summary>Class <c>BoardDTO</c> represents the data of a board in the program's database.
        ///the <c>{X}ColumnName</c> are the names of the columns in the corresponding database table.
        ///<c>ID</c> is the board ID and the key by which the data is found in the corresponding table.
        ///<c>Name</c> is the board name.
        ///<c>CreatorID</c> is the ID of the board's admin.
        ///<c>BacklogLimit</c> is the limit of the Backlog column.
        ///<c>InProgressLimit</c> is the limit of the In Progress column.
        ///<c>DoneLimit</c> is the limit of the Done column.</summary>
        public const string BoardIDColumnName = "ID";
        public const string BoardNameColumnName = "Name";
        public const string CreatorEmailColumnName = "CreatorID";



        public long ID { get => _id; set { _controller.Update(_id, BoardIDColumnName, value); _id = value; } }
        private string _name;
        public string Name { get => _name; set { _controller.Update(ID, BoardNameColumnName, value); _name = value; } }
        private long _creatorID;
        public long CreatorID { get => _creatorID; set { _controller.Update(ID, CreatorEmailColumnName, value); _creatorID = value; } }


        /// <summary>
        /// The constructor of the class
        /// </summary>
        /// <param name="ID">the board ID</param>
        /// <param name="name">the board name</param>
        /// <param name="creatorID">the admin's user ID</param>
        /// <param name="isNew">boolean value that controls whether or not to insert this dto as a new entry in the database</param>
        public BoardDTO(long ID, string name, long creatorID, bool isNew) : base(new BoardDalController())
        {
            _id = ID;
            _name = name;
            _creatorID = creatorID;
            if (isNew) ((BoardDalController)_controller).Insert(this);
        }

    }
}