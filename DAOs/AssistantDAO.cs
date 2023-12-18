using DentalBook.DTOs;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalBook.DAOs
{
    internal class AssistantDAO : DAO
    {

        public static void AddAssistant(Assistant assistant)
        {
            using (MySqlConnection connection = new MySqlConnection(AssistantDAO.ConnectionString))
            {
                connection.Open();
                string insertQuery = "INSERT INTO saradnik (KorisnickoIme, Lozinka) VALUES (@Username, @Password)";
                using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Username", assistant.Username);
                    command.Parameters.AddWithValue("@Password", assistant.Password);
                    command.ExecuteNonQuery();
                }
            }
        }
        public static bool IsAssistant(string username, string password)
        {
            bool result = false;

            using (MySqlConnection connection = new MySqlConnection(AdminDAO.ConnectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM saradnik WHERE KorisnickoIme = @Username AND Lozinka = @Password;";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);

                    result = Convert.ToInt32(command.ExecuteScalar()) == 1;
                }

                connection.Close();
            }

            return result;
        }

        public static List<Assistant> GetAllAssistants()
        {
            List<Assistant> assistants = new List<Assistant>();

            using (MySqlConnection connection = new MySqlConnection(AssistantDAO.ConnectionString))
            {
                connection.Open();

                string query = "SELECT * FROM saradnik";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string username = reader.GetString("KorisnickoIme");
                            string password = reader.GetString("Lozinka");

                            assistants.Add(new Assistant(username, password));
                        }
                    }
                }
            }

            return assistants;
        }

        public static void DeleteAssistant(string username)
        {
            using (MySqlConnection connection = new MySqlConnection(DentistDAO.ConnectionString))
            {
                connection.Open();

                string deleteQuery = "DELETE FROM saradnik WHERE KorisnickoIme = @Username";
                using (MySqlCommand command = new MySqlCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        throw new Exception("Assistant not found.");
                    }
                }
            }
        }

        public static void Update(Assistant assistant)
        {
            using (MySqlConnection connection = new MySqlConnection(AssistantDAO.ConnectionString))
            {
                connection.Open();

                string updateQuery = "UPDATE saradnik SET " +
                                    " Lozinka=@Password " +
                                    " WHERE KorisnickoIme = @Username";

                using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@Username", assistant.Username);
                    command.Parameters.AddWithValue("@Password", assistant.Password);
                    int rowsAffected = command.ExecuteNonQuery();
                }
            }

        }
    }
}
