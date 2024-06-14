namespace Domain.Common.Data
{
    public interface IUnitOfWork
    {
        public Task Complete();
    }
}