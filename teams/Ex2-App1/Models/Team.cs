using System;
using System.Collections.Generic;

namespace Eq1.App1.Model
{
    public class Team
    {
        public Team()
        {
            CreationTime = DateTime.Now;
            Fixtures = new Dictionary<DateTime, string>();

        }
        public Team(string name) : this()
        {
            Name = name;
            Players = new HashSet<Player>();
        }
        public virtual void SignPlayers(IEnumerable<Player> players)
        {
            foreach(var player in players)
            {
                player.Team = this;
                Players.Add(player);
            }
        }
        public virtual Manager Manager { get; set; }
        public virtual int PlayerCount { get; set; }
        public virtual int Id { get; set; }

        public virtual string Name { get; private set; }
        public virtual DateTime CreationTime { get; set; }

        public virtual ICollection<Player> Players { get; private set; }
        public virtual IDictionary<DateTime, string> Fixtures { get; set; }
        public virtual void Sack(Player last)
        {
            Players.Remove(last);
            last.Team = null;
        }

        public virtual void Hire(Player player)
        {
            Players.Add(player);
            player.Team = this;
        }
    }
}