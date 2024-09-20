
using Domain;
using Infrastracture.Interfaces.IRepositories;
using Infrastracture.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Repositories
{
    public class DesignRepository: IDesignRepository
    {
        private readonly StorageDbContext _storageContext;
        private readonly ILogger _logger;

        public DesignRepository(StorageDbContext storageDbContext, ILogger<DesignRepository> logger)
        {
            _storageContext = storageDbContext;
            _logger = logger;
        }

        public async Task<List<DesignModel>> GetDesignsAsync(string term)
        {
            try
            {
                var diagnosisList = new List<DesignModel>();

                var designes = await _storageContext
                    .Design
                    .Where(x => x.DesignName.Contains(term) || x.Description.Contains(term))
                    .ToListAsync();

                foreach (var item in designes)
                {
                    var diagnosis = new DesignModel
                    {
                        DesignId = item.DesignId,
                        DesignName = item.DesignName,
                        Description = item.Description,
                    };
                    diagnosisList.Add(diagnosis);
                }

                return diagnosisList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in: DesignRepository.GetDesignsAsync()");
                throw;
            }
        }
    }
}
