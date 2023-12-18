using DentalBook.DAOs;
using DentalBook.DTOs;
using DentalBook.Themes;
using MaterialDesignThemes.Wpf;
using Org.BouncyCastle.Security.Certificates;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DentalBook
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private string username;
        private readonly PaletteHelper paletteHelper = new PaletteHelper();
        private List<Dentist> dentists = DentistDAO.GetAllDentists();
        private List<Assistant> assistants = AssistantDAO.GetAllAssistants();
        public AdminWindow(string username)
        {
            this.username = username;
            InitializeComponent();
            UsernameTextBlock.Text = username;
            AddDentistsInDataGrid();
            AddAssistantsInDataGrid();
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
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView listView = sender as ListView;
            ListViewItem listViewItem = listView.SelectedItem as ListViewItem;
            if (listViewItem != null)
            {

                switch (listViewItem.Name)
                {
                    case "DentistsItem":
                        DentistsGrid.Visibility = Visibility.Visible;
                        AssistantsGrid.Visibility = Visibility.Collapsed;
                        break;
                    case "AssistantsItem":
                        AssistantsGrid.Visibility = Visibility.Visible;
                        DentistsGrid.Visibility = Visibility.Collapsed;
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

        private void AddDentistsInDataGrid()
        {
            DentistsDataGrid.ItemsSource = dentists;
        }

        private void AddAssistantsInDataGrid()
        {
            AssistantsDataGrid.ItemsSource = assistants;
        }

        private void RefreshDentists()
        {
            dentists = DentistDAO.GetAllDentists();
            DentistsDataGrid.ItemsSource = null;
            AddDentistsInDataGrid();
        }
        private void RefreshAssistants()
        {
            assistants = AssistantDAO.GetAllAssistants();
            AssistantsDataGrid.ItemsSource = null;
            AddAssistantsInDataGrid();
        }

        private void SaveThemeButton(object sender, RoutedEventArgs e)
        {
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

        private void UpdateDentistData(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if (button != null)
            {
                Dentist dentist = button.DataContext as Dentist;
                Schedule scheduleEvenWeeks = ScheduleDAO.Get(dentist.Id, dentist.ShiftEvenWeeks);
                UpdateDentistWindow updateDentistWindow = null;
                if (string.Equals(dentist.ShiftEvenWeeks, dentist.ShiftOddWeeks))
                {
                    updateDentistWindow = new UpdateDentistWindow(dentist,scheduleEvenWeeks, null);
                }
                else
                {
                    Schedule scheduleOddWeeks = ScheduleDAO.Get(dentist.Id, dentist.ShiftOddWeeks);
                    updateDentistWindow = new UpdateDentistWindow(dentist, scheduleEvenWeeks, scheduleOddWeeks);
                }
                updateDentistWindow.ShowDialog();
                updateDentistWindow.Close();
                RefreshDentists();
            }
        }

        private void DeleteDentist(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if (button != null)
            {
                Dentist dentist = button.DataContext as Dentist;
                DentistDAO.DeleteDentist(dentist.Id);
                dentists.Remove(dentist);
                DentistsDataGrid.ItemsSource = null;
                DentistsDataGrid.ItemsSource = dentists;
                MessageBox.Show("Dentist DELETED SUCCESSFULLY!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            }
        }

        private void ShowCreateDentistGrid(object sender, RoutedEventArgs e)
        {
            AddDentistWindow addDentistWindow = new AddDentistWindow();
            addDentistWindow.ShowDialog();
            
            RefreshDentists();
        }


        private void ShowCreateAssistantGrid(object sender, RoutedEventArgs e)
        {
            AddAssistantWindow addAssistantWindow = new AddAssistantWindow(dentists);
            addAssistantWindow.ShowDialog();
            
            RefreshAssistants();
        }

        private void DeleteAssistantButton(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if (button != null)
            {
                Assistant assistant = button.DataContext as Assistant;
                AssistantDAO.DeleteAssistant(assistant.Username);
                assistants.Remove(assistant);
                AssistantsDataGrid.ItemsSource = null;
                AssistantsDataGrid.ItemsSource = assistants;
                MessageBox.Show("Assistant DELETED SUCCESSFULLY!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            }
        }
        private void UpdateAssistantDataButton(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if (button != null)
            {
                Assistant assistant = button.DataContext as Assistant;
                Dictionary<int, string> dentistsIdsNames = DentistHasAssistantDAO.GetAllDentists(assistant.Username);
                UpdateAssistantWindow updateAssistantWindow = new UpdateAssistantWindow(assistant, dentistsIdsNames.Keys.ToList(), dentists);
                updateAssistantWindow.ShowDialog();
                updateAssistantWindow.Close();
                RefreshAssistants();
            }
        }
    }
}
