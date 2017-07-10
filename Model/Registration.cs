using Newtonsoft.Json;
using System;

namespace Model
{
    public class Registration : BaseEntity
    {
        public virtual DateTime? Date { get; set; }

        public virtual double? Hours { get; set; }

        public virtual string Project { get; set; }

        public virtual string Customer { get; set; }

        [JsonIgnore]
        public virtual Invoice Invoice { get; set; }
    }
}
