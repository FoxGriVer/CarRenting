using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TransportRentalSystem.ViewModels.JsonShare
{
    /// <summary>
    /// Принимает данные с клиентской части календаря
    /// </summary>
    public class StartTimeViewModel
    {       
        [JsonProperty(PropertyName = "startDateTime")]
        public DateTime StartDateTime { get; set; }

        [JsonProperty(PropertyName = "endDateTime")]
        public DateTime EndDateTime { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "isAllDay")]
        public bool isAllDay { get; set; }

        [JsonProperty(PropertyName = "eventId")]
        public int EventId { get; set; }

        [JsonProperty(PropertyName = "calendarId")]
        public int CalendarId { get; set; }
    }
}
