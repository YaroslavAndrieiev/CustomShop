using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;

namespace CustomShop
{
    class Program
    {
        public static DataBase<string> db;
        public static User<string> UserOfCurrentSession;

        public delegate void StateHandler(string message);
        public static StateHandler stateHandler;

        public delegate void ColorStateHandler(string message, ConsoleColor color);
        public static ColorStateHandler colorStateHandler;

        public delegate void ClearLog();
        public static event ClearLog clearLog = () => Console.Clear();

        public delegate List<Product> SearchQuery(string query);
        public static event SearchQuery SearchProductsEvent;

        public delegate bool LogIn(string userLogin, string userPassword);
        public static event LogIn LogInEvent;

        public delegate bool RegIn(string userLogin, string userPassword);
        public static event LogIn RegInEvent;

        public delegate void LogOut();
        public static event LogOut LogOutEvent = () => { UserOfCurrentSession.LoginState = false; Introducing(); };

        public delegate void NewOffer(Offer newOffer);
        public static event NewOffer NewOfferEvent;

        public delegate void Pay();
        public static event Pay PayEvent = () => colorStateHandler("Payment passed!",ConsoleColor.Green);

        public delegate void UserActionHandler(User<string> user, string action);
        public static event UserActionHandler UserActionHandlerEvent;

        private static void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }

        private static void ColorMessage(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private static void EnterSearchQuery()
        {
            clearLog();
            colorStateHandler("Enter search query:", ConsoleColor.Red);
            string SearchQuery = GetUserAnswer();
            List<Product> searchedList = SearchProductsEvent(SearchQuery);

            if (searchedList.Count == 0)
            {
                stateHandler("The aren't any products for your query.");
            }
            else
            {
                stateHandler("---Products found---");
                stateHandler("Query: \"" + SearchQuery + "\"");
                clearLog();
                for (int i = 0; i < searchedList.Count; i++)
                {
                    stateHandler(i + 1 + ") " + searchedList.ElementAt(i).Name + " - " + searchedList.ElementAt(i).Price + "$" + "\tCode: " + searchedList.ElementAt(i).VendorCode);
                }
            }
            ShowCurrentUserOptions();
        }
        private static List<Product> GetSearchedList(string SearchQuery)
        {
            List<Product> SearchedList = new List<Product>();
            foreach (Product item in db.ProductsList)
            {
                if (item.Name.ToUpper().Contains(SearchQuery.ToUpper()))
                {
                    SearchedList.Add(item);
                }
            }
            return SearchedList;
        }

        private static void Introducing()
        {
            clearLog();
            stateHandler("-------------BICYCLE SHOP-------------");
            ShowGuestOptions();
        }

        private static void ShowGuestOptions()
        {
            stateHandler("Enabled actions:");
            stateHandler("1) Log in");
            stateHandler("2) Register");
            stateHandler("3) Show products");
            stateHandler("4) Find product\n");
            colorStateHandler("Please choose an action:", ConsoleColor.Red);

            UserActionHandlerEvent(UserOfCurrentSession,GetUserAnswer());
        }

        private static void ShowUserOptions()
        {
            stateHandler("Enabled actions:");
            stateHandler("1) Show products");
            stateHandler("2) Find product");
            stateHandler("3) New offer");
            stateHandler("4) Offers history");
            stateHandler("5) Profile info");
            stateHandler("6) Log out\n");
            colorStateHandler("Please choose an action:", ConsoleColor.Red);

            UserActionHandlerEvent(UserOfCurrentSession, GetUserAnswer());
        }

        private static void ShowAdminOptions()
        {
            stateHandler("Enabled actions:");
            stateHandler("1) Show products");
            stateHandler("2) Find product");
            stateHandler("3) New offer");
            stateHandler("4) All users");
            stateHandler("5) New product");
            stateHandler("6) Change product info");
            stateHandler("7) All offers");
            stateHandler("8) Log out\n");
            colorStateHandler("Please choose an action:", ConsoleColor.Red);

            UserActionHandlerEvent(UserOfCurrentSession, GetUserAnswer());
        }

        private static string GetUserAnswer()
        {
            return (Console.ReadLine());
        }

        private static void ShowProducts()
        {
            stateHandler("---Products---");
            clearLog();
            for (int i = 0; i < db.ProductsList.Count; i++)
            {
                stateHandler(i + 1 + ") " + db.ProductsList.ElementAt(i).Name + " - " + db.ProductsList.ElementAt(i).Price + "$" +"\tCode: "+ db.ProductsList.ElementAt(i).VendorCode);
            }
            ShowCurrentUserOptions();
        }

        private static void GetLoginPassword()
        {
            while (UserOfCurrentSession.LoginState == false)
            {
                clearLog();
                stateHandler("---Log in---");
                colorStateHandler("Enter login:", ConsoleColor.Red);
                string UserLogin = GetUserAnswer();
                colorStateHandler("Enter password:", ConsoleColor.Red);
                string UserPassword = GetUserAnswer();

                if (LogInEvent(UserLogin, UserPassword))
                {
                    clearLog();
                    colorStateHandler("Hello, " + UserOfCurrentSession.Login + ".", ConsoleColor.Green);
                    colorStateHandler("You successfully logged in!\n", ConsoleColor.Green);
                    Thread.Sleep(1000);
                    if (UserOfCurrentSession.Right.Equals(Rights.Admin))
                    {
                        ShowAdminOptions();
                    }
                    else
                    {
                        ShowUserOptions();
                    }
                }
                else
                {
                    colorStateHandler("Login or password doesn't correct!!!", ConsoleColor.Red);
                    Thread.Sleep(2000);
                }
            }
        }
        private static bool Login(string UserLogin, string UserPassword)
        {
            foreach (User<string> User in db.UsersList)
            {
                if (User.Login.ToUpper().Equals(UserLogin.ToUpper()))
                {
                    if (User.Password.ToUpper().Equals(UserPassword.ToUpper()))
                    {
                        User.LoginState = true;
                        UserOfCurrentSession = User;
                        return true;
                    }
                }
            }
            return false;
        }

        private static void GetRegistationInfo()
        {
            while (UserOfCurrentSession.LoginState == false)
            {
                clearLog();
                stateHandler("---Registration---");
                colorStateHandler("Enter login:", ConsoleColor.Red);
                string UserLogin = GetUserAnswer();
                colorStateHandler("Enter password:", ConsoleColor.Red);
                string UserPassword = GetUserAnswer();

                if (UserOfCurrentSession.LoginState == false)
                {
                    if (RegInEvent(UserLogin, UserPassword))
                    {
                        clearLog();
                        colorStateHandler("Your account has been created!", ConsoleColor.Green);
                        ShowCurrentUserOptions();
                    }
                    else
                    {
                        colorStateHandler("Account with login \"" + UserLogin + "\" alredy exist!!!", ConsoleColor.Red);
                        Thread.Sleep(2000);
                    }
                }
            }
        }
        private static bool Register(string UserLogin, string UserPassword)
        {
            foreach (User<string> User in db.UsersList)
            {
                if (User.Login.ToUpper().Equals(UserLogin.ToUpper()))
                {
                    return false;
                }
                else
                {
                    db.AddUser(new User<string>(UserLogin, UserPassword));
                    return true;
                }
            }
            return false;
        }

        private static void ShowCurrentUserOptions()
        {
            colorStateHandler("\nPress any key to show the options!", ConsoleColor.Red);
            Console.ReadKey();
            clearLog();

            if (UserOfCurrentSession.LoginState == false)
            {
                ShowGuestOptions();
            } 
            else
            if (UserOfCurrentSession.Right.Equals(Rights.Admin))
            {
                ShowAdminOptions();
            }
            else
            if (UserOfCurrentSession.Right.Equals(Rights.User))
            {
                ShowUserOptions();
            }
        }

        private static void HandleUserAction(User<string> User, string Action)
        {
            if (UserOfCurrentSession.LoginState == false)
            {
                //Guest actions are enabled
                switch (Action)
                {
                    case "1":
                        GetLoginPassword();
                        break;
                    case "2":
                        GetRegistationInfo();
                        break;
                    case "3":
                        ShowProducts();
                        ShowCurrentUserOptions();
                        break;
                    case "4":
                        EnterSearchQuery();
                        break;
                    default:
                        clearLog();
                        ShowCurrentUserOptions();
                        break;
                }
            }
            else
            {
                if (User.Right.Equals(Rights.Admin))
                {
                    //Admin actions are enabled
                    switch (Action)
                    {
                        case "1":
                            ShowProducts();
                            break;
                        case "2":
                            EnterSearchQuery();
                            break;
                        case "4":
                            ShowAllUsers();
                            break;
                        case "7":
                            ShowAllOffers();
                            break;
                        case "8":
                            LogOutEvent();
                            break;
                        default:
                            ShowCurrentUserOptions();
                            break;                     
                    }
                }
                else
                if (User.Right.Equals(Rights.User))
                {
                    //User actions are enabled
                    switch (Action)
                    {
                        case "1":
                            ShowProducts();
                            break;
                        case "2":
                            EnterSearchQuery();
                            break;
                        case "3":
                            CreateNewOffer();
                            break;
                        case "6":
                            LogOutEvent();
                            break;
                        default:
                            ShowCurrentUserOptions();
                            break;
                    }     
                }
            }
        }

        private static void CreateNewOffer()
        {
            clearLog();
            stateHandler("---Adding products to basket---");
            /*stateHandler("1) Add product by code");*/
            stateHandler("1) Choose from list");
            colorStateHandler("\nPlease choose an action:", ConsoleColor.Red);

            switch (GetUserAnswer())
            {
                /*case "1":
                    clearLog();
                    colorStateHandler("Enter product code:", ConsoleColor.Red);
                    int EnteredCode = Int32.Parse(GetUserAnswer());
                    if (db.GetProductByCode(EnteredCode) is null)
                    {
                        colorStateHandler("Product with this code doesn't exist!", ConsoleColor.Red);
                        CreateNewOffer();
                    }
                    else
                    {
                        //pay
                        NewOfferEvent(new Offer(db.GetProductByCode(EnteredCode),UserOfCurrentSession));
                    }
                    break;*/
                case "1":
                    ShowProducts();
                    colorStateHandler("\nEnter number of product:",ConsoleColor.Red);
                    int EnteredNumber = Int32.Parse(GetUserAnswer());
                    if(EnteredNumber < 0 || EnteredNumber > db.ProductsList.Count + 1)
                    {
                        colorStateHandler("Incorect number!",ConsoleColor.Red);
                        Thread.Sleep(1500);
                    }
                    else
                    {
                        colorStateHandler("\nEnter card number and press \"Enter\" to pay:",ConsoleColor.Red);
                        GetUserAnswer();
                        PayEvent();
                        Offer newOffer = new Offer(db.GetProductByNumber(EnteredNumber), UserOfCurrentSession);
                        newOffer.State = OfferState.Paid;
                        NewOfferEvent(newOffer);
                    }
                    break;
                default:
                    CreateNewOffer();
                    break;
            }
        }
        private static void AddNewOffer(Offer NewOffer)
        {
            db.AddOffer(NewOffer);
            ShowCurrentUserOptions();
        }

        private static void ShowAllUsers()
        {
            clearLog();
            stateHandler("---Users---");
            for(int i=0;i< db.UsersList.Count; i++)
            {
                stateHandler(" № "+ (i+1) + ")");
                stateHandler("\tLogin: " + db.UsersList.ElementAt(i).Login);
                stateHandler("\tName: " + db.UsersList.ElementAt(i).Name);
                stateHandler("\tSurname: " + db.UsersList.ElementAt(i).Surname);
                if (db.UsersList.ElementAt(i).LoginState == false)
                {
                    colorStateHandler("\tLogin state: " + db.UsersList.ElementAt(i).LoginState,ConsoleColor.Yellow);
                } 
                else
                {
                    colorStateHandler("\tLogin state: " + db.UsersList.ElementAt(i).LoginState, ConsoleColor.Green);
                }
            }
            ShowCurrentUserOptions();
        }

        private static void ShowAllOffers()
        {
            clearLog();
            stateHandler("---Offers---");
            Offer currentOffer;
            for (int i = 0; i < db.OffersList.Count; i++)
            {
                currentOffer = db.OffersList.ElementAt(i);

                stateHandler("\n№ " + (i + 1) + ")");
                stateHandler("OfferNumber: " + currentOffer.OfferNumber);
                stateHandler("User login: "+ currentOffer.User.Login);
                stateHandler("Products: ");
                foreach (Product item in currentOffer.ProductsList)
                {
                    stateHandler("\tVendor code:"+ item.VendorCode+" Name:"+item.Name);
                }
                if(currentOffer.State == OfferState.Completed)
                {
                    colorStateHandler("State: "+ currentOffer.State,ConsoleColor.Green);
                } 
                else
                {
                    colorStateHandler("State: " + currentOffer.State, ConsoleColor.Yellow);
                }
            }
            ShowCurrentUserOptions();
        }
        static void Main(string[] args)
        {
            db = new DataBase<string>();
            UserOfCurrentSession = new User<string>();
            stateHandler += ShowMessage;
            colorStateHandler += ColorMessage;
            SearchProductsEvent += GetSearchedList;
            LogInEvent += Login;
            RegInEvent += Register;
            UserActionHandlerEvent += HandleUserAction;
            NewOfferEvent += AddNewOffer;
            Introducing();
        }
    }
}
