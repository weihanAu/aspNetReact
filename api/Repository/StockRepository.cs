namespace api.Repository;

using System.Collections.Generic;
using System.Threading.Tasks;
using api.Interface;
using api.models;
using api.data;
using Microsoft.EntityFrameworkCore;
using api.Dtos;
using api.Dtos.Comment;

public class StockRepository : IStockRepository
{
  private readonly ApplicationDbContext _context;
  public StockRepository(ApplicationDbContext context)
  {
    _context = context;
  }

  public async Task<Stock> AddStockAsync(Stock stockModel)
  {
    await _context.Stocks.AddAsync(stockModel);
    await _context.SaveChangesAsync();
    return stockModel;
  }

  public async Task<Stock?> DeleteStockAsync(int id)
  {
    var stockModel = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);
    if (stockModel == null)
    {
      return null;
    }
    _context.Stocks.Remove(stockModel);
    await _context.SaveChangesAsync();
    return stockModel;
  }

  public Task<List<StockDto>> GetAllStocksAsync()
  {
    return _context.Stocks.Select(s => new StockDto
    {
      Id = s.Id,
      Symbol = s.Symbol,
      CompanyName = s.CompanyName,
      Purchase = s.Purchase,
      LastDiv = s.LastDiv,
      MarketCap = s.MarketCap,
      //using DTO to avoid loop reference issue
      comments = s.Comments.Select(c => new CommentDto
      {
        Id = c.Id,
        Title = c.Title,
        content = c.content,
        createdAt = c.createdAt,
        stockId = c.stockId
      }).ToList(),
    }).ToListAsync();
  }

  public Task<Stock?> GetStockByIdAsync(int id)
  {
    var stock = _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);
    return stock;
  }

  public Task<bool> StockExistsAsync(int id)
  {
    return _context.Stocks.AnyAsync(s => s.Id == id);
  }

  public async Task<Stock?> UpdateStockAsync(int id, UpdateStockDto updateStockDto)
  {
    var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
    if (stockModel == null)
    {
      return null;
    }
    stockModel.Symbol = updateStockDto.Symbol;
    stockModel.CompanyName = updateStockDto.CompanyName;
    stockModel.Purchase = updateStockDto.Purchase;
    stockModel.LastDiv = updateStockDto.LastDiv;
    stockModel.MarketCap = updateStockDto.MarketCap;
    await _context.SaveChangesAsync();
    return stockModel;
  }
}