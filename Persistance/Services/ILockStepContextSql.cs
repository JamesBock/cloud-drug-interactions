using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace UWPLockStep.Persistance.Services
{
    public interface ILockStepContextSql
    {
        DbSet<T> Set<T>() where T : class; // Dont't be afraid to list every DbSet if necessary.
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
