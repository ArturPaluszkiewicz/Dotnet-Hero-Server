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
        private readonly IHeroesService _heroesService;

        public HeroesController(IHeroesService heroesService)
        {
            _heroesService = heroesService;
        }

        // GET: api/Heroes
        [HttpGet]
        public ActionResult<IEnumerable<HeroDTO>> GetHeroes(string? name)
        {
            if(name!=null){
                var heroes = _heroesService.getHeroesByName(name);
                Console.Write(name);
                return Ok(heroes);

            }
            else{
                var heroes = _heroesService.getAllHeroes();
                Console.Write("no name");
                return Ok(heroes);
            }
        }
        // [HttpGet()]
        // public ActionResult<IEnumerable<HeroDTO>> GetHeroes(string name)
        // {
        //    var heroes = _heroesService.getHeroesByName("Wizard Lizard");
        //    Console.Write("asd2");
        //     return Ok(heroes);
        // }
        [HttpGet("{id}")]
        public ActionResult<HeroDTO> GetHero(long id)
        {
           // var hero = await _context.Heroes.FindAsync(id);

            var heroDTO = _heroesService.getHeroById(id);

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
            bool isUpdated = _heroesService.updateHero(id, heroDTO);
            if(!isUpdated){
                return NotFound();
            }
            return NoContent();
        }

        // POST: api/Heroes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<HeroDTO> PostHero(HeroDTO heroDTO)
        {

            if (heroDTO.Name == null){
                return BadRequest();
            }
            var newHero = _heroesService.createHero(heroDTO);

            return Created($"/api/heroes/{newHero.Id}",newHero);
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
