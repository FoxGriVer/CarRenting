using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

using Ext.Net.MVC;

namespace Models.DataBaseModels
{
    [Model(Name = "Car")]
    [JsonWriter(Encode = true, RootProperty = "data")]
    public class Car : BaseClass
    {
        [ModelField(IDProperty = true, UseNull = false)]
        [Field(Ignore = true)]
        public int CAR_ID { get; set; }

        [PresenceValidator]
        public string MARK { get; set; }

        [PresenceValidator]
        public string MODEL { get; set; }

        [PresenceValidator]
        public string GOVERNMENT_NUMBER { get; set; }

        [PresenceValidator]
        public string CAR_TYPE { get; set; }
    }
}


