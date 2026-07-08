using System.Windows;
using System.Windows.Input;
using CensusManagementSystem.Helpers;
using CensusManagementSystem.Services;

namespace CensusManagementSystem.Windows
{
    public partial class LoginWindow : Window
    {
        private readonly AuthService _authService = new AuthService();

        public LoginWindow()
        {
            InitializeComponent();
            TxtUsername.Focus();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            LblError.Visibility = Visibility.Collapsed;

            string username = TxtUsername.Text.Trim();
            string password = TxtPassword.Password;

            if (!ValidationHelper.ValidateRequiredFields(
                (username, "Username"),
                (password, "Password")))
                return;

            var user = _authService.Login(username, password);
            if (user == null)
            {
                LblError.Text = "Invalid username or password.";
                LblError.Visibility = Visibility.Visible;
                return;
            }

            SessionManager.Login(user);

            var dashboard = new DashboardWindow();
            dashboard.Show();
            this.Close();
        }

        private void LblRegister_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var registerWindow = new RegisterWindow();
            registerWindow.Show();
            this.Close();
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                BtnLogin_Click(sender, null);
        }
    }
}
