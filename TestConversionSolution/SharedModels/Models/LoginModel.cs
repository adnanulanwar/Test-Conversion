using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Models
{
    public class LoginModel
    {
        public LoginModel()
        {
            Username = "ics";
            Password = "ics";
            Message = "";
        }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Message { get; set; }
    }
}
