using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentalBook.DTOs;
using Google.Protobuf.Collections;
using MySql.Data.MySqlClient;
namespace DentalBook.DAOs
{
    internal class DentistDAO : DAO
    {

        public static int AddDentist(Dentist dentist)
        {
            int dentistId;
            using (MySqlConnection connection = new MySqlConnection(DentistDAO.ConnectionString))
            {
                connection.Open();
                string insertQuery = "INSERT INTO stomatolog (ImePrezime, SmjenaParnimSedmicama, SmjenaNeparnimSedmicama)" +
                    " VALUES (@FullName,@ShiftEvenWeeks, @ShiftOddWeeks);" +
                    "SELECT LAST_INSERT_ID() ";
                using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@FullName", dentist.FullName);
                    command.Parameters.AddWithValue("@ShiftEvenWeeks", dentist.ShiftEvenWeeks);
                    command.Parameters.AddWithValue("@ShiftOddWeeks", dentist.ShiftOddWeeks);
                    dentistId = Convert.ToInt32(command.ExecuteScalar());
                }


            }
            return dentistId;
        }

        public static List<Dentist> GetAllDentists()
        {
            List<Dentist> dentists = new List<Dentist>();

            using (MySqlConnection connection = new MySqlConnection(DentistDAO.ConnectionString))
            {
                connection.Open();

                string query = "SELECT * FROM stomatolog";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32("Id");
                            string fullName = reader.GetString("ImePrezime");
                            string shiftEvenWeeks = reader.GetString("SmjenaParnimSedmicama");
                            string shiftOddWeeks = reader.GetString("SmjenaNeparnimSedmicama");

                            dentists.Add(new Dentist(id, shiftEvenWeeks, shiftOddWeeks, fullName));
                        }
                    }
                }
            }
            return dentists;
        }

        public static void DeleteDentist(int dentistId)
        {
            using (MySqlConnection connection = new MySqlConnection(DentistDAO.ConnectionString))
            {
                connection.Open();

                string deleteQuery = "DELETE FROM stomatolog WHERE Id = @DentistId";
                using (MySqlCommand command = new MySqlCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@DentistId", dentistId);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        throw new Exception("Dentist not found.");
                    }
                }
            }
        }

        public static string GetShift(DateTime date, int dentistId)
        {
            string shift = "";
            using (MySqlConnection connection = new MySqlConnection(DentistDAO.ConnectionString))
            {
                connection.Open();
                string query;

                Calendar calendar = CultureInfo.CurrentCulture.Calendar;
                int weekNumber = calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

                if (weekNumber % 2 == 0)
                {
                    query = "SELECT SmjenaParnimSedmicama FROM stomatolog WHERE Id=@DentistId";
                }
                else
                {
                    query = "SELECT SmjenaNeparnimSedmicama FROM stomatolog WHERE Id=@DentistId";

                }
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DentistId", dentistId);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            shift = reader.GetString(0);
                        }
                    }
                }
                return shift;
            }
        }

        public static void Update(Dentist dentist)
        {
            using (MySqlConnection connection = new MySqlConnection(DentistDAO.ConnectionString))
            {
                connection.Open();

                string updateQuery = "UPDATE stomatolog SET " +
                                    "SmjenaParnimSedmicama=@ShiftEvenWeeks, SmjenaNeparnimSedmicama=@ShiftOddWeeks," +
                                    "ImePrezime=@FullName WHERE Id = @DentistId";

                using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@ShiftEvenWeeks", dentist.ShiftEvenWeeks);
                    command.Parameters.AddWithValue("@ShiftOddWeeks", dentist.ShiftOddWeeks);
                    command.Parameters.AddWithValue("@FullName", dentist.FullName);
                    command.Parameters.AddWithValue("@DentistId", dentist.Id);

                    int rowsAffected = command.ExecuteNonQuery();
                }
            }

        }

    }
}
