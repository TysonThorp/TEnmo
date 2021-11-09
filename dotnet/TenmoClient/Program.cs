using System;
using System.Collections.Generic;
using TenmoClient.Models;
using TenmoServer.Models;
using Account = TenmoClient.Models.Account;
using PastTransfer = TenmoClient.Models.PastTransfer;
using PendingTransfer = TenmoClient.Models.PendingTransfer;


namespace TenmoClient
{
    class Program
    {
        private static readonly ConsoleService consoleService = new ConsoleService();
        private static readonly AuthService authService = new AuthService();
        private static readonly TransferService transferService = new TransferService();

        static void Main(string[] args)
        {
            Run();
        }

        private static void Run()
        {
            while(true)
            {
                int loginRegister = -1;
                while (loginRegister != 1 && loginRegister != 2)
                {
                    Console.WriteLine("Welcome to TEnmo!");
                    Console.WriteLine("1: Login");
                    Console.WriteLine("2: Register");
                    Console.Write("Please choose an option: ");

                    if (!int.TryParse(Console.ReadLine(), out loginRegister))
                    {
                        Console.WriteLine("Invalid input. Please enter only a number.");
                    }
                    else if (loginRegister == 1)
                    {
                        while (!UserService.IsLoggedIn()) //will keep looping until user is logged in
                        {
                            LoginUser loginUser = consoleService.PromptForLogin();
                            ApiUser user = authService.Login(loginUser);
                            if (user != null)
                            {
                                UserService.SetLogin(user);
                            }
                        }
                    }
                    else if (loginRegister == 2)
                    {
                        bool isRegistered = false;
                        while (!isRegistered) //will keep looping until user is registered
                        {
                            LoginUser registerUser = consoleService.PromptForLogin();
                            isRegistered = authService.Register(registerUser);
                            if (isRegistered)
                            {
                                Console.WriteLine("");
                                Console.WriteLine("Registration successful. You can now log in.");
                                loginRegister = -1; //reset outer loop to allow choice for login
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid selection.");
                    }
                }

                MenuSelection();
            }
        }

        private static void MenuSelection()
        {
            int menuSelection = -1;
            while (menuSelection != 0)
            {
                string logInOut = UserService.IsLoggedIn() ? "Log out" : "Log in";

                Console.WriteLine("");
                Console.WriteLine("Welcome to TEnmo! Please make a selection: ");
                Console.WriteLine("1: View your current balance");
                Console.WriteLine("2: View your past transfers");
                Console.WriteLine("3: View your pending requests");
                Console.WriteLine("4: Send TE bucks");
                Console.WriteLine("5: Request TE bucks");
                Console.WriteLine("6: Log in as different user");
                Console.WriteLine("0: Exit");
                Console.WriteLine("---------");
                Console.Write("Please choose an option: ");

                if (!int.TryParse(Console.ReadLine(), out menuSelection))
                {
                    Console.WriteLine("Invalid input. Please enter only a number.");
                }
                else if (menuSelection == 1)
                {
                    try
                    {
                        decimal? balance = authService.GetBalance();
                        if (balance != null)
                        {

                            Console.WriteLine("Your balance is $" + balance);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                
            }
                else if (menuSelection == 2)
                {
                    Console.WriteLine("------------------------------");
                    Console.WriteLine("Transfers");
                    Console.WriteLine("ID         From/To            Amount");
                    Console.WriteLine("------------------------------");


                    List<PastTransfer> pastTransfers = new List<PastTransfer>(transferService.ViewPastTransfers());

                    foreach (PastTransfer transfer in pastTransfers)
                    {
                        

                        if (transfer.FromName == UserService.GetUserName())
                        {
                            Console.WriteLine($"{transfer.TransferId}       To: {transfer.ToName}         ${transfer.Amount}");
                        }
                        else if (transfer.ToName == UserService.GetUserName())
                        {
                            Console.WriteLine($"{transfer.TransferId}       From: {transfer.FromName}         ${transfer.Amount}");
                        }
                        
                        
                   
                    }
                    Console.WriteLine("Please enter transfer ID to view details (0 to cancel)");
                    
                }
                else if (menuSelection == 3)
                {
                    Console.WriteLine("----------------------------------------");
                    Console.WriteLine("Pending Transfers");
                    Console.WriteLine("ID            To              Amount");
                    Console.WriteLine("----------------------------------------");

                    List<PendingTransfer> pending = new List<PendingTransfer>(transferService.ViewPendingTransfer());

                    foreach (PendingTransfer transfer in pending)
                    {
                        if (transfer.FromName != UserService.GetUserName())
                        {
                            Console.WriteLine($"{transfer.TransferId}       To: {transfer.FromName}         ${transfer.Amount}");
                        }
                    }
                    Console.WriteLine("Please enter transfer ID to approve/reject (0 to cancel):");
                }
                else if (menuSelection == 4)
                {
                    Console.WriteLine("----------------------------------------");
                    Console.WriteLine("Users");
                    Console.WriteLine("ID            Name");
                    Console.WriteLine("----------------------------------------");

                    

                    List<ApiUser> userIds = new List<ApiUser>(UserService.GetUserId());

                    foreach (ApiUser user in userIds)
                    {
                        Console.WriteLine(user.UserId);
                    }

                    //if (transfer.FromName == UserService.GetUserName())
                    //{
                    //    Console.WriteLine($"{userIds}           {UserService.GetUserName()}");

                    //}

                        //string send = transferService.SendTEBucks();

                   

                    //decimal amount = Convert.ToDecimal(Console.ReadLine());
                    //string failure = "Insufficient funds - no money transferred.";

                    //if (accountBalance.Balance < amount)
                    //{
                    //    return failure;
                    //}
                }
                else if (menuSelection == 5)
                {

                }
                else if (menuSelection == 6)
                {
                    Console.WriteLine("");
                    UserService.SetLogin(new ApiUser()); //wipe out previous login info
                    Console.Clear();
                    menuSelection = 0;
                }
                else
                {
                    Console.WriteLine("Goodbye!");
                    Environment.Exit(0);
                }
            }
        }
    }
}
