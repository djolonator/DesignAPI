

using Infrastracture.Models;

namespace Infrastracture.Interfaces.IRepositories
{
    public interface IDesignRepository
    {
        Task<List<DesignModel>> GetDesignsAsync(string term);
        Task<List<DesignCategoryModel>> GetDesignsCategoriesAsync();
    }
}
