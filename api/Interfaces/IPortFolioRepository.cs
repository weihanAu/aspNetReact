using api.models;

namespace  api.Repository
{
  public interface IPortFolioRepository
  {
   Task<List<Stock>> GetUserPortFolios(string userName);
   Task<Portfolio?> AddToPortFolio(Portfolio portfolio);
   Task<bool?> DeleteFromPortFolio(string username,int portfolioId);
  }
}