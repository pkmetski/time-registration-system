using System;

namespace Model
{
    public class QueryArguments
    {
        public int? Id { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public string Project { get; set; }

        public string Customer { get; set; }

        public int? Hours { get; set; }
    }
}
