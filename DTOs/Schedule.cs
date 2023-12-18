using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.RightsManagement;

namespace DentalBook.DTOs
{
    public class Schedule
    {
        public Schedule(bool mon, bool tue, bool wed, bool thu, bool fri, bool sat, bool sun, TimeSpan workFrom, TimeSpan? breakFrom, TimeSpan? breakUntil, TimeSpan workUntil, int dentistId, string shift)
        {
            Mon = mon;
            Tue = tue;
            Wed = wed;
            Thu = thu;
            Fri = fri;
            Sat = sat;
            Sun = sun;
            WorkFrom = workFrom;
            BreakFrom = breakFrom;
            BreakUntil = breakUntil;
            WorkUntil = workUntil;
            DentistId = dentistId;
            Shift = shift;
        }

        public bool Mon { get; set; }
        public bool Tue { get; set; }
        public bool Wed { get; set; }
        public bool Thu { get; set; }
        public bool Fri { get; set; }
        public bool Sat { get; set; }
        public bool Sun { get; set; }

        public TimeSpan WorkFrom { get; set; }

        public TimeSpan? BreakFrom { get; set; }
        public TimeSpan? BreakUntil { get; set; }

        public TimeSpan WorkUntil { get; set; }

        public int DentistId { get; set; }
        public string Shift { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Schedule otherSchedule = (Schedule)obj;
            return Mon == otherSchedule.Mon &&
                   Tue == otherSchedule.Tue &&
                   Wed == otherSchedule.Wed &&
                   Thu == otherSchedule.Thu &&
                   Fri == otherSchedule.Fri &&
                   Sat == otherSchedule.Sat &&
                   Sun == otherSchedule.Sun &&
                   WorkFrom == otherSchedule.WorkFrom &&
                   BreakFrom == otherSchedule.BreakFrom &&
                   BreakUntil == otherSchedule.BreakUntil &&
                   WorkUntil == otherSchedule.WorkUntil &&
                   DentistId == otherSchedule.DentistId &&
                   Shift == otherSchedule.Shift;
        }

        public static bool operator ==(Schedule schedule1, Schedule schedule2)
        {
            if (ReferenceEquals(schedule1, null))
            {
                return ReferenceEquals(schedule2, null);
            }

            return schedule1.Equals(schedule2);
        }

        public static bool operator !=(Schedule schedule1, Schedule schedule2)
        {
            return !(schedule1 == schedule2);
        }
    }
}
