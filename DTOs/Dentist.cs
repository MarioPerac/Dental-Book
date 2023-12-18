using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalBook.DTOs
{
    public class Dentist
    {
        public Dentist(string shiftEvenWeeks, string shiftOddWeeks, string fullName)
        {
            ShiftEvenWeeks = shiftEvenWeeks;
            ShiftOddWeeks = shiftOddWeeks;
            FullName = fullName;
        }

        public Dentist(int id, string shiftEvenWeeks, string shiftOddWeeks, string fullName)
        {
            Id = id;
            ShiftEvenWeeks = shiftEvenWeeks;
            ShiftOddWeeks = shiftOddWeeks;
            FullName = fullName;
        }

        public int Id { get; set; }

        public string ShiftEvenWeeks { get; set; }
        public string ShiftOddWeeks { get; set; }

        public string FullName { get; set; }

    }
}
