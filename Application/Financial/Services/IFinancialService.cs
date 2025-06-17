using Application.Financial.ViewModels;

namespace Application.Financial.Services
{
    public interface IFinancialService
    {
        Task<IList<FinancialViewModel>> FindAllAsync();
        Task<FinancialViewModel?> FindByIdAsync(Guid id);
        Task<bool> UpdateAsync(FinancialViewModel financialViewModel);
        Task<bool> CheckOutAsync(Guid id);
    }
}