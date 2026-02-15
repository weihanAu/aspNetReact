namespace api.Repository;

using System.Collections.Generic;
using System.Threading.Tasks;
using api.Interface;
using api.models;
using api.data;
using Microsoft.EntityFrameworkCore;
using api.Dtos;
using api.Dtos.Comment;
using System.Linq.Expressions;

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
  //modified to accept optional expression callback for filtering
  public async Task<List<StockDto>> GetAllStocksAsync(Expression<Func<Stock, bool>>? callback = null, int pageNumber = 1)
  {
    // 1. 开启 Queryable “生成器”，此时没有任何数据库操作
    var query = _context.Stocks.AsQueryable();

    // 2. 动态叠加过滤条件
    // 如果调用者传了条件（比如 s => s.Symbol == "AAPL"），就拼接到 SQL 的 WHERE 子句中
    if (callback != null)
    {
      query = query.Where(callback);
    }
    int skipNumber = (pageNumber - 1) * 1;
    // 3. 统一执行 Select 投影和最后的异步转换
    // 注意：所有的逻辑会在这一步被翻译成一条高效的 SQL 发送给数据库
    return await query.Select(s => new StockDto
    {
      Id = s.Id,
      Symbol = s.Symbol,
      CompanyName = s.CompanyName,
      Purchase = s.Purchase,
      LastDiv = s.LastDiv,
      MarketCap = s.MarketCap,
      comments = s.Comments.Select(c => new CommentDto
      {
        Id = c.Id,
        Title = c.Title,
        content = c.content,
        createdAt = c.createdAt,
        stockId = c.stockId
      }).ToList(),
    }).Skip(skipNumber).Take(1).ToListAsync();
  }

  public Task<Stock?> GetBySympolAsync(string sympol)
  {
    var stock = _context.Stocks.FirstOrDefaultAsync(s => s.Symbol == sympol);
    return stock;
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