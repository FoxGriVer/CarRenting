using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.DataBaseModels
{
    public class User : BaseClass
    {
        public int USER_ID { get; set; }

        public string FIO { get; set; }

        public string LOGIN { get; set; }

        public string PASSWORD { get; set; }
    }
}