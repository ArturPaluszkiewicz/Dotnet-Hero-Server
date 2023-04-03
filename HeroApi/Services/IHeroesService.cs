using HeroApi.Models;

namespace HeroApi.Services
{
    public interface IHeroesService
    {
        IEnumerable<HeroDTO> getAllHeroes();
        HeroDTO? getById(long id);
        long createHero(HeroDTO heroDTO);
        bool updateHero(long id, HeroDTO heroDTO);
        bool deleteHero(long id);
    }
}