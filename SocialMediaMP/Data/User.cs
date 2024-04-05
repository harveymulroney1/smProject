using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.ComponentModel.DataAnnotations;
using static System.Console; // allows no need for Console. etc
using Microsoft.AspNetCore.Mvc;


namespace SocialMediaMP.Data
{
    public class User
    {
      //  [Key]
        public int User_ID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public DateTime DOB { get; set; }
        public string Bio { get; set; }

        public User(string UN, int UID) 
        {
            
        }
        public User()
        {
            int User_ID;
            string UserName;
            string FirstName;
            string Surname;
            string Email;

        }
        
        public User(DataRow UserRow) // use a userservice if trying to fetch all the data e.g admin page
        {
            User_ID = int.Parse(UserRow["User_ID"].ToString());
            UserName = UserRow["UserName"].ToString();
            FirstName = UserRow["FirstName"].ToString();
            Surname = UserRow["Surname"].ToString();
            Email = UserRow["EmailAddress"].ToString();
            // DOB = DateTime.Parse(UserRow["DOB"].ToString());
            // doesnt currently contain uPASS   
        }

        public User(string UserName, string UserPassword, string Email, string BackupCode) // used to create a new user in the DB 
        {
            DBConnection SocailDB = new DBConnection();
            string sql = "INSERT INTO dbo.UserAccount (UserName,UPassword,EmailAddress,Backupcode) VALUES (@UserName,@UserPassword,@Email,@BackupCode)";
            //string sql = ("INSERT INTO dbo.UserAccount " +
            //    "Set UserName = @Username" +
            //    "UPassword = @UserPassword" +
            //    "EmailAddress = @Email");
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("UserName", UserName)); // passes the values to be added to those collumns
            paras.Add(new SqlParameter("UserPassword", UserPassword));
            paras.Add(new SqlParameter("Email", Email));
            paras.Add(new SqlParameter("BackupCode", BackupCode));
            SocailDB.Insert(sql, paras);
            SocailDB.Close();

        }



        static public List<User> GetUsers()
        {
            List<User> Users = new List<User>();
            string sql = "SELECT * From UserAccount";
            DBConnection SocailDB = new DBConnection();
            DataTable results = SocailDB.Select(sql);
            foreach (DataRow r in results.Rows)
            {
                User u = new User(r);
                Users.Add(u);

            }
            return Users;
        }


        //public static void UserAlter()
        //{


        //    DBConnection SocialDB = new DBConnection();
        //    int UID = global.CurrentUser.User_ID;
        //    WriteLine("What would you like to alter 1.Username 2.Password 3.Email \n OR \n 4. Check BackupCode");
        //    int choicealter = int.Parse(ReadLine());
        //    switch (choicealter) // none of this allows for incorrect cases -> do it in Authentication??? / needs to check if it exists
        //    {
        //        case 1:
        //            List<SqlParameter> altparamsUN = new List<SqlParameter>();
        //            Write("Enter your email address: ");
        //            string EM = ReadLine();
        //            Write("Enter your password: ");
        //            string rawPW = ReadLine();
        //            string hPW = HashPassword(rawPW); // hashes password so it can be compared
        //            if (AuthenticationChecks.UserAlterCheck(EM, hPW) == true)
        //            {
        //                Write("Enter desired username: ");
        //                string newUN = ReadLine();
        //                string sqlUN = "UPDATE dbo.UserAccount SET Username = @newUN WHERE EmailAddress = @EM AND UPassword = @hPW"; // need to go back and check if its changed / if email address exists in DB with authentication
        //                altparamsUN.Add(new SqlParameter("newUN", newUN));
        //                altparamsUN.Add(new SqlParameter("EM", EM)); //fixed by declaring the scalar variables in the "" as the same as the value after it
        //                altparamsUN.Add(new SqlParameter("hPW", hPW));
        //                SocialDB.UpdateParams(sqlUN, altparamsUN);
        //                //SocialNetwork.UpdateUserUN(UID, newUN); // updates social network with newUN so everything is correct
        //                WriteLine("Updated");
        //                System.Threading.Thread.Sleep(1000);

        //            }
        //            else
        //            {
        //                Write("Incorrect Email or Password");
        //                UserAlter();
        //            }
        //            break;
        //        case 2:
        //            List<SqlParameter> altparamsPW = new List<SqlParameter>();
        //            Write("Enter email address");
        //            string UEM = ReadLine();
        //            Write("Enter your current pass");
        //            string oldPW = ReadLine();
        //            string HashedoldPW = HashPassword(oldPW);
        //            if (AuthenticationChecks.UserAlterCheck(UEM, HashedoldPW) == true) // checks if the info is correct then carrys on and update
        //            {
        //                Write("Enter new password");
        //                string newPW = ReadLine();
        //                string newHashPW = HashPassword(newPW);
        //                string sqlPW = "UPDATE dbo.UserAccount SET UPassword = @newHashPW Where UPassword = @oldPW AND EmailAddress = @UEM"; // could do with email or username check if exists on same row for oldPW
        //                altparamsPW.Add(new SqlParameter("newHashPW", newHashPW));
        //                altparamsPW.Add(new SqlParameter("oldPW", oldPW));
        //                altparamsPW.Add(new SqlParameter("UEM", UEM));
        //                SocialDB.UpdateParams(sqlPW, altparamsPW);
        //                // no need to update social network as PW is independant
        //                WriteLine("Updated");
        //                System.Threading.Thread.Sleep(1000);

        //            }

        //            else
        //            {
        //                Write("Incorrect Email or Password");
        //                UserAlter();
        //            }
        //            break;
        //        case 3:
        //            List<SqlParameter> altparamsEM = new List<SqlParameter>();
        //            Write("Current email: ");
        //            string oldEM = ReadLine();
        //            Write("Enter password");
        //            string rawUPW = ReadLine();
        //            string UPW = HashPassword(rawUPW); // hashes raw upass
        //            if (AuthenticationChecks.UserAlterCheck(oldEM, UPW) == true) //checks if the info is correct then carrys on and update 
        //            {
        //                Write("New Email: ");
        //                string newEM = ReadLine();
        //                string sqlEM = "UPDATE dbo.UserAccount SET EmailAddress = @newEM Where EmailAddress = @oldEM AND UPassword = @UPW";
        //                altparamsEM.Add(new SqlParameter("newEM", newEM));
        //                altparamsEM.Add(new SqlParameter("oldEM", oldEM));
        //                altparamsEM.Add(new SqlParameter("UPW", UPW));
        //                SocialDB.UpdateParams(sqlEM, altparamsEM);
        //                //SocialNetwork.UpdateUserEM(UID, newEM); // updates the socialnetwork with the new emailaddress so its all up to date
        //                WriteLine("Updated");
        //                System.Threading.Thread.Sleep(1000);

        //            }
        //            else
        //            {
        //                Write("Incorrect Email or Password");
        //                UserAlter();
        //            }


        //            break;
        //        case 4:
        //            List<SqlParameter> altparamsBC = new List<SqlParameter>();
        //            Write("Enter EmailAddress");
        //            string CEM = ReadLine();
        //            string rawUPass = ReadLine();
        //            string UPass = HashPassword(rawUPass);

        //            if (AuthenticationChecks.UserAlterCheck(CEM, UPass) == true) // checks if the info is correct then carrys on and update
        //            {
        //                string sqlBC = "Select Backupcode FROM dbo.UserAccount WHERE EmailAddress = @CEM AND UPassword = @UPass"; // finds backup code where this exists
        //                altparamsBC.Add(new SqlParameter("UPass", UPass));
        //                altparamsBC.Add(new SqlParameter("CEM", CEM));
        //                DataTable BC = SocialDB.SelectParameters(sqlBC, altparamsBC);
        //                if (BC != null)
        //                {
        //                   // int u = BC
        //                }
        //            }
        //            else
        //            {
        //                Write("Incorrect Email or Password");
        //                UserAlter();
        //            }
        //            break;
        //        default:

        //            break;
        //    }


        //}
        public static string BackupCodeCreator()
        {
            Random random = new Random();

            // Create a variable to store the code
            string code = "";
            for (int i = 0; i < 10; i++)
            {
                code += random.Next(1, 10); // creates a backup code of numbers ranging from 1-10
            }

            if (AuthenticationChecks.BackupCodeCheck(code) == true)
            {

                BackupCodeCreator(); // if the code exists already it calls itself and creates a new one                
            }


            // returns backupcode
            return code; // doesnt let me put in an if statement or it says not all paths return value



        }
        public static void CreateAccount()
        {
            WriteLine("Create your account");
            Write("Enter Username: ");
            string UserName = ReadLine();
            if (AuthenticationChecks.CheckUN(UserName) == true)// checking if the username already exists in DB so no duplicates
            {
                WriteLine("Please try a different username");
                System.Threading.Thread.Sleep(1000);
                CreateAccount();
            }
            Write("Enter Password: ");
            string UserPassword = ReadLine();
            var HashedPW = HashPassword(UserPassword);
            Write("Enter EmailAddress: ");
            string Email = ReadLine();
            if (AuthenticationChecks.CheckEM(Email) == true) // checking if the email already exists in DB so no duplicates - if exists starts to create account again
            {
                Write("Try diff email");
                System.Threading.Thread.Sleep(1000);
                CreateAccount();
            }

            string BackupCode = BackupCodeCreator(); // calls to create the backupcode
            //WriteLine("{0} is your BackupCode \nPlease write down to save your account \nPress Enter once written down", BackupCode); // tells user backupcode
            //ReadLine();


            //global.CurrentUser.Email = Email;
            //global.GetUID(UserName); // calls to get the uid using the username to check in the DB

            User N = new User(UserName, HashedPW, Email, BackupCode);
            //global.CurrentUser = new User(UserName,UserPassword,Email); // testing does it need to be overriden?                       

        }
        public static void Login(string UN, string rawPW)
        {
            //Write("Login:");
            //Write("Enter Username: ");
            //string UserName = ReadLine();
            //Write("Enter Password: "); // need to hash it and compare it to the DB
            //string rawUPass = ReadLine();
            string hashUPass = HashPassword(rawPW);

            if (AuthenticationChecks.Login(UN, hashUPass)) // check if it exists
            {
                //Clear();
                //WriteLine("Legit"); 
                //System.Threading.Thread.Sleep(500);
                // redirect to posts

            }
            else
            {
                int option = 0;
                WriteLine("1. Try Again | 2. Reset Password");
                try
                {
                    option = int.Parse(ReadLine());
                }
                catch (Exception)
                {
                    WriteLine("Try again");
                    System.Threading.Thread.Sleep(1000);
                    Login(null, null); // cant go back to Login
                }
                switch (option)
                {
                    case 1:
                        Login(null, null);
                        break;
                    case 2:
                        ForgotPW();
                        break;
                    default:
                        Login(null, null);
                        break;
                }
            }
            //int option = 0;
            //WriteLine("1. Try Again | 2. Reset Password");
            //try
            //{
            //    option = int.Parse(ReadLine());
            //}
            //catch (Exception)
            //{
            //    WriteLine("Try again");
            //    System.Threading.Thread.Sleep(1000);
            //    Login();
            //}
            //switch (option)
            //{
            //    case 1: Login();
            //        break;
            //    case 2:
            //        ForgotPW();
            //        break;
            //    default:
            //        Login();
            //        break;
            //}
            ReadLine();


        }

        static void ForgotPW()
        {
            List<SqlParameter> altparamsResetPW = new List<SqlParameter>();
            DBConnection SocialDB = new DBConnection();
            Write("Enter New Password");
            string newPW = ReadLine();
            Write("Backup Code:"); // could make it so it shows the username and says is this the right account?
            string Code = ReadLine();
            if (AuthenticationChecks.ForgotPW(Code) == true)
            {
                string sql = "Update dbo.UserAccount SET UPassword = @newPW Where Backupcode = @Code";
                altparamsResetPW.Add(new SqlParameter("newPW", newPW));
                altparamsResetPW.Add(new SqlParameter("Code", Code));
                SocialDB.UpdateParams(sql, altparamsResetPW);
                WriteLine("Reset");
                System.Threading.Thread.Sleep(2000);
                Login(null, null); // need to select the string and pw - not hashed atm???
            }
            else
            {
                WriteLine("Incorrect BackupCode");
                ForgotPW();
            }


        }

        public static string HashPassword(string password)
        {
            SHA256 hash = SHA256.Create();
            var passBytes = Encoding.UTF8.GetBytes(password); // only takes in array of bytes so get the bytes and save as var done as UTF8 -
                                                              // others need to use this to convert back and forth

            var hashedPass = hash.ComputeHash(passBytes); // returns array of bytes of hashed passed
            return Convert.ToHexString(hashedPass); // converting to HexString as its more secure than plain text
                      // returns that value back to where it was referenced

        }

    }
}
