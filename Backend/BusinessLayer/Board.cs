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
    /// this class represents the boards. each board has an ID,
    /// a name, columns of tasks, Admin and members.
    /// </summary>
    public class Board //public for testing
    {
        private int _boardID;
        public int BoardID { get => _boardID; set => _boardID = value; }
        private string name;
        public string Name { get => name; set => name = value; }
        private User _admin;
        public User Admin { get => _admin; set => _admin = value; }
        private Dictionary<int, Column> _columns;
        public Dictionary<int, Column> Columns { get => _columns; }
        private List<User> _members;
        public List<User> Members { get => _members; set => _members = value; }
        private BoardDTO dto;


        /// <summary>
        /// the constructor of the class
        /// </summary>
        /// <param name="name">board name</param>
        /// <param name="admin">the user who create the board</param>
        /// <param name="boardID">the board id (every board has unique id)</param>
        /// <param name="columnID">the next column id</param>
        public Board(string name, User admin, int boardID, int columnID)
        {
            this.name = name;
            this.Admin = admin;
            this.BoardID = boardID;
            this.Members = new List<User>();
            this._columns = new Dictionary<int, Column>();
            _columns.Add(0, new Column("backlog", columnID, this, 0));
            _columns.Add(1, new Column("in progress", columnID + 1, this, 1));
            _columns.Add(2, new Column("done", columnID + 2, this, 2));
            dto = new BoardDTO(boardID, name, admin.ID, true);
        }
        /// <summary>
        /// class constructor from data access layer
        /// </summary>
        /// <param name="board">board DTO</param>
        /// <param name="creator">the board admin</param>
        public Board(BoardDTO board, User creator)
        {
            this.BoardID = (int)board.ID;
            this.name = board.Name;
            Admin = creator;
            this.Members = new List<User>();
            this._columns = new Dictionary<int, Column>();
            this.dto = board;

            ColumnDalController cdc = new ColumnDalController();
            List<DTO> columnDTOList = cdc.Select();
            foreach (ColumnDTO c in columnDTOList)
            {
                if (c.BoardID == BoardID) _columns.Add((int)c.Position, new Column(c));
            }
        }


        /// <summary>
        /// add new task to the backlog column (if there is place for a new task there)
        /// </summary>
        /// <param name="title">the title of the task</param>
        /// <param name="dueDate">the task's due date</param>
        /// <param name="description">task's description</param>
        /// <param name="taskID">the task id (every task has unique id)</param>
        /// <param name="member">board member</param>
        public Task AddTask(string title, DateTime dueDate, string description, int taskID, User member)
        {
            if (IsAMember(member))
                return _columns[0].AddTask(title, dueDate, description, taskID, member, BoardID);
            else
                throw new Exception("only a board member can add tasks");
        }

        public Task AddTask(TaskDTO task)
        {
            return _columns[(int)task.Position].AddTask(task);

        }
        /// <summary>
        /// advance the task's column. task cannot advance if it has been done or the is no place in the column.
        /// </summary>
        /// <param name="columnOrdinal">which column the task at</param>
        /// <param name="taskID">the task id</param>
        /// /// <param name="activeUser">the user which is logged in currently</param>
        public void AdvanceTask(int columnOrdinal, int taskID, User activeUser)
        {
            if (columnOrdinal == Columns.Count - 1)
                throw new ArgumentException("Can't advance a done task.");
            else if (columnOrdinal >= _columns.Count || columnOrdinal < 0 || !_columns.ContainsKey(columnOrdinal))
                throw new ArgumentException("No column of that number.");
            else if (_columns[columnOrdinal + 1].Limit == _columns[columnOrdinal + 1].Tasks.Count && _columns[columnOrdinal + 1].Limit != -1)
                throw new Exception("Can't advance a task into a full column.");
            else
            {
                _columns[columnOrdinal].Tasks[taskID].AdvanceTask(activeUser);
                _columns[columnOrdinal + 1].AddTask(taskID, _columns[columnOrdinal].RemoveTask(taskID));
            }
        }
        /// <summary>
        /// removes the board
        /// </summary>
        public void RemoveBoard()
        {
            foreach (Column c in Columns.Values)
            {
                c.RemoveColumn();
                foreach (Task t in c.Tasks.Values)
                {
                    t.RemoveTask();
                }
            }
            _columns.Clear();
            dto.DeleteDTO();
            dto = null;
        }

        /// <summary>
        /// change the task title. title cannot be changed if the task id is wrong or if the task has been done.
        /// </summary>
        /// <param name="columnOrdinal">the column the task at</param>
        /// <param name="TaskID">the id ot the task</param>
        /// <param name="title">new title for the task</param>
        /// <param name="activeUser">logged in user</param>
        public void UpdateTaskTitle(int columnOrdinal, int taskID, string title, User activeUser)
        {
            if (columnOrdinal == Columns.Count - 1)
                throw new Exception("Can't edit a done task.");
            else if (columnOrdinal >= _columns.Count || columnOrdinal < 0 || !_columns.ContainsKey(columnOrdinal))
                throw new Exception("No column of that number.");
            else _columns[columnOrdinal].UpdateTaskTitle(taskID, title, activeUser);
        }
        /// <summary>
        /// change the task description. description cannot be changed if the task id is wrong or if the task has been done.
        /// </summary>
        /// <param name="columnOrdinal">which column the task at</param>
        /// <param name="taskID">the task id</param>
        /// <param name="description">the new description for the task</param>
        /// <param name="activeUser">logged in user</param>
        public void UpdateTaskDescription(int columnOrdinal, int taskID, string description, User activeUser)
        {
            if (columnOrdinal == Columns.Count - 1)
                throw new Exception("Can't edit a done task.");
            else if (columnOrdinal >= _columns.Count || columnOrdinal < 0 || !_columns.ContainsKey(columnOrdinal))
                throw new Exception("No column of that number.");
            else _columns[columnOrdinal].UpdateTaskDescription(taskID, description, activeUser);
        }
        /// <summary>
        /// change the due date. due date cannot be changed if the task id is wrong or if the task has been done.
        /// </summary>
        /// <param name="columnOrdinal">which column the task at</param>
        /// <param name="TaskID">the task id</param>
        /// <param name="dueDate">the new due date</param>
        /// <param name="activeUser">logged in user</param>
        public void UpdateTaskDueDate(int columnOrdinal, int taskID, DateTime dueDate, User activeUser)
        {
            if (columnOrdinal == Columns.Count - 1)
                throw new Exception("Can't edit a done task.");
            else if (columnOrdinal >= _columns.Count || columnOrdinal < 0 || !_columns.ContainsKey(columnOrdinal))
                throw new Exception("No column of that ordinal.");
            else _columns[columnOrdinal].UpdateTaskDueDate(taskID, dueDate, activeUser);
        }
        /// <summary>
        /// get all in progress tasks
        /// </summary>
        /// <returns>a list of all tasks in the center dictionaries</returns>
        public List<Task> GetInProgress()
        {
            List<Task> list = new List<Task>();
            for (int i = 1; i < Columns.Count - 1; i++)
            {
                foreach (Task t in Columns[i].Tasks.Values)
                {
                    list.Add(t);
                }
            }
            return list;
        }

        /// <summary>
        /// change the column size. the limit cant be less than the column current size
        /// </summary>
        /// <param name="columnOrdinal">which column</param>
        /// <param name="limit">new limit for the column</param>
        /// <param name="activeUser">logged in user</param>
        public void LimitColumn(int columnOrdinal, int limit, User activeUser)
        {
            if (!IsAMember(activeUser)) throw new Exception("Only a member or admin can limit columns.");
            else if (limit < 1 && limit != -1) throw new Exception("Can't limit column size to less than 1, except to -1.");
            else if (columnOrdinal >= _columns.Count || columnOrdinal < 0 || !_columns.ContainsKey(columnOrdinal))
                throw new Exception("Column does not exist");
            else
            {
                if (_columns[columnOrdinal].Tasks.Count > limit && limit != -1)
                    throw new ArgumentException("Can't change column limit if its size is larger than the new limit.");
                else
                {
                    _columns[columnOrdinal].Limit = limit;
                }
            }
        }

        /// <summary>
        /// gets the column's limit
        /// </summary>
        /// <param name="userEmail">logged in user's email</param>
        /// <param name="columnOrdinal">which column</param>
        /// <returns>coulmn's limit</returns>
        public long GetColumnLimit(string userEmail, int columnOrdinal)
        {
            if (!IsAMember(userEmail)) throw new ArgumentException("Only a board member can get a column's limit.");
            if (columnOrdinal >= _columns.Count || columnOrdinal < 0 || !_columns.ContainsKey(columnOrdinal)) throw new ArgumentException("No column of that number.");
            else return _columns[columnOrdinal].Limit;
        }

        /// <summary>
        /// returns the column's name
        /// </summary>
        /// <param name="userEmail">logged in user's email</param>
        /// <param name="columnOrdinal">which column</param>
        /// <returns>column's name</returns>
        public string GetColumnName(string userEmail, int columnOrdinal)
        {
            if (!IsAMember(userEmail)) throw new Exception("Only a board member can get a column's name.");
            if (columnOrdinal >= _columns.Count || columnOrdinal < 0 || !_columns.ContainsKey(columnOrdinal)) throw new ArgumentException("No column by that ordinal.");
            else return _columns[columnOrdinal].ColumnName;
        }

        /// <summary>
        /// returns the column
        /// </summary>
        /// <param name="columnOrdinal">which column</param>
        /// <returns>the column</returns>
        public Column GetColumn(int columnOrdinal)
        {
            if (columnOrdinal >= _columns.Count || columnOrdinal < 0 || !_columns.ContainsKey(columnOrdinal)) throw new ArgumentException("No column by that ordinal.");
            else return _columns[columnOrdinal];
        }

        /// <summary>
        /// add new member for the board
        /// </summary>
        /// <param name="newMember">the new member</param>
        public void JoinBoard(User newMember, bool isNew)
        {
            if (newMember == null) throw new ArgumentException("user cannot be null.");
            if (IsAMember(newMember)) throw new ArgumentException("This user is already a member of this board!");

            newMember.JoinBoard(this, isNew);

            Members.Add(newMember);
        }

        /// <summary>
        /// updates the task's assignee
        /// </summary>
        /// <param name="columnOrdinal">whick column the task is in</param>
        /// <param name="taskID">task's ID</param>
        /// <param name="assignee">new assignee</param>
        public void AssignTask(int columnOrdinal, int taskID, User assignee)
        {
            if (columnOrdinal >= _columns.Count) throw new ArgumentException("No column of that number.");
            else _columns[columnOrdinal].AssignTask(taskID, assignee);
        }
        /// <summary>
        /// check if the user is part of the board
        /// </summary>
        /// <param name="member">user to check</param>
        /// <returns></returns>
        public bool IsAMember(User member)
        {
            return Members.Contains(member) || Admin == member;

        }

        /// <summary>
        /// check if the user is part of the board (by email)
        /// </summary>
        /// <param name="memberEmail">user's email</param>
        /// <returns></returns>
        public bool IsAMember(string memberEmail)
        {
            if (Admin.Email == memberEmail) return true;

            foreach (User u in Members)
            {
                if (u.Email == memberEmail) return true;
            }

            return false;
        }

        public void RenameColumn(int columnOrdinal, string name, User activeUser)
        {
            if (!IsAMember(activeUser))
                throw new Exception("Only board's members can change column's name");
            if (!Columns.ContainsKey(columnOrdinal))
                throw new Exception("Column ordinal does not exist");
            Columns[columnOrdinal].ColumnName = name;
        }

        public void MoveColumn(int currentColumnOrdinal, int newColumnOrdinal, User activeUser)
        {
            if (newColumnOrdinal >= Columns.Count || newColumnOrdinal < 0)
                throw new Exception("passed position out of board bounds");
            if (!IsAMember(activeUser))
                throw new Exception("Only board's members can change column's name");
            if (!Columns.ContainsKey(currentColumnOrdinal))
                throw new Exception("column does not exist");
            Column toMove = Columns[currentColumnOrdinal];
            toMove.MoveColumn(newColumnOrdinal);
            Columns.Remove(currentColumnOrdinal);
            if (currentColumnOrdinal < newColumnOrdinal)
            {
                for (int i = currentColumnOrdinal; i < newColumnOrdinal; i++)
                    ChangeColumnKey(i + 1, i);
            }
            else if (currentColumnOrdinal > newColumnOrdinal)
            {
                for (int i = currentColumnOrdinal; i > newColumnOrdinal; i--)
                    ChangeColumnKey(i - 1, i);
            }
            Columns.Add(newColumnOrdinal, toMove);
        }

        private void ChangeColumnKey(int oldKey, int newKey)
        {
            if (Columns.ContainsKey(newKey))
                throw new Exception("column ordinal already taken");
            Column toChange = Columns[oldKey];
            Columns.Remove(oldKey);
            Columns.Add(newKey, toChange);
            toChange.Position = newKey;
        }

        public Column AddColumn(int columnOrdinal, string columnName, int columnID, User activeUser)
        {
            if (!IsAMember(activeUser))
                throw new Exception("Only board's members can change column's name");
            if (columnOrdinal > Columns.Count || columnOrdinal < 0)
                throw new Exception("A column can be added only in available positions.");
            for (int i = Columns.Count; i > columnOrdinal; i--)
                ChangeColumnKey(i - 1, i);
            Column newColumn = new Column(columnName, columnID, this, columnOrdinal);
            Columns.Add(columnOrdinal, newColumn);
            return newColumn;
        }

        public Column AddColumn(ColumnDTO column)
        {
            Column newColumn = new Column(column);
            _columns.Add((int)column.Position, newColumn);
            return newColumn;
        }

        public void RemoveColumn(int columnOrdinal, User activeUser)
        {
            if (Columns.Count == 2)
                throw new Exception("Boards must have at least two columns.");
            if (!Columns.ContainsKey(columnOrdinal))
                throw new Exception("A column with this ordinal does not exist.");
            if (!IsAMember(activeUser))
                throw new Exception("Only a board member can change a column's name");
            
            int direction = 1;
            if (columnOrdinal >= 1)
                direction = -1;

            if (Columns[columnOrdinal + direction].Limit < Columns[columnOrdinal + direction].Tasks.Count + Columns[columnOrdinal].Tasks.Count && Columns[columnOrdinal + direction].Limit != -1)
                throw new Exception("The adjacent column is too full to receive the removed board's tasks.");

            Columns[columnOrdinal].RemoveColumn();
            foreach (Task task in Columns[columnOrdinal].Tasks.Values)
            {
                Columns[columnOrdinal + direction].AddTask(task.ID, task);
                task.Position = columnOrdinal + direction;
            }
            Columns.Remove(columnOrdinal);
            for (int i = columnOrdinal; i < Columns.Count; i++)
                ChangeColumnKey(i + 1, i);
        }
    }
}