using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Models.DataBaseModels
{
    public class Journal: BaseClass
    {
        public int JOURNAL_OF_ACCOUNTING_ID { get; set; }

        public string FIO_DECLARANT { get; set; }

        public int CAR_ID { get; set; }

        public int DRIVER_ID { get; set; }

        public int DESTINATION_POINT_SENDING_ID { get; set; }

        public int DESTINATION_POINT_ARRIVAL_ID { get; set; }

        public DateTime DEPARTURE_TIME { get; set; }

        public DateTime ARRIVAL_TIME { get; set; }

        public int STATUS_ID { get; set; }

        public string COMMENTS { get; set; }
    }
}