using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalBook.DTOs
{
    public class Assistant
    {
        public Assistant(string userName, string password)
        {
            Username = userName;
            Password = password;
        }

        public string Username { get; set; }

        public string Password { get; set; }

    }
}
