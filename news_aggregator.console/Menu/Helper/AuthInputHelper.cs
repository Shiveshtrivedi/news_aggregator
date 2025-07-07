using news_aggregator.console.Models;
using System;
using System.Text;

namespace news_aggregator.console.Menu.Handler
{
    public static class AuthInputHelper
    {
        public static (string email, string password) ReadLoginCredentials()
        {
            Console.Write("Email: ");
            string email = Console.ReadLine()!;
            Console.Write("Password: ");
            string password = ReadPassword();
            return (email, password);
        }

        public static UserDto ReadSignupDetails()
        {
            Console.Write("Name: ");
            string name = Console.ReadLine()!;
            Console.Write("Email: ");
            string email = Console.ReadLine()!;
            Console.Write("Password: ");
            string password = Console.ReadLine()!;

            return new UserDto
            {
                UserName = name,
                Email = email,
                Password = password
            };
        }

        public static string ReadPassword()
        {
            StringBuilder passwordBuilder = new StringBuilder();
            ConsoleKeyInfo keyInfo;

            do
            {
                keyInfo = Console.ReadKey(intercept: true);

                if (keyInfo.Key == ConsoleKey.Backspace && passwordBuilder.Length > 0)
                {
                    Console.Write("\b \b");
                    passwordBuilder.Remove(passwordBuilder.Length - 1, 1);
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    passwordBuilder.Append(keyInfo.KeyChar);
                    Console.Write("*");
                }
            } while (keyInfo.Key != ConsoleKey.Enter);

            Console.WriteLine();
            return passwordBuilder.ToString();
        }
    }
}
