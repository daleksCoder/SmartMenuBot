using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otus.ToDoList.ConsoleBot;
using SmartMenuBot.Core.Entities;
using SmartMenuBot.Core.Services.Interfaces;

namespace SmartMenuBot.Core.Services.Domain
{
    public class ToDoReportService(IToDoService toDoService) : IToDoReportService
    {
        public (int total, int completed, int active, DateTime generatedAt) GetUserStats(Guid userId)
        {
            var itemsList = toDoService.GetAllByUserId(userId);

            int totalTasks = itemsList.Count;
            int activeTasks = itemsList.Where(item => item.State == ToDoItemState.Active).ToList().Count;
            int completedTasks = totalTasks - activeTasks;

            return (totalTasks, completedTasks, activeTasks, DateTime.UtcNow);
        }
    }
}