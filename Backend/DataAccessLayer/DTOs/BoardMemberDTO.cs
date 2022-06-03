using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    ///<summary>Class <c>BoardMemberDTO</c> represents the members of each board in the program's database.
    ///the <c>{X}ColumnName</c> are the names of the columns in the corresponding database table.
    ///<c>ID</c> is the board ID and of the two keys by which the data is found in the corresponding table.
    ///<c>UserID</c> is the member user's ID and the second key by which the data is found in the table.</summary>
    class BoardMemberDTO : DTO
    {
        public const string IDColumnName = "ID";
        public const string UserIDColumnName = "UserID";

        public long ID { get => _id; set { _controller.Update(_id, IDColumnName, value); _id = value; } }
        private long _userID;
        public long UserID { get => _userID; set { _controller.Update(ID, UserIDColumnName, value); _userID = value; } }
        /// <summary>
        /// The constructor of the class
        /// </summary>
        /// <param name="ID">the board ID</param>
        /// <param name="creatorID">the member's user ID</param>
        /// <param name="isNew">boolean value that controls whether or not to insert this dto as a new entry in the database</param>
        public BoardMemberDTO(long id, long userID, bool isNew) : base(new BoardMemberDalController())
        {
            _id = id;
            _userID = userID;
            if (isNew) ((BoardMemberDalController)_controller).Insert(this);
        }
    }
}