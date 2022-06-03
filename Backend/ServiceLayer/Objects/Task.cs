using System;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public struct Task
    {
        public readonly int ID;
        public readonly DateTime CreationTime;
        public readonly string Title;
        public readonly string Description;
        public readonly DateTime DueDate;
        public readonly string emailAssignee;
        public readonly int Position;
        public Task(int id, DateTime creationTime, string title, string description, DateTime DueDate, string emailAssignee, int position)
        {
            this.ID = id;
            this.CreationTime = creationTime;
            this.Title = title;
            this.Description = description;
            this.DueDate = DueDate;
            this.emailAssignee = emailAssignee;
            this.Position = position;
        }
}
}
