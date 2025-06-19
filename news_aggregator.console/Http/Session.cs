using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.console.Http
{
    public static class Session
    {
        public static int UserId { get; private set; }
        public static string UserName { get; private set; }
        public static string Token { get; private set; }

        public static void SetUser(int userId, string userName, string token)
        {
            UserId = userId;
            UserName = userName;
            Token = token;
        }

        public static void Clear()
        {
            UserId = 0;
            UserName = string.Empty;
            Token = string.Empty;
        }

        public static bool IsLoggedIn => UserId > 0;

        public static bool IsLogoutRequested { get; set; } = false;

        public static void Logout()
        {
            IsLogoutRequested = true;
        }

        public static void Reset()
        {
            IsLogoutRequested = false;
        }
    }

}
