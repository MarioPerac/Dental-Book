using DentalBook.DTOs;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalBook.DAOs
{
    internal class DentistHasAssistantDAO : DAO
    {
        public static void Add(DentistHasAssistant dentistHasAssistants)
        {
            using (MySqlConnection connection = new MySqlConnection(AssistantDAO.ConnectionString))
            {
                connection.Open();
                string insertQuery = "INSERT INTO stomatolog_ima_saradnika (STOMATOLOG_Id, SARADNIK_KorisnickoIme) VALUES (@DentistId, @AssistantUsername)";
                using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                {
                    foreach (int id in dentistHasAssistants.dentistsIds)
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@DentistId", id);
                        command.Parameters.AddWithValue("@AssistantUsername", dentistHasAssistants.assistantUserName);
                        command.ExecuteNonQuery();
                    }
                }


            }
        }

        public static Dictionary<int, string> GetAllDentists(string assistant)
        {
            Dictionary<int, string> dentists = new Dictionary<int, string>();

            using (MySqlConnection connection = new MySqlConnection(DentistDAO.ConnectionString))
            {
                connection.Open();

                string query = "SELECT Id, ImePrezime FROM stomatolog JOIN stomatolog_ima_saradnika ON Id = STOMATOLOG_Id" +
                    " WHERE SARADNIK_KorisnickoIme=@AssistantUsername";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("AssistantUsername", assistant);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32("Id");
                            string fullName = reader.GetString("ImePrezime");

                            dentists.Add(id, fullName);
                        }
                    }
                }
            }
            return dentists;
        }

        public static void Delete(string assistantUsername)
        {
            using (MySqlConnection connection = new MySqlConnection(DentistHasAssistantDAO.ConnectionString))
            {
                connection.Open();

                string deleteQuery = "DELETE FROM stomatolog_ima_saradnika WHERE SARADNIK_KorisnickoIme=@AssistantUsername";
                using (MySqlCommand command = new MySqlCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@AssistantUsername", assistantUsername);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        throw new Exception("Dentist has assistant not found.");
                    }
                }
            }
        }
    }
}
