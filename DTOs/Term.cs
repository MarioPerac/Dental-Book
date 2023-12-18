using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalBook.DTOs
{
    internal class Term
    {
        public Term(TimeSpan timeFrom, TimeSpan timeUtnil)
        {
            TimeFrom = timeFrom;
            TimeUntil = timeUtnil;
        }

        public TimeSpan TimeFrom { get; set; }

        public TimeSpan TimeUntil { get; set; }

        public static List<Term> generateTermsWithBreak(TimeSpan timeFrom, TimeSpan breakFrom, TimeSpan breakUntil, TimeSpan timeUntil)
        {
            List<Term> terms = new List<Term>();

            TimeSpan currentTime = timeFrom;
            TimeSpan modifiedTime = currentTime.Add(TimeSpan.FromMinutes(30));


            while (modifiedTime <= timeUntil)
            {
                if (modifiedTime <= breakFrom || currentTime >= breakUntil)
                {
                    terms.Add(new Term(currentTime, modifiedTime));
                }
                currentTime = modifiedTime;
                modifiedTime = modifiedTime.Add(TimeSpan.FromMinutes(30));
            }

            return terms;

        }


        public static List<Term> generateTerms(TimeSpan timeFrom, TimeSpan timeUntil)
        {
            List<Term> terms = new List<Term>();

            TimeSpan currentTime = timeFrom;
            TimeSpan modifiedTime = currentTime.Add(TimeSpan.FromMinutes(30));


            while (modifiedTime <= timeUntil)
            {
                terms.Add(new Term(currentTime, modifiedTime));

                currentTime = modifiedTime;
                modifiedTime = modifiedTime.Add(TimeSpan.FromMinutes(30));
            }

            return terms;

        }

    }
}
