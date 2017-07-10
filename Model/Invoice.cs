using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Model
{
    public class Invoice : BaseEntity
    {
        [JsonIgnore]
        public virtual ISet<Registration> Registrations { get; set; }

        public virtual double Amount { get; set; }

        public virtual IEnumerable<int> RegistrationsIds
        {
            get { return Registrations.Select(r => r.Id); }
            set { }
        }
    }
}
