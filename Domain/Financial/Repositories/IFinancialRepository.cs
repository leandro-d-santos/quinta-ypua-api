namespace Domain.Financial.Repositories
{
    public interface IFinancialRepository
    {
        IList<Models.Financial> FindAll();
        Models.Financial? FindById(Guid id);
        void Create(Models.Financial financial);
        void Update(Models.Financial financial);
        void CheckOutAsync(Guid id);
    }
}