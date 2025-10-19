using elfBeauty.App.Dto;
using elfBeauty.App.Interfaces;
using elfBeauty.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace elfBeauty.Moq;

public class AestheticApiTest
{
    private readonly Mock<IAestheticSvc> _mockAestheticSvc;
    private readonly AestheticController _aestheticController;

    public AestheticApiTest()
    {
        _mockAestheticSvc = new Mock<IAestheticSvc>();
        _aestheticController = new AestheticController(_mockAestheticSvc.Object);
    }

    #region Action Tests: Api Version - V1 contains In-memory cache

    [Fact]
    public async Task GetAestheticV1_ReturnsOkResult_WhenAestheticExist()
    {
        var aesthics = new List<AestheticDto>
            {
                new AestheticDto {  Name = "Brewery 1", City = "City 1", Phone = "123456" },
                new AestheticDto {  Name = "Brewery 2", City = "City 2", Phone = "654321" }
            };
        _mockAestheticSvc.Setup(s => s.GetAestheticsAsync_Cache(null, null, null, null)).ReturnsAsync(aesthics);

        var result = await _aestheticController.GetAestheticsAsync_Cache(null, null, null, null);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<AestheticDto>>(okResult.Value);

        Assert.Equal(2, returnValue.Count);
    }

    [Fact]
    public async Task GetAestheticV1_ReturnsNotFound_WhenNoAestheticExist()
    {
        _mockAestheticSvc.Setup(s => s.GetAestheticsAsync_Cache(null, null, null, null)).ReturnsAsync(new List<AestheticDto>());

        var result = await _aestheticController.GetAestheticsAsync_Cache(null, null, null, null);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task AutocompleteV1_ReturnsOkResult_WhenAestheticExist()
    {
        var aesthics = new List<string> { "Brewery 1", "City 1", "Brewery 2", "City 2" };
        _mockAestheticSvc.Setup(s => s.Autocomplete_Cache(null)).ReturnsAsync(aesthics);

        var result = await _aestheticController.AutocompleteAsnc_Cache(null);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<string>>(okResult.Value);

        Assert.Equal(4, returnValue.Count);
    }

    [Fact]
    public async Task AutocompleteV1_ReturnsNotFound_WhenNoAestheticExist()
    {
        _mockAestheticSvc.Setup(s => s.Autocomplete_Cache(null)).ReturnsAsync(new List<string>());

        var result = await _aestheticController.AutocompleteAsnc_Cache(null);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    #endregion
}
