using Fusillade;

namespace webapi.DomainServices.Infrastructure.ApiServices
{
    public interface IApiService<T>
    {
        T GetApi(Priority priority);
    }
}
