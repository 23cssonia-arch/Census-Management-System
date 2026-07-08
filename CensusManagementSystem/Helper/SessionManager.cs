using CensusManagementSystem.Models;

namespace CensusManagementSystem.Helpers
{
    public static class SessionManager
    {
        private static User _currentUser;

        public static User CurrentUser
        {
            get { return _currentUser; }
        }

        public static bool IsLoggedIn
        {
            get { return _currentUser != null; }
        }

        public static void Login(User user) => _currentUser = user;

        public static void Logout() => _currentUser = null;
    }
}
