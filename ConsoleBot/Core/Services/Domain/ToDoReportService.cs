using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otus.ToDoList.ConsoleBot;
using SmartMenuBot.Core.Services.Interfaces;

namespace SmartMenuBot.Core.Services.Domain
{
    public class ToDoReportService(IToDoService toDoService) : IToDoReportService
    {
        public (int total, int completed, int active, DateTime generatedAt) GetUserStats(Guid userId)
        {
            var activeTasks = toDoService.GetActiveByUserId(userId).Count;
            int totalTasks = toDoService.GetAllByUserId(userId).Count;
            int completedTasks = totalTasks - activeTasks;

            return (totalTasks, completedTasks, activeTasks, DateTime.UtcNow);
        }
    }
}