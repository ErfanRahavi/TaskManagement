using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Domain.Interfaces
{
    public interface ITaskRepository
    {
        Task<TodoTask?> GetByIdAsync(Guid id);
        Task<IEnumerable<TodoTask>> GetAllAsync();
        Task AddAsync(TodoTask task);
        Task UpdateAsync(TodoTask task);
        Task DeleteAsync(Guid id);
    }
}
