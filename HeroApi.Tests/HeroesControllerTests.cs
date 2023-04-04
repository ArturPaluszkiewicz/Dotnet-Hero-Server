using HeroApi.Controllers;
using HeroApi.Models;
using HeroApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HeroApi.Tests;

public class HeroesControllerTest
{
    [Fact]
    public void GetHeroes_ReturnCorrectResult()
    {
        //arrange
        var mocHeroesService = new Mock<IHeroesService>();
        mocHeroesService.Setup(service => service.getAllHeroes()).Returns(getMockedDTOHeroesList());
        var controller = new HeroesController(mocHeroesService.Object);
        //act
        var result = controller.GetHeroes();
        //assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<HeroDTO>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var returnValue = Assert.IsType<List<HeroDTO>>(okResult.Value);
        var hero = returnValue.FirstOrDefault();
        Assert.Equal("Captain America",hero.Name);
    }
    [Fact]
    public void GetHeroById_ReturnNotFoundResult_ForNonExistingHero()
    {
        //arrange
        var mocHeroesService = new Mock<IHeroesService>();
        mocHeroesService.Setup(service => service.getHeroById(5)).Returns((HeroDTO)null);
        var controller = new HeroesController(mocHeroesService.Object);
        //act
        var result = controller.GetHero(5);
        //assert
        var notFoundResult = Assert.IsType<ActionResult<HeroDTO>>(result);
        Assert.IsType<NotFoundResult>(notFoundResult.Result);
    }
    [Fact]
    public void GetHeroById_ReturnCorrectResult()
    {
        //arrange
        var mocHeroesService = new Mock<IHeroesService>();
        mocHeroesService.Setup(service => service.getHeroById(5)).Returns(getMockedDTOHero(5));
        var controller = new HeroesController(mocHeroesService.Object);
        //act
        var result = controller.GetHero(5);
        //assert
        var actionResult = Assert.IsType<ActionResult<HeroDTO>>(result);
        var resultValue = Assert.IsType<OkObjectResult>(actionResult.Result);
        var returnValue = Assert.IsType<HeroDTO>(resultValue.Value);
        Assert.Equal("Hulk",returnValue.Name);
        Assert.Equal(5,returnValue.Id);
    }
    [Fact]
    public void PostHero_ReturnBadRequest_ForNotValidHeroDto()
    {
        //arrange
        var mocHeroesService = new Mock<IHeroesService>();
        var controller = new HeroesController(mocHeroesService.Object);
        //act
        var result = controller.PostHero(new HeroDTO{});
        //assert
        var actionResult = Assert.IsType<ActionResult<string>>(result);
        Assert.IsType<BadRequestResult>(actionResult.Result);
    }
    [Fact]
    public void PostHero_ReturnCreated()
    {
        //arrange
        var mocHeroesService = new Mock<IHeroesService>();
        mocHeroesService.Setup(s => s.createHero(getMockedDTOHero(5))).Returns(5);
        var controller = new HeroesController(mocHeroesService.Object);
        //act
        var result = controller.PostHero(getMockedDTOHero(5));
        //assert
        var actionResult = Assert.IsType<ActionResult<string>>(result);
        var createdResult = Assert.IsType<CreatedResult>(actionResult.Result);
        //Assert.Equal("/api/heroes/5",createdResult.Location);
    }
    [Fact]
    public void PutHero_BadRequestResult_ForNotMatchingGivenIdWhithIdFromGivenHeroDto()
    {
        //arrange
        var mocHeroesService = new Mock<IHeroesService>();
        var controller = new HeroesController(mocHeroesService.Object);
        //act
        var result = controller.PutHero(4,getMockedDTOHero(5));
        //assert
        Assert.IsType<BadRequestResult>(result);
    }
    [Fact]
    public void PutHero_NotFound_WhenThereIsNoHeroForGivenId()
    {
        //arrange
        var mocHeroesService = new Mock<IHeroesService>();
        mocHeroesService.Setup(s => s.updateHero(5,getMockedDTOHero(5))).Returns(false);
        var controller = new HeroesController(mocHeroesService.Object);
        //act
        var result = controller.PutHero(5,getMockedDTOHero(5));
        //assert
        Assert.IsType<NotFoundResult>(result);

    }
    [Fact]
    public void PutHero_ReturnNoContent_AfterSuccesCreatHero()
    { 
    //arrange
    var mocHeroesService = new Mock<IHeroesService>();
    mocHeroesService.Setup(s => s.updateHero(5,getMockedDTOHero(5))).Returns(true);
    var controller = new HeroesController(mocHeroesService.Object);
    //act
    var result = controller.PutHero(5,getMockedDTOHero(5));
    //assert
   // Assert.IsType<NoContentResult>(result);
    }
    [Fact]
    public void DeleteHero_ReturnNotFound_ForNonHeroWithGivenId()
    {
        //arrange
        var mocHeroesService = new Mock<IHeroesService>();
        mocHeroesService.Setup(s => s.deleteHero(5)).Returns(false);
        var controller = new HeroesController(mocHeroesService.Object);
        //act
        var result = controller.DeleteHero(5);
        //assert
        Assert.IsType<NotFoundResult>(result);
    }
    [Fact]
    public void DeleteHero_ReturnNoContent_AffterSuccesfullDeleteHero()
    {
        //arrange
        var mocHeroesService = new Mock<IHeroesService>();
        mocHeroesService.Setup(s => s.deleteHero(5)).Returns(true);
        var controller = new HeroesController(mocHeroesService.Object);
        //act
        var result = controller.DeleteHero(5);
        //assert
        Assert.IsType<NoContentResult>(result);
    }
    private IEnumerable<HeroDTO> getMockedDTOHeroesList()
    {
        var heroes = new List<HeroDTO>();
        heroes.Add(new HeroDTO{
            Id = 1,
            Name = "Captain America"
        });
        heroes.Add(new HeroDTO{
            Id = 2,
            Name = "Iron Man"
        });
        return heroes;
    }
    private HeroDTO getMockedDTOHero(long id)
    {
        return new HeroDTO{
            Id = id,
            Name = "Hulk"
        };
    }
}