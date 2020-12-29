using AADWebApp.Models;
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
            String Result;

            try
            {
                //GET ACCOUNTS TABLE
                //FOR OVER TABLE, CHECK IF EMAIL AND PASSWORD BOTH MATCH ROW

                if (true)
                {
                    Result = "Login succuessful";
                }
                else
                {
                    Result = "Error login failed - incorrect credentials";
                }
                
            }
            catch
            {
                Result = "Error login failed - server error";
            }

            return Result;
        }

        public String Logout(int AccountID)
        {
            String Result = "";

            //NOT SURE WE NEED THIS OR WHAT IT MIGHT NEED TO DO

            return Result;
        }

        public IEnumerable<Account> GetUsers()
        {
            IEnumerable<Account> Users;

            try
            {
                //GET ACCOUNTS TABLE
                Users = null; //*** TEMPORARY ***
            }
            catch
            {
                Users = null;
            }

            return Users;
        }

        public String AddUser(String Email, String Password, String AccountType)
        {
            String Result;

            try
            {
                //CREATE ACCOUNT TABEL ROW
                Result = "User created succuessfully";
            }
            catch
            {
                Result = "Error user not created";
            }

            return Result;
        }

        public String RemoveUser(int AccountID)
        {
            String Result;

            try
            {
                //DELETE ACCOUNT TABLE ROW
                Result = "User deleted succuessfully";
            }
            catch
            {
                Result = "Error user not deleted";
            }

            return Result;
        }

        public String SetPassword(int AccountID, String Password)
        {
            String Result;

            try
            {
                //UPDATE ACCOUNT TABLE ROW
                Result = "Account password updated succuessfully";
            }
            catch
            {
                Result = "Error account password not updated";
            }

            return Result;
        }
    }
}
