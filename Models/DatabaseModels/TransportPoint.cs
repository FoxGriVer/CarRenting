using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

using Ext.Net.MVC;

namespace Models.DataBaseModels
{
    [Model(Name = "TransportPoint")]
    [JsonWriter(Encode = true, RootProperty = "data")]
    public class TransportPoint : BaseClass
    {

        [ModelField(IDProperty = true, UseNull = false)]
        [Field(Ignore = true)]
        public int DESTINATION_POINT_ID { get; set; }

        [PresenceValidator]
        public string TEXT { get; set; }
    }
}
