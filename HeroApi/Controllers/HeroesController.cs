using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HeroApi.Entities;
using HeroApi.Models;
using HeroApi.Services;

namespace HeroApi.Controllers
{
    [Route("api/heroes")]
    [ApiController]
    public class HeroesController : ControllerBase
    {
        private readonly HeroesService _heroesService;

        public HeroesController(HeroesService heroesService)
        {
            _heroesService = heroesService;
        }

        // GET: api/Heroes
        [HttpGet]
        public ActionResult<IEnumerable<HeroDTO>> GetHeroes()
        {
           // return await _context.Heroes.Select(h => heroToHeroDto(h)).ToListAsync();
           var heroes = _heroesService.getAllHeroes();
            return Ok(heroes);
        }

        [HttpGet("{id}")]
        public ActionResult<HeroDTO> GetHero(long id)
        {
           // var hero = await _context.Heroes.FindAsync(id);

            var heroDTO = _heroesService.getById(id);

            if (heroDTO == null)
            {
                return NotFound();
            }
            return Ok(heroDTO);

        }

        [HttpPut("{id}")]
        public IActionResult PutHero(long id, HeroDTO heroDTO)
        {
            if (id != heroDTO.Id)
            {
                return BadRequest();
            }
            bool isUpdated= _heroesService.updateHero(id, heroDTO);
            if(!isUpdated){
                return NotFound();
            }
            return NoContent();
        }

        // POST: api/Heroes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<Hero> PostHero(HeroDTO heroDTO)
        {

            if (heroDTO.Name == null){
                return BadRequest();
            }
            long newHeroId = _heroesService.createHero(heroDTO);

            return Created($"/api/restaurant/{newHeroId}",null);
        }

        // DELETE: api/Heroes/5
        [HttpDelete("{id}")]
        public IActionResult DeleteHero(long id)
        {
            bool isDeleted = _heroesService.deleteHero(id);
            if(!isDeleted){
                return NotFound();
            }
            return NoContent();
        }
        private static HeroDTO heroToHeroDto(Hero hero) =>
            new HeroDTO{
                Id = hero.Id,
                Name = hero.Name
            };
    }
}
