using System.Collections.Generic;
using AADWebApp.Models;
using Microsoft.AspNetCore.Hosting;

namespace AADWebApp.Services
{
    public class AccountService
    {
        public string Login(string email, string password)
        {
            string result;

            try
            {
                //GET ACCOUNTS TABLE
                //FOR OVER TABLE, CHECK IF EMAIL AND PASSWORD BOTH MATCH ROW

                if (true)
                    result = "Login successful";
                else
                    result = "Error login failed - incorrect credentials";
            }
            catch
            {
                result = "Error login failed - server error";
            }

            return result;
        }

        public string Logout(int accountId)
        {
            var result = "";

            //NOT SURE WE NEED THIS OR WHAT IT MIGHT NEED TO DO

            return result;
        }

        public IEnumerable<Account> GetUsers()
        {
            IEnumerable<Account> users;

            try
            {
                //GET ACCOUNTS TABLE
                users = null; //*** TEMPORARY ***
            }
            catch
            {
                users = null;
            }

            return users;
        }

        public string AddUser(string email, string password, string accountType)
        {
            string result;

            try
            {
                //CREATE ACCOUNT TABLE ROW
                result = "User created successfully";
            }
            catch
            {
                result = "Error user not created";
            }

            return result;
        }

        public string RemoveUser(int accountId)
        {
            string result;

            try
            {
                //DELETE ACCOUNT TABLE ROW
                result = "User deleted successfully";
            }
            catch
            {
                result = "Error user not deleted";
            }

            return result;
        }

        public string SetPassword(int accountId, string password)
        {
            string result;

            try
            {
                //UPDATE ACCOUNT TABLE ROW
                result = "Account password updated successfully";
            }
            catch
            {
                result = "Error account password not updated";
            }

            return result;
        }
    }
}