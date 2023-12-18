using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalBook.DTOs
{
    internal class Intervention
    {

        public int TermId { get; set; }


        public DateTime Date { get; set; }


        public string EmployeeUserName { get; set; }


        public int PatientId { get; set; }


    }
}
