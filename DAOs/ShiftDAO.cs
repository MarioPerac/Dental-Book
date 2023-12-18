using DentalBook.DTOs;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalBook.DAOs
{
    internal class ShiftDAO : DAO
    {
        public static List<string> GetShifts()
        {
            List<string> shifts = new List<string>();

            using (MySqlConnection connection = new MySqlConnection(DentistDAO.ConnectionString))
            {
                connection.Open();
                string selectQuery = "SELECT Naziv FROM smjena";
                using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            shifts.Add(reader.GetString(0));
                        }
                    }
                }

                connection.Close();

            }
            return shifts;
        }
    }
}
