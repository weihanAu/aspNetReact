using api.data;
using api.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace  api.Repository
{
  public class PortFolioRepository : IPortFolioRepository
  {
    private readonly ApplicationDbContext _context;
    private readonly UserManager<AppUser> _UserManager;
    public PortFolioRepository(ApplicationDbContext context, UserManager<AppUser> userManager)
    {
      _context = context;
      _UserManager = userManager;
    }

    public async Task<Portfolio?> AddToPortFolio(Portfolio portfolio)
    {
      await _context.Portfolios.AddAsync(portfolio);
      await _context.SaveChangesAsync();
      return new Portfolio
      {
        AppUserId = portfolio.AppUserId,
        StockId = portfolio.StockId
      };
    }

    public async Task<bool?> DeleteFromPortFolio(string username, int portfolioId)
    {
      var AppUser = await _UserManager.FindByNameAsync(username);
      var portFolio = await _context.Portfolios.FirstOrDefaultAsync(portfolio => portfolio.AppUserId == AppUser.Id && portfolio.StockId == portfolioId);
      if (portFolio == null) return false;
      _context.Portfolios.Remove(portFolio);    
      await _context.SaveChangesAsync();
      return true;
    }

    public async Task<List<Stock>> GetUserPortFolios(string userName)
    {
      var AppUser = await _context.Users.FirstOrDefaultAsync(user => user.UserName == userName);
      var portFolios = await _context.Portfolios
          .Where(portfolio=> portfolio.AppUserId == AppUser.Id)
          .Select(portfolio =>new Stock
          {
            Id = portfolio.Stock.Id,
            Symbol= portfolio.Stock.Symbol,
            CompanyName = portfolio.Stock.CompanyName
          })
          .ToListAsync();
      return portFolios;
    }
  }
}