using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace SmartMenuBot
{
    internal class Program
    {
        static void Main()
        {
            try
            {
                InitBot();
                InterfaceUserCommandDialog();
            }
            catch (Exception ex)
            {
                Console.WriteLine("В приложении произошла непредвиденная ошибка:");
                Console.WriteLine($"Тип: {ex.GetType()}");
                Console.WriteLine($"Сообщение: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");

                if (ex.InnerException != null)
                    Console.WriteLine($"InnerException: {ex.InnerException}");
            }
        }

        static void InitBot()
        {
            string msgStart, msgEnd, msgResult;

            msgStart = "Привет! Добро пожаловать в систему управления обогревом загородного дома!\n\r";
            msgEnd = $"\n\r{GetAvailableBotCommands()}\n\r";

            var strBuilder = new StringBuilder();

            strBuilder.Append(msgStart);
            strBuilder.Append(msgEnd);

            msgResult = strBuilder.ToString();

            Console.Write($"{msgResult}");
        }

        static void InterfaceUserCommandDialog()
        {
            string username = "";

            int taskCountLimit = InitTaskCountLimit();

            int taskLengthLimit = InitTaskLengthLimit();

            List<string> taskList = new List<string>();

            while (true)
            {
                var args = Array.Empty<string>();
                string botCommand = GetBotCommand(username, ref args);

                try
                {
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

                        case "/addtask":
                            AddTaskCommand(username, taskList, taskCountLimit, taskLengthLimit);
                            break;

                        case "/showtasks":
                            ShowTasksCommand(taskList);
                            break;

                        case "/removetask":
                            RemoveTaskCommand(taskList);
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
                catch (TaskCountLimitException exc)
                {
                    Console.WriteLine(exc.Message);
                }
                catch (TaskLengthLimitException exc)
                {
                    Console.WriteLine(exc.Message);
                }
                catch (DuplicateTaskException exc)
                {
                    Console.WriteLine(exc.Message);
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

        static void AddTaskCommand(string username, List<string> taskList, int taskCountLimit, int taskLengthLimit)
        {
            bool isUserAuthorized = !String.IsNullOrEmpty(username);

            string userMessage = $"\n\r{(isUserAuthorized ? (username + ", заполните") : "Заполните")} список задач (/exit - для выхода)";
            Console.WriteLine(userMessage);

            while (true)
            {
                Console.Write("Наименование задачи: ");
                string? input = Console.ReadLine();

                if (input == "/exit")
                    break;

                try
                {
                    ValidateString(input);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                }

                if (taskList.Count == taskCountLimit)
                    throw new TaskCountLimitException(taskCountLimit);

                string trimmedInput = input!.Trim();
                if (trimmedInput.Length > taskLengthLimit)
                    throw new TaskLengthLimitException(trimmedInput.Length, taskLengthLimit);

                if (taskList.Contains(trimmedInput))
                    throw new DuplicateTaskException(trimmedInput);

                taskList.Add(trimmedInput);
            }
        }

        static void ShowTasksCommand(List<string> taskList)
        {
            if (taskList.Count == 0)
            {
                Console.WriteLine("\n\rСписок задач пока не заполнен");
                return;
            }

            Console.WriteLine("\n\rСписок задач:");
            ShowTasks(taskList);
        }

        static void RemoveTaskCommand(List<string> taskList)
        {
            int taskid;

            if (taskList.Count == 0)
            {
                Console.WriteLine("\n\rСписок задач пока не заполнен");
                return;
            }

            Console.WriteLine("\n\rСписок задач:");
            ShowTasks(taskList);

            Console.Write("\n\rВведите ID задачи для удаления: ");
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("\n\rID задачи не может быть пустым!");
                return;
            }

            if (!int.TryParse(input, out taskid))
            {
                Console.WriteLine("\n\rID задачи должен быть числом в диапазоне номеров задач!");
                return;
            }

            if (taskid < 1 || taskid > taskList.Count)
            {
                Console.WriteLine("\n\rID задачи должен быть в допустимом диапазоне номеров задач!");
                return;
            }

            taskList.RemoveAt(taskid-1);
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
            bool isUserAuthorized = !String.IsNullOrEmpty(username);

            string userMessage = $"\n\r{(isUserAuthorized ? (username + ", для") : "Для")} продолжения работы укажите одну из доступных команд: ";

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

            string botCommand = parsewords[0].ToLower();

            return botCommand;
        }

        static string GetAvailableBotCommands(string username = "")
        {
            string msgStart, msgMiddle, msgEnd, msgResult;

            msgStart =
                """                           
                Доступные команды:
                /start      * начало работы с ботом
                /help       * справка по доступным командам
                """;

            msgMiddle = "\n\r/echo       * эхо команда";

            msgEnd =
                $"""
                
                /addTask    * добавление задач в список
                /showTasks  * вывод списка задач
                /removeTask * удаление задачи из списка              
                /info       * информация о версии
                /exit       * завершение работы с ботом
                """;
            
            var strBuilder = new StringBuilder();

            strBuilder.Append(msgStart);
            if (!String.IsNullOrEmpty(username))
                strBuilder.Append(msgMiddle);
            strBuilder.Append(msgEnd);

            msgResult = strBuilder.ToString();

            return msgResult;
        }

        static void ShowTasks(List<string> taskList)
        {
            int i = 0;
            foreach (var item in taskList)
            {
                i++;
                Console.Write($"Наименование задачи ID{i}: {item}\n\r");
            }
        }

        static void GetVerInfo(ref string version, ref DateTime creationDate)
        {
            version = "1.2.3";
            creationDate = new(2025, 3, 1);
        }


        static int InitTaskCountLimit()
        {
            while (true)
            {
                Console.Write("\n\rВведите максимально допустимое количество задач: ");
                string? input = Console.ReadLine();

                try
                {
                    return ParseAndValidateInt(input, 1, 100);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        static int InitTaskLengthLimit()
        {
            while (true)
            {
                Console.Write("\n\rВведите максимально допустимую длину задачи: ");
                string? input = Console.ReadLine();

                try
                {
                    return ParseAndValidateInt(input, 1, 100);
                }
                catch (ArgumentException exc)
                {
                    Console.WriteLine(exc.Message);
                }
            }
        }

        static int ParseAndValidateInt(string? str, int min, int max)
        {
            if (!int.TryParse(str, out int result))
                throw new ArgumentException($"\nВведите корректное целое число в диапазоне от {min} до {max}");

            if (result < min || result > max)
                throw new ArgumentException($"\nЧисло должно быть в диапазоне от {min} до {max}");

            return result;
        }

        static void ValidateString(string? str)
        {
            if (string.IsNullOrWhiteSpace(str))
                throw new ArgumentException("\n\rНаименование задачи не может быть пустым");
        }
    }
}
