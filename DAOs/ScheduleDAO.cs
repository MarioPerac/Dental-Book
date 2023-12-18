using DentalBook.DTOs;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalBook.DAOs
{
    internal class ScheduleDAO : DAO
    {

        public static void AddSchedule(Schedule schedule)
        {
            using (MySqlConnection connection = new MySqlConnection(ScheduleDAO.ConnectionString))
            {
                connection.Open();
                string insertQuery = "INSERT INTO raspored " +
                    "(Smjena, Pon, Uto, Sre, Cet, Pet, Sub, Ned, RadOd, PauzaOd, PauzaDo, RadDo, IdStomatologa)" +
                    " VALUES (@Shift,@Mon,@Tue, @Wed, @Thu, @Fri, @Sat, @Sun, @WorkFrom, @BreakFrom, @BreakUntil, @WorkUntil, @DentistId)";
                using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Shift", schedule.Shift);
                    command.Parameters.AddWithValue("@Mon", schedule.Mon);
                    command.Parameters.AddWithValue("@Tue", schedule.Tue);
                    command.Parameters.AddWithValue("@Wed", schedule.Wed);
                    command.Parameters.AddWithValue("@Thu", schedule.Thu);
                    command.Parameters.AddWithValue("@Fri", schedule.Fri);
                    command.Parameters.AddWithValue("@Sat", schedule.Sat);
                    command.Parameters.AddWithValue("@Sun", schedule.Sun);
                    command.Parameters.AddWithValue("@WorkFrom", schedule.WorkFrom);
                    command.Parameters.AddWithValue("@BreakFrom", schedule.BreakFrom);
                    command.Parameters.AddWithValue("@BreakUntil", schedule.BreakUntil);
                    command.Parameters.AddWithValue("@WorkUntil", schedule.WorkUntil);
                    command.Parameters.AddWithValue("@DentistId", schedule.DentistId);

                    command.ExecuteNonQuery();
                }


            }
        }

        public static Schedule Get(int dentistId, string shift)
        {
            Schedule schedule = null;
            using (MySqlConnection connection = new MySqlConnection(ScheduleDAO.ConnectionString))
            {
                connection.Open();

                string query = "SELECT Pon, Uto, Sre, Cet, Pet, Sub, Ned, RadOd, PauzaOd, PauzaDo, RadDo FROM raspored WHERE IdStomatologa=@DentistId and Smjena=@Shift";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DentistId", dentistId);
                    command.Parameters.AddWithValue("@Shift", shift);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bool mon = reader.GetBoolean("Pon");
                            bool tue = reader.GetBoolean("Uto");
                            bool wed = reader.GetBoolean("Sre");
                            bool thu = reader.GetBoolean("Cet");
                            bool fri = reader.GetBoolean("Pet");
                            bool sat = reader.GetBoolean("Sub");
                            bool sun = reader.GetBoolean("Ned");
                            TimeSpan workFrom = reader.GetTimeSpan("RadOd");
                            TimeSpan? breakFrom = reader.IsDBNull(8) ? (TimeSpan?)null : reader.GetTimeSpan("PauzaOd");
                            TimeSpan? breakUntil = reader.IsDBNull(9) ? (TimeSpan?)null : reader.GetTimeSpan("PauzaDo");
                            TimeSpan workUntil = reader.GetTimeSpan("RadDo");

                            schedule = new Schedule(mon, tue, wed, thu, fri, sat, sun, workFrom, breakFrom, breakUntil, workUntil, dentistId, shift);
                        }
                    }
                }
            }
            return schedule;
        }

        public static void Update(Schedule schedule)
        {
            using (MySqlConnection connection = new MySqlConnection(ScheduleDAO.ConnectionString))
            {
                connection.Open();

                string updateQuery = "UPDATE raspored SET " +
                                     "Pon = @Pon, Uto = @Uto, Sre = @Sre, " +
                                     "Cet = @Cet, Pet = @Pet, Sub = @Sub, " +
                                     "Ned = @Ned, RadOd = @RadOd, PauzaOd = @PauzaOd, " +
                                     "PauzaDo = @PauzaDo, RadDo = @RadDo " +
                                     "WHERE DentistId = @DentistId AND Shift = @Shift";

                using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@Pon", schedule.Mon);
                    command.Parameters.AddWithValue("@Uto", schedule.Tue);
                    command.Parameters.AddWithValue("@Sre", schedule.Wed);
                    command.Parameters.AddWithValue("@Cet", schedule.Thu);
                    command.Parameters.AddWithValue("@Pet", schedule.Fri);
                    command.Parameters.AddWithValue("@Sub", schedule.Sat);
                    command.Parameters.AddWithValue("@Ned", schedule.Sun);
                    command.Parameters.AddWithValue("@RadOd", schedule.WorkFrom);
                    command.Parameters.AddWithValue("@PauzaOd", schedule.BreakFrom);
                    command.Parameters.AddWithValue("@PauzaDo", schedule.BreakUntil);
                    command.Parameters.AddWithValue("@RadDo", schedule.WorkUntil);
                    command.Parameters.AddWithValue("@DentistId", schedule.DentistId);
                    command.Parameters.AddWithValue("@Shift", schedule.Shift);

                    int rowsAffected = command.ExecuteNonQuery();
                }
            }

        }

        public static void Delete(int dentistId, string shift)
        {
            using (MySqlConnection connection = new MySqlConnection(DentistHasAssistantDAO.ConnectionString))
            {
                connection.Open();

                string deleteQuery = "DELETE FROM raspored WHERE IdStomatologa=@DentistId AND Smjena=@Shift";
                using (MySqlCommand command = new MySqlCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@DentistId", dentistId);
                    command.Parameters.AddWithValue("@Shift", shift);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        throw new Exception("Dentist not found.");
                    }
                }
            }
        }

    }
}
