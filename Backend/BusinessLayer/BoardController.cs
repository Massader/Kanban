using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    /// <summary>
    /// this class represents the board controller. from here the user controlls the boards.
    /// all the functions that are in the board starts from here. from create a new board and tasks to edit them and add members to them
    /// </summary>
    class BoardController
    {
        public Dictionary<Tuple<string, string>, Board> Boards = new Dictionary<Tuple<string, string>, Board>();
        private UserController userController;
        private int nextBoardID = 0;
        private int nextTaskID = 0;
        private int nextColumnID = 0;
        /// <summary>
        /// the constructor of the class
        /// </summary>
        /// <param name="userController">defines the usercontroller</param>
        public BoardController(UserController userController)
        {
            this.userController = userController;
        }
        /// <summary>
        /// loads all persistence data relating to the boards.
        /// </summary>
        public void LoadData()
        {
            //loading boardMembers
            BoardMemberDalController bmdc = new BoardMemberDalController();
            List<DTO> boardMembers = bmdc.Select();
            //loading boards
            BoardDalController bdc = new BoardDalController();
            List<DTO> boards = bdc.Select();

            foreach (BoardDTO board in boards)
            {
                bool boardExists = false;
                foreach (Board b in Boards.Values)
                {
                    if (b.BoardID == board.ID)
                    {
                        boardExists = true;
                        break;
                    }
                }
                if (boardExists) continue;
                else
                {
                    User admin = userController.GetUser(board.CreatorID);
                    Board newBoard = new Board(board, admin);
                    admin.JoinBoard(newBoard, false);
                    Boards.Add(new Tuple<string, string>(board.Name, admin.Email), newBoard);
                    if (board.ID >= nextBoardID) nextBoardID = (int)board.ID + 1;
                    foreach (BoardMemberDTO bm in boardMembers)
                    {
                        if (bm.ID == board.ID && bm.UserID != admin.ID)
                        {
                            newBoard.JoinBoard(userController.GetUser(bm.UserID), false);
                        }
                    }
                }
                int[] newNextIDs = UpdateNextID();
                nextColumnID = newNextIDs[1];
                nextTaskID = newNextIDs[2];
            }
        }
        /// <summary>
        /// deletes all persistence data relating to the boards.
        /// </summary>
        public void DeleteData()
        {
            TaskDalController tdc = new TaskDalController();
            tdc.DeleteData();
            BoardMemberDalController bmdc = new BoardMemberDalController();
            bmdc.DeleteData();
            ColumnDalController cdc = new ColumnDalController();
            cdc.DeleteData();
            BoardDalController bdc = new BoardDalController();
            bdc.DeleteData();
        }
        /// <summary>
        /// creates new board for the logged user.
        /// </summary>
        /// <param name="email">the user's email (should be the same as the logged user's email)</param>
        /// <param name="name">the new board's name</param>
        public void CreateBoard(string email, string name)
        {
            //checks for bad input
            if (userController.ActiveUser == null)
                throw new Exception("No user is logged in.");
            if (name == null || name.Equals(""))
                throw new Exception("Must receive board name.");
            if (email != userController.ActiveUser.Email)
                throw new Exception("Can only create boards for logged in users.");
            if (Boards.Keys.Any(key => key.Equals(new Tuple<string, string>(name, email))))
                throw new Exception("User already has a board of this name.");

            //creates new board and adds to registers in BoardController and User
            Board newBoard = new Board(name, userController.ActiveUser, nextBoardID, nextColumnID);
            userController.ActiveUser.JoinBoard(newBoard, true);
            nextBoardID++;
            nextColumnID += 3;
            Boards.Add(new Tuple<string, string>(name, email), newBoard);
        }
        /// <summary>
        /// shows the tasks that are in the "in progress" column.
        /// </summary>
        /// <param name="userEmail">logged user's email</param>
        /// <returns>in progress list</returns>
        public List<Task> ShowInProgress(string userEmail)
        {
            if (userController.ActiveUser == null || userController.ActiveUser.Email != userEmail)
                throw new Exception("User is not logged in.");

            User actUser = userController.ActiveUser;
            string actUserEmail = actUser.Email;
            List<Task> inProList = new List<Task>();
            List<Board> BoardsList = actUser.Boards.Values.ToList<Board>();
            foreach (Board board in BoardsList)
            {
                foreach (Task t in board.GetInProgress())
                {
                    if (t.TaskAssignee == actUserEmail)
                        inProList.Add(t);
                }
            }
            return inProList;
        }
        /// <summary>
        /// delete a board by its name
        /// </summary>
        /// <param name="userEmail">logged in user's email</param>
        /// <param name="creatorEmail">board creator email</param>
        /// <param name="boardName">name of the board</param>
        public void RemoveBoard(string userEmail, string creatorEmail, string boardName)
        {
            if (userController.ActiveUser == null || userController.ActiveUser.Email != userEmail)
                throw new Exception("Passed user email does not match logged in user.");

            Board boardToDelete = GetBoard(boardName, creatorEmail);

            if (boardToDelete.Admin.Email != userEmail)
                throw new Exception("Only board Admin can remove his board.");

            foreach (User user in boardToDelete.Members)
            {
                user.LeaveBoard(boardToDelete.BoardID);
            }

            boardToDelete.Admin.LeaveBoard(boardToDelete.BoardID);
            boardToDelete.RemoveBoard();
            Boards.Remove(new Tuple<string, string>(boardName, creatorEmail));

        }
        /// <summary>
        /// gets the board
        /// </summary>
        /// <param name="name">board's name</param>
        /// <param name="email">logged user's email</param>
        /// <returns>the board</returns>
        private Board GetBoard(string name, string email)
        {
            Board b = Boards[new Tuple<string, string>(name, email)];
            if (b == null) throw new Exception("Passed board does not exist.");
            return b;
        }

        /// <summary>
        /// adds new task
        /// </summary>
        /// <param name="userEmail">logged user's email</param>
        /// <param name="creatorEmail">board creator email</param>
        /// <param name="boardName">the board to which we add the task</param>
        /// <param name="title">task's title</param>
        /// <param name="description">task's description</param>
        /// <param name="dueDate">task's due date</param>
        /// <param name="taskID">task's id</param>
        public Task AddTask(string userEmail, string creatorEmail, string boardName, string title, string description, DateTime dueDate)
        {
            if (userController.ActiveUser == null || userController.ActiveUser.Email != userEmail)
                throw new Exception("Passed user email is not logged in.");
            return GetBoard(boardName, creatorEmail).AddTask(title, dueDate, description, nextTaskID++, userController.ActiveUser);
        }
        /// <summary>
        /// advance the task column
        /// </summary>
        /// <param name="userEmail">logged user's email</param>
        /// <param name="creatorEmail">board's creator email</param>
        /// <param name="boardName">board name</param>
        /// <param name="columnOrdinal">which column the task at</param>
        /// <param name="taskID">task's id</param>
        public void AdvanceTask(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskID)
        {
            if (userController.ActiveUser == null || userController.ActiveUser.Email != userEmail)
                throw new Exception("Passed user email is not logged in.");
            GetBoard(boardName, creatorEmail).AdvanceTask(columnOrdinal, taskID, userController.ActiveUser);
        }
        /// <summary>
        /// change the task title
        /// </summary>
        /// <param name="userEmail">logged user's email</param>
        /// <param name="creatorEmail">board's creator email</param>
        /// <param name="boardName">board name</param>
        /// <param name="columnOrdinal">which column the task at</param>
        /// <param name="taskID">task's id</param>
        /// <param name="title">task's new title</param>
        public void UpdateTaskTitle(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskID, string title)
        {
            if (userController.ActiveUser == null || userController.ActiveUser.Email != userEmail)
                throw new Exception("Passed user email is not logged in.");
            GetBoard(boardName, creatorEmail).UpdateTaskTitle(columnOrdinal, taskID, title, userController.ActiveUser);
        }/// <summary>
         /// change the task's description
         /// </summary>
         /// <param name="userEmail">logged user's email</param>
         /// <param name="creatorEmail">board's creator email</param>
         /// <param name="boardName">board's name</param>
         /// <param name="columnOrdinal">which column the task at</param>
         /// <param name="taskID">task's id</param>
         /// <param name="description">new description</param>
        public void UpdateTaskDescription(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskID, string description)
        {
            if (userController.ActiveUser == null || userController.ActiveUser.Email != userEmail)
                throw new Exception("Passed user email is not logged in.");
            GetBoard(boardName, creatorEmail).UpdateTaskDescription(columnOrdinal, taskID, description, userController.ActiveUser);
        }
        /// <summary>
        /// change the task's due date
        /// </summary>
        /// <param name="userEmail">logged user's email</param>
        /// <param name="creatorEmail">board's creator email</param>
        /// <param name="boardName">board's name</param>
        /// <param name="columnOrdinal">which column the task at</param>
        /// <param name="taskID">task's id</param>
        /// <param name="dueDate">task's new due date</param>
        public void UpdateTaskDueDate(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskID, DateTime dueDate)
        {
            if (userController.ActiveUser == null || userController.ActiveUser.Email != userEmail)
                throw new Exception("Passed user email is not logged in.");
            GetBoard(boardName, creatorEmail).UpdateTaskDueDate(columnOrdinal, taskID, dueDate, userController.ActiveUser);
        }
        /// <summary>
        /// gets the column's name
        /// </summary>
        /// <param name="userEmail">logged user's email</param>
        /// <param name="creatorEmail">board's creator email</param>
        /// <param name="boardName">board's name</param>
        /// <param name="columnOrdinal">which column</param>
        /// <returns>column's name</returns>
        public string GetColumnName(string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            if (userController.ActiveUser == null || userController.ActiveUser.Email != userEmail)
                throw new Exception("Passed user email is not logged in.");
            return GetBoard(boardName, creatorEmail).GetColumnName(userEmail, columnOrdinal);
        }
        /// <summary>
        /// gets the column
        /// </summary>
        /// <param name="userEmail">logged user's email</param>
        /// <param name="creatorEmail">board's creator email</param>
        /// <param name="boardName">board's name</param>
        /// <param name="columnOrdinal">which column</param>
        /// <returns>the column</returns>
        public Column GetColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            if (userController.ActiveUser == null || userController.ActiveUser.Email != userEmail)
                throw new Exception("Passed user email is not logged in.");
            Board board = GetBoard(boardName, creatorEmail);
            if (board.Admin.Email != userEmail && !board.IsAMember(userEmail)) throw new ArgumentException("Email passed is not of a board member.");
            else return board.GetColumn(columnOrdinal);
        }
        /// <summary>
        /// change the column size
        /// </summary>
        /// <param name="userEmail">logged user's email</param>
        /// <param name="creatorEmail">the board creator's email</param>
        /// <param name="boardName">board's name</param>
        /// <param name="columnOrdinal">which column</param>
        /// <param name="limit">new size</param>
        public void LimitColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int limit)
        {
            if (userController.ActiveUser == null || userController.ActiveUser.Email != userEmail)
                throw new Exception("Passed user email is not logged in.");
            GetBoard(boardName, creatorEmail).LimitColumn(columnOrdinal, limit, userController.ActiveUser);
        }
        /// <summary>
        /// gets the column size
        /// </summary>
        /// <param name="userEmail">logged user's email</param>
        /// <param name="creatorEmail">the board creator's email</param>
        /// <param name="boardName">board's name</param>
        /// <param name="columnOrdinal">which column</param>
        /// <returns>returns the limit of the tasks in the column (column size)</returns>
        public int GetColumnLimit(string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            if (userController.ActiveUser == null || userController.ActiveUser.Email != userEmail)
                throw new Exception("Passed user email is not logged in.");
            return (int)GetBoard(boardName, creatorEmail).GetColumnLimit(userEmail, columnOrdinal);
        }
        /// <summary>
        /// update the task's assignee
        /// </summary>
        /// <param name="userEmail">new assignee email</param>
        /// <param name="creatorEmail">creator email</param>
        /// <param name="boardName">the name of the board the task is in</param>
        /// <param name="columnOrdinal">which column</param>
        /// <param name="taskID">task's ID</param>
        /// <param name="assigneeEmail">assagnee's email</param>
        public void AssignTask(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskID, string assigneeEmail)
        {
            if (creatorEmail == null)
                throw new ArgumentNullException("creator email cannot be null");
            else if (boardName == null)
                throw new ArgumentNullException("Board Name cannot be null");
            else if (userController.ActiveUser == null || userController.ActiveUser.Email != userEmail)
                throw new Exception("passed user email is not logged in.");
            else if (!userController.Users.ContainsKey(assigneeEmail))
                throw new Exception("passed assignee does not exist.");
            else if (!GetBoard(boardName, creatorEmail).IsAMember(userController.Users[assigneeEmail]))
                throw new Exception("passed assignee is not a member of this board.");
            else if (!GetBoard(boardName, creatorEmail).IsAMember(userController.ActiveUser))
                throw new Exception("passed user email is not a member of this board.");
            else
            {
                GetBoard(boardName, creatorEmail).AssignTask(columnOrdinal, taskID, userController.Users[assigneeEmail]);
            }
        }
        /// <summary>
        /// add a new member to the board
        /// </summary>
        /// <param name="userEmail">new member's email</param>
        /// <param name="creatorEmail">board creator's email</param>
        /// <param name="boardName">board's name</param>
        public void JoinBoard(string userEmail, string creatorEmail, string boardName)
        {
            if (userController.ActiveUser == null || userController.ActiveUser.Email != userEmail)
                throw new Exception("passed user email is not logged in.");
            GetBoard(boardName, creatorEmail).JoinBoard(userController.ActiveUser, true);
        }

        public void RenameColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, string newColumnName)
        {
            if (userController.ActiveUser == null || userController.ActiveUser.Email != userEmail)
                throw new Exception("passed user email is not logged in.");
            GetBoard(boardName, creatorEmail).RenameColumn(columnOrdinal, newColumnName, userController.ActiveUser);
        }

        public Column AddColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, string columnName)
        {
            if (userController.ActiveUser == null || userController.ActiveUser.Email != userEmail)
                throw new Exception("passed user email is not logged in.");
            Column c = GetBoard(boardName, creatorEmail).AddColumn(columnOrdinal, columnName, nextColumnID, userController.ActiveUser);
            nextColumnID++;
            return c;
        }

        public void MoveColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int shiftSize)
        {
            if (userController.ActiveUser == null || userController.ActiveUser.Email != userEmail)
                throw new Exception("passed user email is not logged in.");
            GetBoard(boardName, creatorEmail).MoveColumn(columnOrdinal, columnOrdinal + shiftSize, userController.ActiveUser);
        }

        public void RemoveColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            if (userController.ActiveUser == null || userController.ActiveUser.Email != userEmail)
                throw new Exception("passed user email is not logged in.");
            GetBoard(boardName, creatorEmail).RemoveColumn(columnOrdinal, userController.ActiveUser);
        }

        private int[] UpdateNextID()
        {
            int maxTaskID = 0;
            int maxColumnID = 0;
            int maxBoardID = 0;
            foreach (Board b in Boards.Values)
            {
                if (b.BoardID >= maxBoardID) maxBoardID = b.BoardID + 1;
                foreach (Column c in b.Columns.Values)
                {
                    if (c.ColumnID >= maxColumnID) maxColumnID = c.ColumnID + 1;
                    foreach (Task t in c.Tasks.Values)
                    {
                        if (t.ID >= maxTaskID) maxTaskID = t.ID + 1;
                    }
                }
            }
            return new int[] { maxBoardID, maxColumnID, maxTaskID };
        }

        public Board GetBoard(string userEmail, string boardName, string creatorEmail)
        {
            if (userController.ActiveUser == null || userController.ActiveUser.Email != userEmail) throw new Exception("Passed user email is not logged in.");
            return GetBoard(boardName, creatorEmail);
        }

        public IList<User> GetBoardMembers(String userEmail, String boardName, String creatorEmail)
        {
            if (userController.ActiveUser == null || userController.ActiveUser.Email != userEmail) throw new Exception("Passed user email is not logged in.");
            IList<User> members = new List<User>();
            Board board = GetBoard(boardName, creatorEmail);
            foreach(User u in board.Members)
            {
                members.Add(u);
            }
            members.Add(board.Admin);
            return members;
        }
    }
}