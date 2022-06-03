using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    /// <summary>
    /// this class represents the user controller. the class holds the users information and the active user.
    /// all the user's function are here. creation of new user is being carried out here.
    /// </summary>
    public class UserController //public for testing
    {
        private Dictionary<String, User> _users;
        public Dictionary<string, User> Users { get => _users; }
        private User _activeUser;
        public User ActiveUser { get => _activeUser; private set { _activeUser = value; } }
        private int nextID = 0;

        /// <summary>
        /// creates new user controller.
        /// </summary>
        public UserController()
        {
            _users = new Dictionary<String, User>();
        }

        /// <summary>
        /// loads all persistence data relating to the users.
        /// </summary>
        public void LoadData()
        {
            UserDalController udc = new UserDalController();
            List<DTO> users = udc.Select();
            foreach (UserDTO user in users)
            {
                if (!this.Users.ContainsKey(user.Email))
                {
                    this.Users.Add(user.Email, new User(user));
                    nextID++;
                }
            }
        }
        /// <summary>
        /// deletes all persistence data relating to the users.
        /// </summary>
        public void DeleteData()
        {
            UserDalController udc = new UserDalController();
            udc.DeleteData();
        }
        /// <summary>
        /// creates new user.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>the user that registered</returns>
        public User Register(string email, string password)
        {
            //verify email is valid and unused.
            if (email == null) throw new Exception("No email input.");
            if (Users.ContainsKey(email)) throw new Exception("This email is already in use.");

            //create user
            User u = new User(nextID, email, password);
            nextID++;
            //add user to list
            Users[email] = u;

            return u;
        }
        /// <summary>
        /// login a user to the system
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>the user that logged in</returns>
        public User Login(String email, String password)
        {

            if (ActiveUser != null) throw new Exception("A user is already logged in, please log out first.");


            if (Users.ContainsKey(email))
            {

                if (Users[email].ValidatePass(password))
                {
                    ActiveUser = Users[email];
                    return Users[email];
                }
                else throw new Exception("Wrong password.");
            }
            else throw new Exception("Email does not exist in the system.");
        }
        /// <summary>
        /// logs out a user from the system.
        /// </summary>
        public void Logout()
        {
            if (ActiveUser != null) ActiveUser = null;
            else throw new Exception("Can not logout when no user is logged in.");
        }
        /// <summary>
        /// returns the user that is logged in
        /// </summary>
        /// <param name="id">user's id</param>
        /// <returns>the user logged in</returns>
        public User GetUser(long id)
        {
            foreach (User u in Users.Values)
            {
                if (u.ID == id)
                {
                    return u;
                }
            }
            return null;
        }
        /// <summary>
        /// return the boards' names of the user
        /// </summary>
        /// <param name="userEmail">user's email</param>
        /// <returns>boards' names</returns>
        public IList<String> GetBoardNames(string userEmail)
        {
            if (ActiveUser == null || ActiveUser.Email != userEmail)
                throw new Exception("Passed user email is not logged in.");
            IList<String> Boards = new List<String>();
            foreach (Board b in ActiveUser.Boards.Values)
            {
                Boards.Add(b.Name);
            }
            return Boards;
        }

        public IList<Board> GetUserBoards(string userEmail)
        {
            return Users[userEmail].Boards.Values.ToList<Board>();
        }
    }

}