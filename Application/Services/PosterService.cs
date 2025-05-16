using Infrastracture.Interfaces.IRepositories;
using Infrastracture.Interfaces.IServices;
using Infrastracture.Models;
using Infrastructure.Abstractions;
using Infrastructure.Abstractions.Errors;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class PosterService : IPosterService
    {
        private readonly IDesignRepository _designRepository;
        private readonly ILogger _logger;

        public PosterService(IDesignRepository designRepository, ILogger<PosterService> logger)
        {
            _designRepository = designRepository;
            _logger = logger;
        }

        public async Task<Result<List<DesignModel>>> SearchDesigns(string term, int pageSize, int page)
        {
            try
            {
                var designModels = new List<DesignModel>();
                var termNoComma = term.Replace(".", string.Empty);
                var result = await _designRepository.GetDesignsAsync(termNoComma, pageSize, page);

                foreach (var item in result)
                {
                    var design = new DesignModel
                    {
                        DesignId = item.DesignId,
                        DesignName = item.DesignName,
                        Description = item.Description,
                    };
                    designModels.Add(design);
                }

                return Result<List<DesignModel>>.Success(designModels);
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
                var result = await _designRepository.GetDesignCategoriesAsync();

                return Result<List<DesignCategoryModel>>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in: DesignService.GetDesignCategories()");
                throw;
            }
        }

        public async Task<Result<List<DesignModel>>> GetGesignsByCategoryIdPaginated(int categoryId, int pageSize, int page)
        {
            try
            {
                var result = await _designRepository.GetDesignsByCategoryIdAsync(categoryId, pageSize, page);
                var designModels = new List<DesignModel>();
                foreach (var item in result)
                {
                    var design = new DesignModel
                    {
                        DesignId = item.DesignId,
                        DesignName = item.DesignName,
                        ImgUrl = item.ImgUrl,
                        MockUrl = item.MockUrl,
                    };
                    designModels.Add(design);
                }

                return Result<List<DesignModel>>.Success(designModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in: DesignService.GetGesignsByCategoryIdPaginated()");
                throw;
            }
        }

        public async Task<Result<DesignModel>> GetDesignByIdAsync(int designId)
        {
            try
            {
                var result = await _designRepository.GetDesignByIdAsync(designId);

                if (result != null)
                {
                    var designModel = new DesignModel
                    {
                        DesignId = result.DesignId,
                        DesignName = result.DesignName,
                        Description = result.Description,
                        ImgUrl = result.ImgUrl,
                        MockUrl = result.MockUrl,
                    };

                    return Result<DesignModel>.Success(designModel);
                }
                else
                {
                    return Result<DesignModel>.Failure(new Error("sa"));
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in: DesignService.GetGesignsByCategoryIdPaginated()");
                throw;
            }
        }

        public async Task<Result<List<DesignModel>>> GetBestsellingDesignsPaginated(int pageSize, int page)
        {
            try
            {
                var result = await _designRepository.GetBestSellersDesigns(pageSize, page);
                var designModels = new List<DesignModel>();
                foreach (var item in result)
                {
                    var design = new DesignModel
                    {
                        DesignId = item.DesignId,
                        DesignName = item.DesignName,
                        ImgUrl = item.ImgUrl,
                        MockUrl = item.MockUrl,
                    };
                    designModels.Add(design);
                }

                return Result<List<DesignModel>>.Success(designModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in: DesignService.GetGesignsByCategoryIdPaginated()");
                throw;
            }
        }

      
    }
}
