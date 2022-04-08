using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Refit;

namespace webapi.DomainServices.Interfaces
{
    [Headers("Content-Type: application/json")]
    public interface ICocktailAPI
    {
        [Get("/api/json/v1/1/random.php")]
        Task<HttpResponseMessage> GetRandomCocktail();

        //[Post("/api/mobile/CargaEvidenciaFallas")]
        //Task<HttpResponseMessage> CargaEvidenciaFallas([Header("Authorization")] string jwtToken, [Body(BodySerializationMethod.Serialized)] CargaEvidenciaFallasRequest request);
    }
}
