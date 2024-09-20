


using Infrastracture.Interfaces.IRepositories;
using Infrastracture.Interfaces.IServices;
using Infrastracture.Models;
using Infrastructure.Abstractions;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class DesignService : IDesignService
    {
        private readonly IDesignRepository _designRepository;
        private readonly ILogger _logger;

        public DesignService(IDesignRepository designRepository, ILogger<DesignService> logger)
        {
            _designRepository = designRepository;
            _logger = logger;
        }

        public async Task<Result<List<DesignModel>>> SearchDesigns(string term)
        {
            try
            {
                var termNoComma = term.Replace(".", string.Empty);
                var result = await _designRepository.GetDesignsAsync(termNoComma);

                return Result.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in: DesignService.SearchDesigns()");
                throw;
            }
        }

        public async Task<Result<List<DesignCategoryModel>>> GetDesignCategoriesAsync()
        {
            try
            {
                var result = await _designRepository.GetDesignsCategoriesAsync();

                return Result.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in: DesignService.GetDesignCategories()");
                throw;
            }
        }
    }
}
