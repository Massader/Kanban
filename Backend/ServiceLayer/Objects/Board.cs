using System;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public struct Board
    {
        public readonly int Id;
        public readonly string Name;
        public readonly string AdminEmail;
        public Board(int id,  string name, string adminEmail)
        {
            this.Id = id;
            this.Name = name;
            this.AdminEmail = adminEmail;
        }
    }
}
