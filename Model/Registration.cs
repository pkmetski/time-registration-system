using System;

namespace Model
{
    public class Registration
    {
        public virtual int Id { get; set; }

        public virtual DateTime? Date { get; set; }

        public virtual double? Hours { get; set; }

        public virtual string Project { get; set; }

        public virtual string Customer { get; set; }
    }
}
