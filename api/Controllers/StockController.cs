
using api.Dtos;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.data;
using api.Interface;
using api.Helper;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using api.Interfaces.IServices;
namespace api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class StockController : ControllerBase
  {
 
    private readonly IStockRepository _stockRepository;
    private readonly IStockService _stockService;
    public StockController(IStockRepository stockRepository,IStockService stockService)
    {
      _stockRepository = stockRepository;
      _stockService = stockService;
    }

    [HttpGet]
    // [Authorize]
    public async Task<IActionResult> GetAll([FromQuery] QuerySearch querySearch)
    {
     var stocks = await _stockService.GetAllStocks(querySearch);
     return Ok(stocks);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute]int id)
    {
      var stock = await _stockRepository.GetStockByIdAsync(id);
      if (stock == null)
      {
        return NotFound();
      }
      return Ok(stock.ToStockDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDto createDto)
    {
      var stockModel = createDto.ToStockDtoFromCreateDto();
      await _stockRepository.AddStockAsync(stockModel);
      return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockDto updateStockDto)
    {
      var stockModel = await _stockRepository.UpdateStockAsync(id, updateStockDto);
      if (stockModel == null)
      {
        return NotFound();
      }
      return Ok(stockModel.ToStockDto());
    } 

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
      var stockModel = await _stockRepository.DeleteStockAsync(id);
      if (stockModel == null)
      {
        return NotFound();
      }
      return NoContent();
    }
  }
}
