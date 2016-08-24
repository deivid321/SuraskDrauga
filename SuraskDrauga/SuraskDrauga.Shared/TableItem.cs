using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuraskDrauga
{
    public class TableItem
    {
        public TableItem(string id, string name, double x, double y, DateTime date)
        {
            this.Id = id;
            this.Name = name;
            X = x;
            Y = y;
            this.Date = date;
        }

        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "x")]
        public double X { get; set; }

        [JsonProperty(PropertyName = "y")]
        public double Y { get; set; }

        [JsonProperty(PropertyName = "date")]
        public DateTime Date { get; set; }
    }
}
