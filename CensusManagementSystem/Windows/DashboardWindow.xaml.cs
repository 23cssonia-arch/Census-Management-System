using CensusManagementSystem.Helpers;
using CensusManagementSystem.Services;
using CensusManagementSystem.Views;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace CensusManagementSystem.Windows
{
    public partial class DashboardWindow : Window
    {
        private readonly DispatcherTimer _timer;

        public DashboardWindow()
        {
            InitializeComponent();
            LoadUserInfo();
            NavigateToDashboard();

            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += (s, e) => LblDateTime.Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy  hh:mm:ss tt");
            _timer.Start();
            LblDateTime.Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy  hh:mm:ss tt");
        }

        private void LoadUserInfo()
        {
            if (SessionManager.IsLoggedIn)
            {
                LblUser.Text = SessionManager.CurrentUser.FullName;
                LblRole.Text = SessionManager.CurrentUser.Role;
            }
        }

        private void NavigateToDashboard()
        {
            ContentFrame.Navigate(new DashboardView());
            HighlightNav("NavDashboard");
        }

        private void NavDashboard_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new DashboardView());
            HighlightNav("NavDashboard");
        }

        private void NavArea_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new AreaManagementView());
            HighlightNav("NavArea");
        }

        private void NavHousehold_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new HouseholdManagementView());
            HighlightNav("NavHousehold");
        }

        private void NavCitizen_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new CitizenManagementView());
            HighlightNav("NavCitizen");
        }

        private void NavReports_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new ReportsView());
            HighlightNav("NavReports");
        }

        private void HighlightNav(string activeName)
        {
            var buttons = new[]
            {
        NavDashboard,
        NavArea,
        NavHousehold,
        NavCitizen,
        NavReports
    };

            foreach (var btn in buttons)
            {
                if (btn == null)
                    continue;

                if (btn.Name == activeName)
                {
                    btn.Background = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#1A5276"));

                    btn.Foreground = Brushes.White;
                }
                else
                {
                    btn.Background = Brushes.Transparent;

                    btn.Foreground = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#AED6F1"));
                }
            }
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to logout?", "Logout",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                SessionManager.Logout();
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
        }
    }
}
