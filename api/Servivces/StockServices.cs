namespace api.Servivces;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using api.Dtos;
using api.Helper;
using api.Interface;
using api.Interfaces.IServices;
using api.models;

public class StockServices : IStockService
{
  private readonly IStockRepository _stockRepository;
  public StockServices(IStockRepository stockRepository)
  {
    _stockRepository = stockRepository;
  } 
  public async Task<List<StockDto>> GetAllStocks(QuerySearch querySearch)
  {
    var allStocks = await _stockRepository.GetAllStocksAsync(
      x => !string.IsNullOrEmpty(querySearch.CompanyName)? x.CompanyName!.ToLower().Contains(querySearch.CompanyName!.ToLower()):true
      && !string.IsNullOrEmpty(querySearch.Symbol)? x.Symbol!.ToLower().Contains(querySearch.Symbol!.ToLower()):true
      ,pageNumber: querySearch.pageNumber>0? querySearch.pageNumber:1
      );
    return allStocks;
  }
}