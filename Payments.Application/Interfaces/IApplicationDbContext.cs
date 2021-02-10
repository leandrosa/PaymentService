using Microsoft.EntityFrameworkCore;
using Payments.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Payments.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Payment> Payments { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
