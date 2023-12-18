using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DentalBook.DAOs
{
    internal class AdminDAO : DAO
    {
        public static bool IsAdmin(string username, string password)
        {
            bool result = false;

            using (MySqlConnection connection = new MySqlConnection(AdminDAO.ConnectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM administrator WHERE KorisnickoIme = @Username AND Lozinka = @Password;";

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
    }
}
