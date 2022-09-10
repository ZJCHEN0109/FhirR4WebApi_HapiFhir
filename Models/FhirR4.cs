using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Day4WebApi.Models
{
    public class FhirR4
    {
        
        public class Entry
        {
            public string fullUrl { get; set; }
            public Resource resource { get; set; }
            public Search search { get; set; }
        }

        public class Link
        {
            public string relation { get; set; }
            public string url { get; set; }
        }

        public class Meta
        {
            public DateTime lastUpdated { get; set; }
            public string versionId { get; set; }
            public string source { get; set; }
        }

        public class Name
        {
            public string use { get; set; }
            public string family { get; set; }
            public List<string> given { get; set; }
        }

        public class Resource
        {
            public string resourceType { get; set; }
            public string id { get; set; }
            public Meta meta { get; set; }
            public Text text { get; set; }
            public List<Name> name { get; set; }
            public string gender { get; set; }
            public string birthDate { get; set; }
        }

        public class Root
        {
            public string resourceType { get; set; }
            public string id { get; set; }
            public Meta meta { get; set; }
            public string type { get; set; }
            public List<Link> link { get; set; }
            public List<Entry> entry { get; set; }
        }

        public class Search
        {
            public string mode { get; set; }
        }

        public class Text
        {
            public string status { get; set; }
            public string div { get; set; }
        }
    }
}
