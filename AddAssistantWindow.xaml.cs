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
    /// Interaction logic for AddAssistantWindow.xaml
    /// </summary>
    public partial class AddAssistantWindow : Window
    {
        private List<Dentist> dentists;
        public AddAssistantWindow(List<Dentist> dentists)
        {
            InitializeComponent();
            this.dentists = dentists;
            AddDentistsInComboBox();
        }

        private void CancelButton(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddDentistsInComboBox()
        {
            foreach (Dentist dentist in dentists)
            {
                DentistsCheckBoxListBox.Items.Add(new CheckBox { Content = dentist.FullName, Tag = dentist.Id });
            }
        }

        private void CreateAssistantButton(object sender, RoutedEventArgs e)
        {
            string assistantUsername = AssistantUsernameTextBox.Text;
            string assistantPassword = AssistantPasswordBox.Password;
            List<int> assistantDentistsIds = new List<int>();


            foreach (CheckBox checkBox in DentistsCheckBoxListBox.Items)
            {
                if (checkBox.IsChecked ?? false)
                {
                    assistantDentistsIds.Add((int)checkBox.Tag);
                    checkBox.IsChecked = false;
                }
            }

            if (string.IsNullOrEmpty(assistantUsername) || string.IsNullOrEmpty(assistantPassword) || assistantDentistsIds.Count == 0)
            {
                MessageBox.Show("Please fill all required fields.", "Create Assistant Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            AssistantDAO.AddAssistant(new Assistant(assistantUsername, assistantPassword));
            DentistHasAssistantDAO.Add(new DentistHasAssistant(assistantDentistsIds, assistantUsername));
            MessageBox.Show("Assistant ADDED SUCCESSFULLY!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
