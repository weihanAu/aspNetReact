using api.models;

namespace api.Interface;

using api.Dtos;
using api.data;

public interface IStockRepository
{
  Task<List<StockDto>> GetAllStocksAsync();
  Task<Stock?> GetStockByIdAsync(int id);

  Task<Stock> AddStockAsync(Stock stockModel);
  Task<Stock?> UpdateStockAsync(int id, UpdateStockDto stockDto);
  Task<Stock?> DeleteStockAsync(int id);
  Task<bool> StockExistsAsync(int id);
} 