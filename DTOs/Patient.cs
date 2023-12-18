using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalBook.DTOs
{
    internal class Patient
    {
        public Patient(string fullName, string phone)
        {
            FullName = fullName;
            Phone = phone;
        }

        public string FullName { get; set; }


        public string Phone { get; set; }
    }
}
