using Fusillade;
using webapi.Data.Entities;
using webapi.DomainServices.Exceptions;
using webapi.DomainServices.Infrastructure;
using webapi.DomainServices.Infrastructure.ApiServices;
using webapi.DomainServices.Interfaces;
using webapi.DomainServices.Interfaces.ApiServices;
using webapi.DTOs;

namespace webapi.DomainServices.ApiServices
{
    public class CocktailApiManagerService : BaseAPI<ICocktailAPI>, ICocktailApiManagerService
    {
        public CocktailApiManagerService(IApiService<ICocktailAPI> remoteApi) : base(remoteApi) { }

        public async Task<APIResponseDTO<Cocktails>> GetRandomCocktail()
        {
            try
            {
                return await CreateRemoteCall<Cocktails>(RemoteRequestAsync(_remoteApi.GetApi(Priority.UserInitiated).GetRandomCocktail()));
            }

            catch (Exception ex)
            {
               throw new WepApiCallException(ex, $"{nameof(CocktailApiManagerService)}.{nameof(GetRandomCocktail)}");
            }
        }
    }
}
