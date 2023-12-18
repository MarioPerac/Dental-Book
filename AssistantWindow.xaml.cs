using DentalBook.DAOs;
using DentalBook.DTOs;
using DentalBook.Themes;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading;
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
    /// Interaction logic for AssistantWindow.xaml
    /// </summary>
    /// 
    public partial class AssistantWindow : Window
    {
        private string username;
        private readonly PaletteHelper paletteHelper = new PaletteHelper();
        public AssistantWindow(string username)
        {
            this.username = username;
            InitializeComponent();
            UsernameTextBlock.Text = username;
            setDentists();

            ThemesComboBox.Items.Add("Dark");
            ThemesComboBox.Items.Add("Medium");
            ThemesComboBox.Items.Add("Light");
            ApplyDefaultTheme();
        }

        private void ApplyDefaultTheme()
        {

            string userKey = username;
            string userValue = "Light";

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            if (config.AppSettings.Settings[userKey] == null)
            {
                config.AppSettings.Settings.Add(userKey, userValue);
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            else
            {
                userValue = config.AppSettings.Settings[userKey].Value;
            }

            SetTheme(userValue);
            ThemesComboBox.SelectedItem = userValue;
        }



        private void setDentists()
        {
            Dictionary<int, string> dentists = DentistHasAssistantDAO.GetAllDentists(username);

            foreach (var dentist in dentists)
            {
                DentistsComboBox.Items.Add(new KeyValuePair<int, string>(dentist.Key, dentist.Value));
                DentistsCalendarComboBox.Items.Add(new KeyValuePair<int, string>(dentist.Key, dentist.Value));
                DentistsInterventionComboBox.Items.Add(new KeyValuePair<int, string>(dentist.Key, dentist.Value));
            }
        }
        private void AssistantMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView listView = sender as ListView;
            ListViewItem listViewItem = listView.SelectedItem as ListViewItem;
            if (listViewItem != null)
            {

                switch (listViewItem.Name)
                {
                    case "PatientReservationItem":

                        PatientReservationGrid.Visibility = Visibility.Visible;
                        DentalCalendarGrid.Visibility = Visibility.Collapsed;
                        CancelGrid.Visibility = Visibility.Collapsed;
                        DentistsInterventionComboBox.SelectedItem = null;
                        CancelInterventionsDataGrid.ItemsSource = null;
                        break;
                    case "DentalCalendarItem":
                        PatientReservationGrid.Visibility = Visibility.Collapsed;
                        DentalCalendarGrid.Visibility = Visibility.Visible;
                        CancelGrid.Visibility = Visibility.Collapsed;
                        DentistsInterventionComboBox.SelectedItem = null;
                        CancelInterventionsDataGrid.ItemsSource = null;
                        break;
                    case "DeleteItem":
                        PatientReservationGrid.Visibility = Visibility.Collapsed;
                        DentalCalendarGrid.Visibility = Visibility.Collapsed;
                        CancelGrid.Visibility = Visibility.Visible;
                        break;
                }
            }
        }

        private void LogOutButton(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            Close();
        }

        private void ResetControls()
        {
            PatientNameSurnameTextBox.Text = null;
            PatientPhoneTextBox.Text = null;
            TermDate.SelectedDate = null;
            DentistsComboBox.SelectedItem = null;
            TermsComboBox.Items.Clear();

        }

        private void ReservateButton(object sender, RoutedEventArgs e)
        {

            string fullName = PatientNameSurnameTextBox.Text;
            string phoneNumber = PatientPhoneTextBox.Text;

            if (string.IsNullOrEmpty(fullName) || TermDate.SelectedDate == null || TermsComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please fill all required fields.", "Reservation Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            else
            {

                DateTime date = TermDate.SelectedDate.Value;
                int termId = ((KeyValuePair<int, string>)TermsComboBox.SelectedItem).Key;

                int patientId = PatientDAO.PatientCheck(fullName);

                if (patientId == -1)
                {
                    patientId = PatientDAO.Add(new Patient(fullName, phoneNumber));
                }

                InterventionDAO.AddRow(termId, date, username, patientId);
                MessageBox.Show("Intervention ADDED SUCCESSFULLY!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            ResetControls();
        }

        private void SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TermDate.SelectedDate.HasValue)
            {

                TermsComboBox.Items.Clear();
                DateTime date = TermDate.SelectedDate.Value;

                if (DentistsComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Please before choosing a date, choose a DENTIST.", "Reservation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    ResetControls();
                    return;
                }

                int dentistId = ((KeyValuePair<int, string>)DentistsComboBox.SelectedItem).Key;
                string shift = DentistDAO.GetShift(date, dentistId);
                Dictionary<int, string> terms = TermDAO.GetTerms(dentistId, date, shift);

                foreach (var term in terms)
                {
                    TermsComboBox.Items.Add(new KeyValuePair<int, string>(term.Key, term.Value));
                }
            }
        }

        private void ShowInterventionButton(object sender, RoutedEventArgs e)
        {

            if (DentistsCalendarComboBox.SelectedItem == null || CalendarDate.SelectedDate == null)
            {
                MessageBox.Show("Please fill all required fields.", "Interventions Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }
            int dentistId = ((KeyValuePair<int, string>)DentistsCalendarComboBox.SelectedItem).Key;
            DateTime date = CalendarDate.SelectedDate.Value;
            List<ScheduledIntervention> interventions = InterventionDAO.ScheduledInterventions(dentistId, date, username);
            CalendarDataGrid.ItemsSource = interventions;
        }

        private void CancelInterventionButton(object sender, RoutedEventArgs e)
        {
            if (CancelInterventionsDataGrid.ItemsSource == null)
            {
                return;
            }

            List<ScheduledIntervention> interventions = (List<ScheduledIntervention>)CancelInterventionsDataGrid.ItemsSource;
            for (int i = 0; i < interventions.Count; i++)
            {
                ScheduledIntervention intervention = interventions[i];
                if (intervention.IsSelected)
                {
                    InterventionDAO.Delete(intervention.TermId, intervention.Date);
                    MessageBox.Show("Intervention/s CANCELED SUCCESSFULLY!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    interventions.RemoveAt(i);
                    CancelInterventionsDataGrid.ItemsSource = null;
                    CancelInterventionsDataGrid.ItemsSource = interventions;
                }
            }

        }

        private void DentistsInterventionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DentistsInterventionComboBox.SelectedItem != null)
            {
                int dentistId = ((KeyValuePair<int, string>)DentistsInterventionComboBox.SelectedItem).Key;

                List<ScheduledIntervention> interventions = InterventionDAO.ScheduledInterventions(dentistId, username);
                CancelInterventionsDataGrid.ItemsSource = interventions;
            }
        }

        private void SaveThemeButton(object sender, RoutedEventArgs e)
        {
            if (ThemesComboBox.SelectedItem == null)
            {
                MessageBox.Show("Theme is not selected.", "Theme Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string selectedTheme = ThemesComboBox.SelectedItem.ToString();
            SetTheme(selectedTheme);
        }

        private void SetTheme(string newTheme)
        {
            ITheme theme = paletteHelper.GetTheme();
            Theme customTheme = new Theme();
            if (newTheme == "Dark")
            {
                theme.SetBaseTheme(new DarkTheme());


            }
            else if (newTheme == "Light")
            {
                theme.SetBaseTheme(new LightTheme());
            }
            else if (newTheme == "Medium")
            {
                theme.SetBaseTheme(new MediumTheme());
            }

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings[username].Value = newTheme;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            paletteHelper.SetTheme(theme);
        }
    }


}
