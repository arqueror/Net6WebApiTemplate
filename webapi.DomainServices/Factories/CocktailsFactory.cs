using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webapi.Data.Entities;
using webapi.DomainServices.Exceptions;
using webapi.DTOs;

namespace webapi.DomainServices.Factories
{
    public static class CocktailsFactory
    {
        public static CocktailDTO ToDTO(this List<Cocktail> coctail)
        {
            try
            {
                return new CocktailDTO()
                {

                };
            }
            catch (Exception ex)
            {
                throw new FactoryDataMappingException(ex, $"{nameof(CocktailsFactory)}.{nameof(ToDTO)}");
            }
        }

        public static CocktailDTO ToDTO(this Cocktail coctail)
        {
            try
            {
                return new CocktailDTO()
                {

                };
            }
            catch (Exception ex)
            {
                throw new FactoryDataMappingException(ex, $"{nameof(CocktailsFactory)}.{nameof(ToDTO)}");
            }
        }
    }
}
