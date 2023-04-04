using HeroApi.Entities;
using HeroApi.Models;

namespace HeroApi.Services
{
    public class HeroesService : IHeroesService
    {
        private readonly HeroContext _context;
        public HeroesService(HeroContext context)
        {
            _context = context;
        }

        public IEnumerable<HeroDTO> getAllHeroes()
        {
            return _context.Heroes.Select(h => heroToHeroDto(h)).ToList();
        }

        public HeroDTO? getHeroById(long id)
        {
            var hero = _context.Heroes.Find(id);
            if (hero is null) return null;
            return heroToHeroDto(hero);
        }
        public long createHero(HeroDTO heroDTO)
        {
            var hero = new Hero{
                Name = heroDTO.Name
            };
            _context.Heroes.Add(hero);
            _context.SaveChanges();

            return hero.Id;
        }
        public bool updateHero(long id, HeroDTO heroDTO)
        {
            var hero = _context.Heroes.Find(id);
            if (hero is null) return false;
            hero.Name = heroDTO.Name;
            _context.SaveChanges();
            return true;
        }
        public bool deleteHero(long id)
        {
            var hero = _context.Heroes.Find(id);
            if (hero is null) return false;
            _context.Heroes.Remove(hero);
            _context.SaveChanges();
            return true;
        }
        private static HeroDTO heroToHeroDto(Hero hero) =>
            new HeroDTO{
                Id = hero.Id,
                Name = hero.Name
            };
    }
}