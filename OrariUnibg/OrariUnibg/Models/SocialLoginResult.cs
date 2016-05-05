using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrariUnibg.Models
{
    public class SocialLoginResult
    {
        public Message Message { get; set; }
    }

    public class Message
    {
        public string SocialId
        {
            get { return Sub; }
            //get { return string.IsNullOrEmpty(Sub) ? Id : Sub; }
        }

        public string Given_name { get; set; }
        public string Surname { get; set; }
        public string Family_name { get; set; }

        public string Picture { get; set; }
        public string Email { get; set; }
        public string Sub { get; set; }
        //public string Id { get; set; }
        public string Name { get; set; }
    }
}
