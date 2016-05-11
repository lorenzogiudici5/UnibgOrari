using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrariUnibg.Models
{
    public class User
    {
        //Personal
        [Newtonsoft.Json.JsonProperty("Id")]
        public string Id { get; set; }
        public string SocialId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        [Newtonsoft.Json.JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }
        [Newtonsoft.Json.JsonProperty("updateddAt")]
        public DateTime UpdatedAt { get; set; }

        //Univerisity
        public string Matricola { get; set; }
        [Newtonsoft.Json.JsonProperty("FacoltaDB")]
        public string FacoltaDB { get; set; }
        [Newtonsoft.Json.JsonProperty("FacoltaId")]
        public int? FacoltaId { get; set; }
        [Newtonsoft.Json.JsonProperty("Laurea")]
        public int? LaureaId { get; set; }
        [Newtonsoft.Json.JsonProperty("Anno")]
        public int? AnnoIndex { get; set; }
    }
}
