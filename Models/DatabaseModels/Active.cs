using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.DataBaseModels
{    
    public class Active: BaseClass
    {
        public int ACTIVE_ID { get; set; }

        public int CAR_ID { get; set; }

        public int DRIVER_ID { get; set; }

        public int STATUS_ID { get; set; }
    }
}