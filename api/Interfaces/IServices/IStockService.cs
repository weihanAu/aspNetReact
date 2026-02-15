using System.Linq.Expressions;
using api.Dtos;
using api.Helper;
using api.models;

namespace api.Interfaces.IServices;
public interface IStockService
{
  Task<List<StockDto>> GetAllStocks(QuerySearch querySearch);
} 