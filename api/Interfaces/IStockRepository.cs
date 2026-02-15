using api.models;

namespace api.Interface;

using api.Dtos;
using api.data;
using System.Linq.Expressions;

public interface IStockRepository
{
  Task<List<StockDto>> GetAllStocksAsync(Expression<Func<Stock, bool>>? callback = null,int pageNumber=1);
  Task<Stock?> GetStockByIdAsync(int id);

  Task<Stock> AddStockAsync(Stock stockModel);
  Task<Stock?> UpdateStockAsync(int id, UpdateStockDto stockDto);
  Task<Stock?> DeleteStockAsync(int id);
  Task<bool> StockExistsAsync(int id);
  Task<Stock?> GetBySympolAsync(string sympol);
} 