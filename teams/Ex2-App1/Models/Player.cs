using System;
using System.Runtime.Serialization;

namespace Eq1.App1.Model
{
    [Serializable]
    public class Player : Human
    {
        public Player()
        {
            Address = new Address();

        }
        [System.Web.Script.Serialization.ScriptIgnore]
        public virtual Team Team { get; set; }

        public virtual int Salary { get; set; }
        public virtual int Age { get; set; }

        public virtual Address Address { get; set; }
        
    }
    public class Address : IEquatable<Address>
    {
        public virtual string Line1 { get; set; }
        public virtual string Line2 { get; set; }

        public bool Equals(Address other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Line1, Line1) && Equals(other.Line2, Line2);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Address)) return false;
            return Equals((Address) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Line1 != null ? Line1.GetHashCode() : 0)*397) ^ (Line2 != null ? Line2.GetHashCode() : 0);
            }
        }
    }
}