using System;
using System.Threading.Tasks;
using System.Windows;

namespace CensusManagementSystem.Windows
{
    public partial class SplashScreen : Window
    {
        private readonly string[] _statusMessages = {
            "Loading database connection...",
            "Initializing modules...",
            "Preparing authentication...",
            "Loading census data...",
            "Almost ready..."
        };

        public SplashScreen()
        {
            InitializeComponent();
        }

        public async Task ShowSplashAsync()
        {
            Show();
            for (int i = 0; i < _statusMessages.Length; i++)
            {
                StatusText.Text = _statusMessages[i];
                await Task.Delay(600);
            }
            StatusText.Text = "Ready!";
            await Task.Delay(400);
        }
    }
}
