using System;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace SmartMenuBot
{
    internal class Program
    {
        static void Main()
        {

            DisplayWelcomeMessage();
            
            InterfaceUserCommandDialog();
        }


        static void DisplayWelcomeMessage()
        {
            string msgStart, msgMiddle, msgEnd, msgResult;

            msgStart = "Привет! Добро пожаловать в систему управления обогревом загородного дома!\n\r";
            msgMiddle = $"\n\r{GetAvailableBotCommands()}\n\r";
            msgEnd = "\n\rДля начала работы рекомендую авторизоваться с помощью команды /start\n\r";

            var strBuilder = new StringBuilder();

            strBuilder.Append(msgStart);
            strBuilder.Append(msgMiddle);
            strBuilder.Append(msgEnd);

            msgResult = strBuilder.ToString();

            Console.Write($"{msgResult}");
        }

        static void InterfaceUserCommandDialog()
        {
            string username = "";

            while (true)
            {
                var args = Array.Empty<string>();
                string botCommand = GetBotCommand(username, ref args);

                switch (botCommand)
                {
                    case "/start":
                        StartCommand(ref username);
                        break;

                    case "/help":
                        HelpCommand(username);
                        break;

                    case "/echo":
                        EchoCommand(username, args);
                        break;

                    case "/info":
                        InfoCommand();
                        break;

                    case "/exit":
                        ExitCommand();
                        return;

                    default:
                        UnknownCommand();
                        break;
                }
            }
        }



        static void StartCommand(ref string username)
        {
            bool isUserAuthorized = !String.IsNullOrEmpty(username);

            if (isUserAuthorized)
            {
                Console.WriteLine($"\n\r{username}, Вы уже авторизовались ранее!");
                return;
            }

            Console.Write("\n\rОтлично! Теперь укажите Ваше имя: ");
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("\n\rИмя не может быть пустым!");
                return;
            }

            username = input;
            Console.WriteLine($"\n\rОчень приятно, {username}! Теперь Вам доступна команда: /echo");
        }

        static void HelpCommand(string username)
        {
            Console.WriteLine($"\n\r{GetAvailableBotCommands(username)}");
        }

        static void EchoCommand(string username, string[] args)
        {
            bool isUserAuthorized = !String.IsNullOrEmpty(username);

            if (!isUserAuthorized)
            {
                Console.WriteLine("\n\rКоманда недоступна");
                return;
            }

            var strBuilder = new StringBuilder();
            foreach (string str in args)
            {
                strBuilder.Append(str + " ");
            }

            string echoString = strBuilder.ToString().Trim();

            bool echoStringIsEmpty = String.IsNullOrWhiteSpace(echoString);

            Console.WriteLine($"\n\r{username}, Ваша echo строка: {(!echoStringIsEmpty ? echoString : "[параметры команды не указаны]")}");            
        }

        static void InfoCommand()
        {
            string version = "";
            DateTime creationDate = new(1, 1, 1);

            GetVerInfo(ref version, ref creationDate);
            Console.WriteLine($"\n\rВерсия системы: {version}, дата создания: {creationDate:dd.MM.yyyy}");
        }

        static void ExitCommand()
        {
            Console.WriteLine("\n\rСпасибо за использование программы! До новых встреч! \n\r");
        }

        static void UnknownCommand()
        {
            Console.WriteLine("\n\rНеизвестная команда");
        }


        static string GetBotCommand(string username, ref string[] args)
        {
            string? input;
            string botCommand;
            string userMessage;

            bool isUserAuthorized = !String.IsNullOrEmpty(username);

            userMessage = $"\n\r{(isUserAuthorized ? (username + ", для") : "Для")} продолжения работы укажите одну из доступных команд: ";

            while (true)
            {
                Console.Write(userMessage);
                input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input))
                    break;

                Console.WriteLine("\n\rИмя команды не может быть пустым!");
            }

            string[] parsewords = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            int length = parsewords.Length - 1;

            Array.Resize(ref args, length);
            Array.Copy(parsewords, 1, args, 0, length);

            botCommand = parsewords[0];

            return botCommand;
        }

        static string GetAvailableBotCommands(string username = "")
        {
            string msgStart, msgMiddle, msgEnd, msgResult;

            msgStart =
                """                           
                Доступные команды:
                /start * начало работы с ботом
                /help  * справка по доступным командам
                """;

            msgMiddle = "\n\r/echo  * эхо команда";

            msgEnd =
                $"""
                
                /info  * информация о версии
                /exit  * завершение работы с ботом
                """;
            
            var strBuilder = new StringBuilder();

            strBuilder.Append(msgStart);
            if (!String.IsNullOrEmpty(username))
                strBuilder.Append(msgMiddle);
            strBuilder.Append(msgEnd);

            msgResult = strBuilder.ToString();

            return msgResult;
        }


        static void GetVerInfo(ref string version, ref DateTime creationDate)
        {
            version = "1.2.3";
            creationDate = new(2025, 3, 1);
        }
    }
}
