using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using XUnitDemo.Extensions;
using XUnitDemo.Models;

namespace XUnitDemo.Services
{
    public class UserService
    {
        //((?=.*\d)(?=.*[a-zA-Z])(?!.*[\W]))
        public const string UsernameRules = 
         "* The length is 3 <= x <= 20 \n" + 
         "* Contain as least 1 UPPER or lower \n" + 
         "* Contain as least 1 digit \n" + 
         "* Allow only underscore char";
        public const string PasswordRules = 
         "* The length is 8 <= x <= 20 \n" + 
         "* Contain as least 1 UPPER \n" +
         "* Contain as least 1 lower \n" +
         "* Contain as least 1 digit \n" +
         "* Contain as least 1 spec char in this list: @ # ! $ %";
        public IEnumerable<Users> GetUsers()
        {
            // get all users
            var users = ConvertJsonToModel.ConvertUsersSync<Users>();
            return users;
        }

        public Users GetSpecifyUser(string username)
        {
            // get specify user by providing username
            var users = ConvertJsonToModel.ConvertUsersSync<Users>();
            if(users == null)
            {
                return null;
            }
            var user = users.Where(u => u.UserName.Equals(username)).FirstOrDefault();
            //user.LogOut();
            return user;
        }

        public bool LogIn(string username, string password)
        {
            var user = GetSpecifyUser(username);
            if(user == null)
            {
                throw new KeyNotFoundException("User is not exist!");
                //return false;
            }
            if(user.Password.Equals(password.MD5Hash()))
            {
                return true;
            }
            return false;
        }

        public bool Register(string username, string password)
        {
            if(IsUsernameValid(username) is false)
            {
                throw new Exception("Username is not match with what we required!");
                //return false;
            }
            if(IsPasswordValid(password) is false)
            {
                throw new Exception("Password is not match with what we required!");
                //return false;
            }
            if (GetSpecifyUser(username) != null)
            {
                throw new Exception("User existed!");
                //return false;
            }
            var newUser = new Users()
            {
                UserName = username,
                Password = password.MD5Hash()
            };
            try
            {
                var path = ConvertJsonToModel.UserFilePath;
                var create = WriteModelToFiles.AppendToExisted<Users>(newUser, path);
                return true;
            }
            catch (Exception)
            {
                throw new SystemException("Error in creating user!");
            }
        }
        public bool IsUsernameValid(string username)
        {
            if (username.Length < 3 || username.Length > 20)
                return false;
            Regex usernamePolicyExpression = new Regex(@"((?=.*\d)(?=.*[a-zA-Z])(?!.*[\W]))");
            return usernamePolicyExpression.IsMatch(username);
        }
        public bool IsPasswordValid(string password)
        {
            if (password.Length < 8 || password.Length > 20)
                return false;
            Regex passwordPolicyExpression = new Regex(@"((?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#!$%]))");
            return passwordPolicyExpression.IsMatch(password);
        }
    }
}
