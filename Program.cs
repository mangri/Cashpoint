using System;
using Cashpoint.ClientIdentification;
using Cashpoint.ClientTransactions;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Cashpoint
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ICard card = new Client();
            (bool cardValidation, string guid) =
                card.validatingCard();
            if (cardValidation)
            {
                IPassword password = new Client();
                (bool passwordValidation, decimal balance) =
                    password.matchingPassword(guid);
                if (passwordValidation)
                {
                    Console.WriteLine("Balance: " + balance);
                    while(true)
                    {
                        char validatedAction = AskingUserForInput();
                        
                        switch (validatedAction)
                        {
                            case '1':
                                IBalance available1 = new Transactions();
                                Console.WriteLine("Balance: " + available1.GetBalance(guid));
                                break;
                            case '2':
                                ITranscript lastFive = new Transactions();
                                lastFive.printTranscript(guid);
                                break;
                            case '3':
                                IBalance available2 = new Transactions();
                                IWithdrawal transactionsToday = new Transactions();
                                (int totalNumber, decimal totalCredit) = 
                                    transactionsToday.TransactionsToday(guid);
                                IWithdrawal newCredit = new Transactions();
                                newCredit.Crediting(guid, totalNumber, totalCredit, available2.GetBalance(guid));
                                break;
                            case '4':
                                Environment.Exit(0);
                                break;

                            default: break;
                        }
                        if (validatedAction == 'Z')
                        {
                            Console.WriteLine("Too many attempts to connect.");
                            Console.WriteLine("The systems was not loaded.");
                            Console.WriteLine("Try again later. Bye.");
                            break;
                        }
                        Console.WriteLine("Proceed with one more request? y/n");
                        string ifContinue = Console.ReadLine();
                        if (ifContinue == "y" || ifContinue == "Y")
                        {
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            Console.WriteLine("Exiting application..");
            Console.ReadLine();
        }
        private static char AskingUserForInput()
        {
            for (int i = 1; i < 6; i++)
            {
                Console.WriteLine("Choose one of the following options:");
                Console.WriteLine("[1] Balance");
                Console.WriteLine("[2] Last transactions");
                Console.WriteLine("[3] Withdrawal");
                Console.WriteLine("[4] Exit");
                string furtherAction = Console.ReadLine();
                char validatedAction;
                bool goodToGo = Char.TryParse(furtherAction, out validatedAction);
                if (goodToGo && '0' < validatedAction && validatedAction < '5')
                {
                    return validatedAction;
                }
                else if (i < 4)
                {
                    Console.WriteLine($"No such option. Try again ({5 - i} attempt(s) left)");
                }
                else if (i == 4)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("The last attempt before exiting");
                    Console.ResetColor();
                }
            }
            return 'Z';
        }
    }
}
