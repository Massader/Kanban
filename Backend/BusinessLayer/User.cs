using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    /// <summary>
    /// this class represents the user. each user has its own ID, email and password.
    /// each user has its boards and users can share boards between them.
    /// </summary>
    public class User //public for testing
    {
        private int _id;
        public int ID { get => _id; private set { _id = value; dto.ID = value; } }
        private String _email;
        public string Email { get => _email; private set { _email = value; dto.Email = value; } }
        private string _password;
        public string Password { get => _password; private set { _password = value; dto.Password = value; } }
        public const int PASS_MIN_LEN = 4;
        public const int PASS_MAX_LEN = 20;
        private Dictionary<int, Board> _boards;
        public Dictionary<int, Board> Boards { get => _boards; }
        private UserDTO dto;
        private Dictionary<int, BoardMemberDTO> memberDTO;

        /// <summary>
        /// User constructor - creates new user.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public User(int id, string email, string password)
        {
            if (!IsValidEmail(email)) throw new Exception("Email format not valid.");
            else
            {
                if (!isLegalPassword(password)) throw new Exception("Password must be between " + PASS_MIN_LEN + " - " + PASS_MAX_LEN + " characters and contain one uppercase letter, one lower case letter and a number.");
                else
                {
                    this._id = id;
                    this._email = email;
                    this._password = password;
                    _boards = new Dictionary<int, Board>();
                    dto = new UserDTO(id, email, password, true);
                    memberDTO = new Dictionary<int, BoardMemberDTO>();
                }
            }
        }
        /// <summary>
        /// User constructor - creates new user.
        /// </summary>
        /// <param name="dto">the UserDTO from which the user's data will be taken</param>
        public User(UserDTO dto)
        {
            this._id = (int)dto.ID;
            this._email = dto.Email;
            this._password = dto.Password;
            _boards = new Dictionary<int, Board>();
            memberDTO = new Dictionary<int, BoardMemberDTO>();
            this.dto = dto;
        }

        /// <summary>
        /// Checks that the password is correct.
        /// </summary>
        /// <param name="password"></param>
        /// <returns>true-if it's the user's password false-if it isn't</returns>
        public bool ValidatePass(String password)
        {
            return Password.Equals(password);
        }

        /// <summary>
        /// checks that the email is in correct form.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>true-valid email. false-invalid email.</returns>
        private bool IsValidEmail(String email)
        {
            if (email == null)
                return false;
            else
            {
                return Regex.IsMatch(email, @"^(\w+)@(\w+)((\.(\w){2,3})+)$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
        }
        /// <summary>
        /// checks that the password is in correct form.
        /// </summary>
        /// <param name="password"></param>
        /// <returns>true-valid password false-invalid password.</returns>
        private bool isLegalPassword(String password)
        {
            bool isLegal = true;
            if (password.Length >= PASS_MIN_LEN && password.Length <= PASS_MAX_LEN)
            {
                bool hasUpper = false;
                bool hasLower = false;
                bool hasNumber = false;
                foreach (char c in password)
                {
                    if (hasUpper & hasLower & hasNumber) break;
                    if (Char.IsUpper(c)) hasUpper = true;
                    if (Char.IsLower(c)) hasLower = true;
                    if (Char.IsDigit(c)) hasNumber = true;
                }
                if (!hasUpper || !hasLower || !hasNumber) isLegal = false;

            }
            else isLegal = false;
            return isLegal;
        }
        /// <summary>
        /// remove the member from the board
        /// </summary>
        /// <param name="boardID">Board's ID</param>
        public void LeaveBoard(int boardID)
        {
            memberDTO[boardID].DeleteDTO();
            memberDTO.Remove(boardID);
            Boards.Remove(boardID);
        }


        /// <summary>
        /// gets the email of the user
        /// </summary>
        /// <returns>string of the email</returns>
        public void JoinBoard(Board board, bool isNew)
        {
            memberDTO.Add(board.BoardID, new BoardMemberDTO(board.BoardID, ID, isNew));
            Boards.Add(board.BoardID, board);
        }

    }
}