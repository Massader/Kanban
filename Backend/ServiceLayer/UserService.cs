using IntroSE.Kanban.Backend.BusinessLayer;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    class UserService
    {
        private UserController UC;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// The constructor of the UserService
        /// </summary>
        /// <param name="UC">The user controller</param>
        public UserService(UserController UC)
        {
            this.UC = UC;
        }
        /// <summary>
        /// loading the user data from the persistance
        /// </summary>
        ///<returns cref="Response">The response of the action</returns>
        public Response LoadData()
        {
            try
            {
                UC.LoadData();
                log.Info("User data loaded.");
                return new Response();
            }
            catch(Exception e)
            {
                log.Fatal(String.Format("User data failed to load - Exception Message: {0}", e.Message));
                return new Response(e.Message);
            }
        }
        /// <summary>
        /// deleting the user data from the persistance
        /// </summary>
        ///<returns cref="Response">The response of the action</returns>
        public Response DeleteData()
        {
            try
            {
                UC.DeleteData();
                log.Info("User data deleted.");
                return new Response();
            }
            catch (Exception e)
            {
                log.Debug(String.Format("User data failed to delete - Exception Message: {0}", e.Message));
                return new Response(e.Message);
            }
        }
        ///<summary>This method registers a new user to the system.</summary>
        ///<param name="email">the user e-mail address, used as the username for logging the system.</param>
        ///<param name="password">the user password.</param>
        ///<returns cref="Response">The response of the action</returns>
        public Response Register(string email, string password)
        {
            try
            {
                UC.Register(email, password);
                log.Info(String.Format("{0} has registered", email));
                return new Response();
                
            }
            catch(Exception e)
            {
                log.Debug(String.Format("{0} has failed to register - Exception Message: {1}", email, e.Message));
                return new Response(e.Message);
            }
        }
        /// <summary>
        /// Log in an existing user
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>A response object with a value set to the user, instead the response should contain a error message in case of an error</returns>
        public Response<User> Login(string email, string password)
        {
            try
            {
                UC.Login(email, password);
                User user = new User(email);
                log.Info(String.Format("{0} has logged in", email));
                return Response<User>.FromValue(user);
            }
            catch(Exception e)
            {
                log.Debug(String.Format("{0} tried to login - Exception Message: {1}", email, e.Message));
                return Response<User>.FromError(e.Message);
            }
        }
        /// <summary>        
        /// Log out an logged in user. 
        /// </summary>
        /// <param name="email">The email of the user to log out</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response Logout(string email)
        {
            try
            {
                UC.Logout();
                log.Info(String.Format("{0} logged out", email));
                return new Response();
            }
            catch (Exception e)
            {
                log.Debug(String.Format("{0} tried logging out - Exception Message: {1}", email, e.Message));
                return new Response(e.Message);
            }
        }
        /// <summary>
        /// Returns the list of board of a user. The user must be logged-in. The function returns all the board names the user created or joined.
        /// </summary>
        /// <param name="userEmail">The email of the user. Must be logged-in.</param>
        /// <returns>A response object with a value set to the board, instead the response should contain a error message in case of an error</returns>
        public Response<IList<String>> GetBoardNames(string userEmail)
        {
            try
            {
                IList<String> Boards = UC.GetBoardNames(userEmail);
                log.Info(String.Format("returned {0} boards' names", userEmail));
                return Response<IList<String>>.FromValue(Boards);
            }
            catch (Exception e)
            {
                log.Debug(String.Format("failed to return {0} boards' name - Exception Message: {1}", userEmail, e.Message));
                return Response<IList<String>>.FromError(e.Message);
            }
        }

        public Response<IList<Board>> GetUserBoards(string userEmail)
        {
            try
            {
                IList<Board> userBoards = new List<Board>();
                IList<BusinessLayer.Board> businessBoards = UC.GetUserBoards(userEmail);
                foreach(BusinessLayer.Board board in businessBoards)
                {
                    userBoards.Add(new Board(board.BoardID, board.Name, board.Admin.Email));
                }
                log.Info(String.Format("got {0} boards", userEmail));
                return Response<IList<Board>>.FromValue(userBoards);
            }
            catch (Exception e)
            {
                log.Debug(String.Format("failed to get {0} boards - Exception Message: {1}", userEmail, e.Message));
                return Response<IList<Board>>.FromError(e.Message);
            }
        }
    }
}
