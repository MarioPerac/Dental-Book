using DentalBook.DAOs;
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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();

        }

        private void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (AssistantDAO.IsAssistant(username, password))
            {
                AssistantWindow assistantWindow = new AssistantWindow(username);
                assistantWindow.Show();
                Close();
            }
            else if (AdminDAO.IsAdmin(username, password))
            {
                AdminWindow adminWindow = new AdminWindow(username);
                adminWindow.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Invalid Username or Password.", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
