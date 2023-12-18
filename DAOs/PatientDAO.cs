using DentalBook.DTOs;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalBook.DAOs
{
    internal class PatientDAO : DAO
    {
        public static int Add(Patient patient)
        {
            int patientId;
            using (MySqlConnection connection = new MySqlConnection(PatientDAO.ConnectionString))
            {
                connection.Open();
                string insertQuery = "INSERT INTO pacijent" +
                    " (ImePrezime, Telefon)" +
                    " VALUES (@FullName,@Phone);" +
                    "SELECT LAST_INSERT_ID() ";
                using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@FullName", patient.FullName);
                    command.Parameters.AddWithValue("@Phone", patient.Phone);
                    patientId = Convert.ToInt32(command.ExecuteScalar());
                }


            }
            return patientId;
        }

        public static int PatientCheck(string fullName)
        {
            int patientId = -1;
            using (MySqlConnection connection = new MySqlConnection(PatientDAO.ConnectionString))
            {
                connection.Open();

                string query = "SELECT idPacijenta FROM pacijent WHERE ImePrezime = @FullName;";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FullName", fullName);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            patientId = reader.GetInt32("idPacijenta");
                        }
                    }
                }
            }

            return patientId;
        }
    }
}
