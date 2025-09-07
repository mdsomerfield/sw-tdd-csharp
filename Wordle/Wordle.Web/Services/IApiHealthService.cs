using Wordle.Web.Models;

namespace Wordle.Web.Services;

public interface IApiHealthService
{
    Task<ApiStatusModel> GetApiStatusAsync();
}