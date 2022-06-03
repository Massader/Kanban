using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    public class ColumnDTO : DTO //public for testing
    {
        ///<summary>Class <c>BoardDTO</c> represents the data of a column in the program's database.
        ///the <c>{X}ColumnName</c> are the names of the columns in the corresponding database table.
        ///<c>ID</c> is the column ID and the key by which the data is found in the corresponding table.
        ///<c>Name</c> is the column name.
        ///<c>Limit</c> is the limit of the column.
        ///<c>Position</c> is the column's position in the board.
        ///<c>BoardID</c> is the ID of the board the column resides in.</summary>
        public const string ColumnIDColumnName = "ID";
        public const string ColumnNameColumnName = "ColumnName";
        public const string LimitColumnName = "ColumnLimit";
        public const string PositionColumnName = "Position";
        public const string BoardIDColumnName = "BoardID";

        public long ID { get => _id; set { _controller.Update(_id, ColumnIDColumnName, value); _id = value; } }
        private string _name;
        public string Name { get => _name; set { _controller.Update(ID, ColumnNameColumnName, value); _name = value; } }
        private long _limit;
        public long Limit { get => _limit; set { _controller.Update(ID, LimitColumnName, value); _limit = value; } }
        private long _position;
        public long Position { get => _position; set { _controller.Update(ID, PositionColumnName, value); _position = value; } }
        private long _boardID;
        public long BoardID { get => _boardID; set { _controller.Update(ID, BoardIDColumnName, value); _boardID = value; } }

        /// <summary>
        /// The constructor of the class
        /// </summary>
        /// <param name="ID">is the column ID and the key by which the data is found in the corresponding table.</param>
        /// <param name="name">is the column name.</param>
        /// <param name="limit">is the limit of the column.</param>
        /// <param name="position">is the column's position in the board.</param>
        /// <param name="boardID">is the ID of the board the column resides in.</param>
        /// <param name="isNew">boolean value that controls whether or not to insert this dto as a new entry in the database</param>
        public ColumnDTO(long ID, string name, long limit, long position, long boardID, bool isNew) : base(new ColumnDalController())
        {
            _id = ID;
            _name = name;
            _limit = limit;
            _position = position;
            _boardID = boardID;
            if (isNew) ((ColumnDalController)_controller).Insert(this);
        }


    }
}