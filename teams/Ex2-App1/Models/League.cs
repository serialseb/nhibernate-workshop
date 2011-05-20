using System;
using System.Collections.Generic;

namespace Eq1.App1.Model
{
    public class League
    {
        public League()
        {
            Teams = new HashSet<Team>();
            TopScorers = new List<Player>();
            Tokens = new Dictionary<string, string>();

        }

        //public virtual ICollection<Human> AuthorizedPersonel { get; set; }

        public virtual IList<Player> TopScorers { get; set; }

        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual ICollection<Team> Teams { get; set; }

        public virtual IDictionary<string, string> Tokens { get; set; }
    }
}