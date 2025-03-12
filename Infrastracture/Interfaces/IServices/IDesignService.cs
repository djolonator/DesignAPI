
using Infrastracture.Models;
using Infrastructure.Abstractions;

namespace Infrastracture.Interfaces.IServices
{
    public interface IDesignService
    {
        Task<Result<List<DesignModel>>> SearchDesigns(string term);
        Task<Result<List<DesignCategoryModel>>> GetDesignCategoriesAsync();
        Task<Result<List<DesignModel>>> GetGesignsByCategoryIdPaginated(int categoryId, int pageSize, int page);
        Task<Result<DesignModel>> GetDesignByIdAsync(int designId);
        Task<Result<List<DesignModel>>> GetBestsellingDesignsPaginated(int pageSize, int page);
    }
}
