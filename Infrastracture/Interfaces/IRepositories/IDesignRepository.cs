

using Domain.Entities;
using Infrastracture.Models;

namespace Infrastracture.Interfaces.IRepositories
{
    public interface IDesignRepository
    {
        Task<List<Design>> GetDesignsAsync(string term, int pageSize, int page);
        Task<List<DesignCategoryModel>> GetDesignCategoriesAsync();
        Task<List<Design>> GetDesignsByCategoryIdAsync(int categoryId, int pageSize, int page);
        Task<Design?> GetDesignByIdAsync(long designId);
        Task<List<Design>> GetBestSellersDesigns(int pageSize, int page);
        
    }
}
