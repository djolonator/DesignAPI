

using Domain.Entities;
using Infrastracture.Models;

namespace Infrastracture.Interfaces.IRepositories
{
    public interface IDesignRepository
    {
        Task<List<Design>> GetDesignsAsync(string term);
        Task<List<DesignCategoryModel>> GetDesignCategoriesAsync();
        Task<List<Design>> GetDesignsByCategoryIdAsync(int categoryId, int pageSize, int page);
        Task<Design?> GetDesignByIdc(int designId);
    }
}
