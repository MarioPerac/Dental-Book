using DentalBook.DAOs;
using DentalBook.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DentalBook
{
    /// <summary>
    /// Interaction logic for UpdateDentistWindow.xaml
    /// </summary>
    public partial class UpdateDentistWindow : Window
    {
        private List<string> shifts = ShiftDAO.GetShifts();
        private Dentist Dentist { get; set; }
        private Schedule ScheduleEvenWeeks { get; set; }
        private Schedule ScheduleOddWeeks { get; set; }
        public UpdateDentistWindow(Dentist dentist, Schedule scheduleEvenWeeks, Schedule scheduleOddWeeks)
        {
            InitializeComponent();
            Dentist = dentist;
            ScheduleEvenWeeks = scheduleEvenWeeks;
            ScheduleOddWeeks = scheduleOddWeeks;
            ShiftEvenWeeksComboBox.ItemsSource = shifts;
            ShiftOddWeeksComboBox.ItemsSource = shifts;

            DentistNameSurnameTextBox.Text = dentist.FullName;
            ShiftEvenWeeksComboBox.SelectedItem = dentist.ShiftEvenWeeks;
            ShiftOddWeeksComboBox.SelectedItem = dentist.ShiftOddWeeks;

            EWSMondayCheckBox.IsChecked = scheduleEvenWeeks.Mon;
            EWSTuesdayCheckBox.IsChecked = scheduleEvenWeeks.Tue;
            EWSWednesdayCheckBox.IsChecked = scheduleEvenWeeks.Wed;
            EWSThursdayCheckBox.IsChecked = scheduleEvenWeeks.Thu;
            EWSFridayCheckBox.IsChecked = scheduleEvenWeeks.Fri;
            EWSStaurdayCheckBox.IsChecked = scheduleEvenWeeks.Sat;
            EWSSundayCheckBox.IsChecked = scheduleEvenWeeks.Sun;

            DateTime today = DateTime.Today;

            EWSWorkFromTimePicker.SelectedTime = today + scheduleEvenWeeks.WorkFrom;
            EWSBreakFromTimePicker.SelectedTime = today + scheduleEvenWeeks.BreakFrom;
            EWSBreakUntilTimePicker.SelectedTime = today + scheduleEvenWeeks.BreakUntil;
            EWSWorkUntilTimePicker.SelectedTime = today + scheduleEvenWeeks.WorkUntil;

            if (scheduleOddWeeks != null)
            {
                ShiftOddWeeksComboBox.Visibility = Visibility.Visible;
                OWSMondayCheckBox.IsChecked = scheduleOddWeeks.Mon;
                OWSTuesdayCheckBox.IsChecked = scheduleOddWeeks.Tue;
                OWSWednesdayCheckBox.IsChecked = scheduleOddWeeks.Wed;
                OWSThursdayCheckBox.IsChecked = scheduleOddWeeks.Thu;
                OWSFridayCheckBox.IsChecked = scheduleOddWeeks.Fri;
                OWSSaturdayCheckBox.IsChecked = scheduleOddWeeks.Sat;
                OWSSundayCheckBox.IsChecked = scheduleOddWeeks.Sun;

                OWSWorkFromTimePicker.SelectedTime = today + scheduleOddWeeks.WorkFrom;
                OWSBreakFromTimePicker.SelectedTime = today + scheduleOddWeeks.BreakFrom;
                OWSBreakUntilTimePicker.SelectedTime = today + scheduleOddWeeks.BreakUntil;
                OWSWorkUntilTimePicker.SelectedTime = today + scheduleOddWeeks.WorkUntil;
            }

        }


        private void SelectionChangedShiftComboBox(object sender, SelectionChangedEventArgs e)
        {

            if (ShiftEvenWeeksComboBox.SelectedItem != null)
            {
                EvenWeeksSchedule.Visibility = Visibility.Visible;
            }

            if (ShiftOddWeeksComboBox.SelectedItem != null)
            {
                OddWeeksSchedule.Visibility = Visibility.Visible;
            }

            if (ShiftEvenWeeksComboBox.SelectedItem != null && ShiftOddWeeksComboBox.SelectedItem != null)
            {
                string evenWeeksShift = ShiftEvenWeeksComboBox.SelectedItem.ToString();
                string oddWeeksShift = ShiftOddWeeksComboBox.SelectedItem.ToString();

                if (evenWeeksShift == oddWeeksShift)
                {
                    OddWeeksSchedule.Visibility = Visibility.Collapsed;
                }
            }
        }
        private void CancelButton(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void UpdateDentistButton(object sender, RoutedEventArgs e)
        {
            bool updated = false;
            string fullName = DentistNameSurnameTextBox.Text;
            if (string.IsNullOrEmpty(fullName) || ShiftEvenWeeksComboBox.SelectedItem == null || ShiftOddWeeksComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please fill all required fields.", "Create Dentist Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string shiftEvenWeeks = ShiftEvenWeeksComboBox.SelectedItem.ToString();
            string shiftOddWeeks = ShiftOddWeeksComboBox.SelectedItem.ToString();
            

            if (shiftEvenWeeks != Dentist.ShiftEvenWeeks || shiftOddWeeks != Dentist.ShiftOddWeeks || fullName != Dentist.FullName)
            {
                DentistDAO.Update(new Dentist(Dentist.Id, shiftEvenWeeks, shiftOddWeeks, fullName));
                updated = true;
            }

            bool ewsMonday = EWSMondayCheckBox.IsChecked ?? false;
            bool ewsTuesday = EWSTuesdayCheckBox.IsChecked ?? false;
            bool ewsWednesday = EWSWednesdayCheckBox.IsChecked ?? false;
            bool ewsThursday = EWSThursdayCheckBox.IsChecked ?? false;
            bool ewsFriday = EWSFridayCheckBox.IsChecked ?? false;
            bool ewsSaturday = EWSStaurdayCheckBox.IsChecked ?? false;
            bool ewsSunday = EWSSundayCheckBox.IsChecked ?? false;
            List<Term> ewsTerms;
            List<Term> owsTerms;
            TimeSpan ewsWorkFrom = EWSWorkFromTimePicker.SelectedTime.Value.TimeOfDay;
            TimeSpan ewsWorkUntil = EWSWorkUntilTimePicker.SelectedTime.Value.TimeOfDay;
            TimeSpan? ewsBreakFrom = null;
            TimeSpan? ewsBreakUntil = null;

            if (EWSBreakFromTimePicker.SelectedTime != null && EWSBreakUntilTimePicker.SelectedTime != null)
            {
                ewsBreakFrom = EWSBreakFromTimePicker.SelectedTime.Value.TimeOfDay;
                ewsBreakUntil = EWSBreakUntilTimePicker.SelectedTime.Value.TimeOfDay;
                ewsTerms = Term.generateTermsWithBreak(ewsWorkFrom, (TimeSpan)ewsBreakFrom, (TimeSpan)ewsBreakUntil, ewsWorkUntil);
            }
            else
            {
                ewsTerms = Term.generateTerms(ewsWorkFrom, ewsWorkUntil);
            }

            Schedule currentScheduleEvenWeeks = new Schedule(ewsMonday, ewsTuesday, ewsWednesday, ewsThursday, ewsFriday, ewsSaturday, ewsSunday, ewsWorkFrom, ewsBreakFrom, ewsBreakUntil, ewsWorkUntil, Dentist.Id, shiftEvenWeeks);



            

            if (shiftEvenWeeks != shiftOddWeeks)
            {
                bool owsMonday = OWSMondayCheckBox.IsChecked ?? false;
                bool owsTuesday = OWSTuesdayCheckBox.IsChecked ?? false;
                bool owsWednesday = OWSWednesdayCheckBox.IsChecked ?? false;
                bool owsThursday = OWSThursdayCheckBox.IsChecked ?? false;
                bool owsFriday = OWSFridayCheckBox.IsChecked ?? false;
                bool owsSaturday = OWSSaturdayCheckBox.IsChecked ?? false;
                bool owsSunday = OWSSundayCheckBox.IsChecked ?? false;

                TimeSpan owsWorkFrom = OWSWorkFromTimePicker.SelectedTime.Value.TimeOfDay;
                TimeSpan owsWorkUntil = OWSWorkUntilTimePicker.SelectedTime.Value.TimeOfDay;
                TimeSpan? owsBreakFrom = null;
                TimeSpan? owsBreakUntil = null;
                if (OWSBreakFromTimePicker.SelectedTime != null && OWSBreakUntilTimePicker.SelectedTime != null)
                {
                    owsBreakFrom = OWSWorkUntilTimePicker.SelectedTime.Value.TimeOfDay;
                    owsBreakUntil = OWSBreakFromTimePicker.SelectedTime.Value.TimeOfDay;
                    owsTerms = Term.generateTermsWithBreak(owsWorkFrom, (TimeSpan)owsBreakFrom, (TimeSpan)owsBreakUntil, owsWorkUntil);
                }
                else
                {
                    owsTerms = Term.generateTerms(owsWorkFrom, owsWorkUntil);
                }

                Schedule currentScheduleOddWeeks = new Schedule(owsMonday, owsTuesday, owsWednesday, owsThursday, owsFriday, owsSaturday, owsSunday, owsWorkFrom, owsBreakFrom, owsBreakUntil, owsWorkUntil, Dentist.Id, shiftOddWeeks);

                
                if(ScheduleEvenWeeks != currentScheduleOddWeeks && ScheduleOddWeeks != currentScheduleEvenWeeks)
                {
                    if (ScheduleEvenWeeks != currentScheduleEvenWeeks)
                    {
                        if (ScheduleEvenWeeks != currentScheduleOddWeeks)
                        {
                            ScheduleDAO.Delete(Dentist.Id, Dentist.ShiftEvenWeeks);
                            TermDAO.Delete(Dentist.Id, Dentist.ShiftEvenWeeks);
                        }
                        ScheduleDAO.AddSchedule(currentScheduleEvenWeeks);
                        TermDAO.AddTerms(ewsTerms, shiftEvenWeeks, Dentist.Id);
                        updated = true;
                    }

                    if (currentScheduleOddWeeks != ScheduleOddWeeks)
                    {
                        if (ScheduleOddWeeks != currentScheduleEvenWeeks && Dentist.ShiftOddWeeks != Dentist.ShiftEvenWeeks)
                        {
                            ScheduleDAO.Delete(Dentist.Id, Dentist.ShiftOddWeeks);
                            TermDAO.Delete(Dentist.Id, Dentist.ShiftOddWeeks);
                        }
                        ScheduleDAO.AddSchedule(currentScheduleOddWeeks);
                        TermDAO.AddTerms(owsTerms, shiftOddWeeks, Dentist.Id);
                        if (!updated)
                        {
                            updated = true;
                        }
                    }

                }

            }
            else
            {

                if (Dentist.ShiftOddWeeks != Dentist.ShiftEvenWeeks)
                {
                    ScheduleDAO.Delete(Dentist.Id, Dentist.ShiftOddWeeks);
                    TermDAO.Delete(Dentist.Id, Dentist.ShiftOddWeeks);
                }

                if (ScheduleEvenWeeks != currentScheduleEvenWeeks)
                {
                   
                    ScheduleDAO.Delete(Dentist.Id, Dentist.ShiftEvenWeeks);
                    TermDAO.Delete(Dentist.Id, Dentist.ShiftEvenWeeks);
                    ScheduleDAO.AddSchedule(currentScheduleEvenWeeks);
                    TermDAO.AddTerms(ewsTerms, shiftEvenWeeks, Dentist.Id);
                    updated = true;
                }
            }


           

            if (updated)
            {
                MessageBox.Show("Dentist UPDATED SUCCESSFULLY!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            else
            {
                MessageBox.Show("No changes made. Dentist NOT UPDATED", "Update Assistant Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
