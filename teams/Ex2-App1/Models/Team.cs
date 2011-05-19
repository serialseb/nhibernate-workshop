using System;
using System.Collections.Generic;

namespace Eq1.App1.Model
{
    public class Team
    {
        public Team()
        {
            CreationTime = DateTime.Now;
        }
        public Team(string name) : this()
        {
            Name = name;
            Players = new HashSet<Player>();
        }

        public virtual Manager Manager { get; set; }
        public virtual int PlayerCount { get; set; }
        public virtual int Id { get; set; }
        //private string name;
        public virtual string Name { get; private set; }
        public virtual DateTime CreationTime { get; set; }

        public virtual ICollection<Player> Players { get; set; }
    }
}