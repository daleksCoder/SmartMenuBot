using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMenuBot.Core.Entities
{
    public class ToDoUser
    {
        public Guid UserId { get; } = Guid.NewGuid();
        public long TelegramUserId { get; }
        public string? TelegramUserName { get; private set; }
        public DateTime RegisteredAt { get; } = DateTime.UtcNow;

        public ToDoUser(long telegramUserId, string? telegramUserName)
        {
            if (telegramUserId <= 0)
                throw new ArgumentException("Поле ID должно быть положительным числом", nameof(telegramUserId));

            TelegramUserId = telegramUserId;
            TelegramUserName = telegramUserName;
        }

        public void ChangeUserName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentNullException(nameof(newName));

            TelegramUserName = newName;
        }

        public override string ToString()
        {
            return $"User {TelegramUserName} (ID: {TelegramUserId})";
        }
    }
}