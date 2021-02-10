using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace myApp
{
    class Program
    {
        /**
        * To test the program, execute the command line
        *  put in path of the file EX: "C:\Users\ROG Strix\source\repos\Assignment\emails.csv"
        */
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Email Program.");
            Console.WriteLine();
            Console.Write("Enter the path of CSV file: ");
            string csvPath = Console.ReadLine();
            if (!File.Exists(csvPath))
            {
                Console.WriteLine("The path of CSV file does not exist.");
                return;
            }

            List<User> users = readCsvFile(csvPath);
            Dictionary<string, List<User>> emailList = validateEmails(users);
            foreach (KeyValuePair<string, List<User>> emailCheck in emailList)
            {
                Console.WriteLine("Validation = {0}", emailCheck.Key);
                foreach (User user in emailCheck.Value)
                {
                    Console.WriteLine("FirstName = {0}, LastName = {1}, Email = {2}"
                        , user.FirstName, user.LastName, user.Email);
                }
                Console.WriteLine();
            }

        }

        /**
        *  Read content of CSV file by CSV file path.
        *  Return list of users object with content of CSV file.
        * 
        * @param csvPath: path of CSV file.
        * @return List<User>: records in CSV file.
        */
        private static List<User> readCsvFile(string csvPath)
        {
            List<User> users = new List<User>();
            string[] lines = File.ReadAllLines(csvPath);
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] columns = line.Split(',');
                User user = new User
                {
                    FirstName = columns[0],
                    LastName = columns[1],
                    Email = columns[2]
                };
                users.Add(user);
            }
            return users;
        }

        /**
        *  validate emails by list of User objects
        *
        * @param users: list of User objects
        * @return Dictionary<string, List<User>>: dictionary of valid/invalid 
        *                                           and list of User objects
        *
        */
        private static Dictionary<string, List<User>> validateEmails(List<User> users)
        {
            Dictionary<string, List<User>> emailStatistics =
                new Dictionary<string, List<User>>();
            List<User> validEmails = new List<User>();
            List<User> invalidEmails = new List<User>();
            Regex regex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                RegexOptions.CultureInvariant | RegexOptions.Singleline);
            foreach (User user in users)
            {
                var email = user.Email;
                if (regex.IsMatch(email))
                {
                    validEmails.Add(user);
                }
                else
                {
                    invalidEmails.Add(user);
                }
            }
            emailStatistics.Add("valid", validEmails);
            emailStatistics.Add("invalid", invalidEmails);
            return emailStatistics;
        }
    }
}
