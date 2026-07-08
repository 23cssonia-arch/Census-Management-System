using CensusManagementSystem.Helpers;
using CensusManagementSystem.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CensusManagementSystem.Windows
{
    public partial class RegisterWindow : Window
    {
        private readonly AuthService _authService = new AuthService();

        public RegisterWindow()
        {
            InitializeComponent();
            TxtFullName.Focus();
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            LblError.Visibility = Visibility.Collapsed;

            string fullName = TxtFullName.Text.Trim();
            string username = TxtUsername.Text.Trim();
            string password = TxtPassword.Password;
            string confirmPassword = TxtConfirmPassword.Password;
            string role = ((ComboBoxItem)CmbRole.SelectedItem).Content.ToString();

            if (!ValidationHelper.ValidateRequiredFields(
                (fullName, "Full Name"),
                (username, "Username"),
                (password, "Password"),
                (confirmPassword, "Confirm Password")))
                return;

            if (password.Length < 6)
            {
                LblError.Text = "Password must be at least 6 characters.";
                LblError.Visibility = Visibility.Visible;
                return;
            }

            if (password != confirmPassword)
            {
                LblError.Text = "Passwords do not match.";
                LblError.Visibility = Visibility.Visible;
                return;
            }

            if (_authService.IsUsernameExists(username))
            {
                LblError.Text = "Username already exists.";
                LblError.Visibility = Visibility.Visible;
                return;
            }

            bool success = _authService.Register(username, password, fullName, role);
            if (success)
            {
                MessageBox.Show("Account created successfully! Please login.", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
            else
            {
                LblError.Text = "Failed to create account. Please try again.";
                LblError.Visibility = Visibility.Visible;
            }
        }

        private void LblLogin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}
