using DentalBook.DTOs;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalBook.DAOs
{
    internal class InterventionDAO : DAO
    {
        public static void AddRow(int termId, DateTime date, string assistantUsername, int patientId)
        {
            using (MySqlConnection connection = new MySqlConnection(InterventionDAO.ConnectionString))
            {
                connection.Open();
                string insertQuery = "INSERT INTO intervencija (IdTermina, Datum, SARADNIK_KorisnickoIme, PACIJENT_idPacijenta) " +
                    "VALUES (@TermId, @Date, @AssistantUsername, @PatientId) ";
                using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@TermId", termId);
                    command.Parameters.AddWithValue("@Date", date);
                    command.Parameters.AddWithValue("@AssistantUsername", assistantUsername);
                    command.Parameters.AddWithValue("@PatientId", patientId);
                    command.ExecuteNonQuery();

                }


            }
        }

        public static List<ScheduledIntervention> ScheduledInterventions(int dentistId, DateTime date, string assistantUsername)
        {

            List<ScheduledIntervention> interverntions = new List<ScheduledIntervention>();
            using (MySqlConnection connection = new MySqlConnection(InterventionDAO.ConnectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand("scheduled_intervention_by_date", connection))
                {

                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@pDentistId", dentistId);
                    command.Parameters.AddWithValue("@pDate", date);
                    command.Parameters.AddWithValue("@pAssistant", assistantUsername);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string fullName = reader.GetString("ImePrezime");
                            string phone = reader.GetString("Telefon");
                            string timeFrom = reader.GetTimeSpan("VrijemeOd").ToString();
                            string timeUntil = reader.GetTimeSpan("VrijemeDo").ToString();
                            interverntions.Add(new ScheduledIntervention(fullName, phone, timeFrom, timeUntil));
                        }
                    }
                }
            }
            return interverntions;

        }

        public static List<ScheduledIntervention> ScheduledInterventions(int dentistId, string assistantUsername)
        {

            List<ScheduledIntervention> interverntions = new List<ScheduledIntervention>();
            using (MySqlConnection connection = new MySqlConnection(DentistDAO.ConnectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand("scheduled_intervention_by_assistant", connection))
                {

                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@pDentistId", dentistId);
                    command.Parameters.AddWithValue("@pAssistant", assistantUsername);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int termId = reader.GetInt32("IdTermina");
                            string fullName = reader.GetString("ImePrezime");
                            string phone = reader.GetString("Telefon");
                            DateTime date = reader.GetDateTime("Datum");
                            string timeFrom = reader.GetTimeSpan("VrijemeOd").ToString();
                            string timeUntil = reader.GetTimeSpan("VrijemeDo").ToString();
                            ScheduledIntervention intervention = new ScheduledIntervention(fullName, phone, timeFrom, timeUntil, date.ToString("dd-MM-yyy"), date, termId);
                            interverntions.Add(intervention);
                        }
                    }
                }
            }
            return interverntions;

        }


        public static void Delete(int termId, DateTime date)
        {
            using (MySqlConnection connection = new MySqlConnection(InterventionDAO.ConnectionString))
            {
                connection.Open();

                string deleteQuery = "DELETE FROM intervencija WHERE IdTermina = @TermId and Datum = @Date";
                using (MySqlCommand command = new MySqlCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@TermId", termId);
                    command.Parameters.AddWithValue("@Date", date);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        throw new Exception("Intervention not found.");
                    }
                }
            }
        }
    }
}
