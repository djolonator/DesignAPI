
using Domain;
using Domain.Entities;
using Infrastracture.Interfaces.IRepositories;
using Infrastracture.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Repositories
{
    public class DesignRepository : IDesignRepository
    {
        private readonly StorageDbContext _storageContext;
        private readonly ILogger _logger;

        public DesignRepository(StorageDbContext storageDbContext, ILogger<DesignRepository> logger)
        {
            _storageContext = storageDbContext;
            _logger = logger;
        }

        public async Task<List<Design>> GetDesignsAsync(string term)
        {
            try
            {
                var designes = await _storageContext
                    .Design
                    .AsNoTracking()
                    .Where(x => x.DesignName.Contains(term) || x.Description.Contains(term))
                    .ToListAsync();

                return designes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in: DesignRepository.GetDesignsAsync()");
                throw;
            }
        }

        public async Task<List<DesignCategoryModel>> GetDesignCategoriesAsync()
        {
            try
            {
                return await _storageContext.Design
                   .AsNoTracking()
                   .GroupBy(d => d.DesignCategory)
                   .Select(g => new DesignCategoryModel
                       {
                           DesignCategoryId = g.Key.DesignCategoryId,
                           DesignCategoryName = g.Key.DesignCategoryName,
                           DesignCount = g.Count()
                       })
                   .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in: DesignRepository.GetDesignsCategories()");
                throw;
            }
        }

        public async Task<List<Design>> GetDesignsByCategoryIdAsync(int categoryId, int pageSize, int page)
        {
            try
            {
                return await _storageContext.Design
                   .AsNoTracking()
                   .Where(d => d.DesignCategoryId == categoryId)
                   .Skip(pageSize*page)
                   .Take(pageSize)
                   .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in: DesignRepository.GetDesignsByCategoryIdAsync()");
                throw;
            }
        }

        public async Task<Design?> GetDesignByIdAsync(int designId) 
        {
            return await _storageContext.Design.AsNoTracking().FirstOrDefaultAsync(d => d.DesignId == designId);
        }
    }
}
