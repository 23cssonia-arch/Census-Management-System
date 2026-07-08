using System.Windows;

namespace CensusManagementSystem
{
    public partial class App : Application
    {
        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            var splash = new CensusManagementSystem.Windows.SplashScreen();

            await splash.ShowSplashAsync();

            var registerWindow = new CensusManagementSystem.Windows.RegisterWindow();
            registerWindow.Show();

            splash.Close();
        }
    }
}