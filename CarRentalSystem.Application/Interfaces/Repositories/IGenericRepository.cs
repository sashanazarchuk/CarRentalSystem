using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> CreateAsync(T entity, CancellationToken token);
        Task PatchAsync(T entity, CancellationToken token);
        Task DeleteAsync(T entity, CancellationToken token);
        Task<T?> GetByIdAsync(int id, CancellationToken token);
        Task<IEnumerable<T>> GetAllAsync(CancellationToken token);
    }
}
