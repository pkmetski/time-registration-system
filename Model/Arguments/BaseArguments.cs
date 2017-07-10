using System;

namespace Model.Arguments
{
    public class BaseArguments
    {
        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public string Project { get; set; }

        public string Customer { get; set; }
    }
}
