using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Models.DataBaseModels
{
    public class JournalReport : BaseClass
    {
        public string FIO_DECLARANT { get; set; }
        public string MARK { get; set; }
        public string MODEL { get; set; }
        public string GOVERNMENT_NUMBER { get; set; }
        public string FIO { get; set; }
        public string EMAIL { get; set; }
        public string POINT_START { get; set; }
        public string POINT_END { get; set; }
        public DateTime DEPARTURE_TIME { get; set; }
        public DateTime ARRIVAL_TIME { get; set; }
        public string STATUS { get; set; }
    }
}
