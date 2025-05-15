using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMenuBot.Core.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(Guid id, string entityType)
            : base($"{entityType} с идентификатором {id} не найдена")
        {
        }
    }
}