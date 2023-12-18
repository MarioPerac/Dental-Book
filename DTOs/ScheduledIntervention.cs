using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalBook.DTOs
{
    public class ScheduledIntervention
    {
        public ScheduledIntervention(string fullName, string phone, string timeFrom, string timeUntil)
        {
            FullName = fullName;
            Phone = phone;
            TimeFrom = timeFrom;
            TimeUntil = timeUntil;
        }

        public ScheduledIntervention(string fullName, string phone, string timeFrom, string timeUntil, string formattedDate, DateTime date, int termId) : this(fullName, phone, timeFrom, timeUntil)
        {
            FormattedDate = formattedDate;
            Date = date;
            TermId = termId;
        }

        public string FullName { get; set; }
        public string Phone { get; set; }
        public string TimeFrom { get; set; }
        public string TimeUntil { get; set; }

        public string FormattedDate { get; set; }
        public DateTime Date { get; set; }

        public int TermId { get; set; }

        public bool IsSelected { get; set; }

    }
}
