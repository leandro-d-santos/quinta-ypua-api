using Data.Context;
using Domain.Common.Data;

namespace Data.Common
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly DbConnection _context;

        public UnitOfWork(DbConnection context)
        {
            _context = context;
        }

        public Task Complete()
        {
            return _context.SaveChangesAsync();
        }
    }
}