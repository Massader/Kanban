using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    ///<summary>Class <c>UserDTO</c> represents the data of a user in the program's database.
    ///the <c>{X}ColumnName</c> are the names of the columns in the corresponding database table.
    ///<c>ID</c> is the user ID and the key by which the data is found in the corresponding table.
    ///<c>Email</c> is the user's email.
    ///<c>Password</c> is the user's password.</summary>
    public class UserDTO : DTO //public for testing
    {
        public const string IDColumnName = "ID";
        public const string EmailColumnName = "Email";
        public const string PasswordColumnName = "Password";

        public long ID { get => _id; set { _controller.Update(_id, IDColumnName, value); _id = value; } }
        private string _email;
        public string Email { get => _email; set { _controller.Update(ID, EmailColumnName, value); _email = value; } }
        private string _password;
        public string Password { set { _password = value; _controller.Update(ID, PasswordColumnName, value); } get => _password; }
        /// <summary>
        /// The constructor of the class
        /// </summary>
        /// <param name="ID">the user ID</param>
        /// <param name="email">the user's email</param>
        /// <param name="password">the user's password</param>
        /// <param name="isNew">boolean value that controls whether or not to insert this dto as a new entry in the database</param>
        public UserDTO(long id, string email, string password, bool isNew) : base(new UserDalController())
        {
            _id = id;
            _email = email;
            _password = password;
            if (isNew) ((UserDalController)_controller).Insert(this);
        }
    }
}