using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalBook.DAOs
{
    internal class DAO
    {
        protected static string ConnectionString { get; } = ConfigurationManager.ConnectionStrings["MySqlDentalBook"].ConnectionString;
        public DAO()
        {
            Debug.WriteLine(ConnectionString);
        }


    }

}
