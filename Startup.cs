using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XUnitDemo.Extensions;
using XUnitDemo.Models;
using XUnitDemo.Services;

namespace XUnitDemo
{
    public class Startup
    {
        public static void Start()
        {
            bool logInState = false;
            UserService repo = new UserService();
            Users user = null;
            while(true)
            {
                string username = user == null ? "" : user.UserName;
                int choice = UIForm(username);
                Console.WriteLine();
                switch (choice)
                {
                    case 1:
                        if (logInState is true)
                        {
                            Console.WriteLine("Message: You have logged already!");
                            break;
                        }
                        user = LogInForm(ref logInState, repo);
                        break;
                    case 2:
                        user = RegisterForm(ref logInState, repo);
                        break;
                    case 3:
                        if(logInState is false)
                        {
                            Console.WriteLine("Message: You need to log in to use this feature!");
                            break;
                        }
                        IEnumerable<Users> userList = repo.GetUsers();
                        Console.WriteLine("All Users: ");
                        userList.LogOutList();
                        break;
                    case 4:
                        SearchForm(logInState, repo);
                        break;
                    case 5:
                        Console.Clear();
                        break;
                    case 6:
                        if(logInState is true)
                        {
                            logInState = false;
                            user = null;
                            Console.WriteLine("Message: Logged Out Successfully");
                        }
                        else
                        {
                            Console.WriteLine("Message: You have not logged yet!");
                        }
                        break;
                    case 7:
                        break;
                    default:
                        Console.WriteLine("Message: Unknown Command!");
                        break;
                }
                if(choice == 7)
                {
                    Console.WriteLine("Message: Good bye!");
                    break;
                }
                Console.WriteLine();
            }
        }
        public static Users InputInfo()
        {
            Console.Write("Username: ");
            string username = Console.ReadLine();
            Console.Write("Password: ");
            string password = Console.ReadLine();
            return new()
            {
                UserName = username,
                Password = password
            };
        }
        public static int UIForm(string username)
        {
            int choice;
            username = username.Equals("") ? "Your" : $"{username}'s";
            Console.WriteLine("1 - Login");
            Console.WriteLine("2 - Register");
            Console.WriteLine("3 - View All Users");
            Console.WriteLine("4 - Search User");
            Console.WriteLine("5 - Clear Console");
            Console.WriteLine("6 - LogOut");
            Console.WriteLine("7 - Close");
            Console.Write($"{username} Choice: ");
            try
            {
                choice = Int32.Parse(Console.ReadLine());
                return choice;
            }
            catch (Exception)
            {
                return 99;
            }
        }
        private static Users LogInForm(ref bool logInState, UserService repo)
        {
            Users temp = InputInfo();
            try
            {
                logInState = repo.LogIn(temp.UserName, temp.Password);
            }
            catch (KeyNotFoundException e)
            {
                Console.WriteLine("Message: " + e.Message);
                logInState = false;
                return null;
            }
            if (logInState is true)
            {
                Console.WriteLine($"Message: Wellcome back {temp.UserName}");
                return temp;
            }
            else
            {
                Console.WriteLine("Message: Wrong password");
                return null;
            }
        }
        private static Users RegisterForm(ref bool logInState, UserService repo)
        {
            Console.WriteLine("**Before typing** ");
            Console.WriteLine("------> Username Rules: \n" + UserService.UsernameRules);
            Console.WriteLine("------> Password Rules: \n" + UserService.PasswordRules);
            Users temp = InputInfo();
            try
            {
                logInState = repo.Register(temp.UserName, temp.Password);
            }
            catch (Exception e)
            {
                Console.WriteLine("Message: " + e.Message);
                logInState = false;
                return null;
            }
            if (logInState is true)
            {
                Console.WriteLine($"Wellcome here, {temp.UserName}");
                return temp;
            }
            else
                return null;
        }
        private static void SearchForm(bool logInState, UserService repo)
        {
            Console.Write("Search: ");
            string search = Console.ReadLine();
            Users searchUser = repo.GetSpecifyUser(search);
            if (searchUser != null)
            {
                Console.WriteLine("----> Result: ");
                if (logInState is true)
                {
                    searchUser.LogOut();
                }
                else
                {
                    searchUser.Password = "Log In to view this field!";
                    searchUser.LogOut();
                }
            }
            else
            {
                Console.WriteLine("Message: No result!");
            }
        }
    }
}
