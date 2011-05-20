using System;
using System.Runtime.Serialization;

namespace Eq1.App1.Model
{
    [Serializable]
    public class Player : Human
    {
        [System.Web.Script.Serialization.ScriptIgnore]
        public virtual Team Team { get; set; }

        public virtual int Salary { get; set; }
        public virtual int Age { get; set; }
    }
}