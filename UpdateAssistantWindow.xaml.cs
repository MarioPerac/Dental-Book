using System;
using DentalBook.DTOs;
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
using DentalBook.DAOs;

namespace DentalBook
{
    /// <summary>
    /// Interaction logic for UpdateAssistantWindow.xaml
    /// </summary>
    public partial class UpdateAssistantWindow : Window
    {
        Assistant assistant;
        List<int> assistantDentistsIds;
        List<Dentist> allDentists;
        public UpdateAssistantWindow(Assistant assistant, List<int> assistantDentistsIds, List<Dentist> allDentists)
        {
            InitializeComponent();
            this.assistant = assistant;
            this.assistantDentistsIds = assistantDentistsIds;
            this.allDentists = allDentists;
            AssistantUsernameTextBox.Text = assistant.Username;
            AssistantPasswordBox.Password = assistant.Password;
            AddDentistsInComboBox();


        }

        private void AddDentistsInComboBox()
        {
            foreach (Dentist dentist in allDentists)
            {
                bool isChecked = assistantDentistsIds.Contains(dentist.Id);
                CheckBox checkBox = new CheckBox { Content = dentist.FullName, IsChecked=isChecked, Tag = dentist.Id };
                DentistsCheckBoxListBox.Items.Add(checkBox);
            }
        }

        private void UpdateDentistsComboBox()
        {
            foreach (CheckBox checkBox in DentistsCheckBoxListBox.Items)
            {
                checkBox.IsChecked = assistantDentistsIds.Contains((int)checkBox.Tag); 
            }
        }
        private void UpdateAssistantButton(object sender, RoutedEventArgs e)
        {
            string password = AssistantPasswordBox.Password;
            bool updated = false;
            if (!string.Equals(password, assistant.Password))
            {
                AssistantDAO.Update(new Assistant(assistant.Username, password));
                assistant.Password = password;
                updated = true;
            }

            List<int> currentAssistantDentistsIds = new List<int>();


            foreach (CheckBox checkBox in DentistsCheckBoxListBox.Items)
            {
                if (checkBox.IsChecked ?? false)
                {
                    currentAssistantDentistsIds.Add((int)checkBox.Tag);
                    checkBox.IsChecked = false;
                }
            }

            if (!currentAssistantDentistsIds.SequenceEqual(assistantDentistsIds))
            {
                DentistHasAssistantDAO.Delete(assistant.Username);
                DentistHasAssistantDAO.Add(new DentistHasAssistant(currentAssistantDentistsIds, assistant.Username));
                assistantDentistsIds = null;
                assistantDentistsIds = currentAssistantDentistsIds;
                UpdateDentistsComboBox();
                if (!updated)
                {
                    updated = true;
                }
            }

            if (updated)
            {
                MessageBox.Show("Assistant UPDATED SUCCESSFULLY!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("No changes made. Assistant NOT UPDATED", "Update Assistant Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void CancelButton(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
