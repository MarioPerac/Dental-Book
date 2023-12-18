using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalBook.DTOs
{
    internal class DentistHasAssistant
    {
        public DentistHasAssistant(List<int> dentistsIds, string assistantUserName)
        {
            this.dentistsIds = dentistsIds;
            this.assistantUserName = assistantUserName;
        }

        public List<int> dentistsIds { get; set; }

        public string assistantUserName { get; set; }
    }
}
