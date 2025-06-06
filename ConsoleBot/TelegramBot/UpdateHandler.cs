﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Otus.ToDoList.ConsoleBot;
using Otus.ToDoList.ConsoleBot.Types;
using SmartMenuBot.Core.Commands;
using SmartMenuBot.Core.Commands.Interfaces;
using SmartMenuBot.Core.Commands.Implementations;
using SmartMenuBot.Core.Services;

namespace SmartMenuBot.TelegramBot
{
    public class UpdateHandler(IEnumerable<IBotCommand> commands) : IUpdateHandler
    {
        private readonly IReadOnlyList<IBotCommand> _commands = commands.ToList();

        public void HandleUpdateAsync(ITelegramBotClient botClient, Update update)
        {
            var context = new CommandContext(update);
            try
            {
                var matchingCommand = _commands.FirstOrDefault(c => c.CanExecute(context));
                if (matchingCommand != null)
                    matchingCommand.Execute(context);
                else
                    new UnknownCommand(botClient).Execute(context);
            }
            catch (Exception ex)
            {
                botClient.SendMessage(update.Message.Chat, ex.Message);
            }
        }
    }
}