using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentalBook.DTOs;
using MySql.Data.MySqlClient;

namespace DentalBook.DAOs
{
    internal class TermDAO : DAO
    {

        public static void AddTerms(List<Term> terms, string shift, int dentistId)
        {
            using (MySqlConnection connection = new MySqlConnection(TermDAO.ConnectionString))
            {
                connection.Open();
                string insertQuery = "INSERT INTO termin" +
                    " (VrijemeOd, VrijemeDo, IdStomatologa, Smjena)" +
                    " VALUES " +
                    "(@TimeFrom, @TimeUntil, @DentistId, @Shift) ";
                using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                {
                    foreach (Term term in terms)
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@TimeFrom", term.TimeFrom);
                        command.Parameters.AddWithValue("@TimeUntil", term.TimeUntil);
                        command.Parameters.AddWithValue("@DentistId", dentistId);
                        command.Parameters.AddWithValue("@Shift", shift);
                        command.ExecuteNonQuery();
                    }
                }

            }
        }

        public static Dictionary<int, string> GetTerms(int dentistId, DateTime date, string shift)
        {
            Dictionary<int, string> terms = new Dictionary<int, string>();

            using (MySqlConnection connection = new MySqlConnection(DentistDAO.ConnectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand("free_terms", connection))
                {

                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@pDentistId", dentistId);
                    command.Parameters.AddWithValue("@pDate", date);
                    command.Parameters.AddWithValue("@pShift", shift);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int termId = reader.GetInt32("IdTermina");
                            string timeFrom = reader.GetTimeSpan("VrijemeOd").ToString();
                            string timeUntil = reader.GetTimeSpan("VrijemeDo").ToString();
                            string term = timeFrom + " - " + timeUntil;
                            terms.Add(termId, term);
                        }
                    }
                }
            }
            return terms;
        }

        public static void Delete(int dentistId, string shift)
        {
            using (MySqlConnection connection = new MySqlConnection(TermDAO.ConnectionString))
            {
                connection.Open();

                string deleteQuery = "DELETE FROM termin WHERE IdStomatologa = @DentistId AND Smjena = @Shift";
                using (MySqlCommand command = new MySqlCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@DentistId", dentistId);
                    command.Parameters.AddWithValue("@Shift", shift);

                    int rowsAffected = command.ExecuteNonQuery();

                    
                }
            }
        }
    }
}
