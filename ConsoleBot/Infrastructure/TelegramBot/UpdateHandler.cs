using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Otus.ToDoList.ConsoleBot;
using Otus.ToDoList.ConsoleBot.Types;
using SmartMenuBot.Core.Commands;
using SmartMenuBot.Core.Services;

namespace SmartMenuBot.Infrastructure.TelegramBot
{
    public class UpdateHandler(IEnumerable<IBotCommand> commands) : IUpdateHandler, IAuthStateProvider
    {
        private List<IBotCommand> Commands { get; } = commands.ToList();

        private List<IBotServiceProvider> Services { get; } =
        [
            new UserService(),
            new ToDoService()
        ];

        public bool IsAuthorized { get; set; }

        public void HandleUpdateAsync(ITelegramBotClient botClient, Update update)
        {
            var context = new CommandContext(this, botClient, update, Services);
            try
            {
                var matchingCommand = Commands.FirstOrDefault(c => c.CanExecute(context));
                if (matchingCommand != null)
                    matchingCommand.Execute(context);
                else
                    new UnknownCommand().Execute(context);
            }
            catch (Exception ex)
            {
                botClient.SendMessage(update.Message.Chat, ex.Message);
            }
        }
    }
}