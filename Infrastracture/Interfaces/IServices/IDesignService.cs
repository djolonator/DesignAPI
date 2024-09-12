
using Infrastracture.Models;
using Infrastructure.Abstractions;

namespace Infrastracture.Interfaces.IServices
{
    public interface IDesignService
    {
        Task<Result<List<DesignModel>>> SearchDesigns(string term);
    }
}
