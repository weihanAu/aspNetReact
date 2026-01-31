using api.Dtos;
using api.models;

namespace api.Mappers;

public static class StockMapper
{
  public static StockDto ToStockDto(this Stock stock)
  {
    return new StockDto
    {
      Id = stock.Id,
      Symbol = stock.Symbol,
      CompanyName = stock.CompanyName,
      Purchase = stock.Purchase,
      LastDiv = stock.LastDiv,
      Industry = stock.Industry,
      MarketCap = stock.MarketCap
    };
  }

  public static Stock ToStockDtoFromCreateDto(this CreateDto createDto)
  {
    return new Stock
    {
      Symbol = createDto.Symbol,
      CompanyName = createDto.CompanyName,
      Purchase = createDto.Purchase,
      LastDiv = createDto.LastDiv,
      Industry = createDto.Industry,
      MarketCap = createDto.MarketCap
    };
  }


}