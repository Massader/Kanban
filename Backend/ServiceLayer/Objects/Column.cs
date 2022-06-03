using System;
using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public struct Column
    {
        public readonly string Name;   
        public readonly int Limit;
        public readonly int Position;
        public readonly List<Task> TaskList;
        public Column(string name, int limit, int position, List<Task> taskList)
        {
            this.Name = name;
            this.Limit = limit;
            this.Position = position;
            this.TaskList = taskList;
        }
    }
}
