using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuraskDrauga
{
    class MainTable
    {
        public MainTable(string v2, double x, double y)
            {
                Name = v2;
                X = x;
                Y = y;
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
