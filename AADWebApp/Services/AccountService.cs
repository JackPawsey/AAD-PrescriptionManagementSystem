using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;

namespace AADWebApp.Services
{
    public class AccountService
    {
        public IWebHostEnvironment WebHostEnvironment { get; }

        public AccountService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        public String Login(String Email, String Password)
        {
            String Result = "";

            return Result;
        }

        public String Logout(int AccountID)
        {
            String Result = "";

            return Result;
        }

        public IEnumerable<int> GetUsers()
        {
            IEnumerable<int> Users = null;

            return Users;
        }

        public String AddUser(String Email, String Password, String AccountType)
        {
            String Result = "";

            return Result;
        }

        public String RemoveUser(int AccountID)
        {
            String Result = "";

            return Result;
        }

        public String SetPassword(int AccountID, String Password)
        {
            String Result = "";

            return Result;
        }
    }
}
