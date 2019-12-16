using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace tenantApp.Models
{
    public class YogaImageInfo
    {
        [JsonProperty("items")]
        public Item[] Item { get; set; }

    }

    public class Item
    {
        [JsonProperty("snippet")]
        public Snippet Snippet { get; set; }

        [JsonProperty("kind")]
        public string kind { get; set; }
    }

    public class Snippet
    {
        [JsonProperty("title")]
        public string title { get; set; }

        [JsonProperty("thumbnails")]
        public Thumbnails Thumbnails { get; set; }
    }

    public class Thumbnails
    {
        [JsonProperty("medium")]
        public Medium Medium { get; set; }
    }

    public class Medium
    {
        [JsonProperty("url")]
        public string url { get; set; }

    }
}
