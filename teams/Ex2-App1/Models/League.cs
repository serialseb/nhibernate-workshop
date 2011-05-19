using System.Collections.Generic;

namespace Eq1.App1.Model
{
    public class League
    {
        public League()
        {
            Teams = new HashSet<Team>();
        }
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual ICollection<Team> Teams { get; set; }
    }
}