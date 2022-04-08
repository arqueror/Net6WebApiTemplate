using System.Threading.Tasks;
using webapi.Data.Entities;
using webapi.DTOs;

namespace webapi.DomainServices.Interfaces.ApiServices
{
    public interface ICocktailApiManagerService
    {
        Task<APIResponseDTO<Cocktails>> GetRandomCocktail();
    }
}
