using DentalBook.DAOs;
using DentalBook.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for AddDentistWindow.xaml
    /// </summary>
    public partial class AddDentistWindow : Window
    {

        List<string> shifts = ShiftDAO.GetShifts();
        public AddDentistWindow()
        {
            InitializeComponent();
            ShiftEvenWeeksComboBox.ItemsSource = shifts;
            ShiftOddWeeksComboBox.ItemsSource = shifts;
        }

        private void CreateDentistButton(object sender, RoutedEventArgs e)
        {
            string fullName = DentistNameSurnameTextBox.Text;
            if (string.IsNullOrEmpty(fullName) || ShiftEvenWeeksComboBox.SelectedItem == null || ShiftOddWeeksComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please fill all required fields.", "Create Dentist Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string shiftEvenWeeks = ShiftEvenWeeksComboBox.SelectedItem.ToString();
            string shiftOddWeeks = ShiftOddWeeksComboBox.SelectedItem.ToString();
            int dentistId = DentistDAO.AddDentist(new Dentist(shiftEvenWeeks, shiftOddWeeks, fullName));
            if (shiftEvenWeeks == shiftOddWeeks)
            {
                bool monday = EWSMondayCheckBox.IsChecked ?? false;
                bool tuesday = EWSTuesdayCheckBox.IsChecked ?? false;
                bool wednesday = EWSWednesdayCheckBox.IsChecked ?? false;
                bool thursday = EWSThursdayCheckBox.IsChecked ?? false;
                bool friday = EWSFridayCheckBox.IsChecked ?? false;
                bool saturday = EWSStaurdayCheckBox.IsChecked ?? false;
                bool sunday = EWSSundayCheckBox.IsChecked ?? false;
                List<Term> terms;

                TimeSpan workFrom = EWSWorkFromTimePicker.SelectedTime.Value.TimeOfDay;
                TimeSpan workUntil = EWSWorkUntilTimePicker.SelectedTime.Value.TimeOfDay;
                TimeSpan? breakFrom = null;
                TimeSpan? breakUntil = null;
                if (EWSBreakFromTimePicker.SelectedTime != null && EWSBreakUntilTimePicker.SelectedTime != null)
                {
                    breakFrom = EWSBreakFromTimePicker.SelectedTime.Value.TimeOfDay;
                    breakUntil = EWSBreakUntilTimePicker.SelectedTime.Value.TimeOfDay;
                    terms = Term.generateTermsWithBreak(workFrom, (TimeSpan)breakFrom, (TimeSpan)breakUntil, workUntil);

                }
                else
                {
                    terms = Term.generateTerms(workFrom, workUntil);
                }

                ScheduleDAO.AddSchedule(new Schedule(monday, tuesday, wednesday, thursday, friday, saturday, sunday, workFrom, breakFrom, breakUntil, workUntil, dentistId, shiftEvenWeeks));
                TermDAO.AddTerms(terms, shiftEvenWeeks, dentistId);
            }
            else
            {
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

                ScheduleDAO.AddSchedule(new Schedule(ewsMonday, ewsTuesday, ewsWednesday, ewsThursday, ewsFriday, ewsSaturday, ewsSunday, ewsWorkFrom, ewsBreakFrom, ewsBreakUntil, ewsWorkUntil, dentistId, shiftEvenWeeks));
                TermDAO.AddTerms(ewsTerms, shiftEvenWeeks, dentistId);

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

                ScheduleDAO.AddSchedule(new Schedule(owsMonday, owsTuesday, owsWednesday, owsThursday, owsFriday, owsSaturday, owsSunday, owsWorkFrom, owsBreakFrom, owsBreakUntil, owsWorkUntil, dentistId, shiftOddWeeks));
                TermDAO.AddTerms(owsTerms, shiftOddWeeks, dentistId);
            }
            MessageBox.Show("Denitst ADDED SUCCESSFULLY!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            ResetControls();
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

        private void ResetControls()
        {

            DentistNameSurnameTextBox.Text = null;
            ShiftEvenWeeksComboBox.SelectedIndex = -1;
            ShiftOddWeeksComboBox.SelectedIndex = -1;
            EWSMondayCheckBox.IsChecked = false;
            EWSTuesdayCheckBox.IsChecked = false;
            EWSWednesdayCheckBox.IsChecked = false;
            EWSThursdayCheckBox.IsChecked = false;
            EWSFridayCheckBox.IsChecked = false;
            EWSStaurdayCheckBox.IsChecked = false;
            EWSSundayCheckBox.IsChecked = false;

            EWSWorkFromTimePicker.SelectedTime = null;
            EWSWorkUntilTimePicker.SelectedTime = null;
            EWSBreakFromTimePicker.SelectedTime = null;
            EWSBreakUntilTimePicker.SelectedTime = null;


            EWSMondayCheckBox.IsChecked = false;
            EWSTuesdayCheckBox.IsChecked = false;
            EWSWednesdayCheckBox.IsChecked = false;
            EWSThursdayCheckBox.IsChecked = false;
            EWSFridayCheckBox.IsChecked = false;
            EWSStaurdayCheckBox.IsChecked = false;
            EWSSundayCheckBox.IsChecked = false;


            EWSWorkFromTimePicker.SelectedTime = null;
            EWSWorkUntilTimePicker.SelectedTime = null;
            EWSBreakFromTimePicker.SelectedTime = null;
            EWSBreakUntilTimePicker.SelectedTime = null;


            OWSMondayCheckBox.IsChecked = false;
            OWSTuesdayCheckBox.IsChecked = false;
            OWSWednesdayCheckBox.IsChecked = false;
            OWSThursdayCheckBox.IsChecked = false;
            OWSFridayCheckBox.IsChecked = false;
            OWSSaturdayCheckBox.IsChecked = false;
            OWSSundayCheckBox.IsChecked = false;

            OWSWorkFromTimePicker.SelectedTime = null;
            OWSWorkUntilTimePicker.SelectedTime = null;
            OWSBreakFromTimePicker.SelectedTime = null;
            OWSBreakUntilTimePicker.SelectedTime = null;

            EvenWeeksSchedule.Visibility = Visibility.Collapsed;
            OddWeeksSchedule.Visibility = Visibility.Collapsed;

        }

        private void CancelButton(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
