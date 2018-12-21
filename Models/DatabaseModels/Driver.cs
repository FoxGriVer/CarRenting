using Ext.Net.MVC;

namespace Models.DataBaseModels
{
    [Model(Name = "Driver")]
    [JsonWriter(Encode = true, RootProperty = "data")]
    public class Driver : BaseClass
    {
        [ModelField(IDProperty = true, UseNull = false)]
        [Field(Ignore = true)]
        public int DRIVER_ID { get; set; }

        [PresenceValidator]
        public string FIO { get; set; }

        [EmailValidator]
        [PresenceValidator]
        public string Email { get; set; }

    }
}
